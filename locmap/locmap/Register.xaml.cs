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
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            txtRegisterResponse.Text = "";
            // if email is invalid, return
            if (!IsValidEmail(txtRegisterEmail.Text))
            {
                txtRegisterResponse.Text = "Email is not valid";
                return;
            }

            // password needs to be at least 4 charachters long
            if (txtRegisterPassword.Password.Length < 4)
            {
                txtRegisterResponse.Text = "Password needs to be at least 4 characters long";
            }


            // if passwords don't match, exit
            if (!txtRegisterPassword.Password.Equals(txtRegisterPasswordConf.Password))
            {
                txtRegisterResponse.Text = "Passwords don't match.";
                return;
            }
            JObject jsonObject = new JObject();
            jsonObject["email"] = txtRegisterEmail.Text;
            jsonObject["username"] = txtRegisterUser.Text;
            jsonObject["password"] = txtRegisterPassword.Password;
            string json = jsonObject.ToString();
            SendRegister(json);
        }

        /// <summary>
        /// Sends register request to API
        /// </summary>
        /// <param name="data">JSON</param>
        private async void SendRegister(string data)
        {
            string status = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppResources.BaseUrl);

                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(AppResources.RegisterUrl, content);

                    // throws exception if statuscode not 200-299 
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody.Contains("created"))
                        status = "Registered succesfully";
                    else
                        status = "Register failed. Try with a different username";
                    
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: "+ ex.Message);
                System.Diagnostics.Debug.WriteLine("JSON Sent with the error: " + data);
                status = "Register failed. Try again later.";

            }
            txtRegisterResponse.Text = status;
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