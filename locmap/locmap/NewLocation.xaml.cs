using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using locmap.Resources;
using System.Net.Http;
using Windows.Devices.Geolocation;
using Newtonsoft.Json.Linq;

namespace locmap
{
    public partial class NewLocation : PhoneApplicationPage
    {

        private CameraCaptureTask cameraTask;
        private Models.Location location;
        private BitmapImage bmp;

        public NewLocation()
        {
            InitializeComponent();

            // bind location
            location = new Models.Location();
            LayoutRoot.DataContext = location;

            cameraTask = new CameraCaptureTask();
            cameraTask.Completed += new EventHandler<PhotoResult>(cameraTask_Completed);
        }

        /// <summary>
        /// Put cameratask visible
        /// </summary>
        private void btnNewLocationCamera_Click(object sender, RoutedEventArgs e)
        {
            cameraTask.Show();
        }

        /// <summary>
        /// When returning from camera
        /// TODO: bind img to somewhere
        /// </summary>
        private void cameraTask_Completed(object sender, PhotoResult e)
        {
            BL.Misc.ShowProgress(this, "Working with image");
            // if photo taken
            if (e.TaskResult == TaskResult.OK)
            {

                bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                imgNewLocationPreview.Source = bmp;
                
                //var tempImg = new WriteableBitmap(bmp);
            }
            BL.Misc.HideProgress(this);

        }


        /// <summary>
        /// Creates new location.
        /// TODO: Add images also
        /// </summary>
        private async void btnNewLocationCreate_Click(object sender, RoutedEventArgs e)
        {
            BL.Misc.ShowProgress(this, "Creating");
            HttpResponseMessage response = await BL.Network.PostApi(AppResources.CreateLocation, location.ToString());
            string status = null;
            BL.Misc.HideProgress(this);

            if (response == null)
            {
                status = AppResources.CheckInternet;
            }
            else if (response.IsSuccessStatusCode)
            {
                string locationJson = response.Content.ReadAsStringAsync().Result;
                JObject locObject = JObject.Parse(locationJson);
                this.location = locObject.ToObject<Models.Location>();
                
                // send photo if necessary
                if (bmp != null)
                    sendImage(location.Id, bmp);
                else
                    status = "Location created!";
            }
            else
            {
                status = "Creation failed";
            }

            if (status != null)
            {
                BL.Misc.showToast(AppResources.AppName, status);
            }
        }

        private void sendImage(string id, BitmapImage bmp)
        {
            BL.Network.PostApi(AppResources.CreateLocation + "/" + id);
        }


        /// <summary>
        /// Get's users coordinates and fills latitude/longitude fields accordingly.
        /// Prompts user if location tracking is disabled in app or phone settings
        /// </summary>
        private async void btnNewLocationCoordinates_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(BL.Misc.getSettingValue(AppResources.LocationKey)) != true)
            {
                BL.Misc.showToast(AppResources.AppName, "Location traking is not enabled");
                return;
            }

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync( maximumAge: TimeSpan.FromMinutes(5),
                                                                                timeout: TimeSpan.FromSeconds(10));
                this.location.Latitude = (float)geoposition.Coordinate.Latitude;
                this.location.Longitude = (float)geoposition.Coordinate.Longitude;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    BL.Misc.showToast(AppResources.AppName, "Location is disabled in phone settings");
                }
                else
                { // dont know what has happened
                    BL.Misc.showToast(AppResources.AppName, "Cannot get location");
                }
            }

        }
    }
}