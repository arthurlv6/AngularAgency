using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Agency.DataAccess.Containers;
using Agency.DataAccess.ViewModels;

namespace Agency.DataAccess.Components
{
    public static class CacheHelper
    {
        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T o, string key) where T : class
        {
            // NOTE: Apply expiration parameters as you see fit.
            // I typically pull from configuration file.

            // In this example, I want an absolute
            // timeout so changes will always be reflected
            // at that time. Hence, the NoSlidingExpiration.
            HttpContext.Current.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddMinutes(1440),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(CacheType key)
        {
            string stringKey = Enum.GetName(typeof(CacheType), key);
            HttpContext.Current.Cache.Remove(stringKey);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                return null;
            }
        }

        public static List<T> GetCache<T>(CacheType key) where T : class
        {
            string stringKey = Enum.GetName(typeof(CacheType), key);
            var temp = Get<List<T>>(stringKey);
            if (temp == null)
            {
                temp = new SettingContainer().Get<T>(i => true).ToList();
                Add(temp, stringKey);
            }
            return temp;
        }
        public static CheckListViewModel CheckListViewModel { get; set; }
        public static ReceiptViewModel ReceiptViewModel { get; set; }
    }
    public enum CacheType
    {
        Company,
        VisaType,
        SchoolType,
        Hospital,
        Requirement
    }
}
