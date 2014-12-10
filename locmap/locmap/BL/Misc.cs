using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;
using locmap.Resources;

namespace locmap.BL
{
    public static class Misc
    {
        public static readonly IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// Put progressbar visible
        /// </summary>
        /// <example>
        /// ShowProgress(this, "Fetching data");
        /// </example>
        /// <param name="page">Page where to put progressbar</param>
        /// <param name="msg">Message</param>
        public static void ShowProgress(System.Windows.DependencyObject page, String msg)
        {
            ProgressIndicator _progressIndicator = new ProgressIndicator();
            _progressIndicator.Text = msg;
            _progressIndicator.IsVisible = true;

            SystemTray.SetProgressIndicator(page, _progressIndicator);
        }

        /// <summary>
        /// Hide progressbar
        /// </summary>
        public static void HideProgress(System.Windows.DependencyObject page)
        {
            ProgressIndicator _progressIndicator = new ProgressIndicator();
            _progressIndicator.IsVisible = false;

            SystemTray.SetProgressIndicator(page, _progressIndicator);
        }


        /// <summary>
        /// Creates and shows toast to user
        /// </summary>
        /// <param name="title">Title for the toast</param>
        /// <param name="message">Toast message</param>
        public static void showToast(string title, string message)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Title = title;
            toast.Message = message;
            toast.Show();
        }


        /// <returns>Token, null if no token found</returns>
        public static string getToken()
        {
            return getSettingValue(AppResources.TokenKey);
        }

        /// <summary>
        /// Saves token
        /// </summary>
        /// <param name="token">This is saved to isolated storage</param>
        public static void saveToken(string token)
        {
            addSetting(AppResources.TokenKey, token);
        }


        /// <summary>
        /// Clears token from isolated storage
        /// </summary>
        public static void removeToken()
        {
            removeSetting(AppResources.TokenKey);
        }


        /// <summary>
        /// Adds key/value pair to isolated storage
        /// </summary>
        public static void addSetting(string key, string value)
        {
            appSettings.Add(key, value);
            appSettings.Save();
        }


        /// <summary>
        /// Clears given key/value pair from isolated storage
        /// </summary>
        /// <param name="key"></param>
        public static void removeSetting(string key)
        {
            appSettings.Remove(key);
            appSettings.Save();
        }

        /// <summary>
        /// Gets value from isolated storage
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Null if no value is found with given key. Value otherwise</returns>
        public static string getSettingValue(string key)
        {
            string value = "";
            if (appSettings.TryGetValue(key, out value))
                return value;
            else
                return null;
        }
    }
}
