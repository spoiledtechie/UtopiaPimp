using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.HtmlControls;

namespace SupportFramework.StaticContent
{
    public static  class Versioned
    {
       static  string _LastBuild = null;
        public static string LastBuild
        {
            get
            {
                if (String.IsNullOrEmpty(_LastBuild))
                {
                    _LastBuild = DateTime.UtcNow.ToString("yyyy.M.d.HH");
                }
                return _LastBuild;
            }
        }
    }
        /// <summary>
    /// Summary description for StaticContent
    /// </summary>
    public static  class JavaScript
    {
      static   string _JsVersion = null;
        public static  string JsVersion
        {
            get
            {
                if (String.IsNullOrEmpty(_JsVersion))
                {
                    try
                    {
                        string jsFile = System.Web.HttpContext.Current.Server.MapPath("~/js/Master.js");
                        FileInfo fi = new FileInfo(jsFile);
                        DateTime lastWriteTime = fi.LastWriteTime;
                        _JsVersion = lastWriteTime.ToString("yyyyMMddHH");
                    }
                    catch
                    {
                        return "1";
                    }
                }
                return _JsVersion;
            }
        }
       
    }
    public static  class CSS
    {
      static   string _CssVersion = null;
        public static string CssVersion
        {
            get
            {
                if (String.IsNullOrEmpty(_CssVersion))
                {
                    try
                    {
                        string cssFile = System.Web.HttpContext.Current.Server.MapPath("~/css/default.css");
                        FileInfo fi = new FileInfo(cssFile);
                        DateTime lastWriteTime = fi.LastWriteTime;
                        _CssVersion = lastWriteTime.ToString("yyyyMMddHH");
                    }
                    catch
                    {
                        return "1";
                    }
                }
                return _CssVersion;
            }
        }
          }


    public static class URLClass
    {
      static  string domainName = null;
        public static string GetDomain
        {
            get
            {
                if (String.IsNullOrEmpty(domainName))
                    domainName = HttpContext.Current.Request.Url.Host.ToLower();

                return domainName;
            }

        }
    }
}