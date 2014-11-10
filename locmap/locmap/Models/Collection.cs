using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace locmap.Models
{
    /// <summary>
    /// Collection of Locations
    /// </summary>
    public class Collection
    {
        private string id;
        private List<Location> locations;
        private string created;
        private string updated;
        private string title;
        private string description;

        /*
         * Get and Set
         */
        #region GetSet
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
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

        public List<Location> Locations
        {
            get { return locations; }
            set { locations = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        #endregion

        /// <summary>
        /// Adds location to this collection
        /// </summary>
        /// <param name="loc">Location to be added</param>
        private void addLocation(Location loc)
        {
            locations.Add(loc);
        }

        /// <summary>
        /// Removes location from this collection.
        /// Doesn't delete the location from database.
        /// </summary>
        /// <param name="loc">Location to be removed</param>
        private void removeLocation(Location loc)
        {
            locations.Remove(loc);
        }
    }
}
