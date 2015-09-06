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
    public class OccupationHandler
    {
        /*This class handles agent occupations of nodes. Alongside the CollissionHandler class, it serves the purpose of
         detecting collissions between agents*/

        public List<CollissionHandler> occupations;
        public int collissionsOccurred;

        public OccupationHandler()
        {
            occupations = new List<CollissionHandler>();
            collissionsOccurred = 0;
        }

        public void Add(Agent agent, enmAgentType type)
        {
            CollissionHandler addition = new CollissionHandler(agent, type);

            //check for collissions
            foreach (CollissionHandler occupation in occupations)
            {
                if (agent.owner == occupation.owner) continue;
                if(string.Equals(agent.location, occupation.location))
                {
                    occupation.collided = true;
                    occupation.collidedWith = agent.ID;
                    addition.collided = true;
                    addition.collidedWith = occupation.agentName;
                    collissionsOccurred++;
                    break;
                }
            }

            occupations.Add(addition);
        }
    }
}
