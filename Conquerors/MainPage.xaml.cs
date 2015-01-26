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
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            frameContainer.Navigate(new Uri("/Pages/MainMenuAlpha.xaml", UriKind.Relative));
        }

        /*In case there a page is not loaded, the following function will load an Error page*/
        private void frameContainer_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e) 
        { 
            e.Handled = true; 
            frameContainer.Navigate(new Uri("/Pages/ErrorPage.xaml", UriKind.Relative)); 
        }
    }
}
