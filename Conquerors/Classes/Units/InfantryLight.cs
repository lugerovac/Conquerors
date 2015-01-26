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
    public class InfantryLight : Unit
    {
        public InfantryLight()
        {
            Agility = Constants.AgilityInfantryLight;
            Vitality = Constants.VitalityInfantryLight;
            Melee = Constants.MeleeInfantryLight;
            RangedFarDistance = Constants.FarDistanceInfantryLight;
            RangedCloseDistance = Constants.CloseDistanceInfantryLight;
            Discipline = Constants.DisciplineInfantryLight;
            GoldCost = Constants.GoldCostInfantryLight;
            GoldUpkeep = Constants.GoldUpkeepInfantryLight;
            FoodUpkeep = Constants.FoodUpkeepInfantryLight;
        }
    }
}
