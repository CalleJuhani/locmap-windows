using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace locmap.BLL
{
    public static class Progress
    {

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
    }
}
