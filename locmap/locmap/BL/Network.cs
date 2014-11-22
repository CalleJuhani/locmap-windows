using locmap.Resources;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace locmap.BL
{
    public static class Network
    {
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
    }
}
