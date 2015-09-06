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

namespace ControlInterface
{
    public interface ControlInterface
    {
        ControlInterfaceHost Host { get; set; }

        string Name { get; }
        string Description { get; }
        string Author { get; }
        string Version { get; }

        Canvas UserControl { get; }

        void Initialize();
        void Dispose();
    }

    public interface ControlInterfaceHost
    {

    }
}
