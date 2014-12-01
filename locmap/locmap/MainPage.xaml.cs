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
        /// Asks user if location tracking is ok
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (BL.Network.appSettings.Contains("LocationConsent"))
            {
                // User has opted in or out of Location
                return;
            }
            else
            {
                MessageBoxResult result =
                    MessageBox.Show("This app accesses your phone's location. Is that ok?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    BL.Network.appSettings["LocationConsent"] = true;
                }
                else
                {
                    BL.Network.appSettings["LocationConsent"] = false;
                }

                BL.Network.appSettings.Save();
            }
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


        private void MenuViewLocation_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ViewLocation.xaml", UriKind.Relative));
        }
    }
}