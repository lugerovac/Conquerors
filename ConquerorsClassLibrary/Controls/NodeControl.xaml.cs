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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Conquerors
{
    public partial class NodeControl : UserControl
    {
        public bool darkened;

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (Selected) rctSelect.Visibility = Visibility.Visible;
                else rctSelect.Visibility = Visibility.Collapsed;
            }
        }

        private enmPlayers _owner;
        public enmPlayers Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                switch (Owner)
                {
                    case enmPlayers.None:
                        elpNode.Stroke = new SolidColorBrush(Colors.White);
                        elpNode.Fill = new SolidColorBrush(Colors.White);
                        break;
                    case enmPlayers.Blue:
                        elpNode.Stroke = new SolidColorBrush(Colors.Blue);
                        elpNode.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                    case enmPlayers.Red:
                        elpNode.Stroke = new SolidColorBrush(Colors.Red);
                        elpNode.Fill = new SolidColorBrush(Colors.Red);
                        break;
                    case enmPlayers.Green:
                        elpNode.Stroke = new SolidColorBrush(Colors.Green);
                        elpNode.Fill = new SolidColorBrush(Colors.Green);
                        break;
                    case enmPlayers.Purple:
                        elpNode.Stroke = new SolidColorBrush(Colors.Purple);
                        elpNode.Fill = new SolidColorBrush(Colors.Purple);
                        break;
                }
            }
        }

        public NodeControl()
        {
            InitializeComponent();
            Selected = false;
            darkened = false;
        }

        /*This function changes the image "inside" the node*/
        public void changeNodeType(string type)
        {
            imgNodeIcon.Source = new BitmapImage(new Uri("NodeIcons/" + type + ".png", UriKind.RelativeOrAbsolute));
        }

        public void darken()
        {
            darkened = true;
            elpNode.Fill = new SolidColorBrush(Colors.LightGray);
            if (Owner == enmPlayers.None) elpNode.Stroke = new SolidColorBrush(Colors.LightGray);
            this.Opacity = Constants.darkenedNodeOpacity;
        }

        public void lighten()
        {
            darkened = false;
            elpNode.Fill = new SolidColorBrush(Colors.White);
            if (Owner == enmPlayers.None) elpNode.Stroke = new SolidColorBrush(Colors.White);
            else
            {
                switch(Owner)
                {
                    case enmPlayers.Red:
                        elpNode.Fill = new SolidColorBrush(Colors.Red);
                        break;
                    case enmPlayers.Blue:
                        elpNode.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                    case enmPlayers.Green:
                        elpNode.Fill = new SolidColorBrush(Colors.Green);
                        break;
                    case enmPlayers.Purple:
                        elpNode.Fill = new SolidColorBrush(Colors.Purple);
                        break;
                }
            }
            this.Opacity = 1;
        }

        public void routeSelect(bool farAway)
        {
            if(farAway) elpNode.Stroke = new SolidColorBrush(Colors.Yellow);
            else elpNode.Stroke = new SolidColorBrush(Colors.Orange);
        }

        public void routeUnselect()
        {
            elpNode.Stroke = new SolidColorBrush(Colors.White);
        }
    }
}
