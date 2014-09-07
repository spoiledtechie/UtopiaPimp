using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Boomers.Utilities.Web
{
  public   class CacheUtil
    {

        public static void getApplicationCache()
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(HttpRuntime.Cache);

            HttpContext.Current.Response.ContentType = "Application/json";
            //Get the physical path to the file.
            //Write the file directly to the HTTP content output stream.
            HttpContext.Current.Response.Write(sJSON);
            HttpContext.Current.Response.End();
        }
    }
}
