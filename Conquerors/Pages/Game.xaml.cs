﻿using System.ServiceModel;
using Conquerors.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Reflection;
using System.Windows.Resources;

namespace Conquerors.Pages
{
    
    public partial class Game : Page
    {
        App app;
        Map map;
        List<Edge> edgeList = new List<Edge>();
        List<Node> ownedNodes = new List<Node>();
        List<Agent> visibleEnemies = new List<Agent>();
        List<ArgHandler> listOfArguments = new List<ArgHandler>();
        List<String> listOfModules = new List<string>();
        List<ControlInterface> modules = new List<ControlInterface>();
        string ModuleName;
        Player ActivePlayer;
        int turn = 0;
        int moraleEffectModifier = 0;
        int goldGainModifier = 0;
        int foodGainModifier = 0;
        int stoneGainModifier = 0;
        bool FoWSetUp = false;

        public Game()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        /// <summary>
        /// The function that is called when the page loads. It sets up the game for the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*Set up the interface for the player when the page loads*/
            app = (App)Application.Current;
            setPlayer();
            setTheMap();
            StartLoadingControlPlugins();
            setPlayerFOW();
            showAgents();
            showResources();
            hideControls();
            unselect();
        }

        /// <summary>
        /// Loads the control plugins
        /// </summary>
        private void StartLoadingControlPlugins()
        {
            ControlModuleLoaderClient proxy = new ControlModuleLoaderClient();
            proxy.GetModuleListCompleted += new EventHandler<GetModuleListCompletedEventArgs>(proxy_GettingModuleListCompleted);
            proxy.GetModuleListAsync();
        }

