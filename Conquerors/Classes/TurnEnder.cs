using System;
using System.Collections.Generic;
using System.IO;
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
    public class TurnEnder
    {
        Map map;
        Player player;

        public TurnEnder(Map _map, Player _player)
        {
            map = _map;
            player = _player;
        }

        public bool Commit()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text File|*.txt";
                if (sfd.ShowDialog().Value)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
                    {
                        /*Player Info*/


                        /*Map Info*/
                        sw.WriteLine(map.NodeCount);
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

                        sw.WriteLine("/map");

                        sw.AutoFlush = true;
                        sw.Flush();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}