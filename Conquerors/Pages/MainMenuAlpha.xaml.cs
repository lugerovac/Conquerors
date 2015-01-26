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
            loadMap();
        }

        /*The following function loads up a file and stores the necessary data intp app.xaml module, from which the next
         page (Game.xaml) will read the data*/
        private void loadMap()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value)
            {
                map = new Map();
                using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                {
                    map.NodeCount = Convert.ToInt32(sr.ReadLine());

                    App app = (App)Application.Current;
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


                    while (true)
                    {
                        string name = sr.ReadLine();
                        int nodeType = Convert.ToInt32(sr.ReadLine());
                        enmPlayers owner = (enmPlayers)Convert.ToInt32(sr.ReadLine());
                        int level = Convert.ToInt32(sr.ReadLine());
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

                        map.AddNodeToList(name, nodeType, owner, level, x, y, listOfConnections);

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
            app.ActivePlayer = enmPlayers.Red;
        }
    }
}
