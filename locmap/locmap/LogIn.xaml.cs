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
using System.IO.IsolatedStorage;

namespace locmap
{

    public partial class LogIn : PhoneApplicationPage
    {
        
        private IsolatedStorageSettings appSettings;
        private const string EmailKey = "locmap_email";
        private const string PasswordKey = "locmap_password";

        public LogIn()
        {
            InitializeComponent();
            appSettings = IsolatedStorageSettings.ApplicationSettings;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // get email from storage
            if (appSettings.Contains(EmailKey))
            {
                txtLogInEmail.Text = (string)appSettings[EmailKey];
                checkRemember.IsChecked = true;
            }
            else txtLogInEmail.Text = "";

            // get pw from storage
            if (appSettings.Contains(PasswordKey))
            {
                txtLogInPassword.Password = (string)appSettings[PasswordKey];
                checkRemember.IsChecked = true;
            }
            else txtLogInPassword.Password = "";
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
            BLL.Progress.ShowProgress(this, "Logging in");

            appSettings.Remove(EmailKey);
            appSettings.Remove(PasswordKey);

            JObject jsonObject = new JObject();
            jsonObject["email"] = txtLogInEmail.Text;
            jsonObject["password"] = txtLogInPassword.Password;
            string json = jsonObject.ToString();

            if ((bool)checkRemember.IsChecked)
            {
                appSettings.Add(EmailKey, txtLogInEmail.Text);
                appSettings.Add(PasswordKey, txtLogInPassword.Password);
            }

            HttpResponseMessage response = await BLL.Network.PostApi(AppResources.LogInUrl, json);

            string status = "";
            if (response == null)
            {
                status = AppResources.CheckInternet;
            }
            else if (response.IsSuccessStatusCode)
            {
                List<string> tokens = response.Headers.GetValues("x-access-token").ToList();
                if (tokens.Count == 1)
                {
                    appSettings.Add(AppResources.TokenKey, tokens[0].ToString());
                    status = "Logged in";
                }
                else status = "Log in failed for a strange reason. Contact app administrator.";
                    
            }
            else
            {
                status = "Login failed. Check your username and password";
            }

            txtLogInStatus.Text = status;
            BLL.Progress.HideProgress(this);
        }

        /// <summary>
        /// Erases email and password from isolated storage when checkbox is unchecked
        /// </summary>
        private void checkRemember_Checked(object sender, RoutedEventArgs e)
        {
            if (!(bool)checkRemember.IsChecked)
            {
                appSettings.Remove(EmailKey);
                appSettings.Remove(PasswordKey);
            }
        }
    }
}