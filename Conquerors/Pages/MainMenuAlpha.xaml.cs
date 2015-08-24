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
            loadMap(enmPlayers.Red);
        }

        /*The following function loads up a file and stores the necessary data intp app.xaml module, from which the next
         page (Game.xaml) will read the data*/
        private void loadMap(enmPlayers activePlayer)
        {
            App app = (App)Application.Current;
            app.initializePlayers();
            app.turn = 1;
            app.ActivePlayer = activePlayer;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value)
            {
                map = new Map();
                using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                {
                    map.NodeCount = Convert.ToInt32(sr.ReadLine());

                    Player blue = new Player(enmPlayers.Blue);
                    Player red = new Player(enmPlayers.Red);
                    Player green = new Player(enmPlayers.Green);
                    Player purple = new Player(enmPlayers.Purple);

                    blue.Gold = Convert.ToInt32(sr.ReadLine());
                    blue.Food = Convert.ToInt32(sr.ReadLine());
                    blue.Stone = Convert.ToInt32(sr.ReadLine());
                    app.setPlayerData(blue);
                    red.Gold = Convert.ToInt32(sr.ReadLine());
                    red.Food = Convert.ToInt32(sr.ReadLine());
                    red.Stone = Convert.ToInt32(sr.ReadLine());
                    app.setPlayerData(red);
                    green.Gold = Convert.ToInt32(sr.ReadLine());
                    green.Food = Convert.ToInt32(sr.ReadLine());
                    green.Stone = Convert.ToInt32(sr.ReadLine());
                    app.setPlayerData(green);
                    purple.Gold = Convert.ToInt32(sr.ReadLine());
                    purple.Food = Convert.ToInt32(sr.ReadLine());
                    purple.Stone = Convert.ToInt32(sr.ReadLine());
                    app.setPlayerData(purple);

                    while (true)
                    {
                        string name = sr.ReadLine();
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

                        if (sr.EndOfStream) break;
                    } //while (true)
                }  //using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                NavigationService.Navigate(new Uri("/Pages/Game.xaml", UriKind.Relative));
            }  //if (ofd.ShowDialog().Value)
        }  //private void loadMap()

        /*This function stored necessary data into the app.xaml module, through which the next page will be able to read the data*/
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App app = (App)Application.Current;
            app.MapProperty = map;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void continueGame(enmPlayers activePlayer)
        {

        }

        private void btnPlayBlue_Click(object sender, RoutedEventArgs e)
        {
            loadMap(enmPlayers.Blue);
        }

        private void btnPlayGreen_Click(object sender, RoutedEventArgs e)
        {
            loadMap(enmPlayers.Green);
        }

        private void btnPlayPurple_Click(object sender, RoutedEventArgs e)
        {
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
                        map.NodeCount = Convert.ToInt32(sr.ReadLine());

                        //we will not be needing the resources, but the data has to be read
                        /*
                        app.BluePlayer.Gold = Convert.ToInt32(sr.ReadLine());
                        app.BluePlayer.Food = Convert.ToInt32(sr.ReadLine());
                        app.BluePlayer.Stone = Convert.ToInt32(sr.ReadLine());
                        app.RedPlayer.Gold = Convert.ToInt32(sr.ReadLine());
                        app.RedPlayer.Food = Convert.ToInt32(sr.ReadLine());
                        app.RedPlayer.Stone = Convert.ToInt32(sr.ReadLine());
                        app.GreenPlayer.Gold = Convert.ToInt32(sr.ReadLine());
                        app.GreenPlayer.Food = Convert.ToInt32(sr.ReadLine());
                        app.GreenPlayer.Stone = Convert.ToInt32(sr.ReadLine());
                        app.PurplePlayer.Gold = Convert.ToInt32(sr.ReadLine());
                        app.PurplePlayer.Food = Convert.ToInt32(sr.ReadLine());
                        app.PurplePlayer.Stone = Convert.ToInt32(sr.ReadLine());
                         */
                        for (int i = 0; i < 12; i++)
                            sr.ReadLine();

                        while (true)
                        {
                            string name = sr.ReadLine();
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

                            if (sr.EndOfStream) break;
                        } //while (true)
                    }  //using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
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
                        if (app.turn == 0)
                        {
                            app.turn = turn;
                        }else if(app.turn != turn)
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
                                    if (node.Owner != playerColor) continue;
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
                                    if (node.Owner != playerColor) continue;
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
                                    if (node.Owner != playerColor) continue;
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
                                    if (node.Owner != playerColor) continue;
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
                return true;
            }catch
            {
                MessageBox.Show("A problem occured and the saves could not be proccessed!");
                return false;
            }
        }

        private void moveAgents()
        {

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
            foreach(Node node in map.nodeList)
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
