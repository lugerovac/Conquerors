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
    public class Node
    {
        const int MAXPLAYERS = 4;  //maximum number of players
        public int arg1;  //extra argument, in case someone needs it
        public bool beingUpgraded = false;
        public int DefenseLevel = 0;
        public int IncomeLevel = 0;
        public double X, Y;

        public NodeControl nodeControl; //link to the control that will appear on the canvas
        public List<string> listOfConnections;  //list of nodes that this node is connected to

        private enmNodeType _nodeType;
        public enmNodeType NodeType
        {
            get { return _nodeType; }
            set
            {
                _nodeType = value;
                changeIcon(NodeType);  //since the type of the nodes changes, gotta change it look as well!
            }
        }

        private enmPlayers _owner;
        public enmPlayers Owner
        {
            get { return _owner; }
            set
            {
                if ((int)NodeType == 0 || (int)NodeType > 6) _owner = 0;  //some types of nodes can't have owners
                else if ((int)value > MAXPLAYERS) _owner = (enmPlayers)1;
                else if (value < 0) _owner = (enmPlayers)MAXPLAYERS;
                else _owner = value;
                nodeControl.Owner = Owner;
            }
        }

        public bool darkened
        {
            get { return nodeControl.darkened; }
        }

        /*So that we don't need to write node.nodeControl.Name and alike, we use this to make it shorter and easier*/
        public string Name
        {
            get { return nodeControl.Name; }
            set { nodeControl.Name = value; }
        }
        public bool Selected
        {
            get { return nodeControl.Selected; }
            set { nodeControl.Selected = value; }
        }

        public Node()
        {
            nodeControl = new NodeControl();
            listOfConnections = new List<string>();
            NodeType = enmNodeType.Plain;
            Owner = 0;

            arg1 = 0;
        }  //constructor

        /*This is used to set the node's type when we load the node from a file*/
        public void setNodeTypeByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    NodeType = enmNodeType.Plain;
                    break;
                case 1:
                    NodeType = enmNodeType.Metropolis;
                    break;
                case 2:
                    NodeType = enmNodeType.Castle;
                    break;
                case 3:
                    NodeType = enmNodeType.City;
                    break;
                case 4:
                    NodeType = enmNodeType.Church;
                    break;
                case 5:
                    NodeType = enmNodeType.Village;
                    break;
                case 6:
                    NodeType = enmNodeType.Mine;
                    break;
                case 7:
                    NodeType = enmNodeType.Forest;
                    break;
                case 8:
                    NodeType = enmNodeType.Mountains;
                    break;
                case 9:
                    NodeType = enmNodeType.Riverside;
                    break;
                case 10:
                    NodeType = enmNodeType.MountainPass;
                    break;
            }  //switch
        }  //setNodeTypeByIndex

        /*This function is called when the user moves his mouse wheel in order to change the node's type*/
        public void changeNodeType(int direction)
        {
            if (direction < 0)
            {
                if (NodeType == enmNodeType.Plain) NodeType = enmNodeType.MountainPass;
                else NodeType -= 1;
            }
            else if (direction > 0)
            {
                if (NodeType == enmNodeType.MountainPass) NodeType = enmNodeType.Plain;
                else NodeType += 1;
            }  //if : else
            Owner = Owner;  //looks trivial, but due to the set property this will alter some of the variables
        }  //changeNodeType

        /*This function loads the image onto the node*/
        private void changeIcon(enmNodeType NodeType)
        {
            switch (NodeType)
            {
                case enmNodeType.Plain:
                    nodeControl.changeNodeType("Plain");
                    break;
                case enmNodeType.Metropolis:
                    nodeControl.changeNodeType("Metropolis");
                    break;
                case enmNodeType.Castle:
                    nodeControl.changeNodeType("Castle");
                    break;
                case enmNodeType.City:
                    nodeControl.changeNodeType("City");
                    break;
                case enmNodeType.Church:
                    nodeControl.changeNodeType("Church");
                    break;
                case enmNodeType.Village:
                    nodeControl.changeNodeType("Village");
                    break;
                case enmNodeType.Mine:
                    nodeControl.changeNodeType("Mine");
                    break;
                case enmNodeType.Forest:
                    nodeControl.changeNodeType("Forest");
                    break;
                case enmNodeType.Mountains:
                    nodeControl.changeNodeType("Mountains");
                    break;
                case enmNodeType.Riverside:
                    nodeControl.changeNodeType("Riverside");
                    break;
                case enmNodeType.MountainPass:
                    nodeControl.changeNodeType("Mountain_Pass");
                    break;
            }  //switch
        }  //changeIcon

        public void addConnection(string connect)
        {
            listOfConnections.Add(connect);
        }  //addConnection

        /*This function removes a connection with a given node if it exists*/
        public void removeConnection(string connect)
        {
            if (listOfConnections.Contains(connect))
                listOfConnections.Remove(connect);
        }  //removeConnection

        /*This function removes all of the Node's connections*/
        public void cleanConnections()
        {
            listOfConnections.Clear();
        }  //cleanConnections

        /*This function checks if this node is connected with another one*/
        public bool isConnectedto(string nodeName)
        {
            foreach (string connection in listOfConnections)
                if (string.Equals(nodeName, connection))
                    return true;

            return false;
        }  //isConnectedto

        public void darken()
        {
            nodeControl.darken();
        }

        public void lighten()
        {
            nodeControl.lighten();
        }

        public void routeSelect(bool farAway)
        {
            nodeControl.routeSelect(farAway);
        }

        public void routeUnselect()
        {
            nodeControl.routeUnselect();
        }
    }
}
