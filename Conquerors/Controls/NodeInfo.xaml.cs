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
    public partial class NodeInfo : UserControl
    {
        /*This simple control shows the data related to a selected node*/

        public NodeInfo()
        {
            InitializeComponent();
        }

        public void showNodeInfo(Node node, bool showLevel)
        {
            txtLevel.Visibility = Visibility.Collapsed;

            txtType.Text = node.NodeType.ToString();
            if ((int)node.NodeType != 0 && (int)node.NodeType <= 6 && !showLevel) 
            {
                txtLevel.Visibility = Visibility.Visible;
                txtLevel.Text = "Lv. " + node.DefenseLevel.ToString();
            }
        }

        public void showNodeGain(string resource, int gain)
        {
            imgResource.Visibility = Visibility.Visible;
            txtProfit.Visibility = Visibility.Visible;

            imgResource.Source = new BitmapImage(new Uri("Resources/" + resource + ".png", UriKind.RelativeOrAbsolute));
            txtProfit.Text = " + " + gain.ToString();
        }

        public void hideNodeGain()
        {
            imgResource.Visibility = Visibility.Collapsed;
            txtProfit.Visibility = Visibility.Collapsed;
        }
    }
}
