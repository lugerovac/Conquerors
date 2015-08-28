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
using System.IO;

namespace Conquerors.Pages
{
    public partial class MainMenuAlpha : Page
    {
        /*
         * New save and Load elements to implement:
         * - Commanders battling states
         */

        enum enmMergeState
        {
            Unloaded = 1,
            MapLoaded = 2,
            RedPlayerLoaded = 3,
            BluePlayerLoaded = 4,
            GreenPlayerLoaded = 5,
            PurplePlayerLoaded = 6
        }
        enmMergeState mergeState = enmMergeState.Unloaded;

        Map map;

        public MainMenuAlpha()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnEditor_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Editor.xaml", UriKind.Relative));
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            resetMerging();
            loadMap(enmPlayers.Red);
        }

        /*The following function loads up a file and stores the necessary data intp app.xaml module, from which the next
         page (Game.xaml) will read the data*/
        private void loadMap(enmPlayers activePlayer)
        {
            try
            {
                App app = (App)Application.Current;
                map = new Map();
                app.initializePlayers();
                app.ActivePlayer = activePlayer;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog().Value)
                {
                    map = new Map();
                    using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                    {
                        app.turn = Convert.ToInt32(sr.ReadLine());
                        map.NodeCount = Convert.ToInt32(sr.ReadLine());

                        while (true)
                        {
                            string name = sr.ReadLine();
                            if (string.Equals(name, "/map")) break;
                            int nodeType = Convert.ToInt32(sr.ReadLine());
                            enmPlayers owner = (enmPlayers)Convert.ToInt32(sr.ReadLine());
                            int defenseLevel = Convert.ToInt32(sr.ReadLine());
                            double x = Convert.ToDouble(sr.ReadLine()) - 80;
                            double y = Convert.ToDouble(sr.ReadLine()) - 17;

                            List<string> listOfConnections = new List<string>();
                            bool i = true;
                            while (i)
                            {
                                string reader = sr.ReadLine();
                                if (string.Equals(reader, "/connections")) //with this we will know until when we have to read the connections
                                    i = false;
                                else listOfConnections.Add(reader);
                            }  //while (i)

                            map.AddNodeToList(name, nodeType, owner, defenseLevel, 0, x, y, listOfConnections);
                        } //while (true)

                        while(true)
                        {
                            string color = sr.ReadLine();
                            if (string.Equals(color, "/players")) break;

                            enmPlayers playerColor = (enmPlayers)Convert.ToInt32(color);
                            Player player = new Player(playerColor);
                            player.Gold = Convert.ToInt32(sr.ReadLine());
                            player.Food = Convert.ToInt32(sr.ReadLine());
                            player.Stone = Convert.ToInt32(sr.ReadLine());
                            player.AgentCounter = Convert.ToInt32(sr.ReadLine());

                            //read assassins
                            while(true)
                            {
                                string id = sr.ReadLine();
                                if (string.Equals(id, "/assassins")) break;
                                string loc = sr.ReadLine();
                                Assassin a = new Assassin(id, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);
                                
                                string moving = sr.ReadLine();
                                if (string.Equals(moving, "True")) a.moving = true;
                                else a.moving = false;
                                while (a.moving)
                                {
                                    string nodeName = sr.ReadLine();
                                    if (string.Equals(nodeName, "/movement")) break;
                                    foreach (Node node in app.MapProperty.nodeList)
                                    {
                                        if (string.Equals(nodeName, node.Name))
                                        {
                                            a.movementRoute.Add(node);
                                            break;
                                        }  //if(string.Equals(nodeName, node.Name))
                                    }  //foreach(Node node in app.MapProperty.nodeList)
                                }  //while(a.moving)
                                player.Assassins.Add(a);
                            } //while(true)

                            //Read Scouts
                            while (true)
                            {
                                string id = sr.ReadLine();
                                if (string.Equals(id, "/scouts")) break;
                                string loc = sr.ReadLine();
                                Scout a = new Scout(id, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);

                                string moving = sr.ReadLine();
                                if (string.Equals(moving, "True")) a.moving = true;
                                else a.moving = false;

                                while (a.moving)
                                {
                                    string nodeName = sr.ReadLine();
                                    if (string.Equals(nodeName, "/movement")) break;
                                    foreach (Node node in app.MapProperty.nodeList)
                                    {
                                        if (string.Equals(nodeName, node.Name))
                                        {
                                            a.movementRoute.Add(node);
                                            break;
                                        }  //if(string.Equals(nodeName, node.Name))
                                    }  //foreach(Node node in app.MapProperty.nodeList)
                                }  //while(a.moving)
                                player.Scouts.Add(a);
                            }  //while(true)

                            //Read Stewards
                            while (true)
                            {
                                string id = sr.ReadLine();
                                if (string.Equals(id, "/stewards")) break;
                                string loc = sr.ReadLine();
                                Steward a = new Steward(id, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);
                                a.working = Convert.ToInt32(sr.ReadLine());

                                string moving = sr.ReadLine();
                                if (string.Equals(moving, "True")) a.moving = true;
                                else a.moving = false;

                                while (a.moving)
                                {
                                    string nodeName = sr.ReadLine();
                                    if (string.Equals(nodeName, "/movement")) break;
                                    foreach (Node node in app.MapProperty.nodeList)
                                    {
                                        if (string.Equals(nodeName, node.Name))
                                        {
                                            a.movementRoute.Add(node);
                                            break;
                                        }  //if(string.Equals(nodeName, node.Name))
                                    }  //foreach(Node node in app.MapProperty.nodeList)
                                }  //while(a.moving)
                                player.Stewards.Add(a);
                            }  //while(true)

                            //Read Commanders
                            while (true)
                            {
                                string id = sr.ReadLine();
                                if (string.Equals(id, "/commanders")) break;
                                string loc = sr.ReadLine();
                                Commander a = new Commander(id, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);

                                string moving = sr.ReadLine();
                                if (string.Equals(moving, "True")) a.moving = true;
                                else a.moving = false;
                                while (a.moving)
                                {
                                    string nodeName = sr.ReadLine();
                                    if (string.Equals(nodeName, "/movement")) break;
                                    foreach (Node node in map.nodeList)
                                    {
                                        if (string.Equals(nodeName, node.Name))
                                        {
                                            a.movementRoute.Add(node);
                                            break;
                                        }  //if(string.Equals(nodeName, node.Name))
                                    }  //foreach(Node node in app.MapProperty.nodeList)
                                }  //while(a.moving)

                                string lacksArmy = sr.ReadLine();
                                if (string.Equals(lacksArmy, "False"))
                                {
                                    a.army.LightInfantry = Convert.ToInt32(sr.ReadLine());
                                    a.army.HeavyInfantry = Convert.ToInt32(sr.ReadLine());
                                    a.army.LightCavalry = Convert.ToInt32(sr.ReadLine());
                                    a.army.HeavyCavalry = Convert.ToInt32(sr.ReadLine());
                                    a.army.Archers = Convert.ToInt32(sr.ReadLine());
                                    a.army.Musketeers = Convert.ToInt32(sr.ReadLine());
                                }
                                player.Commanders.Add(a);
                            }  //while(true)

                            //Read armies
                            while (true)
                            {
                                Army army = new Army();
                                army.location = sr.ReadLine();
                                if (string.Equals(army.location, "/armies")) break;

                                army.LightInfantry = Convert.ToInt32(sr.ReadLine());
                                army.HeavyInfantry = Convert.ToInt32(sr.ReadLine());
                                army.LightCavalry = Convert.ToInt32(sr.ReadLine());
                                army.HeavyCavalry = Convert.ToInt32(sr.ReadLine());
                                army.Archers = Convert.ToInt32(sr.ReadLine());
                                army.Musketeers = Convert.ToInt32(sr.ReadLine());

                                player.Armies.Add(army);
                            }
                            app.setPlayerData(player);
                        }
                    }  //using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                    NavigationService.Navigate(new Uri("/Pages/Game.xaml", UriKind.Relative));
                }  //if (ofd.ShowDialog().Value)
            }catch
            {
                MessageBox.Show("A problem occurred or the file is invalid!");
            }
        }  //private void loadMap()

        /*This function stored necessary data into the app.xaml module, through which the next page will be able to read the data*/
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App app = (App)Application.Current;
            app.turn++;
            app.MapProperty = map;
        }
        

        private void btnPlayBlue_Click(object sender, RoutedEventArgs e)
        {
            resetMerging();
            loadMap(enmPlayers.Blue);
        }

        private void btnPlayGreen_Click(object sender, RoutedEventArgs e)
        {
            resetMerging();
            loadMap(enmPlayers.Green);
        }

        private void btnPlayPurple_Click(object sender, RoutedEventArgs e)
        {
            resetMerging();
            loadMap(enmPlayers.Purple);
        }

        private bool saveMergeMapUpload()
        {
            try
            {
                App app = (App)Application.Current;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog().Value)
                {
                    map = new Map();
                    using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                    {
                        app.turn = Convert.ToInt32(sr.ReadLine());
                        app.turn++;
                        map.NodeCount = Convert.ToInt32(sr.ReadLine());

                        while (true)
                        {
                            string name = sr.ReadLine();
                            if (string.Equals(name, "/map")) break;
                            int nodeType = Convert.ToInt32(sr.ReadLine());
                            enmPlayers owner = (enmPlayers)Convert.ToInt32(sr.ReadLine());
                            int defenseLevel = Convert.ToInt32(sr.ReadLine());
                            double x = Convert.ToDouble(sr.ReadLine());
                            double y = Convert.ToDouble(sr.ReadLine());

                            List<string> listOfConnections = new List<string>();
                            bool i = true;
                            while (i)
                            {
                                string reader = sr.ReadLine();
                                if (string.Equals(reader, "/connections")) //with this we will know until when we have to read the connections
                                    i = false;
                                else listOfConnections.Add(reader);
                            }  //while (i)

                            map.AddNodeToList(name, nodeType, owner, defenseLevel, 0, x, y, listOfConnections);
                        } //while (true)
                    }  //using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                    app.MapProperty = map;
                    return true;
                }
                else
                {
                    return false;
                }
            }catch
            {
                MessageBox.Show("An unknown problem occurred or a wrong file was uploaded!");
                return false;
            }
        }

        private bool saveMergeSaveUpload(enmPlayers playerColor)
        {
            App app = (App)Application.Current;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog().Value)
                {
                    using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                    {
                        int turn = Convert.ToInt32(sr.ReadLine());
                        if(app.turn != turn)
                        {
                            MessageBox.Show("The save file is incompatible with earlier uploaded ones!");
                            return false;
                        }

                        enmPlayers loadedPlayerColor = (enmPlayers)Convert.ToInt32(sr.ReadLine());
                        if (loadedPlayerColor != playerColor)
                        {
                            MessageBox.Show("Wrong file was loaded!");
                            return false;
                        }

                        Player loadedPlayer = app.getPlayer(playerColor);
                        loadedPlayer.Gold = Convert.ToInt32(sr.ReadLine());
                        loadedPlayer.Food = Convert.ToInt32(sr.ReadLine());
                        loadedPlayer.Stone = Convert.ToInt32(sr.ReadLine());
                        loadedPlayer.AgentCounter = Convert.ToInt32(sr.ReadLine());

                        //Read Assassins
                        while(true)
                        {
                            string line = sr.ReadLine();
                            if (string.Equals(line, "/assassins")) break;
                            string loc = sr.ReadLine();
                            Assassin a = new Assassin(line, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);
                            
                            string moving = sr.ReadLine();
                            if (string.Equals(moving, "True")) a.moving = true;
                            else a.moving = false;
                            
                            while(a.moving)
                            {
                                string nodeName = sr.ReadLine();
                                if (string.Equals(nodeName, "/movement")) break;
                                foreach(Node node in app.MapProperty.nodeList)
                                {
                                    if(string.Equals(nodeName, node.Name))
                                    {
                                        a.movementRoute.Add(node);
                                        break;
                                    }  //if(string.Equals(nodeName, node.Name))
                                }  //foreach(Node node in app.MapProperty.nodeList)
                            }  //while(a.moving)
                            loadedPlayer.Assassins.Add(a);
                        }  //while(true)

                        //Read Scouts
                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (string.Equals(line, "/scouts")) break;
                            string loc = sr.ReadLine();
                            Scout a = new Scout(line, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);

                            string moving = sr.ReadLine();
                            if (string.Equals(moving, "True")) a.moving = true;
                            else a.moving = false;

                            while (a.moving)
                            {
                                string nodeName = sr.ReadLine();
                                if (string.Equals(nodeName, "/movement")) break;
                                foreach (Node node in app.MapProperty.nodeList)
                                {
                                    if (string.Equals(nodeName, node.Name))
                                    {
                                        a.movementRoute.Add(node);
                                        break;
                                    }  //if(string.Equals(nodeName, node.Name))
                                }  //foreach(Node node in app.MapProperty.nodeList)
                            }  //while(a.moving)

                            loadedPlayer.Scouts.Add(a);
                        }  //while(true)

                        //Read Stewards
                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (string.Equals(line, "/stewards")) break;
                            string loc = sr.ReadLine();
                            Steward a = new Steward(line, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);
                            a.working = Convert.ToInt32(sr.ReadLine());

                            string moving = sr.ReadLine();
                            if (string.Equals(moving, "True")) a.moving = true;
                            else a.moving = false;

                            while (a.moving)
                            {
                                string nodeName = sr.ReadLine();
                                if (string.Equals(nodeName, "/movement")) break;
                                foreach (Node node in app.MapProperty.nodeList)
                                {
                                    if (string.Equals(nodeName, node.Name))
                                    {
                                        a.movementRoute.Add(node);
                                        break;
                                    }  //if(string.Equals(nodeName, node.Name))
                                }  //foreach(Node node in app.MapProperty.nodeList)
                            }  //while(a.moving)

                            loadedPlayer.Stewards.Add(a);
                        }  //while(true)

                        //Read Commanders
                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (string.Equals(line, "/commanders")) break;
                            string loc = sr.ReadLine();
                            Commander a = new Commander(line, Constants.assassinGoldUpkeep, Constants.assassinFoodUpkeep, loc, playerColor);

                            string moving = sr.ReadLine();
                            if (string.Equals(moving, "True")) a.moving = true;
                            else a.moving = false;

                            while (a.moving)
                            {
                                string nodeName = sr.ReadLine();
                                if (string.Equals(nodeName, "/movement")) break;
                                foreach (Node node in app.MapProperty.nodeList)
                                {
                                    if (string.Equals(nodeName, node.Name))
                                    {
                                        a.movementRoute.Add(node);
                                        break;
                                    }  //if(string.Equals(nodeName, node.Name))
                                }  //foreach(Node node in app.MapProperty.nodeList)
                            }  //while(a.moving)

                            string lacksArmy = sr.ReadLine();
                            if(string.Equals(lacksArmy, "False"))
                            {
                                a.army.LightInfantry = Convert.ToInt32(sr.ReadLine());
                                a.army.HeavyInfantry = Convert.ToInt32(sr.ReadLine());
                                a.army.LightCavalry = Convert.ToInt32(sr.ReadLine());
                                a.army.HeavyCavalry = Convert.ToInt32(sr.ReadLine());
                                a.army.Archers = Convert.ToInt32(sr.ReadLine());
                                a.army.Musketeers = Convert.ToInt32(sr.ReadLine());
                            }

                            loadedPlayer.Commanders.Add(a);
                        }  //while(true)

                        //Read armies
                        while(true)
                        {
                            Army army = new Army();
                            army.location = sr.ReadLine();
                            if (string.Equals(army.location, "/armies")) break;

                            army.LightInfantry = Convert.ToInt32(sr.ReadLine());
                            army.HeavyInfantry = Convert.ToInt32(sr.ReadLine());
                            army.LightCavalry = Convert.ToInt32(sr.ReadLine());
                            army.HeavyCavalry = Convert.ToInt32(sr.ReadLine());
                            army.Archers = Convert.ToInt32(sr.ReadLine());
                            army.Musketeers = Convert.ToInt32(sr.ReadLine());

                            loadedPlayer.Armies.Add(army);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }catch
            {
                MessageBox.Show("An unknown problem occurred!");
                return false;
            }
        }

        private void btnMerge_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            switch(mergeState)
            {
                case enmMergeState.Unloaded:
                    app.clearData();
                    map = new Map();
                    bool mapLoaded = saveMergeMapUpload();
                    if(mapLoaded)
                    {
                        mergeState = enmMergeState.MapLoaded;
                        btnMerge.Content = "Load RED Player Save Game";
                    }
                    break;
                case enmMergeState.MapLoaded:
                    bool pl = saveMergeSaveUpload(enmPlayers.Red);
                    if (pl)
                    {
                        mergeState = enmMergeState.RedPlayerLoaded;
                        btnMerge.Content = "Load BLUE Player Save Game";
                    }else resetMerging();
                    break;
                case enmMergeState.RedPlayerLoaded:
                    pl = saveMergeSaveUpload(enmPlayers.Blue);
                    if (pl)
                    {
                        mergeState = enmMergeState.BluePlayerLoaded;
                        btnMerge.Content = "Load GREEN Player Save Game";
                    }
                    else resetMerging();
                    break;
                case enmMergeState.BluePlayerLoaded:
                    pl = saveMergeSaveUpload(enmPlayers.Green);
                    if (pl)
                    {
                        mergeState = enmMergeState.GreenPlayerLoaded;
                        btnMerge.Content = "Load PURPLE Player Save Game";
                    }
                    else resetMerging();
                    break;
                case enmMergeState.GreenPlayerLoaded:
                    pl = saveMergeSaveUpload(enmPlayers.Purple);
                    if (pl)
                    {
                        mergeState = enmMergeState.PurplePlayerLoaded;
                        btnMerge.Content = "Merge the saves";
                    }
                    else resetMerging();
                    break;
                case enmMergeState.PurplePlayerLoaded:
                    bool ms = mergeSaves();
                    resetMerging();
                    break;
            }
        }

        private bool mergeSaves()
        {
            try 
            {
                progressHoldingUpgrades();
                moveAgents();
                resetMovements();
                killAgents();
                createMapFile();
                return true;
            }catch
            {
                MessageBox.Show("A problem occured and the saves could not be proccessed!");
                return false;
            }
        }

        private void resetMovements()
        {
            App app = (App)Application.Current;
            foreach(Player player in app.players)
            {
                foreach(Commander a in player.Commanders)
                {
                    if (a.movementRoute.Count != 0)
                    {
                        List<Node> route = new List<Node>();
                        route.Add(app.MapProperty.findNode(a.location));
                        foreach (Node n in a.movementRoute)
                            route.Add(n);
                        a.moving = true;
                        a.movementRoute = route;
                    }
                }
                foreach (Scout a in player.Scouts)
                {
                    if (a.movementRoute.Count != 0)
                    {
                        List<Node> route = new List<Node>();
                        route.Add(app.MapProperty.findNode(a.location));
                        foreach (Node n in a.movementRoute)
                            route.Add(n);
                        a.moving = true;
                        a.movementRoute = route;
                    }
                }
                foreach (Steward a in player.Stewards)
                {
                    if (a.movementRoute.Count != 0)
                    {
                        List<Node> route = new List<Node>();
                        route.Add(app.MapProperty.findNode(a.location));
                        foreach (Node n in a.movementRoute)
                            route.Add(n);
                        a.moving = true;
                        a.movementRoute = route;
                    }
                }
                foreach (Assassin a in player.Assassins)
                {
                    if (a.movementRoute.Count != 0)
                    {
                        List<Node> route = new List<Node>();
                        route.Add(app.MapProperty.findNode(a.location));
                        foreach (Node n in a.movementRoute)
                            route.Add(n);
                        a.moving = true;
                        a.movementRoute = route;
                    }
                }
            }
        }

        private void killAgents()
        {
            App app = (App)Application.Current;
            if (app.KilledAgents.Count == 0) return;
            foreach(Player player in app.players)
            {
                foreach(Steward a in player.Stewards)
                {
                    if(app.KilledAgents.Contains(a.ID))
                    {
                        app.KilledAgents.Remove(a.ID);
                        player.Stewards.Remove(a);
                    }
                    if (app.KilledAgents.Count == 0) return;
                }
            }
        }

        private void createMapFile()
        {
            App app = (App)Application.Current;
            Player blue = app.getPlayer(enmPlayers.Blue);
            Player red = app.getPlayer(enmPlayers.Red);
            Player green = app.getPlayer(enmPlayers.Green);
            Player purple = app.getPlayer(enmPlayers.Purple);
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text File|*.txt";
                if (sfd.ShowDialog().Value)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                    {
                        sw.WriteLine(app.turn.ToString());
                        sw.WriteLine(app.MapProperty.NodeCount);

                        foreach (Node node in app.MapProperty.nodeList)
                        {
                            sw.WriteLine(node.Name);
                            sw.WriteLine((int)node.NodeType);
                            sw.WriteLine((int)node.Owner);
                            sw.WriteLine(node.DefenseLevel);

                            sw.WriteLine(node.nodeControl.GetValue(Canvas.LeftProperty).ToString());
                            sw.WriteLine(node.nodeControl.GetValue(Canvas.TopProperty).ToString());

                            foreach (string connection in node.listOfConnections)
                                sw.WriteLine(connection);
                            sw.WriteLine("/connections");
                        }

                        sw.WriteLine("/map");

                        foreach(Player player in app.players)
                        {
                            sw.WriteLine((int)player.color);
                            sw.WriteLine(player.Gold.ToString());
                            sw.WriteLine(player.Food.ToString());
                            sw.WriteLine(player.Stone.ToString());
                            sw.WriteLine(player.AgentCounter.ToString());

                            foreach(Assassin a in player.Assassins)
                            {
                                sw.WriteLine(a.ID);
                                sw.WriteLine(a.location);
                                sw.WriteLine(a.moving);
                                if (a.moving)
                                {
                                    foreach (Node node in a.movementRoute)
                                        sw.WriteLine(node.Name);
                                    sw.WriteLine("/movement");
                                }
                            }
                            sw.WriteLine("/assassins");
                            foreach (Scout a in player.Scouts)
                            {
                                sw.WriteLine(a.ID);
                                sw.WriteLine(a.location);
                                sw.WriteLine(a.moving);
                                if (a.moving)
                                {
                                    foreach (Node node in a.movementRoute)
                                        sw.WriteLine(node.Name);
                                    sw.WriteLine("/movement");
                                }
                            }
                            sw.WriteLine("/scouts");
                            foreach (Steward a in player.Stewards)
                            {
                                sw.WriteLine(a.ID);
                                sw.WriteLine(a.location);
                                sw.WriteLine(a.working);
                                sw.WriteLine(a.moving);
                                if (a.moving)
                                {
                                    foreach (Node node in a.movementRoute)
                                        sw.WriteLine(node.Name);
                                    sw.WriteLine("/movement");
                                }
                            }
                            sw.WriteLine("/stewards");
                            foreach (Commander a in player.Commanders)
                            {
                                sw.WriteLine(a.ID);
                                sw.WriteLine(a.location);
                                sw.WriteLine(a.moving);
                                if (a.moving)
                                {
                                    foreach (Node node in a.movementRoute)
                                        sw.WriteLine(node.Name);
                                    sw.WriteLine("/movement");
                                }

                                sw.WriteLine(a.army.isEmpty());
                                if (!a.army.isEmpty())
                                {
                                    sw.WriteLine(a.army.LightInfantry);
                                    sw.WriteLine(a.army.HeavyInfantry);
                                    sw.WriteLine(a.army.LightCavalry);
                                    sw.WriteLine(a.army.HeavyCavalry);
                                    sw.WriteLine(a.army.Archers);
                                    sw.WriteLine(a.army.Musketeers);
                                }
                            }
                            sw.WriteLine("/commanders");

                            foreach (Army army in player.Armies)
                            {
                                sw.WriteLine(army.location);
                                sw.WriteLine(army.LightInfantry);
                                sw.WriteLine(army.HeavyInfantry);
                                sw.WriteLine(army.LightCavalry);
                                sw.WriteLine(army.HeavyCavalry);
                                sw.WriteLine(army.Archers);
                                sw.WriteLine(army.Musketeers);
                            }
                            sw.WriteLine("/armies");
                        }
                        sw.WriteLine("/players");
                        sw.AutoFlush = true;
                        sw.Flush();
                    }  //using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                }
            }catch
            {
                MessageBox.Show("A problem occurred during saving!");
            }
        }

        private void moveAgents()
        {
            App app = (App)Application.Current;
            OccupationHandler occupations = new OccupationHandler();
            bool movesRemain = false;

            foreach(Player player in app.players)
            {
                enmPlayers color = player.color;
                foreach(Commander commander in player.Commanders)
                {
                    if(commander.moving)
                    {
                        Node destination = commander.movementRoute.First();
                        commander.location = destination.Name;
                        commander.movementRoute.Remove(destination);
                        commander.movement--;
                        if (commander.movement == 0)
                        {
                            commander.movement = Constants.commanderMovement;
                            commander.moving = false;
                        }
                        else if (commander.movementRoute.Count == 0)
                        {
                            commander.movement = Constants.scoutMovement;
                            commander.moving = false;
                        }
                        else movesRemain = true;

                        occupations.Add(commander, enmAgentType.Commander);
                    }else
                    {
                        occupations.Add(commander, enmAgentType.Commander);
                    }
                }  //foreach(Commander commander in player.Commanders)
                foreach (Steward steward in player.Stewards)
                {
                    if (steward.moving)
                    {
                        Node destination = steward.movementRoute.First();
                        steward.location = destination.Name;
                        steward.movementRoute.Remove(destination);
                        steward.movement--;
                        if (steward.movement == 0)
                        {
                            steward.movement = Constants.stewardMovement;
                            steward.moving = false;
                        }
                        else if (steward.movementRoute.Count == 0)
                        {
                            steward.movement = Constants.scoutMovement;
                            steward.moving = false;
                        }
                        else movesRemain = true;

                        occupations.Add(steward, enmAgentType.Steward);
                    }
                    else
                    {
                        occupations.Add(steward, enmAgentType.Steward);
                    }
                }  //foreach (Steward steward in player.Stewards)

                foreach (Scout scout in player.Scouts)
                {
                    if (scout.moving)
                    {
                        Node destination = scout.movementRoute.First();
                        scout.location = destination.Name;
                        scout.movementRoute.Remove(destination);
                        scout.movement--;
                        if (scout.movement == 0)
                        {
                            scout.movement = Constants.scoutMovement;
                            scout.moving = false;
                        }
                        else if (scout.movementRoute.Count == 0)
                        {
                            scout.movement = Constants.scoutMovement;
                            scout.moving = false;
                        }
                        else movesRemain = true;

                        occupations.Add(scout, enmAgentType.Scout);
                    }
                    else
                    {
                        occupations.Add(scout, enmAgentType.Scout);
                    }
                }  //foreach (Scout scout in player.Scouts)

                foreach (Assassin assassin in player.Assassins)
                {
                    if (assassin.moving)
                    {
                        Node destination = assassin.movementRoute.First();
                        assassin.location = destination.Name;
                        assassin.movementRoute.Remove(destination);
                        assassin.movement--;
                        if (assassin.movement == 0)
                        {
                            assassin.movement = Constants.assassinMovement;
                            assassin.moving = false;
                        } else if (assassin.movementRoute.Count == 0)
                        {
                            assassin.movement = Constants.assassinMovement;
                            assassin.moving = false;
                        }
                        else movesRemain = true;

                        occupations.Add(assassin, enmAgentType.Assassin);
                    }
                    else
                    {
                        occupations.Add(assassin, enmAgentType.Assassin);
                    }
                }  //foreach (Assassin assassin in player.Assassins)
            }  //foreach(Player player in app.players)

            if (occupations.collissionsOccurred != 0) handleCollissions(occupations);
            if (movesRemain) moveAgents();
        }  //private void moveAgents()

        private void handleCollissions(OccupationHandler occupations)
        {
            App app = (App)Application.Current;
            CollissionHandler agent1 = new CollissionHandler();
            CollissionHandler agent2 = new CollissionHandler();
            bool agent1Found = false;
            foreach(CollissionHandler occupation in occupations.occupations)
            {
                if (!occupation.collided) continue;
                if (!agent1Found)
                {
                    agent1 = occupation;
                    agent1Found = true;
                }
                else if (string.Equals(occupation.agentName, agent1.collidedWith))
                {
                    agent2 = occupation;
                    break;
                }
            }

            //check the rules for collissions
            switch(agent1.agentType)
            {
                case enmAgentType.Commander:
                    if(agent2.agentType == enmAgentType.Commander)
                    {
                        //the two commanders prepare to battle one another
                        commenceBattle(agent1.agentName, agent2.agentName);
                    }
                    if(agent2.agentType == enmAgentType.Steward)
                    {
                        //steward is killed by the commander unless the steward is in his owner's node in which case he is blocked
                        //...from moving and upgrading
                        Node loc = app.MapProperty.findNode(agent2.location);
                        if (loc.Owner != agent2.owner)
                            app.KilledAgents.Add(agent2.agentName);
                    }
                    if(agent2.agentType == enmAgentType.Assassin)
                    {
                        //if the commander has his own assassin attached to his army, there is a chance enemy is detected
                        //else the assassin attaches intself to the enemy commander and the player can in next turn order the execution
                        //if the player has ordered his assassin to not attach himself anywhere, the collission is ignored
                    }
                    if(agent2.agentType == enmAgentType.Scout)
                    {
                        //a dice is cast to check if the enemy commander detected the scout. If detected, the scout is killed
                        //if not detected and the player told his scout to follow an enemy army, he is attacked to the enemy commander
                        //else, the collission is ignored
                    }
                    break;
                case enmAgentType.Steward:
                    if (agent2.agentType == enmAgentType.Commander)
                    {
                        //steward is killed by the commander unless the steward is in his owner's node in which case he is blocked
                        //...from moving and upgrading
                        Node loc = app.MapProperty.findNode(agent1.location);
                        if (loc.Owner != agent1.owner)
                            app.KilledAgents.Add(agent1.agentName);
                    }
                    if (agent2.agentType == enmAgentType.Steward)
                    {
                        //the collission is ignored
                    }
                    if (agent2.agentType == enmAgentType.Assassin)
                    {
                        //a dice is cast and the assassin can
                        //- kill the steward
                        //- get killed by the steward
                        //- start following the steward until he kills him
                        //all above are ignored if the assassin was ordered to ignore others
                    }
                    if (agent2.agentType == enmAgentType.Scout)
                    {
                        //the collission is ignored
                    }
                    break;
                case enmAgentType.Assassin:
                    if (agent2.agentType == enmAgentType.Commander)
                    {
                        //if the commander has his own assassin attached to his army, there is a chance enemy is detected
                        //else the assassin attaches intself to the enemy commander and the player can in next turn order the execution
                        //if the player has ordered his assassin to not attach himself anywhere, the collission is ignored
                    }
                    if (agent2.agentType == enmAgentType.Steward)
                    {
                        //a dice is cast and the assassin can
                        //- kill the steward
                        //- get killed by the steward
                        //- start following the steward until he kills him
                        //all above are ignored if the assassin was ordered to ignore others
                    }
                    if (agent2.agentType == enmAgentType.Assassin)
                    {
                        //a dice is cast and one of the assassins is killed
                    }
                    if (agent2.agentType == enmAgentType.Scout)
                    {
                        //a dice is cast and the assassin can
                        //- kill the scout
                        //- get killed by the scout
                        //- start following the steward until he kills him
                        //- the two don't notice one another
                        //all above are ignored if the assassin was ordered to ignore others
                    }
                    break;
                case enmAgentType.Scout:
                    if (agent2.agentType == enmAgentType.Commander)
                    {
                        //a dice is cast to check if the enemy commander detected the scout. If detected, the scout is killed
                        //if not detected and the player told his scout to follow an enemy army, he is attacked to the enemy commander
                        //else, the collission is ignored
                    }
                    if (agent2.agentType == enmAgentType.Steward)
                    {
                        //the collission is ignored
                    }
                    if (agent2.agentType == enmAgentType.Assassin)
                    {
                        //a dice is cast and the assassin can
                        //- kill the scout
                        //- get killed by the scout
                        //- start following the steward until he kills him
                        //- the two don't notice one another
                        //all above are ignored if the assassin was ordered to ignore others
                    }
                    if (agent2.agentType == enmAgentType.Scout)
                    {
                        //a dice is cast and one of the scouts die or they don't notice one another
                    }
                    break;
            }

            occupations.collissionsOccurred--;
            if (occupations.collissionsOccurred != 0) handleCollissions(occupations);
        }

        private void commenceBattle(string name1, string name2)
        {
            App app = (App)Application.Current;
            string battlefield = "";
            int counter = 2;
            foreach(Player player in app.players)
            {
                foreach(Commander commander in player.Commanders)
                {
                    if(string.Equals(commander.ID, name1))
                    {
                        commander.inBattle = true;
                        commander.battling = name2;
                        if (counter == 2) battlefield = commander.location;
                        else if (counter == 1) commander.location = battlefield;
                        counter--;
                        break;
                    } else if(string.Equals(commander.ID, name2))
                    {
                        commander.inBattle = true;
                        commander.battling = name1;
                        if (counter == 2) battlefield = commander.location;
                        else if (counter == 1) commander.location = battlefield;
                        counter--;
                        break;
                    }
                }
                if (counter == 0) break;
            }
        }

        private void progressHoldingUpgrades()
        {
            App app = (App)Application.Current;
            List<string> nodesToBeupgraded = new List<string>();
            foreach(Player player in app.players)
            {
                foreach(Steward s in player.Stewards)
                {
                    if(s.working == 0) continue;
                    s.working--;
                    if (s.working == 0) nodesToBeupgraded.Add(s.location);
                }
            }

            if (nodesToBeupgraded.Count == 0) return;
            foreach(Node node in app.MapProperty.nodeList)
            {
                if(nodesToBeupgraded.Contains(node.Name))
                {
                    node.DefenseLevel++;
                    nodesToBeupgraded.Remove(node.Name);
                    if (nodesToBeupgraded.Count == 0) return;
                }
            }
        }

        private void resetMerging()
        {
            App app = (App)Application.Current;
            app.clearData();
            mergeState = enmMergeState.Unloaded;
            btnMerge.Content = "Merge Saves - upload the map";
        }
    }
}
