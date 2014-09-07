using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Boomers.Utilities.Web
{
    public static class Versioned
    {
        static string _LastBuild = null;
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
    public class JavaScript
    {

        private string _javaScriptFile;
        /// <summary>
        /// http://codingforcharity.org/utopiapimp/js/Master.js
        /// </summary>
        public string JavaScriptFile
        {
            get { return _javaScriptFile; }
            set { _javaScriptFile = value; }
        }

        string _JsVersion = null;
        public string JsVersion
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
                        _JsVersion = lastWriteTime.ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                        return "1";
                    }
                }
                return _JsVersion;
            }
        }
        HtmlGenericControl _JsDocument = null;

        public HtmlGenericControl JsDefault
        {
            get
            {
                if (_JsDocument == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl();
                    js.TagName = "script";
                    js.Attributes["src"] = String.Format(JavaScriptFile + "?v={0}", JsVersion);
                    js.Attributes["type"] = "text/javascript";
                    _JsDocument = js;
                }
                return _JsDocument;
            }
        }
    }
    public class CSS
    {
        private string _cssFile;
        /// <summary>
        /// http://codingforcharity.org/utopiapimp/css/default.css
        /// </summary>
        public string CssFile
        {
            get { return _cssFile; }
            set { _cssFile = value; }
        }

        string _CssVersion = null;
        public string CssVersion
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
                        _CssVersion = lastWriteTime.ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                        return "1";
                    }
                }
                return _CssVersion;
            }
        }
        HtmlLink _CssDocument = null;

        public HtmlLink CssDefault
        {
            get
            {
                if (_CssDocument == null)
                {
                    HtmlLink css = new HtmlLink();
                    css.Href = String.Format(CssFile + "?v={0}", CssVersion);
                    css.Attributes["rel"] = "stylesheet";
                    css.Attributes["type"] = "text/css";
                    css.Attributes["media"] = "all";
                    _CssDocument = css;
                }
                return _CssDocument;
            }
        }



    }
}