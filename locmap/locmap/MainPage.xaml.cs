using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using locmap.Resources;

namespace locmap
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens Log In Screen
        /// </summary>
        private void MenuLogIn_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LogIn.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Opens Add Location screen
        /// </summary>
        private void MenuLocation_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewLocation.xaml", UriKind.Relative));
        }

        /// <summary>
        /// TODO: Open add new route -screen
        /// </summary>
        private void MenuRoute_Click(object sender, EventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Register.xaml", UriKind.Relative));
        }

    }
}