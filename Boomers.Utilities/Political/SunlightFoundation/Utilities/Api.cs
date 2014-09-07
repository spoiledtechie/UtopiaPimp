using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Political.SunlightFoundation.Utilities
{
  public   class Api
    {
        private static string API_KEY = "ccaf7f09eafa45cfbdd5047e34ee7b10";

        public static string ApiKey
        {
            get { return API_KEY; }
            set { API_KEY = value; }
        }
    }
}
