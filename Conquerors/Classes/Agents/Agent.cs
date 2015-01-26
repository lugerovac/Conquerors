using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Conquerors
{
    public abstract class Agent
    {
        /*This is an abstract class from which all Agents inherit and defines the Agent's Sprite, upkeep costs,
         location, owner and the ID*/

        public AgentControl Sprite;
        public int goldUpkeepCost;
        public int foodUpkeepCost;
        public string location;
        public enmPlayers owner;
        public int ID;
        public int movement;
        public bool moving = false;
        public List<Node> movementRoute = new List<Node>();

        public bool Selected
        {
            get { return Sprite.Selected; }
            set { Sprite.Selected = value; }
        }

        /*This constructor gets called whenever an Agent is added to the game, no matter its type*/
        public Agent(int _ID, int _goldUpkeepCost, int _foodUpkeepCost, string _location, enmPlayers _owner)
        {
            ID = _ID;
            goldUpkeepCost = _goldUpkeepCost;
            foodUpkeepCost = _foodUpkeepCost;
            location = _location;
            owner = _owner;

            inheritedSetUp();  //this calls the function which executes code unique for each agent type
        }

        public abstract void inheritedSetUp();
        public void Select()
        {
            Selected = true;
        }

        public void Unselect()
        {
            Selected = false;
        }

        public int getMovementValue()
        {
            return movement;
        }
    }
}
