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
    /*In this class, all the necessary data related to the player is being contained and handled*/
    public class Player
    {
        private int _gold, _stone, _food, _morale;
        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }

        public int Stone
        {
            get { return _stone; }
            set { _stone = value; }
        }

        public int Food
        {
            get { return _food; }
            set { _food = value; }
        }

        public int Morale
        {
            get { return _morale; }
            set { _morale = value; }
        }

        public int AgentCounter = 0;
        public List<Scout> Scouts = new List<Scout>();
        public List<Assassin> Assassins = new List<Assassin>();
        public List<Steward> Stewards = new List<Steward>();
        public List<Commander> Commanders = new List<Commander>();
        public List<Army> Armies = new List<Army>();

        public enmPlayers color;

        public Player()
        {

        }

        public Player(enmPlayers _color)
        {
            color = _color;
        }

        public void addScout(Scout scout)
        {
            AgentCounter++;
            Scouts.Add(scout);
        }

        public void addAssassin(Assassin assassin)
        {
            AgentCounter++;
            Assassins.Add(assassin);
        }

        public void addSteward(Steward steward)
        {
            AgentCounter++;
            Stewards.Add(steward);
        }

        public void addCommander(Commander commander)
        {
            AgentCounter++;
            Commanders.Add(commander);
        }
    }
}
