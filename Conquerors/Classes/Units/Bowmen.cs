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
    public class Bowmen : Unit
    {
        public Bowmen()
        {
            Agility = Constants.AgilityBowmen;
            Vitality = Constants.VitalityBowmen;
            Melee = Constants.MeleeBowmen;
            RangedFarDistance = Constants.FarDistanceBowmen;
            RangedCloseDistance = Constants.CloseDistanceBowmen;
            Discipline = Constants.DisciplineBowmen;
            GoldCost = Constants.GoldCostBowmen;
            GoldUpkeep = Constants.GoldUpkeepBowmen;
            FoodUpkeep = Constants.FoodUpkeepBowmen;
        }
    }
}
