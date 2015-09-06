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
    public class BattleReport
    {
        enum BattleState
        {
            uninitialized,
            initialized,
            battling,
            over
        }

        Player player1;
        Player player2;
        Army p1Army;
        Army p2Army;
        Player winner;
        BattleState battleState;
        public List<string> report;

        public BattleReport(Player _player1, Player _player2, Army _p1Army, Army _p2Army)
        {
            player1 = _player1;
            player2 = _player2;
            p1Army = _p1Army;
            p2Army = _p2Army;
            battleState = BattleState.uninitialized;
            report = new List<string>();
        }

        public void Write(string reportElement)
        {
            report.Add(reportElement);
        }
    }
}
