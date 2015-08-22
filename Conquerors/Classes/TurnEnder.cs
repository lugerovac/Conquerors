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
                        sw.WriteLine(player.color);
                        sw.WriteLine(player.Gold);
                        sw.WriteLine(player.Food);
                        sw.WriteLine(player.Stone);

                        sw.Write(player.AgentCounter);
                        foreach(Assassin a in player.Assassins)
                        {
                            sw.WriteLine(a.ID);
                            sw.WriteLine(a.location);
                            sw.WriteLine(a.moving);
                            if(a.moving)
                            {
                                foreach(Node node in a.movementRoute)
                                    sw.WriteLine(node.Name);
                                sw.WriteLine("/movement");
                            }
                        }
                        sw.WriteLine("/assassins");
                        foreach (Scout a in player.Scouts)
                        {
                            sw.WriteLine(a.ID);
                            sw.WriteLine(a.location);
                            sw.WriteLine(a.moving);
                            if (a.moving)
                            {
                                foreach (Node node in a.movementRoute)
                                    sw.WriteLine(node.Name);
                                sw.WriteLine("/movement");
                            }
                        }
                        sw.WriteLine("/scouts");
                        foreach (Steward a in player.Stewards)
                        {
                            sw.WriteLine(a.ID);
                            sw.WriteLine(a.location);
                            sw.WriteLine(a.moving);
                            sw.WriteLine(a.working);
                            if (a.moving)
                            {
                                foreach (Node node in a.movementRoute)
                                    sw.WriteLine(node.Name);
                                sw.WriteLine("/movement");
                            }
                        }
                        sw.WriteLine("/stewards");
                        foreach (Commander a in player.Commanders)
                        {
                            sw.WriteLine(a.ID);
                            sw.WriteLine(a.location);
                            sw.WriteLine(a.moving);
                            if (a.moving)
                            {
                                foreach (Node node in a.movementRoute)
                                    sw.WriteLine(node.Name);
                                sw.WriteLine("/movement");
                            }

                            sw.WriteLine(a.army.isEmpty());
                            if(!a.army.isEmpty())
                            {
                                sw.WriteLine(a.army.location);
                                sw.WriteLine(a.army.LightInfantry);
                                sw.WriteLine(a.army.HeavyInfantry);
                                sw.WriteLine(a.army.LightCavalry);
                                sw.WriteLine(a.army.HeavyCavalry);
                                sw.WriteLine(a.army.Archers);
                                sw.WriteLine(a.army.Musketeers);
                            }
                        }
                        sw.WriteLine("/commanders");

                        foreach(Army army in player.Armies)
                        {
                            sw.WriteLine(army.location);
                            sw.WriteLine(army.LightInfantry);
                            sw.WriteLine(army.HeavyInfantry);
                            sw.WriteLine(army.LightCavalry);
                            sw.WriteLine(army.HeavyCavalry);
                            sw.WriteLine(army.Archers);
                            sw.WriteLine(army.Musketeers);
                        }
                        sw.WriteLine("/armies");

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