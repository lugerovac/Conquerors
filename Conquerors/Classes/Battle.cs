using System;
using System.Collections.Generic;
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
    public interface Battle
    {
        //BattleReport report;

        void Initialize(Player p1, Player p2, Army p1Army, Army p2Army, Node location);
        BattleReport GetBattleReport();
        Army ReturnArmyState(Player player);
        
        void LoadUnitStats();
        void LoadNodeModifiers();
        void LoadOtherModifiers();
        void StartBattle();
        void WriteIntoReport();
        void EndBattle();
    }
}
