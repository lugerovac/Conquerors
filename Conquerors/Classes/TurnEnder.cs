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
        /*This class is called by the game when the player ends his turn. It saves all the necessary information into a file
         which would then, once all players have finished their turns, be used to progress the game*/

        Player player;
        int turn;

        public TurnEnder(Player _player, int _turn)
        {
            player = _player;
            turn = _turn;
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
                        sw.WriteLine(turn);
                        sw.WriteLine(Convert.ToInt32(player.color));
                        sw.WriteLine(player.Gold);
                        sw.WriteLine(player.Food);
                        sw.WriteLine(player.Stone);

                        sw.WriteLine(player.AgentCounter);
                        foreach(Assassin a in player.Assassins)
                        {
                            sw.WriteLine(a.ID);
                            sw.WriteLine(a.location);
                            sw.WriteLine(a.moving);
                            if(a.moving)
                            {
                                a.movementRoute.Remove(a.movementRoute[0]); //so that the location doesn't end up being the first destination
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
                                a.movementRoute.Remove(a.movementRoute[0]);
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
                            sw.WriteLine(a.working);
                            sw.WriteLine(a.moving);
                            if (a.moving)
                            {
                                a.movementRoute.Remove(a.movementRoute[0]);
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
                                a.movementRoute.Remove(a.movementRoute[0]);
                                foreach (Node node in a.movementRoute)
                                    sw.WriteLine(node.Name);
                                sw.WriteLine("/movement");
                            }

                            sw.WriteLine(a.army.isEmpty());
                            if(!a.army.isEmpty())
                            {
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
                    }
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}