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
    public class Commander : Agent
    {
        public Army army = new Army();
        private bool _inBattle;
        public bool inBattle
        {
            get { return _inBattle; }
            set 
            {
                _inBattle = value;
                if (inBattle)
                {
                    moving = false;
                    movementRoute.Clear();
                }
            }
        }
        public string battling;

        public Commander(string ID, int goldCost, int foodCost, string location, enmPlayers player) 
            : base(ID, goldCost, foodCost, location, player)
        {
        }

        public override void inheritedSetUp()
        {
            sprite = new AgentControl(owner, enmAgentType.Commander);
            movement = Constants.commanderMovement;
            inBattle = false;
        }

        /// <summary>
        /// Adds one unit to the army
        /// </summary>
        /// <param name="unit">Type of the unit that is being added</param>
        public void addUnit(enmUnitType unit)
        {
            switch (unit)
            {
                case enmUnitType.Archers:
                    army.Archers++;
                    break;
                case enmUnitType.Musketeers:
                    army.Musketeers++;
                    break;
                case enmUnitType.LightInfantry:
                    army.LightInfantry++;
                    break;
                case enmUnitType.HeavyInfantry:
                    army.HeavyInfantry++;
                    break;
                case enmUnitType.LightCavalry:
                    army.LightCavalry++;
                    break;
                case enmUnitType.HeavyCavalry:
                    army.HeavyCavalry++;
                    break;
            }
        }

        /// <summary>
        /// Removes a unit from the army
        /// </summary>
        /// <param name="unit">Type of the unit that is being removed</param>
        public void removeUnit(enmUnitType unit)
        {
            switch(unit)
            {
                case enmUnitType.Archers:
                    if(army.Archers != 0) army.Archers--;
                    break;
                case enmUnitType.Musketeers:
                    if (army.Musketeers != 0) army.Musketeers--;
                    break;
                case enmUnitType.LightInfantry:
                    if (army.LightInfantry != 0) army.LightInfantry--;
                    break;
                case enmUnitType.HeavyInfantry:
                    if (army.HeavyInfantry != 0) army.HeavyInfantry--;
                    break;
                case enmUnitType.LightCavalry:
                    if (army.LightCavalry != 0) army.LightCavalry--;
                    break;
                case enmUnitType.HeavyCavalry:
                    if (army.HeavyCavalry != 0) army.HeavyCavalry--;
                    break;
            }
        }
    }
}
