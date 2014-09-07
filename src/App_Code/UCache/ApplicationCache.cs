using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp;
using PimpLibrary.Utopia;

namespace Pimp.UCache
{
    /// <summary>
    /// Summary description for ApplicationCache
    /// </summary>
    public class ApplicationCache
    {
    


        /// <summary>
        /// gets the application settings
        /// </summary>
        /// <returns></returns>
        public static ApplicationSettings getApplicationSettings()
        {
            ApplicationSettings data = null;
            if (HttpRuntime.Cache["ApplicationSettings"] == null)
            {
                data = new ApplicationSettings();
                HttpRuntime.Cache.Add("ApplicationSettings", data, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                return data;
            }
            return (ApplicationSettings)HttpRuntime.Cache["ApplicationSettings"];
        }
    }
}