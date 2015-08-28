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
    public class Scout : Agent
    {
        public bool ignoreEnemies;

        public Scout(string ID, int goldCost, int foodCost, string location, enmPlayers player) 
            : base(ID, goldCost, foodCost, location, player)
        {
        }

        public override void inheritedSetUp()
        {
            sprite = new AgentControl(owner, enmAgentType.Scout);
            movement = Constants.scoutMovement;
            ignoreEnemies = false;
        }
    }
}
