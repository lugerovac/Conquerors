using System;
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
    public class Steward : Agent
    {
        public int working = 0;

        public Steward(int ID, int goldCost, int foodCost, string location, enmPlayers player) 
            : base(ID, goldCost, foodCost, location, player)
        {
        }

        public override void inheritedSetUp()
        {
            Sprite = new AgentControl(owner, enmSpriteType.Steward);
            movement = Constants.stewardMovement;
        }

        public bool Works()
        {
            if (working == 0) return false;
            else return true;
        }
    }
}
