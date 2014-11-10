using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace locmap
{
    public partial class LogIn : PhoneApplicationPage
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void txtLogInRegister_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Register.xaml", UriKind.Relative));    
        }
    }
}