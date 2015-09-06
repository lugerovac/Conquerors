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
    public class BattleTerrain : Battle
    {
        private BattleReport report;

        public void Initialize(Player p1, Player p2, Army p1Army, Army p2Army, Node location)
        {
            //TODO
        }

        public BattleReport GetBattleReport()
        {
            return null;
        }
        public Army ReturnArmyState(Player player)
        {
            return null;
        }

        public void LoadUnitStats()
        {

        }
        public void LoadNodeModifiers()
        {

        }
        public void LoadOtherModifiers()
        {

        }
        public void StartBattle()
        {
            /*
             Terrain Battle
             * Phase 1-2: Archer and Light Cavalry (represent horse archers) do damage on X units; Musketeers do Long Distance Damage
             * Phase 3: Archers and Musketeers do damage; Musketeers do extra damage
             * Phase 4: Infantry and Musketeers clash; Cavalry has X% chance to hit a regiment;
             * This Phase repeats until morale of one side crumbles
             * Phase 5: Crumbled Army starts to retreat; winning musketeers, archers and cavalry do damage to defeated enemy;
             * If their morale is above X, retreating Heavy Cavalry and Heavy Infantry might stand behind to protect the faster units
             * Phase 6: Light Cavalry and Archers do damage to retreating units that did not stay behind
             * Battle finishes when each defeated regiment had either retreated or been destroyed
             */
        }
        public void WriteIntoReport()
        {

        }
        public void EndBattle()
        {

        }
    }
}
