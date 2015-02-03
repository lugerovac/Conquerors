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
    public static class Dijkstra
    {
        private class DijkstraNode
        {
            public Node nodeElement;
            public Node formerNode;
            public int distance;
            public bool marked = false;

            public DijkstraNode(Node _node, int _distance)
            {
                nodeElement = _node;
                distance = _distance;
            }

            public DijkstraNode(Node _node, int _distance, Node _former)
            {
                nodeElement = _node;
                distance = _distance;
                formerNode = _former;
            }

            public DijkstraNode(Node _node)
            {
                nodeElement = _node;
                distance = -1;
                formerNode = new Node();
                formerNode.Name = "noname";
            }

            public DijkstraNode()
            {
                formerNode = new Node();
                formerNode.Name = "noname";
                distance = -1;
                //used for temporary declarations
            }
        }

        public static List<Node> FindPath(List<Node> network, Node startNode, Node endNode, Player player)
        {
            List<DijkstraNode> DijkstraNetwork = new List<DijkstraNode>();
            DijkstraNode lastAddedNode = new DijkstraNode();

            foreach(Node node in network)
            {
                DijkstraNode dNode;

                if (string.Equals(startNode.Name, node.Name))
                {
                    dNode = new DijkstraNode(startNode, 0);
                    dNode.marked = true;
                    lastAddedNode = dNode;
                }
                else
                    dNode = new DijkstraNode(node);

                DijkstraNetwork.Add(dNode);
            }

            int markCounter = 1;
            while(true)
            {
                foreach(string neighborName in lastAddedNode.nodeElement.listOfConnections)
                {
                    foreach(DijkstraNode dNode in DijkstraNetwork)
                    {
                        if (dNode.marked) continue;

                        if (string.Equals(dNode.nodeElement.Name, neighborName))
                        {
                            if (dNode.distance == -1)
                            {
                                dNode.distance = lastAddedNode.distance + 1;
                                dNode.formerNode = lastAddedNode.nodeElement;
                            }
                            else if (dNode.distance > lastAddedNode.distance + 1)
                            {
                                dNode.distance = lastAddedNode.distance + 1;
                                dNode.formerNode = lastAddedNode.nodeElement;
                            }
                        }  //if (string.Equals(dNode.nodeElement.Name, neighborName))
                    }  //foreach(DijkstraNode dNode in DijkstraNetwork)
                }  //foreach(string neighborName in lastAddedNode.nodeElement.listOfConnections)

                DijkstraNode closestNode = new DijkstraNode();
                foreach(DijkstraNode dNode in DijkstraNetwork)
                {
                    if (dNode.marked) continue;

                    if (closestNode.distance == -1)
                        closestNode = dNode;

                    if (dNode.distance != -1 && dNode.distance < closestNode.distance)
                        closestNode = dNode;
                    else if (dNode.distance != -1 && dNode.distance == closestNode.distance)
                    {
                        if (dNode.nodeElement.Owner == player.color)
                            closestNode = dNode;
                        else if(dNode.nodeElement.Owner == enmPlayers.None)
                            closestNode = dNode;
                    }
                }
                closestNode.marked = true;
                lastAddedNode = closestNode;
                markCounter++;
                //MessageBox.Show(closestNode.nodeElement.Name + ": " + closestNode.formerNode.Name + ": " + closestNode.distance);

                if (markCounter == DijkstraNetwork.Count) break;
            }  //while(true)

            List<Node> returnValue = new List<Node>();
            returnValue.Add(endNode);
            Node currentNode = endNode;

            while(true)
            {
                foreach (DijkstraNode dNode in DijkstraNetwork)
                {
                    if(string.Equals(currentNode.Name, dNode.nodeElement.Name))
                    {
                        currentNode = dNode.formerNode;
                        returnValue.Add(currentNode);
                        if (string.Equals(currentNode.Name, startNode.Name))
                        {
                            returnValue.Reverse();
                            return returnValue;
                        }
                    }
                }  //foreach(DijkstraNode dNode in dijkstraList)
            }  //while(true)
        }
    }
}
