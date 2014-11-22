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

        /// <summary>
        /// Set values to email and password -fields if necessary
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            String email = "";
            // check if user came from register -page
            if (NavigationContext.QueryString.TryGetValue("email", out email))
            {
                txtLogInEmail.Text = email;
                txtLogInPassword.Focus();
                return;
            }

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
            BL.Misc.ShowProgress(this, "Logging in");

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

            HttpResponseMessage response = await BL.Network.PostApi(AppResources.LogInUrl, json);

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
                    appSettings.Remove(AppResources.TokenKey);
                    appSettings.Add(AppResources.TokenKey, tokens[0].ToString());
                    BL.Misc.showToast("locmap", "Logged in!");
                    // TODO: Navigate to somewhere perhaps?
                }
                else status = "Log in failed for a strange reason. Try again later.";
                    
            }
            else
            {
                status = "Login failed. Check your username and password";
            }
            txtLogInStatus.Text = status;
            BL.Misc.HideProgress(this);
        }

        /// <summary>
        /// Erases email and password from isolated storage when checkbox is unchecked
        /// </summary>
        private void checkRemember_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!(bool)checkRemember.IsChecked)
            {
                appSettings.Remove(EmailKey);
                appSettings.Remove(PasswordKey);
            }
        }

    }
}