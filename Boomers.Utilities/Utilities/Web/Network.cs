using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.NetworkInformation;

namespace Boomers.Utilities.Web
{
    public class Network
    {
        /// <summary>
        /// gets the mac address of the current computer.  It always returns the first nic card.
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                                return ni.GetPhysicalAddress().ToString();
            }
            return string.Empty;
            }

        public static string IPAddress
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        /// <summary>
        /// The current Server name
        /// </summary>
        /// <returns>returns only the server name.</returns>
        public static string ServerName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            }
        }
        /// <summary>
        /// checks if connection is SSL or not.
        /// </summary>
        public static bool IsSSL
        {
            get
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"] == "1")
                    return true;
                else
                    return false;
            }
        }
            
    }
}
