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
        public Player RedPlayer = new Player(enmPlayers.Red);
        public Player BluePlayer = new Player(enmPlayers.Blue);
        public Player GreenPlayer = new Player(enmPlayers.Green);
        public Player PurplePlayer = new Player(enmPlayers.Purple);

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

        public void setPlayerData(enmPlayers color, int gold, int food, int stone, int agentCounter,
            List<Scout> Scouts, List<Assassin> Assassins, List<Steward> Stewards, List<Commander> Commanders, List<Army> Armies)
        {
            switch (color)
            {
                case enmPlayers.Blue:
                    BluePlayer.Gold = gold;
                    BluePlayer.Food = food;
                    BluePlayer.Stone = stone;
                    BluePlayer.AgentCounter = agentCounter;
                    BluePlayer.Scouts = Scouts;
                    BluePlayer.Assassins = Assassins;
                    BluePlayer.Stewards = Stewards;
                    BluePlayer.Commanders = Commanders;
                    BluePlayer.Armies = Armies;
                    break;
                case enmPlayers.Green:
                    GreenPlayer.Gold = gold;
                    GreenPlayer.Food = food;
                    GreenPlayer.Stone = stone;
                    GreenPlayer.AgentCounter = agentCounter;
                    GreenPlayer.Scouts = Scouts;
                    GreenPlayer.Assassins = Assassins;
                    GreenPlayer.Stewards = Stewards;
                    GreenPlayer.Commanders = Commanders;
                    GreenPlayer.Armies = Armies;
                    break;
                case enmPlayers.Purple:
                    PurplePlayer.Gold = gold;
                    PurplePlayer.Food = food;
                    PurplePlayer.Stone = stone;
                    PurplePlayer.AgentCounter = agentCounter;
                    PurplePlayer.Scouts = Scouts;
                    PurplePlayer.Assassins = Assassins;
                    PurplePlayer.Stewards = Stewards;
                    PurplePlayer.Commanders = Commanders;
                    PurplePlayer.Armies = Armies;
                    break;
                case enmPlayers.Red:
                    RedPlayer.Gold = gold;
                    RedPlayer.Food = food;
                    RedPlayer.Stone = stone;
                    RedPlayer.AgentCounter = agentCounter;
                    RedPlayer.Scouts = Scouts;
                    RedPlayer.Assassins = Assassins;
                    RedPlayer.Stewards = Stewards;
                    RedPlayer.Commanders = Commanders;
                    RedPlayer.Armies = Armies;
                    break;
            }
        }

        public void clearData()
        {
            turn = 0;
            _mapProperty = new Map();
            ActivePlayer = enmPlayers.None;
            ActivePlayerNodes = new List<Node>();
            recruitmentCheck = false;
            RedPlayer = new Player(enmPlayers.Red);
            BluePlayer = new Player(enmPlayers.Blue);
            GreenPlayer = new Player(enmPlayers.Green);
            PurplePlayer = new Player(enmPlayers.Purple);
        }
    }
}
