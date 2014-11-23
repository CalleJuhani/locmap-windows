﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace locmap
{
    public partial class NewLocation : PhoneApplicationPage
    {

        private CameraCaptureTask cameraTask;
        private Models.Location location;
        
        public NewLocation()
        {
            InitializeComponent();

            location = new Models.Location();
            LayoutRoot.DataContext = location;
            cameraTask = new CameraCaptureTask();
            cameraTask.Completed += new EventHandler<PhotoResult>(cameraTask_Completed);
        }

        /// <summary>
        /// Put cameratask visible
        /// </summary>
        private void btnNewLocationCamera_Click(object sender, RoutedEventArgs e)
        {
            cameraTask.Show();
        }

        /// <summary>
        /// When returning from camera
        /// TODO: bind img to somewhere
        /// </summary>
        private void cameraTask_Completed(object sender, PhotoResult e)
        {
            BL.Misc.ShowProgress(this, "Working with image");
            // if photo taken
            if (e.TaskResult == TaskResult.OK)
            {
                
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                imgNewLocationPreview.Source = bmp;
                //var tempImg = new WriteableBitmap(bmp);
            }
            BL.Misc.HideProgress(this);

        }

        private void btnNewLocationCreate_Click(object sender, RoutedEventArgs e)
        {
            BL.Misc.showToast("JSON", this.location.ToString());
        }
    }
}