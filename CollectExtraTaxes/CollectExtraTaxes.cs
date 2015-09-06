using Conquerors;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CollectExtraTaxes
{
    public class CollectExtraTaxes : ControlInterface
    {
        CollectExtraTaxesControl control = new CollectExtraTaxesControl();
        public UserControl userControl { get; set; }
        public int MoraleGainModifier { get; set; }
        public int GoldGainModifier { get; set; }
        public int FoodGainModifier { get; set; }
        public int StoneGainModifier { get; set; }
        public List<ArgHandler> ListOfArguments { get; set; }
        public Map Map { get; set; }
        public List<Player> Players { get; set; }
        private Player _activePlayer;
        public Player ActivePlayer 
        { 
            get
            {
                return _activePlayer;
            }
            set
            {
                _activePlayer = value;
                if(ActivePlayer.Gold < 100)
                {
                    control.LayoutRoot.Opacity = 0.5;
                    disabled = true;
                }
                else
                {
                    control.LayoutRoot.Opacity = 1;
                    disabled = false;
                }
            }
        }
        private bool disabled;
        public Node SelectedNode { get; set; }
        public Agent SelectedAgent { get; set; }
        public bool instanced {get; set;}

        public CollectExtraTaxes()
        {
            userControl = (UserControl)control;
            userControl.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Run());
            instanced = false;
        }

        public bool CheckAvailiability()
        {
            bool returnValue = true;
            if (SelectedNode.Owner != ActivePlayer.color)
                returnValue = false;
            else if (SelectedNode.NodeType != enmNodeType.City
                && SelectedNode.NodeType != enmNodeType.Village
                && SelectedNode.NodeType != enmNodeType.Metropolis)
                returnValue = false;
            return returnValue;
        }

        public void Run()
        {
            if (!disabled)
            {
                ActivePlayer.Gold += 100;
                MoraleGainModifier -= 5;
            }
        }

        public void ResetState()
        {
            MoraleGainModifier = 0;
        }

        public void PreMergeOperations()
        {

        }

        public void PostMergeOperations()
        {

        }
    }
}
