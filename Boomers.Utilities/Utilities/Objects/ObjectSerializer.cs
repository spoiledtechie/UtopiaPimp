using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Objects
{
    public class ObjectSerializer
    {
        public static string toJson(object obj)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string sJSON = oSerializer.Serialize(obj);

                return sJSON;
            }
            catch { }
            return string.Empty;
        }
    }
}
