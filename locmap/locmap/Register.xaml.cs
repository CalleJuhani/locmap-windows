using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using locmap.Resources;
using System.Text.RegularExpressions;

namespace locmap
{
    public partial class Register : PhoneApplicationPage
    {
        public Register()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Registers user. Internet connection required
        /// </summary>
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            
            txtRegisterResponse.Text = "";
            // if email is invalid, return
            if (!IsValidEmail(txtRegisterEmail.Text))
            {
                txtRegisterResponse.Text = "Email is not valid";
                return;
            }

            string pwMsg = checkPassword(txtRegisterPassword.Password, txtRegisterPasswordConf.Password);
        
            if (pwMsg != null)
            {
                txtRegisterResponse.Text = pwMsg;
                return;
            }

            JObject jsonObject = new JObject();
            jsonObject["email"] = txtRegisterEmail.Text;
            jsonObject["username"] = txtRegisterUser.Text;
            jsonObject["password"] = txtRegisterPassword.Password;
            string json = jsonObject.ToString();
            
            HttpResponseMessage response = await BL.Network.PostApi(AppResources.RegisterUrl, json);
            string status = "";
            // check how request went and change status accordingly
            if (response == null)
            {
                status = AppResources.CheckInternet;
            }
            else if (response.IsSuccessStatusCode)
            {
                BL.Misc.showToast("locmap", "Registered succesfully!");
                
                NavigationService.Navigate(new Uri("/LogIn.xaml?email=" + txtRegisterEmail.Text, UriKind.Relative));
            } 
            else if ( ((int)response.StatusCode) == 400)
            {
                status = "Registeration failed. Try again with a different username and/or email";
            }
            else
            {
                status = AppResources.InternalProblems;
            }

            txtRegisterResponse.Text = status;
        }


        /// <summary>
        /// Check if password fills criteria.
        /// - At least 8 char long
        /// - At least 1 char A-Z and 1 number
        /// - Passwords match
        /// </summary>
        /// <param name="password">Password to check</param>
        /// <returns>Errormessage if password doesn't fill criteria. Null if password OK</returns>
        private string checkPassword(string password, string passworfConf)
        {
            string errMsg = null;
            if (password.Length < 8)
            {
                errMsg = "Password needs to be at least 8 characters long";
            }
            else if (Regex.IsMatch(password, ".*[A-Z].*") && Regex.IsMatch(password, ".*[0-9].*"))
            {
                errMsg = "Password needs to contain at least one number and one letter A-Z";
            }
            // check if pw:s match
            else if (!password.Equals(passworfConf))
            {
                errMsg = "Passwords don't match.";
            }
            return errMsg;
        }


        /// <summary>
        /// Just checks that theres @ and . somewhere
        /// Note that this is "invalid". But good enough as I don't want to make it too strict
        /// More on why this is not enough:
        /// http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx/
        /// </summary>
        bool IsValidEmail(string email)
        {
            return email.Contains('@') && email.Contains('.');
        }
    }
}