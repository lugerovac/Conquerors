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

namespace ReformVillageToCastle
{
    public class ReformVillageToCastle : ControlInterface
    {
        ReformVillageToCastleControl control = new ReformVillageToCastleControl();
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
                if (ActivePlayer.Gold < 300 || ActivePlayer.Stone < 150)
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
        public bool instanced { get; set; }

        public ReformVillageToCastle()
        {
            userControl = (UserControl)control;
            userControl.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Run());
            instanced = false;
        }

        public bool CheckAvailiability()
        {
            bool returnValue = true;
            if (SelectedNode.Owner != ActivePlayer.color || SelectedNode.NodeType != enmNodeType.Village)
                returnValue = false;
            //TODO once sieging is not implemented, check if besieged
            return returnValue;
        }

        public void Run()
        {
            if (!disabled)
            {
                ActivePlayer.Gold -= 300;
                ActivePlayer.Stone -= 150;
                SelectedNode.NodeType = enmNodeType.Castle;
                SelectedNode.DefenseLevel = 1;
                ArgHandler arg1 = new ArgHandler("ReformVillageToCastle" + ActivePlayer.color.ToString() + SelectedNode.Name,
                    SelectedNode.Name);
                ListOfArguments.Add(arg1);
            }
        }

        public void ResetState()
        {

        }

        public void PreMergeOperations()
        {
            List<ArgHandler> ArgumentsToRemove = new List<ArgHandler>();
            foreach(ArgHandler arg in ListOfArguments)
            {
                if(arg.ArgumentName.Contains("ReformVillageToCastle"))
                {
                    string nodeToUpgrade = arg.Argument;
                    foreach(Node node in Map.nodeList)
                    {
                        if(string.Equals(nodeToUpgrade, node.Name))
                        {
                            node.NodeType = enmNodeType.Castle;
                            ArgumentsToRemove.Add(arg);
                            break;
                        }
                    }
                }
            }

            foreach(ArgHandler arg in ArgumentsToRemove)
            {
                ListOfArguments.Remove(arg);
            }
        }

        public void PostMergeOperations()
        {

        }
    }
}
