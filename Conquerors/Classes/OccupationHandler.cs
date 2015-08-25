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
        List<string> blueOccupation;
        List<string> redOccupation;
        List<string> greenOccupation;
        List<string> purpleOccupation;

        public OccupationHandler()
        {
            blueOccupation = new List<string>();
            redOccupation = new List<string>();
            greenOccupation = new List<string>();
            purpleOccupation = new List<string>();
        }

        public void Add(string nodeName, enmPlayers playerColor)
        {
            switch (playerColor)
            {
                case enmPlayers.Blue:
                    blueOccupation.Add(nodeName);
                    break;
                case enmPlayers.Red:
                    redOccupation.Add(nodeName);
                    break;
                case enmPlayers.Green:
                    greenOccupation.Add(nodeName);
                    break;
                case enmPlayers.Purple:
                    purpleOccupation.Add(nodeName);
                    break;
            }
        }

        public bool collides(string nodeName, enmPlayers playerColor)
        {
            if (playerColor != enmPlayers.Blue && blueOccupation.Contains(nodeName))
                return true;
            if (playerColor != enmPlayers.Red && redOccupation.Contains(nodeName))
                return true;
            if (playerColor != enmPlayers.Green && greenOccupation.Contains(nodeName))
                return true;
            if (playerColor != enmPlayers.Purple && purpleOccupation.Contains(nodeName))
                return true;
            return false;
        }
    }
}
