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
    public partial class NodeLevel : UserControl
    {
        /*This control is used by the Editor to allow the mapmaker to define the current upgrade level of a node*/

        private int _level;
        public int Level
        {
            get { return _level; }
            set 
            { 
                _level = value;
                txtNodeLevel.Text = Level.ToString();
            }
        }

        public NodeLevel()
        {
            InitializeComponent();
        }

        private void txtNodeLevel_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int newValue = Convert.ToInt32(txtNodeLevel.Text);
                Level = newValue;
            }catch
            {
                Level = Level;
            }
        }
    }
}
