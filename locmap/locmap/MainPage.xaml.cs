﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using locmap.Resources;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace locmap
{
    public partial class MainPage : PhoneApplicationPage
    {

        private Geolocator geolocator;
        private MapOverlay userMapOverlay;
        private MapLayer userMapLayer;
        private MapLayer locationsMapLayer;
        private List<Models.Location> locations;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            userMapOverlay = new MapOverlay();
            userMapLayer = new MapLayer();
            locationsMapLayer = new MapLayer();
            locations = new List<Models.Location>();
            fillLocationList();
        }

        /// <summary>
        /// Gets locations from API
        /// </summary>
        private async void fillLocationList()
        {
            BL.Misc.ShowProgress(this, "Fetching data");
            HttpResponseMessage response = await BL.Network.GetApi(AppResources.getLocationsUrl);
            string result = response.Content.ReadAsStringAsync().Result;
            JObject resBody = JObject.Parse(result);
            JArray locArray = JArray.Parse(resBody.GetValue("locations").ToString());

            locations = locArray.ToObject<List<Models.Location>>();
            getLocations();
            BL.Misc.HideProgress(this);
        }

        /// <summary>
        /// Asks user if location tracking is ok
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // change login/logout text in menu
            if (BL.Misc.getToken() != null)
            {
                ApplicationBarMenuItem logout = this.ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
                logout.Text = "log out";
            }

            string allowLocation = BL.Misc.getSettingValue(AppResources.LocationKey);
            
            // ask user if location tracking is ok
            if (allowLocation == null)
            {
                MessageBoxResult result =
                    MessageBox.Show("This app accesses your phone's location. Is that ok?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    BL.Misc.addSetting(AppResources.LocationKey, "true");
                }
                else
                {
                    BL.Misc.addSetting(AppResources.LocationKey, "false");
                }
            }

            // if user allows it, show his/her location on map by default
            if (Convert.ToBoolean(BL.Misc.getSettingValue(AppResources.LocationKey)) != true)
            {
                BL.Misc.showToast(AppResources.AppName, "Location traking is not enabled");
                return;
            }
            
            Ellipse userLocation = new Ellipse();
            userLocation.Fill = new SolidColorBrush(Colors.Yellow);
            userLocation.Stroke = new SolidColorBrush(Colors.Black);
            userLocation.StrokeThickness = 4;
            userLocation.Height = 20;
            userLocation.Width = 20;
            userLocation.Opacity = 50;

            userMapOverlay = new MapOverlay();
            userMapOverlay.Content = userLocation;
            userMapOverlay.PositionOrigin = new Point(0.5, 0.5);

            userMapLayer = new MapLayer();
            userMapLayer.Add(userMapOverlay);

            geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.MovementThreshold = 15; //meters
            geolocator.PositionChanged += geolocator_PositionChanged;
            
        }


        /// <summary>
        /// Fills maplayer with markers, uses locations list
        /// </summary>
        private void getLocations()
        {
            MapOverlay locOverlay;
            locationsMapLayer.Clear();
            foreach (Models.Location loc in locations)
            {
                Pushpin locationPin = new Pushpin();
                locationPin.Content = loc.Title;
                locationPin.GeoCoordinate = new System.Device.Location.GeoCoordinate((double)loc.Latitude, (double)loc.Longitude);
                locationPin.Visibility = System.Windows.Visibility.Visible;
                locationPin.Tag = loc.Id;
                locationPin.Tap += pinPushed;
                locOverlay = new MapOverlay();
                locOverlay.Content = locationPin;
                locOverlay.GeoCoordinate = new System.Device.Location.GeoCoordinate((double)loc.Latitude, (double)loc.Longitude);
                locationsMapLayer.Add(locOverlay);
            }
            mainPageMap.Layers.Add(locationsMapLayer);
        }

        /// <summary>
        /// Tap event for pushpin, opens ViewLocation for tapped location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pinPushed(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin locId = sender as Pushpin;
            NavigationService.Navigate(new Uri("/ViewLocation.xaml?id=" + locId.Tag, UriKind.Relative));
        }


        /// <summary>
        /// User has moved: changes map focus accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Position Changed");
            Dispatcher.BeginInvoke(() =>
            {
                mainPageMap.Center = new System.Device.Location.GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                mainPageMap.ZoomLevel = 14;
                userMapOverlay.GeoCoordinate = mainPageMap.Center;
                if (mainPageMap.Layers.Count() == 0)
                {
                    // Add the MapLayer to the Map.
                    mainPageMap.Layers.Add(userMapLayer);
                }
            });
        }

        /// <summary>
        /// Opens Log In Screen
        /// </summary>
        private void MenuLogIn_Click(object sender, EventArgs e)
        {
            if (BL.Misc.getToken() != null)
            {
                Logout();
            }
            else
            {
                NavigationService.Navigate(new Uri("/LogIn.xaml", UriKind.Relative));
            }
        }


        /// <summary>
        /// Logs out user. If network not available, only clears token from appsettings.
        /// </summary>
        private async void Logout()
        {
            HttpResponseMessage response = await BL.Network.PostApi(AppResources.LogOutUrl, "");
            BL.Misc.removeToken();
            BL.Misc.showToast(AppResources.AppName, "Logged out");
            ApplicationBarMenuItem login = this.ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            login.Text = "log in";
        }

        /// <summary>
        /// Opens Add Location screen
        /// </summary>
        private void MenuLocation_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/NewLocation.xaml", UriKind.Relative));
        }


        private void MenuRoute_Click(object sender, EventArgs e)
        {
            //
        }


    }
}