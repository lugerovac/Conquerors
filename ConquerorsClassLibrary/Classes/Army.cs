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
    public class Army
    {
        public string location;

        public int LightInfantry = 0;
        public int HeavyInfantry = 0;
        public int LightCavalry = 0;
        public int HeavyCavalry = 0;
        public int Archers = 0;
        public int Musketeers = 0;

        public bool isEmpty()
        {
            if (LightInfantry == 0 && HeavyInfantry == 0 && LightCavalry == 0 && HeavyCavalry == 0 && Archers == 0 && Musketeers == 0)
                return true;
            return false;
        }
    }
}
