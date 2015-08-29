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
    public partial class ArmyInfo : UserControl
    {
        public ArmyInfo()
        {
            InitializeComponent();
        }

        public void showArmyInfo(Commander commander)
        {
            txtLIquantity.Text = commander.army.LightInfantry.ToString();
            txtHIquantity.Text = commander.army.HeavyInfantry.ToString();
            txtLCquantity.Text = commander.army.LightCavalry.ToString();
            txtHCquantity.Text = commander.army.HeavyCavalry.ToString();
            txtARquantity.Text = commander.army.Archers.ToString();
            txtMUquantity.Text = commander.army.Musketeers.ToString();
        }
    }
}
