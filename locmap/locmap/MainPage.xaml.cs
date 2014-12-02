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
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace locmap
{
    public partial class MainPage : PhoneApplicationPage
    {

        private Geolocator geolocator;
        private MapOverlay mapOverlay;
        private MapLayer mapLayer;

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

            // if user allowa it, show his/her location on map by default
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

            mapOverlay = new MapOverlay();
            mapOverlay.Content = userLocation;
            mapOverlay.PositionOrigin = new Point(0.5, 0.5);

            mapLayer = new MapLayer();
            mapLayer.Add(mapOverlay);

            geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.MovementThreshold = 15; //meters
            geolocator.PositionChanged += geolocator_PositionChanged;
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Position Changed");
            Dispatcher.BeginInvoke(() =>
            {
                mainPageMap.Center = new System.Device.Location.GeoCoordinate(args.Position.Coordinate.Latitude, args.Position.Coordinate.Longitude);
                mainPageMap.ZoomLevel = 14;
                mapOverlay.GeoCoordinate = mainPageMap.Center;
                if (mainPageMap.Layers.Count() == 0)
                {
                    // Add the MapLayer to the Map.
                    mainPageMap.Layers.Add(mapLayer);
                }
            });
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