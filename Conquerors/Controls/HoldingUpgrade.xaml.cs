using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Conquerors
{
    public partial class HoldingUpgrade : UserControl
    {
        /*The following values define the maximum number of upgrades that each holding is allowed to have, 
         as well as the costs per level*/

        int maxMetropolisDefense = 7;
        int[] metropolisGoldCosts = { 10, 20, 30, 40, 50, 75, 100 };
        int[] metropolisStoneCosts = { 20, 30, 40, 50, 75, 100, 150 };
        int[] metropolisTimeCosts = { 3, 3, 5, 5, 10, 10, 20 };

        int maxCastleDefense = 5;
        int[] castleGoldCosts = { 10, 20, 30, 40, 50 };
        int[] castleStoneCosts = { 20, 30, 40, 50, 75 };
        int[] castleTimeCosts = { 3, 3, 5, 5, 10 };

        int maxCityDefense = 4;
        int[] cityGoldCosts = { 20, 35, 50, 65 };
        int[] cityStoneCosts = { 20, 25, 45, 65 };
        int[] cityTimeCosts = { 3, 3, 5, 5 };

        int maxChurchDefense = 3;
        int[] churchGoldCosts = { 10, 30, 50 };
        int[] churchStoneCosts = { 20, 30, 50 };
        int[] churchTimeCosts = { 3, 5, 10 };

        int maxVillageDefense = 2;
        int[] villageGoldCosts = { 10, 10 };
        int[] villageStoneCosts = { 20, 30 };
        int[] villageTimeCosts = { 3, 5 };

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (Enabled)
                    ControlRoot.Opacity = 1;
                else
                    ControlRoot.Opacity = 0.5;
            }
        }

        public HoldingUpgrade()
        {
            InitializeComponent();
        }

        /*In case the player selects a node that is being upgrades, the costs of upgrading must be hidden and only the remaining
         amount of time will be displayed*/
        public void showProgress(Steward steward)
        {
            Enabled = false;

            imgGold.Visibility = Visibility.Collapsed;
            txtGoldCost.Visibility = Visibility.Collapsed;
            imgStone.Visibility = Visibility.Collapsed;
            txtStoneCost.Visibility = Visibility.Collapsed;

            txtTime.Text = steward.working.ToString();
        }

        /*The following function checks if the holding can be further upgraded*/
        public bool checkUpgrades(Node node)
        {
            switch(node.NodeType)
            {
                case enmNodeType.Metropolis:
                    if (node.DefenseLevel == maxMetropolisDefense)
                        return false;
                    else
                        return true;
                case enmNodeType.Castle:
                    if (node.DefenseLevel == maxCastleDefense)
                        return false;
                    else
                        return true;
                case enmNodeType.City:
                    if (node.DefenseLevel == maxCityDefense)
                        return false;
                    else
                        return true;
                case enmNodeType.Church:
                    if (node.DefenseLevel == maxChurchDefense)
                        return false;
                    else
                        return true;
                case enmNodeType.Village:
                    if (node.DefenseLevel == maxVillageDefense)
                        return false;
                    else
                        return true;
            }
            return false;
        }

        /*The following function shows the costs necessary for an upgrade, disabled the ability to upgrade in case there
         is not enough gold or stone and, in case the holding is being upgraded, progress will be shown instead of costs*/
        public void show(Node node, Steward steward, Player player)
        {
            imgGold.Visibility = Visibility.Visible;
            txtGoldCost.Visibility = Visibility.Visible;
            imgStone.Visibility = Visibility.Visible;
            txtStoneCost.Visibility = Visibility.Visible;

            Enabled = true;

            if(steward.working != 0)
                showProgress(steward);
            else if (node.NodeType == enmNodeType.Metropolis)
            {
                txtGoldCost.Text = metropolisGoldCosts[node.DefenseLevel].ToString();
                txtStoneCost.Text = metropolisStoneCosts[node.DefenseLevel].ToString();
                txtTime.Text = metropolisTimeCosts[node.DefenseLevel].ToString();

                if (player.Gold < metropolisGoldCosts[node.DefenseLevel] || player.Stone < metropolisStoneCosts[node.DefenseLevel])
                    Enabled = false;
            }
            else if(node.NodeType == enmNodeType.Castle)
            {
                txtGoldCost.Text = castleGoldCosts[node.DefenseLevel].ToString();
                txtStoneCost.Text = castleStoneCosts[node.DefenseLevel].ToString();
                txtTime.Text = castleTimeCosts[node.DefenseLevel].ToString();

                if (player.Gold < castleGoldCosts[node.DefenseLevel] || player.Stone < castleStoneCosts[node.DefenseLevel])
                    Enabled = false;
            }
            else if (node.NodeType == enmNodeType.City)
            {
                txtGoldCost.Text = cityGoldCosts[node.DefenseLevel].ToString();
                txtStoneCost.Text = cityStoneCosts[node.DefenseLevel].ToString();
                txtTime.Text = cityTimeCosts[node.DefenseLevel].ToString();

                if (player.Gold < cityGoldCosts[node.DefenseLevel] || player.Stone < cityStoneCosts[node.DefenseLevel])
                    Enabled = false;
            }
            else if (node.NodeType == enmNodeType.Church)
            {
                txtGoldCost.Text = churchGoldCosts[node.DefenseLevel].ToString();
                txtStoneCost.Text = churchStoneCosts[node.DefenseLevel].ToString();
                txtTime.Text = churchTimeCosts[node.DefenseLevel].ToString();

                if (player.Gold < churchGoldCosts[node.DefenseLevel] || player.Stone < churchStoneCosts[node.DefenseLevel])
                    Enabled = false;
            }
            else if (node.NodeType == enmNodeType.Village)
            {
                txtGoldCost.Text = villageGoldCosts[node.DefenseLevel].ToString();
                txtStoneCost.Text = villageStoneCosts[node.DefenseLevel].ToString();
                txtTime.Text = villageTimeCosts[node.DefenseLevel].ToString();

                if (player.Gold < villageGoldCosts[node.DefenseLevel] || player.Stone < villageStoneCosts[node.DefenseLevel])
                    Enabled = false;
            }
        }

        public int getTime(Node node)
        {
            switch(node.NodeType)
            {
                case enmNodeType.Metropolis:
                    return metropolisTimeCosts[node.DefenseLevel];
                case enmNodeType.Castle:
                    return castleTimeCosts[node.DefenseLevel];
                case enmNodeType.City:
                    return cityTimeCosts[node.DefenseLevel];
                case enmNodeType.Church:
                    return churchTimeCosts[node.DefenseLevel];
                case enmNodeType.Village:
                    return villageTimeCosts[node.DefenseLevel];
            }
            return 0;
        }

        public int getGoldCosts(Node node)
        {
            switch (node.NodeType)
            {
                case enmNodeType.Metropolis:
                    return metropolisGoldCosts[node.DefenseLevel];
                case enmNodeType.Castle:
                    return castleGoldCosts[node.DefenseLevel];
                case enmNodeType.City:
                    return cityGoldCosts[node.DefenseLevel];
                case enmNodeType.Church:
                    return churchGoldCosts[node.DefenseLevel];
                case enmNodeType.Village:
                    return villageGoldCosts[node.DefenseLevel];
            }
            return 0;
        }

        public int getStoneCosts(Node node)
        {
            switch (node.NodeType)
            {
                case enmNodeType.Metropolis:
                    return metropolisStoneCosts[node.DefenseLevel];
                case enmNodeType.Castle:
                    return castleStoneCosts[node.DefenseLevel];
                case enmNodeType.City:
                    return cityStoneCosts[node.DefenseLevel];
                case enmNodeType.Church:
                    return churchStoneCosts[node.DefenseLevel];
                case enmNodeType.Village:
                    return villageStoneCosts[node.DefenseLevel];
            }
            return 0;
        }
    }
}
