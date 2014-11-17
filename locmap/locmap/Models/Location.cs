using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            set { latitude = value; }
        }


        public float Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }


        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        public string Description
        {
            get { return description; }
            set { description = value; }
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
    }
}
