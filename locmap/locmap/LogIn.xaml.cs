using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using locmap.Resources;

namespace locmap
{
    public partial class LogIn : PhoneApplicationPage
    {
        public LogIn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open Register -Page 
        /// </summary>
        private void txtLogInRegister_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Register.xaml", UriKind.Relative));    
        }

        /// <summary>
        /// Logs user in. Updates status accordingly 
        /// </summary>
        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string token;
            JObject jsonObject = new JObject();
            jsonObject["email"] = txtLogInEmail.Text;
            jsonObject["password"] = txtLogInPassword.Password;
            string json = jsonObject.ToString();

            HttpResponseMessage response = await BLL.Network.PostApi(AppResources.LogInUrl, json);

            string status = "";
            if (response == null)
            {
                status = AppResources.CheckInternet;
            }
            else if (response.IsSuccessStatusCode)
            {
                List<string> tokens = (List<string>)response.Headers.GetValues("x-access-token");
                if (tokens.Count == 1)
                {
                    token = tokens[0];
                    status = "Logged in";
                }
                else status = "Log in failed for a strange reason. Contact app administrator.";
                    
            }
            else
            {
                status = "Login failed. Check your username and password";
            }

            txtLogInStatus.Text = status;
        }
    }
}