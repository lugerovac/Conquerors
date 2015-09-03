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
        List<Node> ownedNodes = new List<Node>();
        bool garrisonArmyExists = false;
        bool visitingArmyExists = false;

        /// <summary>
        /// Initialization function that sets up the initial status of the interface
        /// </summary>
        public RecruitmentInterface()
        {
            InitializeComponent();
            setIcons();
            setInterface();
        }

        /// <summary>
        /// Sets the numerical values
        /// </summary>
        private void setInterface()
        {
            App app = (App)Application.Current;

            //Garrison set-up
            garrisonArmyExists = false;
            foreach (Army army in app.Recruitment_player.Armies)
            {
                if (string.Equals(app.Recruitment_node, army.location))
                {
                    garrisonArmyExists = true;
                    ctrlGarrisonArchers.Quantity = army.Archers;
                    ctrlGarrisonLightInfantry.Quantity = army.LightInfantry;
                    ctrlGarrisonHeavyInfantry.Quantity = army.HeavyInfantry;
                    ctrlGarrisonLightCavalry.Quantity = army.LightCavalry;
                    ctrlGarrisonHeavyCavalry.Quantity = army.HeavyCavalry;
                    ctrlGarrisonMusketeers.Quantity = army.Musketeers;
                }
            }

            if(!garrisonArmyExists)
            {
                ctrlGarrisonArchers.Quantity = 0;
                ctrlGarrisonLightInfantry.Quantity = 0;
                ctrlGarrisonHeavyInfantry.Quantity = 0;
                ctrlGarrisonLightCavalry.Quantity = 0;
                ctrlGarrisonHeavyCavalry.Quantity = 0;
                ctrlGarrisonMusketeers.Quantity = 0;
            }

            //Visiting Army set-up
            visitingArmyExists = false;
            foreach(Commander commander in app.Recruitment_player.Commanders)
            {
                if(string.Equals(commander.location, app.Recruitment_node))
                {
                    visitingArmyExists = true;
                    ctrlArmyArchers.Quantity = commander.army.Archers;
                    ctrlArmyLightInfantry.Quantity = commander.army.LightInfantry;
                    ctrlArmyHeavyInfantry.Quantity = commander.army.HeavyInfantry;
                    ctrlArmyLightCavalry.Quantity = commander.army.LightCavalry;
                    ctrlArmyHeavyCavalry.Quantity = commander.army.HeavyCavalry;
                    ctrlArmyMusketeers.Quantity = commander.army.Musketeers;

                    showVisitingArmyControls();
                }
            }

            if(!visitingArmyExists)
            {
                ctrlArmyArchers.Quantity = 0;
                ctrlArmyLightInfantry.Quantity = 0;
                ctrlArmyHeavyInfantry.Quantity = 0;
                ctrlArmyLightCavalry.Quantity = 0;
                ctrlArmyHeavyCavalry.Quantity = 0;
                ctrlArmyMusketeers.Quantity = 0;
                hideVisitingArmyControls();
            }

            showResources();
            showRecruitmentButtons();
        }

        private void showVisitingArmyControls()
        {
            ctrlArmyArchers.Visibility = Visibility.Visible;
            ctrlArmyLightInfantry.Visibility = Visibility.Visible;
            ctrlArmyHeavyInfantry.Visibility = Visibility.Visible;
            ctrlArmyLightCavalry.Visibility = Visibility.Visible;
            ctrlArmyHeavyCavalry.Visibility = Visibility.Visible;
            ctrlArmyMusketeers.Visibility = Visibility.Visible;

            btnArDown.Visibility = Visibility.Visible;
            btnArUp.Visibility = Visibility.Visible;
            btnLIDown.Visibility = Visibility.Visible;
            btnLIUp.Visibility = Visibility.Visible;
            btnHIDown.Visibility = Visibility.Visible;
            btnHIUp.Visibility = Visibility.Visible;
            btnLCDown.Visibility = Visibility.Visible;
            btnLCUp.Visibility = Visibility.Visible;
            btnHCDown.Visibility = Visibility.Visible;
            btnHCUp.Visibility = Visibility.Visible;
            btnMuDown.Visibility = Visibility.Visible;
            btnMuUp.Visibility = Visibility.Visible;
        }

        private void hideVisitingArmyControls()
        {
            ctrlArmyArchers.Visibility = Visibility.Collapsed;
            ctrlArmyLightInfantry.Visibility = Visibility.Collapsed;
            ctrlArmyHeavyInfantry.Visibility = Visibility.Collapsed;
            ctrlArmyLightCavalry.Visibility = Visibility.Collapsed;
            ctrlArmyHeavyCavalry.Visibility = Visibility.Collapsed;
            ctrlArmyMusketeers.Visibility = Visibility.Collapsed;

            btnArDown.Visibility = Visibility.Collapsed;
            btnArUp.Visibility = Visibility.Collapsed;
            btnLIDown.Visibility = Visibility.Collapsed;
            btnLIUp.Visibility = Visibility.Collapsed;
            btnHIDown.Visibility = Visibility.Collapsed;
            btnHIUp.Visibility = Visibility.Collapsed;
            btnLCDown.Visibility = Visibility.Collapsed;
            btnLCUp.Visibility = Visibility.Collapsed;
            btnHCDown.Visibility = Visibility.Collapsed;
            btnHCUp.Visibility = Visibility.Collapsed;
            btnMuDown.Visibility = Visibility.Collapsed;
            btnMuUp.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Enables or disables the ability to recruit or dismiss soldiers depending on resources and other criteria
        /// </summary>
        private void showRecruitmentButtons()
        {
            //Light Infantry
            if (ctrlResources.Gold < Constants.GoldCostInfantryLight
                || ctrlResources.GoldGain < Constants.GoldUpkeepInfantryLight
                || ctrlResources.FoodGain < Constants.FoodUpkeepInfantryLight)
            {
                btnAddLI.IsEnabled = false;
            }
            else
            {
                btnAddLI.IsEnabled = true;
            }

            if (ctrlGarrisonLightInfantry.Quantity == 0)
            {
                btnRemoveLI.IsEnabled = false;
                btnLIDown.IsEnabled = false;
            }
            else
            {
                btnRemoveLI.IsEnabled = true;
                btnLIDown.IsEnabled = true;
            }

            if(ctrlArmyLightInfantry.Quantity == 0)
            {
                btnLIUp.IsEnabled = false;
            }
            else
            {
                btnLIUp.IsEnabled = true;
            }

            //Heavy Infantry
            if (ctrlResources.Gold < Constants.GoldCostInfantryHeavy
                || ctrlResources.GoldGain < Constants.GoldUpkeepInfantryHeavy
                || ctrlResources.FoodGain < Constants.FoodUpkeepInfantryHeavy)
            {
                btnAddHI.IsEnabled = false;
            }
            else
            {
                btnAddHI.IsEnabled = true;
            }

            if (ctrlGarrisonHeavyInfantry.Quantity == 0)
            {
                btnRemoveHI.IsEnabled = false;
                btnHIDown.IsEnabled = false;
            }
            else
            {
                btnRemoveHI.IsEnabled = true;
                btnHIDown.IsEnabled = true;
            }

            if (ctrlArmyHeavyInfantry.Quantity == 0)
            {
                btnHIUp.IsEnabled = false;
            }
            else
            {
                btnHIUp.IsEnabled = true;
            }

            //Light Cavalry
            if (ctrlResources.Gold < Constants.GoldCostCavalryLight
                || ctrlResources.GoldGain < Constants.GoldUpkeepCavalryLight
                || ctrlResources.FoodGain < Constants.FoodUpkeepCavalryLight)
            {
                btnAddLC.IsEnabled = false;
            }
            else
            {
                btnAddLC.IsEnabled = true;
            }

            if (ctrlGarrisonLightCavalry.Quantity == 0)
            {
                btnRemoveLC.IsEnabled = false;
                btnLCDown.IsEnabled = false;
            }
            else
            {
                btnRemoveLC.IsEnabled = true;
                btnLCDown.IsEnabled = true;
            }

            if (ctrlArmyLightCavalry.Quantity == 0)
            {
                btnLCUp.IsEnabled = false;
            }
            else
            {
                btnLCUp.IsEnabled = true;
            }

            //Heavy Cavalry
            if (ctrlResources.Gold < Constants.GoldCostCavalryHeavy
                || ctrlResources.GoldGain < Constants.GoldUpkeepCavalryHeavy
                || ctrlResources.FoodGain < Constants.FoodUpkeepCavalryHeavy)
            {
                btnAddHC.IsEnabled = false;
            }
            else
            {
                btnAddHC.IsEnabled = true;
            }

            if (ctrlGarrisonHeavyCavalry.Quantity == 0)
            {
                btnRemoveHC.IsEnabled = false;
                btnHCDown.IsEnabled = false;
            }
            else
            {
                btnRemoveHC.IsEnabled = true;
                btnHCDown.IsEnabled = true;
            }

            if (ctrlArmyHeavyCavalry.Quantity == 0)
            {
                btnHCUp.IsEnabled = false;
            }
            else
            {
                btnHCUp.IsEnabled = true;
            }

            //Archers
            if (ctrlResources.Gold < Constants.GoldCostBowmen
                || ctrlResources.GoldGain < Constants.GoldUpkeepBowmen
                || ctrlResources.FoodGain < Constants.FoodUpkeepBowmen)
            {
                btnAddAR.IsEnabled = false;
            }
            else
            {
                btnAddAR.IsEnabled = true;
            }

            if (ctrlGarrisonArchers.Quantity == 0)
            {
                btnRemoveAR.IsEnabled = false;
                btnArDown.IsEnabled = false;
            }
            else
            {
                btnRemoveAR.IsEnabled = true;
                btnArDown.IsEnabled = true;
            }

            if (ctrlArmyArchers.Quantity == 0)
            {
                btnArUp.IsEnabled = false;
            }
            else
            {
                btnArUp.IsEnabled = true;
            }

            //Musketeers
            if (ctrlResources.Gold < Constants.GoldCostMusketeer
                || ctrlResources.GoldGain < Constants.GoldUpkeepMusketeer
                || ctrlResources.FoodGain < Constants.FoodUpkeepMusketeer)
            {
                btnAddMU.IsEnabled = false;
            }
            else
            {
                btnAddMU.IsEnabled = true;
            }

            if (ctrlGarrisonMusketeers.Quantity == 0)
            {
                btnRemoveMU.IsEnabled = false;
                btnMuDown.IsEnabled = false;
            }
            else
            {
                btnRemoveMU.IsEnabled = true;
                btnMuDown.IsEnabled = true;
            }

            if (ctrlArmyMusketeers.Quantity == 0)
            {
                btnMuUp.IsEnabled = false;
            }
            else
            {
                btnMuUp.IsEnabled = true;
            }
        }

        /// <summary>
        /// Function that gets called when the recruitment window closes
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Sets the look of the icons on the interface
        /// </summary>
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

        /// <summary>
        /// Function that shows initial resources
        /// </summary>
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
            App app = (App)Application.Current;
            Player ActivePlayer = app.Recruitment_player;
            ownedNodes = app.ActivePlayerNodes;
            ctrlResources.GoldGain = ctrlResources.FoodGain = ctrlResources.StoneGain = ctrlResources.Morale = 0;
            foreach (Node node in ownedNodes)
            {
                if (node.NodeType == enmNodeType.City)
                    ctrlResources.GoldGain += Constants.CityGoldGain;
                if (node.NodeType == enmNodeType.Mine)
                    ctrlResources.StoneGain += Constants.MineStoneGain;
                if (node.NodeType == enmNodeType.Village)
                    ctrlResources.FoodGain += Constants.VillageFoodGain;
                if (node.NodeType == enmNodeType.Metropolis)
                {
                    ctrlResources.GoldGain += Constants.CityGoldGain;
                    ctrlResources.StoneGain += Constants.MineStoneGain;
                    ctrlResources.FoodGain += Constants.VillageFoodGain;
                    ctrlResources.Morale = ActivePlayer.Morale;
                }  //if
            }  //foreach

            foreach(Army army in ActivePlayer.Armies)
            {
                ctrlResources.GoldGain -= (Constants.GoldUpkeepBowmen * army.Archers);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryHeavy * army.HeavyCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryLight * army.LightCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryHeavy * army.HeavyInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryLight * army.LightInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepMusketeer * army.Musketeers);

                ctrlResources.FoodGain -= (Constants.FoodUpkeepBowmen * army.Archers);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryHeavy * army.HeavyCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryLight * army.LightCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryHeavy * army.HeavyInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryLight * army.LightInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepMusketeer * army.Musketeers);
            }

            foreach (Commander c in ActivePlayer.Commanders)
            {
                ctrlResources.GoldGain -= (Constants.GoldUpkeepBowmen * c.army.Archers);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryHeavy * c.army.HeavyCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepCavalryLight * c.army.LightCavalry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryHeavy * c.army.HeavyInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepInfantryLight * c.army.LightInfantry);
                ctrlResources.GoldGain -= (Constants.GoldUpkeepMusketeer * c.army.Musketeers);

                ctrlResources.FoodGain -= (Constants.FoodUpkeepBowmen * c.army.Archers);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryHeavy * c.army.HeavyCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepCavalryLight * c.army.LightCavalry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryHeavy * c.army.HeavyInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepInfantryLight * c.army.LightInfantry);
                ctrlResources.FoodGain -= (Constants.FoodUpkeepMusketeer * c.army.Musketeers);
            }

            ctrlResources.GoldGain -= (ActivePlayer.Commanders.Count * Constants.commanderGoldUpkeep);
            ctrlResources.GoldGain -= (ActivePlayer.Stewards.Count * Constants.stewardGoldUpkeep);
            ctrlResources.GoldGain -= (ActivePlayer.Assassins.Count * Constants.assassinGoldUpkeep);
            ctrlResources.GoldGain -= (ActivePlayer.Scouts.Count * Constants.scoutGoldUpkeep);

            ctrlResources.FoodGain -= (ActivePlayer.Commanders.Count * Constants.commanderFoodUpkeep);
            ctrlResources.FoodGain -= (ActivePlayer.Stewards.Count * Constants.stewardFoodUpkeep);
            ctrlResources.FoodGain -= (ActivePlayer.Assassins.Count * Constants.assassinFoodUpkeep);
            ctrlResources.FoodGain -= (ActivePlayer.Scouts.Count * Constants.scoutFoodUpkeep);
        }

        /// <summary>
        /// What follows are several functions that allow the player to move soldiers between the garrison and the army
        /// </summary>
        private void moveUp(enmUnitType unitType)
        {
            addUnit(unitType, false);

            App app = (App)Application.Current;
            foreach(Commander commander in app.Recruitment_player.Commanders)
            {
                if(string.Equals(commander.location, app.Recruitment_node))
                {
                    commander.removeUnit(unitType);
                    break;
                }
            }
            setInterface();
        }

        private void moveDown(enmUnitType unitType)
        {
            removeUnit(unitType, false);

            App app = (App)Application.Current;
            foreach (Commander commander in app.Recruitment_player.Commanders)
            {
                if (string.Equals(commander.location, app.Recruitment_node))
                {
                    commander.addUnit(unitType);
                    break;
                }
            }
            setInterface();
        }

        private void btnLIUp_Click(object sender, RoutedEventArgs e)
        {
            if(ctrlArmyLightInfantry.Quantity > 0)
            {
                moveUp(enmUnitType.LightInfantry);
            }
        }

        private void btnLIDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonLightInfantry.Quantity > 0)
            {
                moveDown(enmUnitType.LightInfantry);
            }
        }

        private void btnHIUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyHeavyInfantry.Quantity > 0)
            {
                moveUp(enmUnitType.HeavyInfantry);
            }
        }

        private void btnHIDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonHeavyInfantry.Quantity > 0)
            {
                moveDown(enmUnitType.HeavyInfantry);
            }
        }

        private void btnLCUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyLightCavalry.Quantity > 0)
            {
                moveUp(enmUnitType.LightCavalry);
            }
        }

        private void btnLCDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonLightCavalry.Quantity > 0)
            {
                moveDown(enmUnitType.LightCavalry);
            }
        }

        private void btnHCUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyHeavyCavalry.Quantity > 0)
            {
                moveUp(enmUnitType.HeavyCavalry);
            }
        }

        private void btnHCDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonHeavyCavalry.Quantity > 0)
            {
                moveDown(enmUnitType.HeavyCavalry);
            }
        }

        private void btnArUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyArchers.Quantity > 0)
            {
                moveUp(enmUnitType.Archers);
            }
        }

        private void btnArDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonArchers.Quantity > 0)
            {
                moveDown(enmUnitType.Archers);
            }
        }

        private void btnMuUp_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlArmyMusketeers.Quantity > 0)
            {
                moveUp(enmUnitType.Musketeers);
            }
        }

        private void btnMuDown_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlGarrisonMusketeers.Quantity > 0)
            {
                moveDown(enmUnitType.Musketeers);
            }
        }

        private void addUnit(enmUnitType unitType, bool hiring)
        {
            App app = (App)Application.Current;
            Player ActivePlayer = app.Recruitment_player;

            if (!garrisonArmyExists)
            {
                Army newArmy = new Army();
                newArmy.location = app.Recruitment_node;
                ActivePlayer.Armies.Add(newArmy);
                garrisonArmyExists = true;
            }

            foreach(Army army in ActivePlayer.Armies)
            {
                if (string.Equals(army.location, app.Recruitment_node))
                {
                    switch(unitType)
                    {
                        case enmUnitType.Archers:
                            army.Archers++;
                            if(hiring)
                            {
                                ActivePlayer.Gold -= Constants.GoldCostBowmen;
                            }
                            break;
                        case enmUnitType.HeavyCavalry:
                            army.HeavyCavalry++;
                            if (hiring)
                            {
                                ActivePlayer.Gold -= Constants.GoldCostCavalryHeavy;
                            }
                            break;
                        case enmUnitType.LightCavalry:
                            army.LightCavalry++;
                            if (hiring)
                            {
                                ActivePlayer.Gold -= Constants.GoldCostCavalryLight;
                            }
                            break;
                        case enmUnitType.HeavyInfantry:
                            army.HeavyInfantry++;
                            if (hiring)
                            {
                                ActivePlayer.Gold -= Constants.GoldCostInfantryHeavy;
                            }
                            break;
                        case enmUnitType.LightInfantry:
                            army.LightInfantry++;
                            if (hiring)
                            {
                                ActivePlayer.Gold -= Constants.GoldCostInfantryLight;
                            }
                            break;
                        case enmUnitType.Musketeers:
                            army.Musketeers++;
                            if (hiring)
                            {
                                ActivePlayer.Gold -= Constants.GoldCostMusketeer;
                            }
                            break;
                    } //switch
                    break;
                } //if
            }//foreach

            if (hiring) setInterface();
        }

        private void btnAddLI_Click(object sender, RoutedEventArgs e)
        {
            addUnit(enmUnitType.LightInfantry, true);
        }

        private void btnAddHI_Click(object sender, RoutedEventArgs e)
        {
            addUnit(enmUnitType.HeavyInfantry, true);
        }

        private void btnAddLC_Click(object sender, RoutedEventArgs e)
        {
            addUnit(enmUnitType.LightCavalry, true);
        }

        private void btnAddHC_Click(object sender, RoutedEventArgs e)
        {
            addUnit(enmUnitType.HeavyCavalry, true);
        }

        private void btnAddAR_Click(object sender, RoutedEventArgs e)
        {
            addUnit(enmUnitType.Archers, true);
        }

        private void btnAddMU_Click(object sender, RoutedEventArgs e)
        {
            addUnit(enmUnitType.Musketeers, true);
        }

        private void removeUnit(enmUnitType unitType, bool resetInterface)
        {
            App app = (App)Application.Current;
            Player ActivePlayer = app.Recruitment_player;

            bool removeArmy = false;
            Army armyToRemove = new Army();  //placeholder
            foreach (Army army in ActivePlayer.Armies)
            {
                if (string.Equals(army.location, app.Recruitment_node))
                {
                    switch (unitType)
                    {
                        case enmUnitType.Archers:
                            army.Archers--;
                            break;
                        case enmUnitType.HeavyCavalry:
                            army.HeavyCavalry--;
                            break;
                        case enmUnitType.LightCavalry:
                            army.LightCavalry--;
                            break;
                        case enmUnitType.HeavyInfantry:
                            army.HeavyInfantry--;
                            break;
                        case enmUnitType.LightInfantry:
                            army.LightInfantry--;
                            break;
                        case enmUnitType.Musketeers:
                            army.Musketeers--;
                            break;
                    } //switch

                    if (army.isEmpty())
                    {
                        removeArmy = true;
                        armyToRemove = army;
                    }
                    break;
                } //if
            }//foreach

            if(removeArmy)
                ActivePlayer.Armies.Remove(armyToRemove);

            if (resetInterface) setInterface();
        }

        private void btnRemoveLI_Click(object sender, RoutedEventArgs e)
        {
            removeUnit(enmUnitType.LightInfantry, true);
        }

        private void btnRemoveHI_Click(object sender, RoutedEventArgs e)
        {
            removeUnit(enmUnitType.HeavyInfantry, true);
        }

        private void btnRemoveLC_Click(object sender, RoutedEventArgs e)
        {
            removeUnit(enmUnitType.LightCavalry, true);
        }

        private void btnRemoveHC_Click(object sender, RoutedEventArgs e)
        {
            removeUnit(enmUnitType.HeavyCavalry, true);
        }

        private void btnRemoveAR_Click(object sender, RoutedEventArgs e)
        {
            removeUnit(enmUnitType.Archers, true);
        }

        private void btnRemoveMU_Click(object sender, RoutedEventArgs e)
        {
            removeUnit(enmUnitType.Musketeers, true);
        }
    }
}
