using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Boomers.Utilities.Web;
using System.Threading.Tasks;

using SupportFramework;

namespace SupportFramework.Users
{
    /// <summary>
    /// Summary description for AuditUser
    /// </summary>
    public class Audit
    {
        /// <summary>
        /// Inserts the information for the User who visited the site.
        /// </summary>
        public static void submitAudit()
        {
         
               CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();
               CS_Code.Global_User_Audit UUA = new CS_Code.Global_User_Audit();
               UUA.ApplicationId = Applications.Instance.ApplicationId;
               UUA.ActiveXControls = Convert.ToInt32(HttpContext.Current.Request.Browser.ActiveXControls);
               UUA.AnonymousID = "";
               UUA.Auth_User = HttpContext.Current.Request.ServerVariables["AUTH_USER"];
               UUA.Browser_String = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
               UUA.BrowserScreen_Resolution = HttpContext.Current.Request.Browser.ScreenPixelsWidth + "x" + HttpContext.Current.Request.Browser.ScreenPixelsHeight;
               UUA.Current_URL = HttpContext.Current.Request.Url.ToString();
               UUA.Current_User_Path = HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"];
               UUA.DateTime = DateTime.UtcNow;
               UUA.HTTP_Host = "";
               UUA.IP_Address = HttpContext.Current.Request.UserHostAddress;
               UUA.IsMobile = Convert.ToInt32(Mobile.IsMobile);
               UUA.JavaScript = Convert.ToInt32(HttpContext.Current.Request.Browser.EcmaScriptVersion);
               UUA.Logon_User = HttpContext.Current.Request.ServerVariables["LOGON_User"];
               UUA.Previous_URL = "";
               UUA.Remote_User = HttpContext.Current.Request.ServerVariables["REMOTE_USER"];
               UUA.User_ID = Users.Memberships.getUserID();
               UUA.Win16 = Convert.ToInt32(HttpContext.Current.Request.Browser.Win16);
               UUA.Win32 = Convert.ToInt32(HttpContext.Current.Request.Browser.Win32);
               UUA.Windows_Platform = HttpContext.Current.Request.Browser.Platform;
               db.Global_User_Audits.InsertOnSubmit(UUA);
               db.SubmitChanges();
  
        }
    }
}