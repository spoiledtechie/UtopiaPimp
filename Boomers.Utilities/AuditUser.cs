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

/// <summary>
/// Summary description for AuditUser
/// </summary>
public class AuditUser
{
    public AuditUser()
    {
    }
    /// <summary>
    /// Inserts the information for the User who visited the site.
    /// </summary>
    public static void Audit()
    {
        SupportFramework.GlobalDataContext db = new SupportFramework.GlobalDataContext();
        SupportFramework.Global_User_Audit UUA = new SupportFramework.Global_User_Audit();
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
        UUA.IsMobile =Convert.ToInt32(SpoiledTechie.Utilities.MobileBrowsing.IsMobile);
        UUA.JavaScript = Convert.ToInt32(HttpContext.Current.Request.Browser.JavaScript);
        UUA.Logon_User = HttpContext.Current.Request.ServerVariables["LOGON_User"];
        UUA.Previous_URL = "";
        UUA.Remote_User = HttpContext.Current.Request.ServerVariables["REMOTE_USER"];
        UUA.User_ID = DataAccessGlobal.LoginID();
        UUA.Win16 = Convert.ToInt32(HttpContext.Current.Request.Browser.Win16);
        UUA.Win32 = Convert.ToInt32(HttpContext.Current.Request.Browser.Win32);
        UUA.Windows_Platform = HttpContext.Current.Request.Browser.Platform;
        db.Global_User_Audits.InsertOnSubmit(UUA);
        db.SubmitChanges();
    }
}
