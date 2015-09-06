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
    public class Constants
    {
        /*Resource Gains per holding*/
        public const int CityGoldGain = 60;
        public const int VillageFoodGain = 60;
        public const int MineStoneGain = 3;
        public const int ChurchMoraleGain = 5;

        /*Agent Deficits*/
        public const int commanderGoldCost = 20;
        public const int commanderGoldUpkeep = 2;
        public const int commanderFoodUpkeep = 0;

        public const int stewardGoldCost = 20;
        public const int stewardGoldUpkeep = 2;
        public const int stewardFoodUpkeep = 1;

        public const int assassinGoldCost = 20;
        public const int assassinGoldUpkeep = 2;
        public const int assassinFoodUpkeep = 0;

        public const int scoutGoldCost = 10;
        public const int scoutGoldUpkeep = 1;
        public const int scoutFoodUpkeep = 1;

        /*Opacity Values*/
        public const double darkenedNodeOpacity = 0.65;
        public const double darkenedSpriteOpacity = 0.5;

        /*Agent Movement Definitions*/
        public const int commanderMovement = 3;
        public const int stewardMovement = 2;
        public const int scoutMovement = 3;
        public const int assassinMovement = 2;

        /*Military Unit attributes*/
        public const int GoldCostInfantryLight = 3;
        public const int GoldCostInfantryHeavy = 7;
        public const int GoldCostCavalryLight = 5;
        public const int GoldCostCavalryHeavy = 10;
        public const int GoldCostBowmen = 4;
        public const int GoldCostMusketeer = 8;

        public const int GoldUpkeepInfantryLight = 0;
        public const int GoldUpkeepInfantryHeavy = 2;
        public const int GoldUpkeepCavalryLight = 1;
        public const int GoldUpkeepCavalryHeavy = 3;
        public const int GoldUpkeepBowmen = 0;
        public const int GoldUpkeepMusketeer = 3;

        public const int FoodUpkeepInfantryLight = 2;
        public const int FoodUpkeepInfantryHeavy = 1;
        public const int FoodUpkeepCavalryLight = 2;
        public const int FoodUpkeepCavalryHeavy = 1;
        public const int FoodUpkeepBowmen = 3;
        public const int FoodUpkeepMusketeer = 1;

        public const int AgilityInfantryLight = 2;
        public const int AgilityInfantryHeavy = 1;
        public const int AgilityCavalryLight = 3;
        public const int AgilityCavalryHeavy = 2;
        public const int AgilityBowmen = 2;
        public const int AgilityMusketeer = 1;

        public const double VitalityInfantryLight = 30;
        public const double VitalityInfantryHeavy = 40;
        public const double VitalityCavalryLight = 20;
        public const double VitalityCavalryHeavy = 40;
        public const double VitalityBowmen = 20;
        public const double VitalityMusketeer = 30;

        public const double MeleeInfantryLight = 30;
        public const double MeleeInfantryHeavy = 40;
        public const double MeleeCavalryLight = 30;
        public const double MeleeCavalryHeavy = 40;
        public const double MeleeBowmen = 10;
        public const double MeleeMusketeer = 30;

        public const double FarDistanceInfantryLight = 0;
        public const double FarDistanceInfantryHeavy = 0;
        public const double FarDistanceCavalryLight = 0;
        public const double FarDistanceCavalryHeavy = 0;
        public const double FarDistanceBowmen = 40;
        public const double FarDistanceMusketeer = 15;

        public const double CloseDistanceInfantryLight = 0;
        public const double CloseDistanceInfantryHeavy = 0;
        public const double CloseDistanceCavalryLight = 0;
        public const double CloseDistanceCavalryHeavy = 0;
        public const double CloseDistanceBowmen = 30;
        public const double CloseDistanceMusketeer = 45;

        public const double DisciplineInfantryLight = 0.25;
        public const double DisciplineInfantryHeavy = 0.75;
        public const double DisciplineCavalryLight = 0.5;
        public const double DisciplineCavalryHeavy = 0.75;
        public const double DisciplineBowmen = 0.25;
        public const double DisciplineMusketeer = 0.75;
    }
}
