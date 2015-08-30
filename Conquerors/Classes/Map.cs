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

namespace Conquerors
{
    public class Map
    {
        /*This class serves to save the information about the nodes in order to make handling them simpler*/
        public int NodeCount = 0;
        public List<Node> nodeList;

        public Map()
        {
            nodeList = new List<Node>();
        }

        public void AddNodeToList(string name, int nodeType, enmPlayers owner, int defenseLevel, int incomeLevel, double x, double y, List<String> listOfConnections)
        {
            Node node = new Node();
            node.Name = name;
            node.setNodeTypeByIndex(nodeType);
            node.Owner = owner;
            node.DefenseLevel = defenseLevel;
            node.IncomeLevel = incomeLevel;
            node.nodeControl.SetValue(Canvas.LeftProperty, x);
            node.nodeControl.SetValue(Canvas.TopProperty, y);

            foreach (string connection in listOfConnections)
                node.addConnection(connection);

            nodeList.Add(node);
        }

        public Node findNode(string nodeName)
        {
            foreach (Node node in nodeList)
            {
                if (string.Equals(node.Name, nodeName))
                    return node;
            }
            return null;
        }
    }
}
