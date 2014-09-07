using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;

namespace Boomers.Utilities
{
     public class Culture
     {
         /// <summary>
         /// Gets the Culture info of current Browser settings
         /// </summary>
         /// <returns></returns>
         public static CultureInfo ResolveCulture()
         {
             string[] languages = HttpContext.Current.Request.UserLanguages;
             if (languages == null || languages.Length == 0)
                 return null;
             try
             {
                 string language = languages[0].ToLowerInvariant().Trim();
                 return CultureInfo.CreateSpecificCulture(language);
             }
             catch (ArgumentException)
             { return null; }
         }
         /// <summary>
         /// Gets the region info of current culture settings.
         /// </summary>
         /// <returns></returns>
         public static RegionInfo ResolveCountry()
         {
             CultureInfo culture = ResolveCulture();
             if (culture != null)
                 return new RegionInfo(culture.LCID);
             return null;
         }
     }
}
