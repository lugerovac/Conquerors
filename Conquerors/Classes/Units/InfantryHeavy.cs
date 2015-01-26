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
    public class InfantryHeavy : Unit
    {
        public InfantryHeavy()
        {
            Agility = Constants.AgilityInfantryHeavy;
            Vitality = Constants.VitalityInfantryHeavy;
            Melee = Constants.MeleeInfantryHeavy;
            RangedFarDistance = Constants.FarDistanceInfantryHeavy;
            RangedCloseDistance = Constants.CloseDistanceInfantryHeavy;
            Discipline = Constants.DisciplineInfantryHeavy;
            GoldCost = Constants.GoldCostInfantryHeavy;
            GoldUpkeep = Constants.GoldUpkeepInfantryHeavy;
            FoodUpkeep = Constants.FoodUpkeepInfantryHeavy;
        }
    }
}
