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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Conquerors
{
    public partial class UnitSlot : UserControl
    {
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                txtNumberOfUnits.Text = Quantity.ToString();
            }
        }

        public UnitSlot()
        {
            InitializeComponent();
        }

        public void setImage(enmUnitType unitType)
        {
            switch(unitType)
            {
                case enmUnitType.LightInfantry:
                    imgUnit.Source = new BitmapImage(new Uri("UnitImages/Unit_LI.png", UriKind.RelativeOrAbsolute));
                    break;
                case enmUnitType.HeavyInfantry:
                    imgUnit.Source = new BitmapImage(new Uri("UnitImages/Unit_HI.png", UriKind.RelativeOrAbsolute));
                    break;
                case enmUnitType.LightCavalry:
                    imgUnit.Source = new BitmapImage(new Uri("UnitImages/Unit_LC.png", UriKind.RelativeOrAbsolute));
                    break;
                case enmUnitType.HeavyCavalry:
                    imgUnit.Source = new BitmapImage(new Uri("UnitImages/Unit_HC.png", UriKind.RelativeOrAbsolute));
                    break;
                case enmUnitType.Archers:
                    imgUnit.Source = new BitmapImage(new Uri("UnitImages/Unit_Ar.png", UriKind.RelativeOrAbsolute));
                    break;
                case enmUnitType.Musketeers:
                    imgUnit.Source = new BitmapImage(new Uri("UnitImages/Unit_Mu.png", UriKind.RelativeOrAbsolute));
                    break;
            }
        }
    }
}
