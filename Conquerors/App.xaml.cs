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

namespace Conquerors
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        /***Player Managment***/
        private Map _mapProperty;
        public Map MapProperty
        {
            get { return _mapProperty; }
            set { _mapProperty = value; }
        }

        private enmPlayers _activePlayer;
        public enmPlayers ActivePlayer
        {
            get { return _activePlayer; }
            set { _activePlayer = value; }
        }

        public int turn = 0;
        public List<Player> players = new List<Player>();

        public List<Node> ActivePlayerNodes = new List<Node>();
        public string Recruitment_node;
        private bool recruitmentCheck = false;
        private Player _recruitmentPlayer = new Player();
        public Player Recruitment_player
        {
            get 
            { 
                if(recruitmentCheck)
                {
                    return _recruitmentPlayer;
                }
                else
                {
                    return null;
                }
            }
            set 
            {
                recruitmentCheck = true;
                _recruitmentPlayer = value;
            }
        }

        public Player getPlayer(enmPlayers color)
        {
            foreach(Player player in players)
            {
                if (player.color == color)
                    return player;
            }
            return null;
        }

        public Player getPlayer()
        {
            foreach (Player player in players)
            {
                if (player.color == ActivePlayer)
                    return player;
            }
            return null;
        }

        public void setPlayerData(Player player)
        {
            Player playerToReplace = new Player();
            foreach(Player pl in players)
            {
                if(player.color == pl.color)
                {
                    playerToReplace = pl;
                    break;
                }
            }
            players.Remove(playerToReplace);
            players.Add(player);
        }

        public void setPlayerData(enmPlayers color, int gold, int food, int stone, int agentCounter,
            List<Scout> Scouts, List<Assassin> Assassins, List<Steward> Stewards, List<Commander> Commanders, List<Army> Armies)
        {
            foreach(Player player in players)
            {
                if (player.color != color) continue;
                player.Gold = gold;
                player.Food = food;
                player.Stone = stone;
                player.AgentCounter = agentCounter;
                player.Scouts = Scouts;
                player.Assassins = Assassins;
                player.Stewards = Stewards;
                player.Commanders = Commanders;
                player.Armies = Armies;
                break;
            }
        }

        public void initializePlayers()
        {
            players.Clear();
            players.Add(new Player(enmPlayers.Red));
            players.Add(new Player(enmPlayers.Blue));
            players.Add(new Player(enmPlayers.Green));
            players.Add(new Player(enmPlayers.Purple));
        }

        public void clearData()
        {
            turn = 0;
            _mapProperty = new Map();
            ActivePlayer = enmPlayers.None;
            ActivePlayerNodes = new List<Node>();
            recruitmentCheck = false;
            initializePlayers();
        }
    }
}
