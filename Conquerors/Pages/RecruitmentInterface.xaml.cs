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
using System.Windows.Navigation;

namespace Conquerors.Pages{
    public partial class RecruitmentInterface : Page
    {
        public RecruitmentInterface()
        {
            InitializeComponent();
            setIcons();
            setInterface();
        }

        private void setInterface()
        {
            App app = (App)Application.Current;
            foreach (Army army in app.Recruitment_player.Armies)
            {
                if (string.Equals(app.Recruitment_node, army.location))
                {
                    ctrlGarrisonArchers.Quantity = army.Archers;
                    ctrlGarrisonLightInfantry.Quantity = army.LightInfantry;
                    ctrlGarrisonHeavyInfantry.Quantity = army.HeavyInfantry;
                    ctrlGarrisonLightCavalry.Quantity = army.LightCavalry;
                    ctrlGarrisonHeavyCavalry.Quantity = army.HeavyCavalry;
                    ctrlGarrisonMusketeers.Quantity = army.Musketeers;
                }
            }

            //TODO implement army reading
            ctrlArmyArchers.Quantity = 0;
            ctrlArmyLightInfantry.Quantity = 0;
            ctrlArmyHeavyInfantry.Quantity = 0;
            ctrlArmyLightCavalry.Quantity = 0;
            ctrlArmyHeavyCavalry.Quantity = 0;
            ctrlArmyMusketeers.Quantity = 0;

            showResources();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App app = (App)Application.Current;
            foreach (Army army in app.Recruitment_player.Armies)
            {
                if (string.Equals(app.Recruitment_node, army.location))
                {
                    army.Archers = ctrlGarrisonArchers.Quantity;
                    army.LightInfantry = ctrlGarrisonLightInfantry.Quantity;
                    army.HeavyInfantry = ctrlGarrisonHeavyInfantry.Quantity;
                    army.LightCavalry = ctrlGarrisonLightCavalry.Quantity;
                    army.HeavyCavalry = ctrlGarrisonHeavyCavalry.Quantity;
                    army.Musketeers = ctrlGarrisonMusketeers.Quantity;
                }
            }
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

        private void showResources()
        {
            calculateResourceGain();

            App app = (App)Application.Current;
            ctrlResources.Gold = app.Recruitment_player.Gold;
            ctrlResources.Stone = app.Recruitment_player.Stone;
            ctrlResources.Food = app.Recruitment_player.Food;
            ctrlResources.Morale = app.Recruitment_player.Morale;
        }

        private void calculateResourceGain()
        {

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setInterface();
        }
    }
}
