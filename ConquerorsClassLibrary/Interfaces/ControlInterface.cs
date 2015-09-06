using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Conquerors
{
    public interface ControlInterface
    {
        /// <summary>
        /// Defines the User Control of the module. The width of the control must be EXACTLY 80 while the Height must be between
        /// 25 and 100. The suggested background color is #FFA86767 while the suggested foreground color for fonts is "#FFF18E49".
        /// It is also suggested to add <Rectangle Width="80" Height="50" Stroke="Beige"/> to make a border around the control
        /// </summary>
        UserControl userControl {get; set;}

        /// <summary>
        /// User this to affect the player's Morale gain/lose per turn (you need to use arguments and a PreMerge or PostMerge operation to not make them permanent)
        /// </summary>
        int MoraleGainModifier { get; }

        /// <summary>
        /// User this to affect the player's gold gain/lose per turn (you need to use arguments and a PreMerge or PostMerge operation to not make them permanent)
        /// </summary>
        int GoldGainModifier { get; }

        /// <summary>
        /// User this to affect the player's Food gain/lose per turn (you need to use arguments and a PreMerge or PostMerge operation to not make them permanent)
        /// </summary>
        int FoodGainModifier { get; }

        /// <summary>
        /// User this to affect the player's Stone gain/lose per turn (you need to use arguments and a PreMerge or PostMerge operation to not make them permanent)
        /// </summary>
        int StoneGainModifier { get; }

        /// <summary>
        /// Use this to store arguments for merging proccess and later turns. ArgHandler is a simple class made of two strings:
        /// name of the argument and an argument. For the name of the argument it is suggested to use the module's name,
        /// color of the active player and then the actual name of the argument.
        /// </summary>
        List<ArgHandler> ListOfArguments { get; set; }

        /// <summary>
        /// Defines the map property of the current game, or more specifically the nodes. Changing the value here will affect 
        /// the nodes of the current player, but THOSE CHANGES WILL NOT BE SAVED WHEN THE TURN ENDS. You need to use the arguments
        /// and a PreMerge or PostMerge operation
        /// </summary>
        Map Map { get; set; }

        /// <summary>
        /// Gives you the list of all the players and their properties. It is suggested to not change values here because properties
        /// of the non-active players will not be saved after the turn ends (use arguments and the PreMerge or PostMerge operations)
        /// </summary>
        List<Player> Players { get; set; }

        /// <summary>
        /// Value of the Active Player. Most or all changes done here will be saved when the turn ends
        /// </summary>
        Player ActivePlayer { get; set; }

        /// <summary>
        /// Pointer at the Selected Node. Use if the control depend upon a certain selected node and to check availiability. Mind
        /// you that it is possible that the value is null so check properly with CheckAvailiability() operation
        /// </summary>
        Node SelectedNode { set; }

        /// <summary>
        /// Pointer at the Selected Agent. Use if the control depend upon a certain selected agent and to check availiability. Mind
        /// you that it is possible that the value is null so check properly with CheckAvailiability() operation
        /// </summary>
        Agent SelectedAgent { set; }

        /// <summary>
        /// Used by application to check if the module has been instanced. THE MODULE ITSELF SHOULD SET THIS TO FALSE
        /// IN A CONSTRUCTOR AND THEN IGNORE IT!
        /// </summary>
        bool instanced { get; set;}

        /// <summary>
        /// Called by the application to check when the control should show to the player. Use with SelectedNode, SelectedAgent
        /// and ActivePlayer
        /// </summary>
        /// <returns>True is the moduel should show or False if not</returns>
        bool CheckAvailiability();

        /// <summary>
        /// Connect this method with the MouseLeftButtonDown event. Do not use MouseLeftButtonup because it is used by the application
        /// to run its own method after Module's Run() which will check for Resource Gain Modifiers
        /// </summary>
        void Run();

        /// <summary>
        /// Called by the application to reset the control
        /// </summary>
        void ResetState();

        /// <summary>
        /// After the Save Merger uploads the map file and all the save files, it will call this operation for each Module. Use this to
        /// change the state of Maps and Players before the Save Merger starts handling the available data
        /// </summary>
        void PreMergeOperations();

        /// <summary>
        /// After Save Merger is done handling the data, it will call this function for each Module. Use this to change the state of the
        /// map and players just before a new map file is generated
        /// </summary>
        void PostMergeOperations();
    }
}
