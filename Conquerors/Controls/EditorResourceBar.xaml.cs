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
    public partial class EditorResourceBar : UserControl
    {
        /*This control is used by the Editor component and allows the mapmaker to define the amount of resources that each
         player has at the start of the game*/

        public Player bluePlayer = new Player(enmPlayers.Blue);
        public Player redPlayer = new Player(enmPlayers.Red);
        public Player greenPlayer = new Player(enmPlayers.Green);
        public Player purplePlayer = new Player(enmPlayers.Purple);

        /*The enumeration SelectedPlayer keeps track of whose resoruces are being shown. Once the Editor component orders
         the change of who is the selected player, the necessary data is displayed*/
        private enmPlayers _selectedPlayer = enmPlayers.Blue;
        private enmPlayers SelectedPlayer
        {
            get { return _selectedPlayer; }
            set 
            { 
                _selectedPlayer = value;
                Style style = new Style();
                switch(_selectedPlayer)
                {
                    case enmPlayers.Blue:
                        style = stpResources.Resources["btnStyleBluePlayer"] as Style;
                        txtGold.Text = bluePlayer.Gold.ToString();
                        txtFood.Text = bluePlayer.Food.ToString();
                        txtStone.Text = bluePlayer.Stone.ToString();
                        break;
                    case enmPlayers.Red:
                        style = stpResources.Resources["btnStyleRedPlayer"] as Style;
                        txtGold.Text = redPlayer.Gold.ToString();
                        txtFood.Text = redPlayer.Food.ToString();
                        txtStone.Text = redPlayer.Stone.ToString();
                        break;
                    case enmPlayers.Green:
                        style = stpResources.Resources["btnStyleGreenPlayer"] as Style;
                        txtGold.Text = greenPlayer.Gold.ToString();
                        txtFood.Text = greenPlayer.Food.ToString();
                        txtStone.Text = greenPlayer.Stone.ToString();
                        break;
                    case enmPlayers.Purple:
                        style = stpResources.Resources["btnStylePurplePlayer"] as Style;
                        txtGold.Text = purplePlayer.Gold.ToString();
                        txtFood.Text = purplePlayer.Food.ToString();
                        txtStone.Text = purplePlayer.Stone.ToString();
                        break;
                }
                btnPlayer.Style = style;
            }
        }

        public EditorResourceBar()
        {
            InitializeComponent();
        }

        public void refresh()
        {
            SelectedPlayer = SelectedPlayer;
        }

        public void SelectPlayer(enmPlayers player)
        {
            if (player == enmPlayers.None) return;
            SelectedPlayer = player;
        }

        /*The following function changes who the selected player is*/
        private void btnPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPlayer == enmPlayers.Purple) SelectedPlayer = enmPlayers.Blue;
            else SelectedPlayer++;
        }

        /*The following function update the data whenever the mapmaker enters a number into a textbox. In case the symbol that
         was typed was not a number, the exception will be caught and rather than the data being updated, the SelectedPlayer
         value will be set back to the old one, automatically removing the unwanted symbol*/
        private void txtGold_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int newValue = Convert.ToInt32(txtGold.Text);
                switch (SelectedPlayer)
                {
                    case enmPlayers.Blue:
                        bluePlayer.Gold = newValue;
                        break;
                    case enmPlayers.Red:
                        redPlayer.Gold = newValue;
                        break;
                    case enmPlayers.Green:
                        greenPlayer.Gold = newValue;
                        break;
                    case enmPlayers.Purple:
                        purplePlayer.Gold = newValue;
                        break;
                }
            }
            catch
            {
                SelectedPlayer = SelectedPlayer;
            }
        }

        private void txtFood_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int newValue = Convert.ToInt32(txtFood.Text);
                switch (SelectedPlayer)
                {
                    case enmPlayers.Blue:
                        bluePlayer.Food = newValue;
                        break;
                    case enmPlayers.Red:
                        redPlayer.Food = newValue;
                        break;
                    case enmPlayers.Green:
                        greenPlayer.Food = newValue;
                        break;
                    case enmPlayers.Purple:
                        purplePlayer.Food = newValue;
                        break;
                }
            }
            catch
            {
                SelectedPlayer = SelectedPlayer;
            }
        }

        private void txtStone_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int newValue = Convert.ToInt32(txtStone.Text);
                switch (SelectedPlayer)
                {
                    case enmPlayers.Blue:
                        bluePlayer.Stone = newValue;
                        break;
                    case enmPlayers.Red:
                        redPlayer.Stone = newValue;
                        break;
                    case enmPlayers.Green:
                        greenPlayer.Stone = newValue;
                        break;
                    case enmPlayers.Purple:
                        purplePlayer.Stone = newValue;
                        break;
                }
            }
            catch
            {
                SelectedPlayer = SelectedPlayer;
            }
        }
    }
}
