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
    public class CavalryHeavy : Unit
    {
        public CavalryHeavy()
        {
            Agility = Constants.AgilityCavalryHeavy;
            Vitality = Constants.VitalityCavalryHeavy;
            Melee = Constants.MeleeCavalryHeavy;
            RangedFarDistance = Constants.FarDistanceCavalryHeavy;
            RangedCloseDistance = Constants.CloseDistanceCavalryHeavy;
            Discipline = Constants.DisciplineCavalryHeavy;
            GoldCost = Constants.GoldCostCavalryHeavy;
            GoldUpkeep = Constants.GoldUpkeepCavalryHeavy;
            FoodUpkeep = Constants.FoodUpkeepCavalryHeavy;
        }
    }
}
