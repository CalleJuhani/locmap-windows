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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;

namespace locmap
{
    public partial class ViewLocation : PhoneApplicationPage
    {
        Models.Location location;

        public ViewLocation()
        {
            InitializeComponent();
            this.location = new Models.Location();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string locationId;
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.TryGetValue("id", out locationId))
            {
                HttpResponseMessage response = await BL.Network.GetApi(AppResources.getLocationsUrl + "/" + locationId);
                string result = response.Content.ReadAsStringAsync().Result;
                JObject resBody = JObject.Parse(result);
                this.location = resBody.ToObject<Models.Location>();

                txtViewLocationTitle.Text = location.Title;
                txtViewLocationDescription.Text = location.Description;

                if (this.location.Images != null)
                {
                    if (this.location.Images.Count > 0)
                        loadImages(location.Images);
                }
            }
        }

        private void loadImages(List<string> images)
        {
            foreach (string image in images)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(AppResources.BaseUrl + AppResources.PostImage + "/" + image));
                spImageViewer.Children.Add(img);
            }
        }
    }
}