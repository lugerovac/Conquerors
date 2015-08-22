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
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.IO;

namespace Conquerors.Pages
{
    public partial class Editor : Page
    {
        Map map = new Map();
        public Editor()
        {
            InitializeComponent();
            initializeLists();
            NodeLevel.Visibility = Visibility.Collapsed;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        bool AddNodeActive = false;  //keeps track of the state of btnAddNode
        bool connectNodesActive = false;  //keeps track of the state of btnConnect
        bool MovingNode = false;  //are we moving a node?
        bool QDown = false;
        bool ctrlDown = false;
        List<Node> nodeListRemove;
        List<Edge> edgeList;  //list of edges

        /*All lists are initialized here*/
        private void initializeLists()
        {
            map.nodeList = new List<Node>();
            nodeListRemove = new List<Node>();
            edgeList = new List<Edge>();
        }  //initializeLists()

        /*executes when btnAddNode is clicked. It only changes a few bool variables and changes the appearance of btnAddNode*/
        /*in some situations, it "clicks on" btnConnect*/
        private void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            if (AddNodeActive)
            {
                Style stil = stpToolbar.Resources["btnStyleNodeInnactive"] as Style;                
                btnAddNode.Style = stil;
            }
            else
            {
                if(connectNodesActive) btnConnect_Click(sender, e);
                Style stil = stpToolbar.Resources["btnStyleNodeActive"] as Style;
                btnAddNode.Style = stil;
            }  //if (AddNodeActive) : else
            AddNodeActive = !AddNodeActive;
        }

