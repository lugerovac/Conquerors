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
    public abstract class Unit
    {
        public int Agility;
        public double Vitality;
        public double Melee;
        public double RangedFarDistance;
        public double RangedCloseDistance;
        public double Discipline;
        public double Experience;

        public int GoldCost;
        public int GoldUpkeep;
        public int FoodUpkeep;
    }
}
