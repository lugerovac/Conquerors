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
    public partial class AgentControl : UserControl
    {
        /*This is a control which is used to implement the sprite of a given agent*/

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

        public AgentControl(enmPlayers color, enmAgentType type)
        {
            InitializeComponent();
            Selected = false;
            setIcon(color, type);
        }

        /*The following function sets up the icon of the selected agent. By using the ability to convert an enumeration into a
         string, the number of lines of code is greatly reduced and all conditionals are unnecessary*/
        public void setIcon(enmPlayers color, enmAgentType type)
        {
            imgSprite.Source = new BitmapImage(new Uri("Agents/Sprite" + type.ToString() + color.ToString() + ".png", UriKind.RelativeOrAbsolute));
        }
    }
}