        /*executes when someone clicks the left mouse button*/
        private void cnvMapa_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AddNodeActive) AddNodeWithCursor(e);  //add a node onto the canvas
        }  //cnvMapa_MouseLeftButtonDown

        /*adds a node onto cursor's location*/
        private void AddNodeWithCursor(MouseButtonEventArgs el)
        {
            unselectNodes(true);  //unselect other nodes

            Point cursorposition = el.GetPosition(null); 
            Node newNode = new Node();
            newNode.nodeControl.SetValue(Canvas.LeftProperty, cursorposition.X - 105);  //sets the X coordination of the node
            newNode.nodeControl.SetValue(Canvas.TopProperty, cursorposition.Y - 25);  //sets the Y coordination of the node
            newNode.Name = "Node='" + map.NodeCount++.ToString() + "'";  //sets the node's name which is also its Primary Key
            cnvMapa.Children.Add(newNode.nodeControl);

            /*next we add a few events handlers to the nodes*/
            newNode.nodeControl.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => NodeSelect(sender, e, newNode.nodeControl.Name));
            newNode.nodeControl.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => NodeMoveActivate(sender, e, newNode.nodeControl.Name));

            map.nodeList.Add(newNode);  //add the node into the list so we can easily find it
            newNode.Selected = true;  //selects the new node
        }  //AddNodeWithCursor

        /*executes when the user clicks on the node with the left mouse button. 
         It basically enables the node to be moved, in case one is selected*/
        private void NodeMoveActivate(object sender, MouseButtonEventArgs e, string p)
        {
            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    if (string.Equals(node.Name, p)) MovingNode = true;
                    else return;
                }  //if
            } //foreach     
        }  //NodeMoveActivate

        /*executes when the left mouse button is released*/
        private void NodeSelect(object sender, MouseButtonEventArgs e, string p)
        {
            MovingNode = false;
            if (AddNodeActive) return;
            if (!connectNodesActive && !ctrlDown) unselectNodes(true);

            foreach (Node node in map.nodeList)
            {
                if (string.Equals(node.Name, p))
                {
                    node.Selected = true;
                    ctrlResources.SelectPlayer(node.Owner);

                    if ((int)node.NodeType != 0 & (int)node.NodeType < 6)
                    {
                        NodeLevel.Visibility = Visibility.Visible;
                        NodeLevel.Level = node.DefenseLevel;
                    }
                }  //if
            }  //foreach

            if (connectNodesActive) connectTwoNodes();
        }  //NodeSelect

        /*This function removes the lines and re-connects the nodes. 
         *It is necessary for smooth transitions and is also called after a file is loaded. Basically, it refreshes the canvas.
         *This function does NOT alter the Database*/
        private void connectAllNodes()
        {
            foreach (Edge edge in edgeList)
                cnvMapa.Children.Remove(edge.connection);  //we remove of the lines from the canvas
            edgeList.Clear();

            /*we check for each two nodes if they are connected and, if they are, we connect them with lines*/
            foreach (Node node1 in map.nodeList)
            {
                node1.arg1 = 1;  //extra argument that allows us to not check a pair of nodes more than once
                foreach (Node node2 in map.nodeList)
                {
                    if (string.Equals(node1.Name, node2.Name) || node2.arg1 == 1) continue;  //if the two nodes are the same or if already checked, then we ignore it
                    if (node1.isConnectedto(node2.Name))  //check if the nodes are supposed to be connected
                    {
                        /*we have to extract the exact coordinates of the nodes*/
                        double x1, x2, y1, y2;
                        x1 = x2 = y1 = y2 = 0;

                        GeneralTransform getPosition1 = node1.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset1 = getPosition1.Transform(new Point(0, 0));
                        x1 = offset1.X;
                        y1 = offset1.Y;

                        GeneralTransform getPosition2 = node2.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset2 = getPosition2.Transform(new Point(0, 0));
                        x2 = offset2.X;
                        y2 = offset2.Y;

                        /*now that we know where our two nodes are, we connect them*/
                        Edge edge = new Edge(x1, x2, y1, y2, node1.Name, node2.Name);
                        cnvMapa.Children.Add(edge.connection);
                        edgeList.Add(edge);

                        /*Silverlight puts every next object on top of one another and this would pose us a problem: instead 
                         *of clicking on a node, we would actually end up clicking on a line that is above it.
                         For that reaon, we will remove and add the node onto the canvas, thus putting them on top of the lines*/
                        cnvMapa.Children.Remove(node1.nodeControl);
                        cnvMapa.Children.Add(node1.nodeControl);
                        cnvMapa.Children.Remove(node2.nodeControl);
                        cnvMapa.Children.Add(node2.nodeControl);
                    }  //if
                }  //foreach
            }  //foreach
            foreach (Node node in map.nodeList) node.arg1 = 0;
        }  //connectAllNodes

        /*So that we don't lose performance by always refreshing teh entire canvas, we will only refresh the necessary conenctions*/
        private void reconnectNode(Node node)
        {
            foreach (Node node2 in map.nodeList)
            {
                if (string.Equals(node.Name, node2.Name)) continue;
                if (!node.isConnectedto(node2.Name)) continue;
                foreach(Edge edge in edgeList)
                {
                    if(edge.nodes.Contains(node.Name) && edge.nodes.Contains(node2.Name))
                    {
                        cnvMapa.Children.Remove(edge.connection);

                        double x1, x2, y1, y2;
                        GeneralTransform getPosition1 = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset1 = getPosition1.Transform(new Point(0, 0));
                        x1 = offset1.X;
                        y1 = offset1.Y;

                        GeneralTransform getPosition2 = node2.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset2 = getPosition2.Transform(new Point(0, 0));
                        x2 = offset2.X;
                        y2 = offset2.Y;

                        edge.redefine(x1, x2, y1, y2);
                        cnvMapa.Children.Add(edge.connection);
                    }

                    cnvMapa.Children.Remove(node2.nodeControl);
                    cnvMapa.Children.Add(node2.nodeControl);
                }
                cnvMapa.Children.Remove(node.nodeControl);
                cnvMapa.Children.Add(node.nodeControl);
            }
        }  //reconnectNode

        /*Here we connect two selected nodes, both visually and in the database*/
        private void connectTwoNodes()
        {
            byte numberOfSelectedNodes = 0;
            double x1, x2, y1, y2;
            x1 = x2 = y1 = y2 = 0;
            string node1name = "", node2name = "";

            foreach (Node node in map.nodeList)
            {
                /*we check for the selected nodes and extract their coordinates*/
                if (node.Selected)
                {
                    numberOfSelectedNodes++;
                    if (numberOfSelectedNodes == 1)
                    {
                        GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset = getPosition.Transform(new Point(0, 0));
                        x1 = offset.X;
                        y1 = offset.Y;
                        node1name = node.Name;
                    }
                    else
                    {
                        /*if nodes are already connected, re abort the operation*/
                        if(node.isConnectedto(node1name))
                        {
                            numberOfSelectedNodes = 0;
                            unselectNodes(false);
                            break;
                        }

                        GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset = getPosition.Transform(new Point(0, 0));
                        x2 = offset.X;
                        y2 = offset.Y;
                        node2name = node.Name;

                        unselectNodes(false);
                        break;  //found both nodes, no need to keep the loop going
                    }  //if : else
                }  //if
            } //foreach 

            if (numberOfSelectedNodes == 2)
            {
                /*Now we add the line*/
                Edge edge = new Edge(x1, x2, y1, y2, node1name, node2name);
                cnvMapa.Children.Add(edge.connection);
                edgeList.Add(edge);

                /*now that we added visual connections, it is time to connect the nodes in the Database as well*/
                foreach (Node node in map.nodeList)
                {
                    if (string.Equals(node.Name, node1name))
                    {
                        node.addConnection(node2name);
                        cnvMapa.Children.Remove(node.nodeControl);
                        cnvMapa.Children.Add(node.nodeControl);
                    }  //if
                    if (string.Equals(node.Name, node2name))
                    {
                        node.addConnection(node1name);
                        cnvMapa.Children.Remove(node.nodeControl);
                        cnvMapa.Children.Add(node.nodeControl);
                    }  //if
                }  //foreach
            }  //if
        }  //connectTwoNodes

        /*Since clicking on buttons is annoying, we have this so the user can use his mouse to change between adding and connecting nodes
         Basically, this function just clicks on the buttons*/
        private void cnvMapa_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;  //we tell Silverlight to not show its menu when the user right clicks on the canvas
            if (!AddNodeActive && !connectNodesActive) btnAddNode_Click(sender, e);
            else btnConnect_Click(sender, e);
        }  //cnvMapa_MouseRightButtonDown

        /*Very often we want to unselect all nodes. TRhis function does thta for us*/
        private void unselectNodes(bool save)
        {
            NodeLevel.Visibility = Visibility.Collapsed;

            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    if(save) node.DefenseLevel = NodeLevel.Level;
                    node.Selected = false;
                }
            }
        }  //unselectNodes

        /*When the mouse moves, this calls the function that also moves a node with it when necessary*/
        private void cnvMapa_MouseMove(object sender, MouseEventArgs e)
        {
            if (MovingNode) moveTheNode(e);
        }  //cnvMapa_MouseMove

        /*This function allows the node to be moved*/
        private void moveTheNode(MouseEventArgs e)
        {
            foreach (Node node in map.nodeList)
            {
                if (node.Selected)
                {
                    /*Here we change the Node's coordinates*/
                    Point cursorPosition = e.GetPosition(null);
                    node.nodeControl.SetValue(Canvas.LeftProperty, cursorPosition.X - 105);
                    node.nodeControl.SetValue(Canvas.TopProperty, cursorPosition.Y - 25);
                    reconnectNode(node);
                }  //if
            }  //foreach
        }  //moveTheNode

        /*This executes when teh user clicks on btnConnect*/
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (connectNodesActive) btnConnect.Content = "Connect!";
            else
            {
                if (AddNodeActive) btnAddNode_Click(sender, e);
                btnConnect.Content = "CONNECTING!";
            }
            connectNodesActive = !connectNodesActive;
        }  //btnConnect_Click

        /*This function removes the node from the canvas and the Database*/
        private void deleteNode(string nodeToDelete)
        {
            foreach (Node node in map.nodeList)
            {
                node.removeConnection(nodeToDelete); //removes the connection between nodes

                if (string.Equals(node.Name, nodeToDelete))
                {
                    nodeListRemove.Add(node);
                }  //if
            }  //foreach

            removeNodes();
            connectAllNodes();  //refresh the canvas
        }  //deleteNode

        private void removeNodes()
        {
            foreach(Node node in nodeListRemove)
            {
                cnvMapa.Children.Remove(node.nodeControl);
                map.nodeList.Remove(node);
            }
            nodeListRemove.Clear();
        }  //removeNodes

        /*This function gets called whenever the user presses a key on the keyboard*/
        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)  //the user wants to delete a node
            {
                string nodetoDelete = "";
                foreach (Node node in map.nodeList)
                {
                    if (node.Selected)
                    {
                        nodetoDelete = node.Name;
                        break;
                    }  //if
                }  //foreach
                deleteNode(nodetoDelete);
            }
            else if (e.Key == Key.End)  //the user wants to disconnect the node from other nodes
            {
                string nodetoClean = "";
                foreach (Node node in map.nodeList)
                {
                    if (node.Selected)
                    {
                        nodetoClean = node.Name;
                        break;
                    }  //if
                }  //foreach
                removeNodeConnections(nodetoClean);
            }
            else if (e.Key == Key.S && false)  //does not work for some reason
            {
                saveMap();
            }
            else if (e.Key == Key.O && false)  //does not work for some reason
            {
                loadMap();
            }
            else if(e.Key == Key.Q)
            {
                QDown = true;
            }
            else if(e.Key == Key.Ctrl)
            {
                ctrlDown = true;
            }
        }  //LayoutRoot_KeyDown

        /*Function that gets called when the user releases a key*/
        private void LayoutRoot_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) QDown = false;
            else if (e.Key == Key.Ctrl) ctrlDown = false;

        }  //LayoutRoot_KeyUp

        /*This function cleans the connection with the selected node*/
        private void removeNodeConnections(string nodetoClean)
        {
            foreach (Node node in map.nodeList)
            {
                node.removeConnection(nodetoClean);
                if (string.Equals(node.Name, nodetoClean))
                    node.cleanConnections();
            }  //foreach
            connectAllNodes();
        }  //removeNodeConnections

        /*Gets called when the user moves the mouse wheel*/
        private void cnvMapa_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!QDown) changeNodeType(e.Delta);
            else changeNodeOwner(e.Delta);
        }  //cnvMapa_MouseWheel

        /*This function changes the owner of the node*/
        private void changeNodeOwner(int delta)
        {
            foreach (Node node in map.nodeList)
            {
                if(node.Selected)
                {
                    if (delta > 0) node.Owner += 1;
                    else node.Owner -= 1;
                }
            }
        }  //changeNodeOwner

        /*This function chanegs the node's type depending on the direction of the mouse wheel*/
        private void changeNodeType(int delta)
        {
            foreach (Node node in map.nodeList)
            {
                if (node.Selected) node.changeNodeType(delta);
            }  //foreach
        }  //changeNodeType

        /*This function gets called when we click on btnSave
         It basically saves the whole map into a file*/
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveMap();
        }  //btnSave_Clic

        private void saveMap()
        {
            SaveFileDialog sfd = new SaveFileDialog();  //call the Save File Dialogue so the user can choose where to save the file
            sfd.Filter = "Text File|*.txt";  //the default file type, this line will be removed in the final version or replaced
            if (sfd.ShowDialog().Value)  //in case the user decided to save the file...
            {
                using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                {
                    sw.WriteLine(map.NodeCount);
                    sw.WriteLine(ctrlResources.bluePlayer.Gold.ToString());
                    sw.WriteLine(ctrlResources.bluePlayer.Food.ToString());
                    sw.WriteLine(ctrlResources.bluePlayer.Stone.ToString());
                    sw.WriteLine(ctrlResources.redPlayer.Gold.ToString());
                    sw.WriteLine(ctrlResources.redPlayer.Food.ToString());
                    sw.WriteLine(ctrlResources.redPlayer.Stone.ToString());
                    sw.WriteLine(ctrlResources.greenPlayer.Gold.ToString());
                    sw.WriteLine(ctrlResources.greenPlayer.Food.ToString());
                    sw.WriteLine(ctrlResources.greenPlayer.Stone.ToString());
                    sw.WriteLine(ctrlResources.purplePlayer.Gold.ToString());
                    sw.WriteLine(ctrlResources.purplePlayer.Food.ToString());
                    sw.WriteLine(ctrlResources.purplePlayer.Stone.ToString());

                    foreach (Node node in map.nodeList)
                    {
                        sw.WriteLine(node.Name);
                        sw.WriteLine((int)node.NodeType);
                        sw.WriteLine((int)node.Owner);
                        sw.WriteLine(node.DefenseLevel);

                        GeneralTransform getPosition = node.nodeControl.TransformToVisual(Application.Current.RootVisual as UIElement);
                        Point offset = getPosition.Transform(new Point(0, 0));
                        sw.WriteLine(offset.X.ToString());
                        sw.WriteLine(offset.Y.ToString());

                        foreach (string connection in node.listOfConnections)
                            sw.WriteLine(connection);
                        sw.WriteLine("/connections");
                    }  //foreach

                    sw.AutoFlush = true;
                    sw.Flush();
                }  //using file
            }  //if
        }

        private void btnLoad_Click(object sender1, RoutedEventArgs e1)
        {
            loadMap();
        }  //btnLoad_Click

        /*This function loads a map from a textual file. But first, it clears the canvas and the Database*/
        private void loadMap()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog().Value)
            {
                resetState();  //in case we are loadign a file, clean the canvas and the Database
                using (StreamReader sr = new StreamReader(ofd.File.OpenRead()))
                {
                    map.NodeCount = Convert.ToInt32(sr.ReadLine());

                    ctrlResources.bluePlayer.Gold = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.bluePlayer.Food = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.bluePlayer.Stone = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.redPlayer.Gold = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.redPlayer.Food = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.redPlayer.Stone = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.greenPlayer.Gold = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.greenPlayer.Food = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.greenPlayer.Stone = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.purplePlayer.Gold = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.purplePlayer.Food = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.purplePlayer.Stone = Convert.ToInt32(sr.ReadLine());
                    ctrlResources.refresh();

                    while (true)
                    {
                        Node newNode = new Node();
                        newNode.Name = sr.ReadLine();
                        newNode.setNodeTypeByIndex(Convert.ToInt32(sr.ReadLine()));
                        newNode.Owner = (enmPlayers) Convert.ToInt32(sr.ReadLine());
                        newNode.DefenseLevel = Convert.ToInt32(sr.ReadLine());

                        newNode.nodeControl.SetValue(Canvas.LeftProperty, Convert.ToDouble(sr.ReadLine()) - 80);
                        newNode.nodeControl.SetValue(Canvas.TopProperty, Convert.ToDouble(sr.ReadLine()) - 17);
                        cnvMapa.Children.Add(newNode.nodeControl);

                        bool i = true;
                        while (i)
                        {
                            string reader = sr.ReadLine();
                            if (string.Equals(reader, "/connections")) //with this we will know until when we have to read the connections
                                i = false;
                            else
                                newNode.addConnection(reader);
                        }

                        newNode.nodeControl.MouseLeftButtonUp += new MouseButtonEventHandler((sender, e) => NodeSelect(sender, e, newNode.nodeControl.Name));
                        newNode.nodeControl.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => NodeMoveActivate(sender, e, newNode.nodeControl.Name));
                        map.nodeList.Add(newNode);

                        if (sr.EndOfStream) break;
                    }
                }  //using file
                connectAllNodes();
            }  //if
        }

        /*This function burns the whole state until only ash remains*/
        private void resetState()
        {
            foreach (Edge edge in edgeList)
                cnvMapa.Children.Remove(edge.connection);
            foreach (Node node in map.nodeList)
                cnvMapa.Children.Remove(node.nodeControl);

            edgeList.Clear();
            nodeListRemove.Clear();
            map.nodeList.Clear();

            AddNodeActive = false;
            connectNodesActive = false;
            MovingNode = false;
            map.NodeCount = 0;
        }//resetState
    }
}
