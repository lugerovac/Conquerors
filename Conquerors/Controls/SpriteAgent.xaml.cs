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
    public partial class SpriteAgent : UserControl
    {
        private enmPlayers _owner;
        public enmPlayers Owner
        {
            get { return _owner; }
            set
            {
                _owner = Owner;
                setSprite();
            }
        }

        private enmSpriteType _type;
        public enmSpriteType SpriteType
        {
            get { return SpriteType; }
            set
            {
                _type = SpriteType;
                setSprite();
            }
        }

        public SpriteAgent(enmPlayers owner, enmSpriteType spriteType)
        {
            InitializeComponent();

            Owner = owner;
            SpriteType = spriteType;

            setSprite();
        }

        private void setSprite()
        {
            string type = "", color = "";

            switch(SpriteType)
            {
                case enmSpriteType.Commander:
                    type = "Commander";
                    break;
                case enmSpriteType.Steward:
                    type = "Steward";
                    break;
                case enmSpriteType.Assassin:
                    type = "Assassin";
                    break;
                case enmSpriteType.Scout:
                    type = "Scout";
                    break;
            }

            switch(Owner)
            {
                case enmPlayers.Blue:
                    color = "Blue";
                    break;
                case enmPlayers.Red:
                    color = "Red";
                    break;
                case enmPlayers.Green:
                    color = "Green";
                    break;
                case enmPlayers.Purple:
                    color = "Purple";
                    break;
            }

            imgSprite.Source = new BitmapImage(new Uri("Agents/Sprite" + type + color + ".png", UriKind.RelativeOrAbsolute)); 
        }
    }
}
