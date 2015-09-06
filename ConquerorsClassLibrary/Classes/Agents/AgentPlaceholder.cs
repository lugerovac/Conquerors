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
    public class AgentPlaceholder : Agent
    {
        /*The class does not serve any other use but being a placeholder*/

        public override void inheritedSetUp()
        {
        }

        public AgentPlaceholder(string ID, int goldCost, int foodCost, string location, enmPlayers player) 
            : base(ID, goldCost, foodCost, location, player)
        {
        }
    }
}
