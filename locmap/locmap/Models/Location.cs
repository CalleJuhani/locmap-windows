using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace locmap.Models
{
    /// <summary>
    /// Class hold info for single location
    /// </summary>
    public class Location : INotifyPropertyChanged
    {
        // Occurs when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;

        private float latitude;
        private float longitude;
        private string title;
        private string id;
        private string description;
        private string updated;
        private string created;
        private List<string> images;


        #region Constructors

        public Location()
        {
            this.latitude = 0f;
            this.longitude = 0f;
            this.title = "";
            this.id = null;
            this.description = "";
            this.updated = "";
            this.created = "";
            this.images = new List<string>();
        }

        #endregion

        /**
         * Get and set
         */
        #region GetSet
        public List<string> Images
        {
            get { return images; }
            set { images = value; }
        }


        public float Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                OnPropertyChanged("Latitude");
            }
        }


        public float Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                OnPropertyChanged("Longitude");
            }
        }


        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }


        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }


        public string Updated
        {
            get { return updated; }
            set { updated = value; }
        }


        public string Created
        {
            get { return created; }
            set { created = value; }
        }
        #endregion


        /// <summary>
        /// Adds image to current location
        /// </summary>
        /// <param name="img">URL of the image</param>
        public void addImage(string img)
        {
            images.Add(img);
        }

        /// <summary>
        /// Removes reference to image from this location
        /// </summary>
        /// <param name="img">Image to remove</param>
        public void removeImage(string img)
        {
            images.Remove(img);
        }


        /// <returns>Location as JSON</returns>
        public override string ToString()
        {
            JObject loc = new JObject();
            loc.Add("title", Title);
            loc.Add("description", Description);
            loc.Add("latitude", Latitude);
            loc.Add("longitude", Longitude);

            return loc.ToString();
        }

        /// <summary>
        /// Virtual method to call the Property Changed method
        /// </summary>
        /// <param name="propertyName">The name of the property which has changed.</param>
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }


}
