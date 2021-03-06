﻿using System;
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
    public partial class HireSteward : UserControl
    {
        int GoldCost = Constants.stewardGoldCost;
        int GoldUpkeep = Constants.stewardGoldUpkeep;
        int FoodUpkeep = Constants.stewardFoodUpkeep;

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (Enabled) ControlRoot.Opacity = 1;
                else ControlRoot.Opacity = 0.5;
            }
        }

        public string RelatedNodeName;

        public HireSteward()
        {
            InitializeComponent();
            showCosts();
        }

        private void showCosts()
        {
            txtCostAndUpkeep.Text = GoldCost.ToString();
            if (GoldUpkeep != 0) txtCostAndUpkeep.Text += "/" + GoldUpkeep.ToString();
            txtFoodUpkeep.Text = FoodUpkeep.ToString();
            if (FoodUpkeep == 0)
            {
                txtFoodUpkeep.Visibility = Visibility.Collapsed;
                FoodIcon.Visibility = Visibility.Collapsed;
            }
        }
    }
}
