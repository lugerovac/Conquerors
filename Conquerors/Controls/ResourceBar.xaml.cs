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
    public partial class ResourceBar : UserControl
    {
        /*This control displays the current amount of resources the active player has as well as the gain of each resource per turn*/
        public ResourceBar()
        {
            InitializeComponent();
        }

        private int _gold, _food, _stone, _morale;
        private int _goldGain, _foodGain, _stoneGain;

        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                txtGold.Text = " " + _gold;
            }
        }

        public int GoldGain
        {
            get { return _goldGain; }
            set
            {
                _goldGain = value;
                if(_goldGain >= 0) txtGoldGain.Text = " +" + _goldGain;
                else txtGoldGain.Text = " -" + Math.Abs(_goldGain);
            }
        }

        public int Food
        {
            get { return _food; }
            set
            {
                _food = value;
                txtFood.Text = " " + _food;
            }
        }

        public int FoodGain
        {
            get { return _foodGain; }
            set
            {
                _foodGain = value;
                if(_foodGain >= 0) txtFoodGain.Text = " +" + _foodGain;
                else txtFoodGain.Text = " -" + Math.Abs(_foodGain);
            }
        }

        public int Stone
        {
            get { return _stone; }
            set
            {
                _stone = value;
                txtStone.Text = " " + _stone;
            }
        }

        public int StoneGain
        {
            get { return _stoneGain; }
            set
            {
                _stoneGain = value;
                if(_stoneGain >= 0) txtStoneGain.Text = " +" + _stoneGain;
                else txtStoneGain.Text = " -" + Math.Abs(_stoneGain);
            }
        }

        public int Morale
        {
            get { return _morale; }
            set
            {
                _morale = value;
                txtMorale.Text = " " + _morale + "%";
            }
        }
    }
}
