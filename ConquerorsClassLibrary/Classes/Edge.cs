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
    public class Edge
    {
        /*This is a simple class which is used to define the graphical element of paths between nodes*/

        public Line connection;
        public string nodes;

        public Edge(double x1, double x2, double y1, double y2, string n1, string n2)
        {
            redefine(x1, x2, y1, y2);
            nodes = n1 + " : " + n2;
        }

        /*tThe following function sets up or changes the coordinates of the edge. This allows us to change only one edge when
         a node gets moved by the player, thus slightly improving the optimization*/
        public void redefine(double x1, double x2, double y1, double y2)
        {
            connection = new Line();
            connection.X1 = x1 - 55;
            connection.X2 = x2 - 55;
            connection.Y1 = y1 + 7;
            connection.Y2 = y2 + 7;
            connection.Stroke = new SolidColorBrush(Colors.White);
            connection.StrokeThickness = 15;
        }
    }
}
