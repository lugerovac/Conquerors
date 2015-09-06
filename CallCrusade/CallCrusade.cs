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

namespace CallCrusade
{
    public class CallCrusade : ControlInterface
    {
        CallCrusadeControl control = new CallCrusadeControl();
        public UserControl userControl { get; set; }
        public int MoraleGainModifier { get; set; }
        public int GoldGainModifier { get; set; }
        public int FoodGainModifier { get; set; }
        public int StoneGainModifier { get; set; }
        public List<ArgHandler> ListOfArguments { get; set; }
        public Map Map { get; set; }
        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Node SelectedNode { get; set; }
        public Agent SelectedAgent { get; set; }
        public bool instanced { get; set; }
        private bool disabled;

        public CallCrusade()
        {
            userControl = (UserControl)control;
            userControl.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Run());
            instanced = false;
        }

        public bool CheckAvailiability()
        {
            if (SelectedNode.Owner != ActivePlayer.color || SelectedNode.NodeType != enmNodeType.Metropolis)
                return false;

            int NodeCounter = 0;
            foreach(Node node in Map.nodeList)
            {
                if(node.Owner == ActivePlayer.color)
                {
                    NodeCounter++;
                    if(NodeCounter > 1)
                        return false;
                }
            }

            foreach(ArgHandler arg in ListOfArguments)
            {
                if(arg.ArgumentName.Contains("CallCrusade" + ActivePlayer.color.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        public void Run()
        {
            Random rnd = new Random();
            foreach(Node node in Map.nodeList)
            {
                if(node.NodeType == enmNodeType.Church)
                {
                    string ID = ActivePlayer.color.ToString() + "Commander" + ActivePlayer.AgentCounter.ToString();
                    Commander newCommander = new Commander(ID,  Constants.commanderGoldUpkeep, Constants.commanderFoodUpkeep, node.Name, ActivePlayer.color);
                    newCommander.army.LightInfantry = rnd.Next(5, 15);
                    newCommander.army.HeavyInfantry = rnd.Next(1, 10);
                    newCommander.army.LightCavalry = rnd.Next(1, 10);
                    newCommander.army.HeavyCavalry = rnd.Next(0, 5);
                    newCommander.army.Archers = rnd.Next(5, 25);
                    newCommander.army.Musketeers = rnd.Next(0, 5);
                    ActivePlayer.addCommander(newCommander);
                }
            }
            ArgHandler argument = new ArgHandler("CallCrusade" + ActivePlayer.color + "Blocked1", "Crusade Already Called");
            ListOfArguments.Add(argument);
        }

        public void ResetState()
        {

        }

        public void PreMergeOperations()
        {
            foreach(Player player in Players)
            {
                ArgHandler argument = new ArgHandler("CallCrusade" + player.color + "Blocked2", "Crusade now Unavailable");
                ListOfArguments.Add(argument);
            }
        }

        public void PostMergeOperations()
        {

        }
    }
}
