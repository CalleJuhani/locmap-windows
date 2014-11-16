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
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
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
            
            HttpResponseMessage response = await BLL.Network.PostApi(AppResources.RegisterUrl, json);
            string status = "";
            // check how request went and change status accordingly
            if (response == null)
            {
                status = "Problems with connecting to the Internet. Check your connection and try again";
            }
            else if (response.IsSuccessStatusCode)
            {
                status = "Registered successfully";
            } 
            // todo: check if statuscode is 500+ (internal server error etc)
            else
            {
                status = "Registeration failed. Try again with a different username and/or email";
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