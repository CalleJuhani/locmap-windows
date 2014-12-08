using locmap.Resources;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace locmap.BL
{
    public static class Network
    {


        /// <summary>
        /// Sends HTTP GET request to API
        /// </summary>
        /// <param name="url">AppResources.BaseUrl + this url</param>
        /// <returns>HTTP response from server if connected to Internet. Null if not</returns>
        public static async Task<HttpResponseMessage> GetApi(string url)
        {
            // if no internet connection detected
            if (!isConnectedToNetwork()) return null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppResources.BaseUrl);
                    HttpResponseMessage response = await client.GetAsync(url);
                    return response;
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Error with http get to: " + url);
            }
            return null;
        }

        /// <summary>
        /// Sends POST request to locmap api 
        /// </summary>
        /// <param name="url">This URL get's appended to AppResources.BaseUrl</param>
        /// <param name="data">JSON data to send</param>
        /// <returns>HTTP Response from server if connected to internet. Null if exception occurs</returns>
        public static async Task<HttpResponseMessage> PostApi(string url, string data)
        {
            // if no internet connection detected
            if (!isConnectedToNetwork()) return null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppResources.BaseUrl);
                    string token = Misc.getToken();
                    if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    return response;
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("JSON Sent with the error: " + data);

            }
            // just in case
            return null;
        }

        public static bool isConnectedToNetwork()
        {
            var ni = NetworkInterface.NetworkInterfaceType;

            bool IsConnected = false;
            if ((ni == NetworkInterfaceType.Wireless80211) || (ni == NetworkInterfaceType.MobileBroadbandCdma) || (ni == NetworkInterfaceType.MobileBroadbandGsm))
                IsConnected = true;
            else if (ni == NetworkInterfaceType.None)
                IsConnected = false;
            return IsConnected;
        }

        /// <summary>
        /// Send multiform-data request to API
        /// </summary>
        /// <param name="url">Where to send post msg</param>
        /// <param name="bmp">BitmapImage</param>
        /// <param name="locID">Location id</param>
        /// <returns>HTTPResponseMessage</returns>
        public static async Task<HttpResponseMessage> PostImgApi(string url, System.Windows.Media.Imaging.BitmapImage bmp, string locID)
        {
            // if no internet connection detected
            if (!isConnectedToNetwork()) return null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppResources.BaseUrl);
                    string token = Misc.getToken();
                    if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    //HttpRequestMessage request = new HttpRequestMessage();

                    byte[] img = ImageToByte(bmp);

                    MultipartFormDataContent mfdc = new MultipartFormDataContent();
                    mfdc.Add(new StringContent(locID), "location");
                    mfdc.Add(new StreamContent(new MemoryStream(img)), "image", "tmp.jpg");
                    HttpResponseMessage response =  await client.PostAsync(url, mfdc);
                    return response;
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine("Post image error: " + ex.Message);
            }
            return null;
        }

        public static Byte[] ImageToByte(BitmapImage imageSource)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap btmMap = new WriteableBitmap
                    (imageSource.PixelWidth, imageSource.PixelHeight);

                // write an image into the stream
                Extensions.SaveJpeg(btmMap, ms,
                    imageSource.PixelWidth, imageSource.PixelHeight, 0, 100);

                return ms.ToArray();
            }
        }
    }
}