        private void proxy_GettingModuleListCompleted(object sender, GetModuleListCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("There was an error while trying to connect to the service!");
            }
            else
            {
                foreach(string f in e.Result)
                {
                    listOfModules.Add(f);
                }
                LoadControlPlugins(listOfModules.First());
            }
        }

        private void LoadControlPlugins(string moduleToLoad)
        {
            ModuleName = moduleToLoad.Replace("C:/Users/lugerovac/Documents/visual studio 2013/Projects/Conquerors/Conquerors.Web/ClientBin/", string.Empty);
            ModuleName = ModuleName.Replace(".xap", string.Empty);

            WebClient _webClient = null;
            if (null == _webClient)
            {//initialize the downloader
                _webClient = new WebClient();
                _webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(onOpenReadCompleted);
            }

            if (_webClient.IsBusy)
            {//needs to cancel the previous loading
                _webClient.CancelAsync();
                return;
            }

            Uri xapUrl = new Uri(moduleToLoad, UriKind.RelativeOrAbsolute);
            _webClient.OpenReadAsync(xapUrl);
        }

        private void onOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (null != e.Error)
            {
                MessageBox.Show("There was an error while trying to load the module! " + e.Error.Message);
                return;
            }

            if (e.Cancelled)
            {
                MessageBox.Show("Module loading was cancelled!");
                return;
            }

            Assembly aDLL = GetAssemblyFromPackage(ModuleName + ".dll", e.Result);
            if (null == aDLL)
            {
                MessageBox.Show("Module downloaded but failed to extract the assembly.");
                return;
            }

            ControlInterface content = aDLL.CreateInstance(ModuleName + "." + ModuleName) as ControlInterface;
            modules.Add(content);

            listOfModules.Remove(listOfModules.First());
            if (listOfModules.Count != 0) 
                LoadControlPlugins(listOfModules.First());
        }

        private Assembly GetAssemblyFromPackage(string assemblyName, Stream xapStream)
        {
            Assembly aDLL = null;

            Uri assemblyUri = new Uri(assemblyName, UriKind.Relative);
            StreamResourceInfo resPackage = new StreamResourceInfo(xapStream, null);
            if (null == resPackage)
                return aDLL;

            StreamResourceInfo resAssembly = Application.GetResourceStream(resPackage, assemblyUri);
            if (null == resAssembly)
                return aDLL;

            AssemblyPart part = new AssemblyPart();
            aDLL = part.Load(resAssembly.Stream);

            return aDLL;
        }

        /// <summary>
        /// This function handles selection of enemy commanders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="ID">Enemy commander's ID</param>
        /// <param name="owner">Enemy commander's owner</param>
        private void enemyCommanderSelect(object sender, MouseButtonEventArgs e, string ID, enmPlayers owner)
        {
            foreach(Player player in app.players)
            {
                if (player.color == owner)
                {
                    foreach (Commander commander in player.Commanders)
                    {
                        if (string.Equals(commander.ID, ID))
                        {
                            commander.Select();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function searches for nodes that are visible to the player, aka not hidden by the Fog of War
        /// </summary>
        /// <returns>List of nodes that are fully visible to the player</returns>
        private List<string> giveSurveiledNodes()
        {
            List<string> output = new List<string>();
            foreach(Node node in map.nodeList)
            {
                if (!node.darkened)
                    output.Add(node.Name);
            }
            return output;
        }

        /// <summary>
        /// Shows the agents that are added by loading a saved game
        /// </summary>
        public void showAgents()
        {
            List<string> surveiledNodes = giveSurveiledNodes();
            foreach(Player player in app.players)
            {
                foreach(Commander a in player.Commanders)
                {
                    if (player.color != ActivePlayer.color && !a.visible)
                    {
                        if (!surveiledNodes.Contains(a.location)) continue;
                        double x = 0, y = 0;
                        foreach (Node node in map.nodeList)
                        {
                            if (string.Equals(a.location, node.Name))
                            {
                                GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                                Point offset = getPosition.Transform(new Point(0, 0));
                                x = offset.X;
                                y = offset.Y;
                                break;
                            }
                        }

                        a.visible = true;
                        a.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => enemyCommanderSelect(sender, e, a.ID, player.color));
                        a.sprite.SetValue(Canvas.LeftProperty, x - 70);
                        a.sprite.SetValue(Canvas.TopProperty, y + 10);
                        cnvMapa.Children.Add(a.sprite);
                        if (a.moving) a.sprite.Opacity = Constants.darkenedSpriteOpacity;
                        visibleEnemies.Add(a);
                    }
                    else if(!a.visible)
                    {
                        double x = 0, y = 0;
                        foreach (Node node in map.nodeList)
                        {
                            if (string.Equals(a.location, node.Name))
                            {
                                GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                                Point offset = getPosition.Transform(new Point(0, 0));
                                x = offset.X;
                                y = offset.Y;
                                break;
                            }
                        }

                        a.visible = true;
                        a.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => CommanderSelect(sender, e, a.ID));
                        a.sprite.SetValue(Canvas.LeftProperty, x - 70);
                        a.sprite.SetValue(Canvas.TopProperty, y + 10);
                        cnvMapa.Children.Add(a.sprite);
                        if (a.moving) a.sprite.Opacity = Constants.darkenedSpriteOpacity;
                    }
                }  //Commanders

                foreach (Steward a in player.Stewards)
                {
                    if (player.color != ActivePlayer.color && !a.visible)
                    {
                        //for now don't show enemy stewards
                        continue;
                    }
                    else if (!a.visible)
                    {
                        double x = 0, y = 0;
                        foreach (Node node in map.nodeList)
                        {
                            if (string.Equals(a.location, node.Name))
                            {
                                GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                                Point offset = getPosition.Transform(new Point(0, 0));
                                x = offset.X;
                                y = offset.Y;
                                break;
                            }
                        }

                        a.visible = true;
                        a.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => StewardSelect(sender, e, a.ID));
                        a.sprite.SetValue(Canvas.LeftProperty, x - 70);
                        a.sprite.SetValue(Canvas.TopProperty, y + 10);
                        cnvMapa.Children.Add(a.sprite);
                        if (a.Works() || a.moving)
                            a.sprite.Opacity = Constants.darkenedSpriteOpacity;
                    }
                }  //Stewards

                foreach (Assassin a in player.Assassins)
                {
                    if (player.color != ActivePlayer.color && !a.visible)
                    {
                        //for now don't show enemy assassins
                        continue;
                    }
                    else if (!a.visible)
                    {
                        double x = 0, y = 0;
                        foreach (Node node in map.nodeList)
                        {
                            if (string.Equals(a.location, node.Name))
                            {
                                GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                                Point offset = getPosition.Transform(new Point(0, 0));
                                x = offset.X;
                                y = offset.Y;
                                break;
                            }
                        }

                        a.visible = true;
                        a.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => AssassinSelect(sender, e, a.ID));
                        a.sprite.SetValue(Canvas.LeftProperty, x - 70);
                        a.sprite.SetValue(Canvas.TopProperty, y + 10);
                        cnvMapa.Children.Add(a.sprite);
                        if (a.moving) a.sprite.Opacity = Constants.darkenedSpriteOpacity;
                    }
                }  //Assassins

                foreach (Scout a in ActivePlayer.Scouts)
                {
                    if (player.color != ActivePlayer.color && !a.visible)
                    {
                        //for now don't show enemy assassins
                        continue;
                    }
                    else if (!a.visible)
                    {
                        double x = 0, y = 0;
                        foreach (Node node in map.nodeList)
                        {
                            if (string.Equals(a.location, node.Name))
                            {
                                GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                                Point offset = getPosition.Transform(new Point(0, 0));
                                x = offset.X;
                                y = offset.Y;
                                break;
                            }
                        }

                        a.visible = true;
                        a.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => ScoutSelect(sender, e, a.ID));
                        a.sprite.SetValue(Canvas.LeftProperty, x - 70);
                        a.sprite.SetValue(Canvas.TopProperty, y + 10);
                        cnvMapa.Children.Add(a.sprite);
                        if (a.moving) a.sprite.Opacity = Constants.darkenedSpriteOpacity;
                    }
                }  //Scouts
            }

            foreach(Node node in map.nodeList)
            {
                if (node.darkened) continue;
                setSpritePositionsOnANode(node.Name);
            }
        }

        /// <summary>
        /// This function hides the player controls when a different node or agent is selected
        /// </summary>
        private void hideControls()
        {
            foreach (UserControl uc in stpPlayerControls.Children)
                uc.Visibility = Visibility.Collapsed;
            ctrlResources.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// This function calculates the profits and deficits of the player
        /// </summary>
        void calculateResourceGain()
        {
            ctrlResources.GoldGain = ctrlResources.FoodGain = ctrlResources.StoneGain = ctrlResources.Morale = 0;
            ActivePlayer.Morale = 0;

            foreach (Node node in ownedNodes)
            {
                if (node.NodeType == enmNodeType.Church)
                    ActivePlayer.Morale += Constants.ChurchMoraleGain;
                if (node.NodeType == enmNodeType.City)
                    ctrlResources.GoldGain += Constants.CityGoldGain;
                if (node.NodeType == enmNodeType.Mine)
                    ctrlResources.StoneGain += Constants.MineStoneGain;
                if (node.NodeType == enmNodeType.Village)
                    ctrlResources.FoodGain += Constants.VillageFoodGain;
                if (node.NodeType == enmNodeType.Metropolis)
                {
                    ActivePlayer.Morale += Constants.ChurchMoraleGain;
                    ctrlResources.GoldGain += Constants.CityGoldGain;
                    ctrlResources.StoneGain += Constants.MineStoneGain;
                    ctrlResources.FoodGain += Constants.VillageFoodGain;
                }  //if
            }  //foreach

            foreach (Army army in ActivePlayer.Armies)
            {
                ctrlResources.GoldGain -= (Constants.GoldUpkeepBowmen * army.Archers);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryHeavy * army.HeavyCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryLight * army.LightCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryHeavy * army.HeavyInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryLight * army.LightInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepMusketeer * army.Musketeers);

                ctrlResources.FoodGain -= (Constants.FoodUpkeepBowmen * army.Archers);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryHeavy * army.HeavyCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryLight * army.LightCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryHeavy * army.HeavyInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryLight * army.LightInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepMusketeer * army.Musketeers);
            }

            foreach(Commander c in ActivePlayer.Commanders)
            {
                ctrlResources.GoldGain -= (Constants.GoldUpkeepBowmen * c.army.Archers);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryHeavy * c.army.HeavyCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryLight * c.army.LightCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryHeavy * c.army.HeavyInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryLight * c.army.LightInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepMusketeer * c.army.Musketeers);

                ctrlResources.FoodGain -= (Constants.FoodUpkeepBowmen * c.army.Archers);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryHeavy * c.army.HeavyCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryLight * c.army.LightCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryHeavy * c.army.HeavyInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryLight * c.army.LightInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepMusketeer * c.army.Musketeers);
            }

            ctrlResources.GoldGain -= (ActivePlayer.Commanders.Count * Constants.commanderGoldUpkeep);
            ctrlResources.GoldGain -= (ActivePlayer.Stewards.Count * Constants.stewardGoldUpkeep);
            ctrlResources.GoldGain -= (ActivePlayer.Assassins.Count * Constants.assassinGoldUpkeep);
            ctrlResources.GoldGain -= (ActivePlayer.Scouts.Count * Constants.scoutGoldUpkeep);

            ctrlResources.FoodGain -= (ActivePlayer.Commanders.Count * Constants.commanderFoodUpkeep);
            ctrlResources.FoodGain -= (ActivePlayer.Stewards.Count * Constants.stewardFoodUpkeep);
            ctrlResources.FoodGain -= (ActivePlayer.Assassins.Count * Constants.assassinFoodUpkeep);
            ctrlResources.FoodGain -= (ActivePlayer.Scouts.Count * Constants.scoutFoodUpkeep);

            ctrlResources.GoldGain += goldGainModifier;
            ctrlResources.FoodGain += foodGainModifier;
            ctrlResources.StoneGain += stoneGainModifier;
            ActivePlayer.Morale += moraleEffectModifier;
        }

        void showResources()
        {
            calculateResourceGain();

            ctrlResources.Gold = ActivePlayer.Gold;
            ctrlResources.Stone = ActivePlayer.Stone;
            ctrlResources.Food = ActivePlayer.Food;
            ctrlResources.Morale = ActivePlayer.Morale;
        }

        private void setPlayer()
        {
            turn = app.turn;
            ActivePlayer = app.getPlayer();
        }

        /*This function define the "Fog of War", aka the nodes which the player can see and the nodes which he cannot see.
         That is, enemy armies will only be shown in light-colored nodes*/
        private void setPlayerFOW()
        {
            if (FoWSetUp)
            {
                foreach (Node node in map.nodeList)
                    node.lighten();
            }

            FoWSetUp = true;
            List<string> visitedNodes = new List<string>();
            List<string> scoutingNodes = new List<string>();
            foreach (Commander a in ActivePlayer.Commanders)
                if (!visitedNodes.Contains(a.location)) visitedNodes.Add(a.location);
            foreach (Steward a in ActivePlayer.Stewards)
                if (!visitedNodes.Contains(a.location)) visitedNodes.Add(a.location);
            foreach (Assassin a in ActivePlayer.Assassins)
                if (!visitedNodes.Contains(a.location)) visitedNodes.Add(a.location);
            foreach (Scout a in ActivePlayer.Scouts)
            {
                if (!visitedNodes.Contains(a.location)) visitedNodes.Add(a.location);
                if (!scoutingNodes.Contains(a.location)) scoutingNodes.Add(a.location);
            }

            foreach (Node node in map.nodeList)
            {
                string nodeToRmove = "";
                foreach(string sn in scoutingNodes)
                {
                    if(node.isConnectedto(sn))
                    {
                        if (!visitedNodes.Contains(node.Name))
                            visitedNodes.Add(node.Name);
                        nodeToRmove = sn;
                        break;
                    }
                }
                if (string.Equals(nodeToRmove, ""))
                    scoutingNodes.Remove(nodeToRmove);
            }

            foreach (Node node in map.nodeList)
            {
                if (node.Owner == ActivePlayer.color) continue;

                bool z = true;
                if (visitedNodes.Contains(node.Name)) z = false;
                if (z)
                {
                    foreach (string vn in visitedNodes)
                        if (node.isConnectedto(vn)) z = false;
                }

                if (z)
                {
                    foreach (Node ownedNode in ownedNodes)
                        if (node.isConnectedto(ownedNode.Name)) z = false;
                }

                if (z)
                {
                    node.darken();
                }
            }
        }

        /// <summary>
        /// This function sets the nodes onto the map and connects them to functions that have to be called when a player clicks on them
        /// </summary>
        void setTheMap()
        {
            map = app.MapProperty;

            foreach (Node node in map.nodeList)
            {
                node.nodeControl.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => NodeSelect(sender, e, node.nodeControl.Name));
                node.nodeControl.MouseRightButtonUp += new MouseButtonEventHandler((sender, e) => moveToNode(sender, e, node.nodeControl.Name));
                cnvMapa.Children.Add(node.nodeControl);
                if (node.Owner == ActivePlayer.color) ownedNodes.Add(node);
            }

            connectNodes();
        }

        /// <summary>
        /// Creates a movement path for the agent which the player seeks to move (doesn't actually move the agent, that is done later)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="p">Destination node</param>
        private void moveToNode(object sender, MouseButtonEventArgs e, string p)
        {
            bool wasSelected = false;
            Agent selectedAgent = new AgentPlaceholder("0", 0, 0, "", enmPlayers.None);
            foreach (Commander commander in ActivePlayer.Commanders)
            {
                if (commander.Selected)
                {
                    wasSelected = true;
                    selectedAgent = commander;
                    break;
                }
            }

            if (!wasSelected)
            {
                foreach (Steward steward in ActivePlayer.Stewards)
                {
                    if (steward.Works()) continue;
                    if (steward.Selected)
                    {
                        wasSelected = true;
                        selectedAgent = steward;
                        break;
                    }
                }
            }

            if (!wasSelected)
            {
                foreach (Scout scout in ActivePlayer.Scouts)
                {
                    if (scout.Selected)
                    {
                        wasSelected = true;
                        selectedAgent = scout;
                        break;
                    }
                }
            }

            if (!wasSelected)
            {
                foreach (Assassin assassin in ActivePlayer.Assassins)
                {
                    if (assassin.Selected)
                    {
                        wasSelected = true;
                        selectedAgent = assassin;
                        break;
                    }
                }
            }

            if (!wasSelected) return;

            selectedAgent.movementRoute.Clear();
            selectedAgent.moving = false;
            selectedAgent.sprite.Opacity = 1;

            Node startNode = new Node();
            Node endNode = new Node();
            foreach (Node node in map.nodeList)
            {
                if (string.Equals(node.Name, selectedAgent.location))
                    startNode = node;
                if (string.Equals(node.Name, p))
                    endNode = node;
            }
            if (string.Equals(startNode.Name, endNode.Name)) return;
            selectedAgent.movementRoute = Dijkstra.FindPath(map.nodeList, startNode, endNode, ActivePlayer);
            selectedAgent.moving = true;
            selectedAgent.sprite.Opacity = 0.5;
            routeConnect(selectedAgent);
        }

        /// <summary>
        /// Graphically shows the movement route for the agent, in two different colors depending on when he will arive there
        /// </summary>
        /// <param name="agent"></param>
        void routeConnect(Agent agent)
        {
            routeUnselect();
            int movementCounter = agent.getMovementValue();
            bool farAway = false;
            Node formerNode = new Node();
            foreach (Node node in agent.movementRoute)
            {
                node.routeSelect(farAway);
                if (movementCounter != agent.getMovementValue())
                {
                    foreach (Edge edge in edgeList)
                    {
                        if (edge.nodes.Contains(node.Name) && edge.nodes.Contains(formerNode.Name))
                        {
                            if (farAway) edge.connection.Stroke = new SolidColorBrush(Colors.Yellow);
                            else edge.connection.Stroke = new SolidColorBrush(Colors.Orange);
                            break;
                        }
                    }
                }
                movementCounter--;
                if (movementCounter < 0) farAway = true;
                formerNode = node;
            }
            spriteRefresh();
        }

        /// <summary>
        /// Removes the movement route after the agent is unselected
        /// </summary>
        private void routeUnselect()
        {
            foreach (Node node in map.nodeList)
                node.routeUnselect();

            foreach (Edge edge in edgeList)
                cnvMapa.Children.Remove(edge.connection);

            edgeList.Clear();
            connectNodes();
        }

        /// <summary>
        /// Selects the node and shows necessary controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="p">Name of the selected node</param>
        private void NodeSelect(object sender, MouseButtonEventArgs e, string p)
        {
            unselect();

            foreach (Node node in map.nodeList)
            {
                if (string.Equals(node.Name, p))
                {
                    node.Selected = true;
                    showNodeControls(node);
                    btnEndTurn.Content = node.Name;
                    break;
                }  //if
            }  //foreach
        } //NodeSelect()

        /*Each node has its own set of controls that can be used on it. The following function checks which are those controls
         and makes them visible on the side-bar*/
        private void showNodeControls(Node selectedNode)
        {
            NodeInfo.Visibility = Visibility.Visible;
            NodeInfo.showNodeInfo(selectedNode, selectedNode.darkened);
            switch (selectedNode.NodeType)
            {
                case enmNodeType.Mine:
                    NodeInfo.showNodeGain("Stone", Constants.MineStoneGain);
                    break;
                case enmNodeType.City:
                    NodeInfo.showNodeGain("Gold", Constants.CityGoldGain);
                    break;
                case enmNodeType.Church:
                    NodeInfo.showNodeGain("Morale", Constants.ChurchMoraleGain);
                    break;
                case enmNodeType.Village:
                    NodeInfo.showNodeGain("Food", Constants.VillageFoodGain);
                    break;
                default:
                    NodeInfo.hideNodeGain();
                    break;
            }

            /*Can a commander be hired?*/
            if (selectedNode.Owner == ActivePlayer.color &&
                (selectedNode.NodeType == enmNodeType.Castle
                || selectedNode.NodeType == enmNodeType.Church
                || selectedNode.NodeType == enmNodeType.Metropolis))
            {
                HireCommander.Visibility = Visibility.Visible;
                if (ActivePlayer.Gold < Constants.commanderGoldCost
                    || ctrlResources.GoldGain < Constants.commanderGoldUpkeep)
                {
                    HireCommander.Enabled = false;
                }
                else
                {
                    HireCommander.Enabled = true;
                }
            }


            /*Can a steward be hired?*/
            if (selectedNode.Owner == ActivePlayer.color &&
                (selectedNode.NodeType == enmNodeType.City
                || selectedNode.NodeType == enmNodeType.Church
                || selectedNode.NodeType == enmNodeType.Metropolis))
            {
                HireSteward.Visibility = Visibility.Visible;
                if (ActivePlayer.Gold < Constants.stewardGoldCost
                    || ctrlResources.GoldGain < Constants.stewardGoldUpkeep)
                {
                    HireSteward.Enabled = false;
                }
                else
                {
                    HireSteward.Enabled = true;
                }
            }

            /*Can an assassin be hired?*/
            if (selectedNode.Owner == ActivePlayer.color &&
                (selectedNode.NodeType == enmNodeType.City
                || selectedNode.NodeType == enmNodeType.Metropolis))
            {
                HireAssassin.Visibility = Visibility.Visible;
                if (ActivePlayer.Gold < Constants.assassinGoldCost
                    || ctrlResources.GoldGain < Constants.assassinGoldUpkeep)
                {
                    HireAssassin.Enabled = false;
                }
                else
                {
                    HireAssassin.Enabled = true;
                }
            }

            /*Can a scout be hired?*/
            if (selectedNode.Owner == ActivePlayer.color &&
                (selectedNode.NodeType == enmNodeType.Castle
                || selectedNode.NodeType == enmNodeType.City
                || selectedNode.NodeType == enmNodeType.Metropolis))
            {
                HireScout.Visibility = Visibility.Visible;
                if (ActivePlayer.Gold < Constants.scoutGoldCost
                    || ctrlResources.GoldGain < Constants.scoutGoldUpkeep
                    || ctrlResources.FoodGain < Constants.scoutFoodUpkeep)
                {
                    HireScout.Enabled = false;
                }
                else
                {
                    HireScout.Enabled = true;
                }
            }

            if (selectedNode.Owner == ActivePlayer.color &&
                (selectedNode.NodeType == enmNodeType.Metropolis
                || selectedNode.NodeType == enmNodeType.Castle
                || selectedNode.NodeType == enmNodeType.City))
            {
                HireArmy.Visibility = Visibility.Visible;
            }

            /*Can a Steward improve this holding?*/
            if (selectedNode.Owner == ActivePlayer.color &&
                (selectedNode.NodeType == enmNodeType.Castle
                || selectedNode.NodeType == enmNodeType.City
                || selectedNode.NodeType == enmNodeType.Church
                || selectedNode.NodeType == enmNodeType.Metropolis
                || selectedNode.NodeType == enmNodeType.Village))
            {
                HoldingUpgrade.Visibility = Visibility.Collapsed;
                foreach (Steward steward in ActivePlayer.Stewards)
                {
                    if (steward.location == selectedNode.Name)
                    {
                        if (selectedNode.beingUpgraded)
                        {
                            if (steward.working == 0) continue;  //some other steward is upgrading this Holding so ignore this one
                            HoldingUpgrade.showProgress(steward);
                        }
                        if (HoldingUpgrade.checkUpgrades(selectedNode))
                        {
                            HoldingUpgrade.Visibility = Visibility.Visible;
                            HoldingUpgrade.show(selectedNode, steward, ActivePlayer);
                            break;
                        }
                    }
                }
            }

            ShowModularControls(selectedNode);
        }

        private void ShowModularControls(Node selectedNode)
        {
            foreach (ControlInterface control in modules)
            {
                control.Map = map;
                control.Players = app.players;
                control.ActivePlayer = ActivePlayer;
                control.SelectedNode = selectedNode;
                control.ListOfArguments = listOfArguments;
                if (control.CheckAvailiability())
                {
                    if (control.instanced)
                    {
                        control.userControl.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        UserControl newControl = control.userControl;
                        if (newControl.Width == 80 && newControl.Height > 25 && newControl.Height < 100)
                        {
                            stpPlayerControls.Children.Add(control.userControl);
                            control.userControl.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => ControlActivated(control));
                            control.instanced = true;
                        }
                    }
                }
            }
        }

        private void ControlActivated(ControlInterface control)
        {
            moraleEffectModifier += control.MoraleGainModifier;
            goldGainModifier += control.GoldGainModifier;
            foodGainModifier += control.FoodGainModifier;
            stoneGainModifier += control.StoneGainModifier;

            control.ResetState();
            showResources();
            showAgents();
            setPlayerFOW();
            routeUnselect();
            showAgents();
            showControls();
        }

        private void showStewardControls(Steward selectedSteward)
        {
            //undefined for now, but like with the nodes each agent will have its own set of controls 
            //that will be shown in the side bar
        }

        /// <summary>
        /// Shows user control for the selected node or agent
        /// </summary>
        private void showControls()
        {
            hideControls();

            foreach (Node node in map.nodeList)
            {
                if (!node.Selected) continue;
                showNodeControls(node);
                return;
            }

            foreach(Commander commander in ActivePlayer.Commanders)
            {
                if (!commander.Selected) continue;
                ArmyInfo.Visibility = Visibility.Visible;
                return;
            }

            foreach (Steward steward in ActivePlayer.Stewards)
            {
                if (!steward.Selected) continue;
                showStewardControls(steward);
                return;
            }
        }

        private void unselect()
        {
            if(frmDialogue.Visibility == Visibility.Visible)
            {
                showResources();
            }

            frmDialogue.Visibility = Visibility.Collapsed;
            frmDialogue.Navigate(new Uri("/Pages/ErrorPage.xaml", UriKind.Relative));
            routeUnselect();
            foreach (Node node in map.nodeList)
                node.Selected = false;
            foreach (Commander commander in ActivePlayer.Commanders)
                commander.Unselect();
            foreach (Steward steward in ActivePlayer.Stewards)
                steward.Unselect();
            foreach (Scout scout in ActivePlayer.Scouts)
                scout.Unselect();
            foreach (Assassin assassin in ActivePlayer.Assassins)
                assassin.Unselect();
            foreach (Agent agent in visibleEnemies)
                agent.Unselect();
            hideControls();
            spriteRefresh();
        }

        private void spriteRefresh()
        {
            foreach (Agent agent in ActivePlayer.Commanders)
            {
                cnvMapa.Children.Remove(agent.sprite);
                cnvMapa.Children.Add(agent.sprite);
            }
            foreach (Agent agent in ActivePlayer.Stewards)
            {
                cnvMapa.Children.Remove(agent.sprite);
                cnvMapa.Children.Add(agent.sprite);
            }
            foreach (Agent agent in ActivePlayer.Scouts)
            {
                cnvMapa.Children.Remove(agent.sprite);
                cnvMapa.Children.Add(agent.sprite);
            }
            foreach (Agent agent in ActivePlayer.Assassins)
            {
                cnvMapa.Children.Remove(agent.sprite);
                cnvMapa.Children.Add(agent.sprite);
            }
            foreach(Agent agent in visibleEnemies)
            {
                cnvMapa.Children.Remove(agent.sprite);
                cnvMapa.Children.Add(agent.sprite);
            }
        }

        /// <summary>
        /// Visually connects the nodes
        /// </summary>
        void connectNodes()
        {
            foreach (Node node1 in map.nodeList)
            {
                node1.arg1 = 1;
                foreach (Node node2 in map.nodeList)
                {
                    if (string.Equals(node1.Name, node2.Name) || node2.arg1 == 1) continue;
                    if (node1.isConnectedto(node2.Name))
                    {
                        double x1, x2, y1, y2;
                        x1 = x2 = y1 = y2 = 0;

                        GeneralTransform getPosition1 = node1.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset1 = getPosition1.Transform(new Point(0, 0));
                        x1 = offset1.X;
                        y1 = offset1.Y;

                        GeneralTransform getPosition2 = node2.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset2 = getPosition2.Transform(new Point(0, 0));
                        x2 = offset2.X;
                        y2 = offset2.Y;

                        Edge edge = new Edge(x1, x2, y1, y2, node1.Name, node2.Name);
                        if (node1.darkened && node2.darkened)
                            edge.connection.Stroke = new SolidColorBrush(Colors.LightGray);
                        cnvMapa.Children.Add(edge.connection);
                        edgeList.Add(edge);

                        cnvMapa.Children.Remove(node1.nodeControl);
                        cnvMapa.Children.Add(node1.nodeControl);
                        cnvMapa.Children.Remove(node2.nodeControl);
                        cnvMapa.Children.Add(node2.nodeControl);
                    }
                } //foreach
            } //foreach

            foreach (Node node in map.nodeList)
                node.arg1 = 0;
        }

        private void btnNextMove_Click(object sender, RoutedEventArgs e)
        {
            //not yet done, this button will show the player which moves he did not yet make (armies that have yet to be moved,
            //holdings that can be upgrades, etc.)
        }

        /// <summary>
        /// Runs when the player clicks on anything on the canvas (besides the controls). It unselects everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cnvMapa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            unselect();
        }

        /*The following fre functions handle the addition of new Agents into the game*/
        private void btnHireScout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!HireScout.Enabled) return;
            else addScout();
        }

        private void addScout()
        {
            string nodeName = "";
            double x = 0, y = 0;
            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    nodeName = node.Name;
                    GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = getPosition.Transform(new Point(0, 0));
                    x = offset.X;
                    y = offset.Y;
                    break;
                }
            }

            string ID = ActivePlayer.color.ToString() + "Scout" + ActivePlayer.AgentCounter.ToString();
            Scout newScout = new Scout(ID, Constants.scoutGoldUpkeep, Constants.scoutFoodUpkeep, nodeName, ActivePlayer.color);
            newScout.visible = true;
            newScout.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => ScoutSelect(sender, e, newScout.ID));
            newScout.sprite.SetValue(Canvas.LeftProperty, x - 80);
            newScout.sprite.SetValue(Canvas.TopProperty, y + 10);
            ActivePlayer.addScout(newScout);
            cnvMapa.Children.Add(newScout.sprite);
            setSpritePositionsOnANode(nodeName);

            ActivePlayer.Gold -= Constants.scoutGoldCost;
            showResources();
            showControls();
        }

        private void ScoutSelect(object sender, MouseButtonEventArgs e, string ID)
        {
            foreach (Scout scout in ActivePlayer.Scouts)
            {
                if (string.Equals(scout.ID, ID))
                {
                    scout.Select();
                    if (scout.moving) routeConnect(scout);
                    break;
                }
            }
        }

        private void HireAssassin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!HireAssassin.Enabled) return;
            else addAssassin();
        }

        private void addAssassin()
        {
            string nodeName = "";
            double x = 0, y = 0;
            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    nodeName = node.Name;
                    GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = getPosition.Transform(new Point(0, 0));
                    x = offset.X;
                    y = offset.Y;
                    break;
                }
            }

            string ID = ActivePlayer.color.ToString() + "Assassin" + ActivePlayer.AgentCounter.ToString();
            Assassin newAssassin = new Assassin(ID, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, nodeName, ActivePlayer.color);
            newAssassin.visible = true;
            newAssassin.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => AssassinSelect(sender, e, newAssassin.ID));
            newAssassin.sprite.SetValue(Canvas.LeftProperty, x - 80);
            newAssassin.sprite.SetValue(Canvas.TopProperty, y + 10);
            ActivePlayer.addAssassin(newAssassin);
            cnvMapa.Children.Add(newAssassin.sprite);
            setSpritePositionsOnANode(nodeName);

            ActivePlayer.Gold -= Constants.assassinGoldCost;
            showResources();
            showControls();
        }

        private void AssassinSelect(object sender, MouseButtonEventArgs e, string ID)
        {
            foreach (Assassin assassin in ActivePlayer.Assassins)
            {
                if (string.Equals(assassin.ID, ID))
                {
                    assassin.Select();
                    if (assassin.moving) routeConnect(assassin);
                    break;
                }
            }
        }

        private void HireSteward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!HireSteward.Enabled) return;
            else addSteward();
        }


        private void addSteward()
        {
            string nodeName = "";
            double x = 0, y = 0;
            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    nodeName = node.Name;
                    GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = getPosition.Transform(new Point(0, 0));
                    x = offset.X;
                    y = offset.Y;
                    break;
                }
            }

            string ID = ActivePlayer.color.ToString() + "Steward" + ActivePlayer.AgentCounter.ToString();
            Steward newSteward = new Steward(ID, Constants.stewardGoldUpkeep, Constants.stewardFoodUpkeep, nodeName, ActivePlayer.color);
            newSteward.visible = true;
            newSteward.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => StewardSelect(sender, e, newSteward.ID));
            newSteward.sprite.SetValue(Canvas.LeftProperty, x - 80);
            newSteward.sprite.SetValue(Canvas.TopProperty, y + 10);
            ActivePlayer.addSteward(newSteward);
            cnvMapa.Children.Add(newSteward.sprite);
            setSpritePositionsOnANode(nodeName);

            ActivePlayer.Gold -= Constants.stewardGoldCost;
            showResources();
            showControls();
        }

        private void StewardSelect(object sender, MouseButtonEventArgs e, string ID)
        {
            foreach (Steward steward in ActivePlayer.Stewards)
            {
                if (string.Equals(steward.ID, ID))
                {
                    steward.Select();
                    if (steward.moving) routeConnect(steward);
                    showStewardControls(steward);
                    break;
                }
            }
        }

        private void HireCommander_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!HireCommander.Enabled) return;
            else addCommander();
        }

        private void addCommander()
        {
            string nodeName = "";
            double x = 0, y = 0;
            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    nodeName = node.Name;
                    GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                    Point offset = getPosition.Transform(new Point(0, 0));
                    x = offset.X;
                    y = offset.Y;
                    break;
                }
            }

            string ID = ActivePlayer.color.ToString() + "Commander" + ActivePlayer.AgentCounter.ToString();
            Commander newCommander = new Commander(ID, Constants.commanderGoldUpkeep, Constants.commanderFoodUpkeep, nodeName, ActivePlayer.color);
            newCommander.visible = true;
            newCommander.sprite.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => CommanderSelect(sender, e, newCommander.ID));
            newCommander.sprite.SetValue(Canvas.LeftProperty, x - 70);
            newCommander.sprite.SetValue(Canvas.TopProperty, y + 10);
            ActivePlayer.addCommander(newCommander);
            cnvMapa.Children.Add(newCommander.sprite);
            setSpritePositionsOnANode(nodeName);

            ActivePlayer.Gold -= Constants.commanderGoldCost;
            showControls();
            showResources();
        }

        private void CommanderSelect(object sender, MouseButtonEventArgs e, string ID)
        {
            foreach (Commander commander in ActivePlayer.Commanders)
            {
                if (string.Equals(commander.ID, ID))
                {
                    commander.Select();
                    ArmyInfo.showArmyInfo(commander);
                    ArmyInfo.Visibility = Visibility.Visible;
                    if (commander.moving) routeConnect(commander);
                    break;
                }
            }
        }

        /// <summary>
        /// Sets the position of all sprites on the node. Necessary when there are more sagents located on one
        /// </summary>
        /// <param name="nodeName"></param>
        private void setSpritePositionsOnANode(string nodeName)
        {
            Node node = new Node();
            foreach (Node n in map.nodeList)
            {
                if (string.Equals(nodeName, n.Name))
                {
                    node = n;
                    break;
                }
            }

            int counter = 0;
            List<Agent> agents = new List<Agent>();

            foreach (Player player in app.players)
            {
                foreach (Commander commander in player.Commanders)
                {
                    if (commander.visible && string.Equals(commander.location, node.Name))
                    {
                        counter++;
                        agents.Add(commander);
                        cnvMapa.Children.Remove(commander.sprite);
                    }
                }

                foreach (Steward steward in player.Stewards)
                {
                    if (steward.visible && string.Equals(steward.location, node.Name))
                    {
                        counter++;
                        agents.Add(steward);
                        cnvMapa.Children.Remove(steward.sprite);
                    }
                }

                foreach (Scout scout in player.Scouts)
                {
                    if (scout.visible && string.Equals(scout.location, node.Name))
                    {
                        counter++;
                        agents.Add(scout);
                        cnvMapa.Children.Remove(scout.sprite);
                    }
                }

                foreach (Assassin assassin in player.Assassins)
                {
                    if (assassin.visible && string.Equals(assassin.location, node.Name))
                    {
                        counter++;
                        agents.Add(assassin);
                        cnvMapa.Children.Remove(assassin.sprite);
                    }
                }
            }

            if (counter == 0) return;

            double x;
            GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
            Point offset = getPosition.Transform(new Point(0, 0));
            x = offset.X;

            double distances = 15;
            double xPos = 90;

            foreach (Agent agent in agents)
            {
                agent.sprite.SetValue(Canvas.LeftProperty, x - xPos);
                cnvMapa.Children.Add(agent.sprite);
                xPos -= distances;
            }
        }
        //70 - middle; 90 - left; 50 - right

        /*The following functions handle the upgrading of holdings*/
        private void HoldingUpgrade_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (HoldingUpgrade.Enabled)
                upgradeHolding();
        }

        private void upgradeHolding()
        {
            Node selectedNode = new Node();
            foreach (Node node in ownedNodes)
            {
                if (node.Selected)
                {
                    selectedNode = node;
                    break;
                }
            }

            foreach (Steward steward in ActivePlayer.Stewards)
            {
                if (steward.working == 0 && steward.location == selectedNode.Name)
                {
                    ActivePlayer.Gold -= HoldingUpgrade.getGoldCosts(selectedNode);
                    ActivePlayer.Stone -= HoldingUpgrade.getStoneCosts(selectedNode);
                    steward.working = HoldingUpgrade.getTime(selectedNode);
                    selectedNode.beingUpgraded = true;
                    steward.sprite.Opacity = Constants.darkenedSpriteOpacity;
                }
            }
            showResources();
            showControls();
        }

        private void cnvMapa_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            bool wasSelected = false;
            Agent selectedAgent = new AgentPlaceholder("0", 0, 0, "", enmPlayers.None);
            foreach (Commander commander in ActivePlayer.Commanders)
            {
                if (commander.Selected)
                {
                    wasSelected = true;
                    selectedAgent = commander;
                    break;
                }
            }

            if (!wasSelected)
            {
                foreach (Steward steward in ActivePlayer.Stewards)
                {
                    if (steward.Works()) continue;
                    if (steward.Selected)
                    {
                        wasSelected = true;
                        selectedAgent = steward;
                        break;
                    }
                }
            }

            if (!wasSelected)
            {
                foreach (Scout scout in ActivePlayer.Scouts)
                {
                    if (scout.Selected)
                    {
                        wasSelected = true;
                        selectedAgent = scout;
                        break;
                    }
                }
            }

            if (!wasSelected)
            {
                foreach (Assassin assassin in ActivePlayer.Assassins)
                {
                    if (assassin.Selected)
                    {
                        wasSelected = true;
                        selectedAgent = assassin;
                        break;
                    }
                }
            }

            if (!wasSelected) return;

            selectedAgent.movementRoute.Clear();
            selectedAgent.moving = false;
            selectedAgent.sprite.Opacity = 1;
            routeUnselect();
            spriteRefresh();
        }

        private void HireArmy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            frmDialogue.Visibility = Visibility.Collapsed;
            frmDialogue.Navigate(new Uri("/Pages/MainMenuAlpha.xaml", UriKind.Relative));
            Player rctrPlayer = app.Recruitment_player;
            if(rctrPlayer != null)
            {
                ActivePlayer = rctrPlayer;
            }

            app.Recruitment_player = ActivePlayer;
            foreach(Node node in ownedNodes)
            {
                if(node.Selected)
                {
                    app.Recruitment_node = node.Name;
                    break;
                }
            }

            hideControls();
            app.ActivePlayerNodes = ownedNodes;
            frmDialogue.Visibility = Visibility.Visible;
            frmDialogue.Navigate(new Uri("/Pages/RecruitmentInterface.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Ends the turn. First, it adds the resource gains and then calls the function which saves the game data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEndTurn_Click(object sender, RoutedEventArgs e)
        {
            ActivePlayer.Gold += ctrlResources.GoldGain;
            ActivePlayer.Food += ctrlResources.FoodGain;
            ActivePlayer.Stone += ctrlResources.StoneGain;
            TurnEnder TE = new TurnEnder(ActivePlayer, turn, listOfArguments);
            bool commited = TE.Commit();

            if (commited)
            {
                MessageBox.Show("Everything was committed successfully!");
                app.clearData();
                NavigationService.Navigate(new Uri("/Pages/MainMenuAlpha.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("An error occurred!");
                ActivePlayer.Gold -= ctrlResources.GoldGain;
                ActivePlayer.Food -= ctrlResources.FoodGain;
                ActivePlayer.Stone -= ctrlResources.StoneGain;
            }
        }
    }
}