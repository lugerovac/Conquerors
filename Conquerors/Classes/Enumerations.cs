using System;
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
    /*This is an empty class where enumerations are declared to make it easier to keep a track of them*/

    public enum enmSpriteType  //defines the type of the agent
    {
        Commander = 1,
        Steward = 2,
        Assassin = 3,
        Scout = 4
    }

    public enum enmPlayers  //defines the player and his color
    {
        None = 0,
        Blue = 1,
        Red = 2,
        Green = 3,
        Purple = 4
    }

    public enum enmNodeType  //defines the types of a node
    {
        Plain = 0,
        Metropolis = 1,
        Castle = 2,
        City = 3,
        Church = 4,
        Village = 5,
        Mine = 6,
        Forest = 7,
        Mountains = 8,
        Riverside = 9,
        MountainPass = 10
    } 

    public class Enumerations
    {
    }
}
