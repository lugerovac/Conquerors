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
    public class Musketeer : Unit
    {
        public Musketeer()
        {
            Agility = Constants.AgilityMusketeer;
            Vitality = Constants.VitalityMusketeer;
            Melee = Constants.MeleeMusketeer;
            RangedFarDistance = Constants.FarDistanceMusketeer;
            RangedCloseDistance = Constants.CloseDistanceMusketeer;
            Discipline = Constants.DisciplineMusketeer;
            GoldCost = Constants.GoldCostMusketeer;
            GoldUpkeep = Constants.GoldUpkeepMusketeer;
            FoodUpkeep = Constants.FoodUpkeepMusketeer;
        }
    }
}
