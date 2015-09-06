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
    public class CollissionHandler
    {
        /*This class handles collissions, aka it serves to store data necessary for agent collission detections*/

        public enmPlayers owner;
        public string location;
        public string agentName;
        public enmAgentType agentType;
        public bool collided;
        public string collidedWith;

        public CollissionHandler()
        {
        }

        public CollissionHandler(Agent agent, enmAgentType type)
        {
            owner = agent.owner;
            location = agent.location;
            agentName = agent.ID;
            agentType = type;
            collided = false;
            collidedWith = "";
        }
    }
}
