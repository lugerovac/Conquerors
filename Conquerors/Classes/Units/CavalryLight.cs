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
    public class CavalryLight : Unit
    {
        public CavalryLight()
        {
            Agility = Constants.AgilityCavalryLight;
            Vitality = Constants.VitalityCavalryLight;
            Melee = Constants.MeleeCavalryLight;
            RangedFarDistance = Constants.FarDistanceCavalryLight;
            RangedCloseDistance = Constants.CloseDistanceCavalryLight;
            Discipline = Constants.DisciplineCavalryLight;
            GoldCost = Constants.GoldCostCavalryLight;
            GoldUpkeep = Constants.GoldUpkeepCavalryLight;
            FoodUpkeep = Constants.FoodUpkeepCavalryLight;
        }
    }
}
