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

namespace Conquerors.Pages
{
    public partial class RecruitmentInterface : UserControl
    {
        public RecruitmentInterface()
        {
            InitializeComponent();
            setIcons();

            ctrlArmyArchers.Quantity = 5;
            ctrlArmyHeavyCavalry.Quantity = 6;
            ctrlArmyHeavyInfantry.Quantity = 8;
            ctrlArmyLightCavalry.Quantity = 2;
            ctrlArmyLightInfantry.Quantity = 0;
            ctrlArmyMusketeers.Quantity = 10;

            ctrlGarrisonArchers.Quantity = 20;
            ctrlGarrisonHeavyCavalry.Quantity = 0;
            ctrlGarrisonHeavyInfantry.Quantity = 5;
            ctrlGarrisonLightCavalry.Quantity = 1;
            ctrlGarrisonLightInfantry.Quantity = 12;
            ctrlGarrisonMusketeers.Quantity = 0;
        }

        private void setIcons()
        {
            ctrlArmyLightInfantry.setImage(enmUnitType.LightInfantry);
            ctrlGarrisonLightInfantry.setImage(enmUnitType.LightInfantry);

            ctrlArmyHeavyInfantry.setImage(enmUnitType.HeavyInfantry);
            ctrlGarrisonHeavyInfantry.setImage(enmUnitType.HeavyInfantry);

            ctrlArmyLightCavalry.setImage(enmUnitType.LightCavalry);
            ctrlGarrisonLightCavalry.setImage(enmUnitType.LightCavalry);

            ctrlArmyHeavyCavalry.setImage(enmUnitType.HeavyCavalry);
            ctrlGarrisonHeavyCavalry.setImage(enmUnitType.HeavyCavalry);

            ctrlArmyArchers.setImage(enmUnitType.Archers);
            ctrlGarrisonArchers.setImage(enmUnitType.Archers);

            ctrlArmyMusketeers.setImage(enmUnitType.Musketeers);
            ctrlGarrisonMusketeers.setImage(enmUnitType.Musketeers);
        }

        private void btnLIUp_Click(object sender, RoutedEventArgs e)
        {
            if(ctrlArmyLightInfantry.Quantity > 0)
            {
                ctrlArmyLightInfantry.Quantity--;
                ctrlGarrisonLightInfantry.Quantity++;
            }
        }

        private void btnLIDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonLightInfantry.Quantity > 0)
            {
                ctrlGarrisonLightInfantry.Quantity--;
                ctrlArmyLightInfantry.Quantity++;
            }
        }

        private void btnHIUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyHeavyInfantry.Quantity > 0)
            {
                ctrlArmyHeavyInfantry.Quantity--;
                ctrlGarrisonHeavyInfantry.Quantity++;
            }
        }

        private void btnHIDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonHeavyInfantry.Quantity > 0)
            {
                ctrlGarrisonHeavyInfantry.Quantity--;
                ctrlArmyHeavyInfantry.Quantity++;
            }
        }

        private void btnLCUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyLightCavalry.Quantity > 0)
            {
                ctrlArmyLightCavalry.Quantity--;
                ctrlGarrisonLightCavalry.Quantity++;
            }
        }

        private void btnLCDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonLightCavalry.Quantity > 0)
            {
                ctrlGarrisonLightCavalry.Quantity--;
                ctrlArmyLightCavalry.Quantity++;
            }
        }

        private void btnHCUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyHeavyCavalry.Quantity > 0)
            {
                ctrlArmyHeavyCavalry.Quantity--;
                ctrlGarrisonHeavyCavalry.Quantity++;
            }
        }

        private void btnHCDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonHeavyCavalry.Quantity > 0)
            {
                ctrlGarrisonHeavyCavalry.Quantity--;
                ctrlArmyHeavyCavalry.Quantity++;
            }
        }

        private void btnArUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyArchers.Quantity > 0)
            {
                ctrlArmyArchers.Quantity--;
                ctrlGarrisonArchers.Quantity++;
            }
        }

        private void btnArDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonArchers.Quantity > 0)
            {
                ctrlGarrisonArchers.Quantity--;
                ctrlArmyArchers.Quantity++;
            }
        }

        private void btnMuUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyMusketeers.Quantity > 0)
            {
                ctrlArmyMusketeers.Quantity--;
                ctrlGarrisonMusketeers.Quantity++;
            }
        }

        private void btnMuDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonMusketeers.Quantity > 0)
            {
                ctrlGarrisonMusketeers.Quantity--;
                ctrlArmyMusketeers.Quantity++;
            }
        }
    }
}
;