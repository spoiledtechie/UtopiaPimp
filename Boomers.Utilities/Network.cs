using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ScottsFramework
{
    class Network
    {
        public string IPAddress()
        {
            string IPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (IPAddress == "" || IPAddress == null)
                IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return IPAddress;
        }
    }
}
