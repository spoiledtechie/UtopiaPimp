using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web.SessionState;

/// <summary>
/// Summary description for SQLStatementsCS
/// </summary>
public class DataAccessGlobal
{
    public DataAccessGlobal()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    /// <summary>
    /// The connection string of the system.
    /// </summary>
    /// <returns></returns>
    public static string ConnectionStringID()
    { return ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString; }
    /// <summary>
    /// gets the UserID of the username.
    /// </summary>
    /// <param name="UserName"></param>
    /// <returns></returns>
    public static System.Guid LoginID(string UserName)
    {
        MembershipUser CurrentUser = Membership.GetUser(UserName);
        return new System.Guid(CurrentUser.ProviderUserKey.ToString());
    }
    /// <summary>
    /// Gets UserID From Currently Logged in user.
    /// </summary>
    /// <returns></returns>
    public static System.Guid LoginID()
    {
        MembershipUser CurrentUser = Membership.GetUser(HttpContext.Current.User.Identity.Name);
        try
        { return new System.Guid(CurrentUser.ProviderUserKey.ToString()); }
        catch
        { return new System.Guid("00000000-0000-0000-0000-000000000000"); }
    }
    public static string UserEmail()
    {
        MembershipUser CurrentUser = Membership.GetUser();
        return CurrentUser.Email;
    }
    /// <summary>
    /// gets USer Email
    /// </summary>
    /// <param name="userName">Username for user</param>
    /// <returns>User Email</returns>
    public static string UserEmail(string userName)
    {
        MembershipUser CurrentUser = Membership.GetUser(userName);
        return CurrentUser.Email;
    }
    /// <summary>
    /// gets User Email from User ID.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User name</returns>
    public static string UserEmail(Guid userID)
    {
        MembershipUser CurrentUser = Membership.GetUser(userID);
        if (CurrentUser == null)
            return "anonymous";
        return CurrentUser.Email;
    }
    /// <summary>
    /// gets Username from Login ID
    /// </summary>
    /// <param name="LoginID">Guid Login ID</param>
    /// <returns>Username</returns>
    public static string UserName(string LoginID)
    {
        if (LoginID.IsValidGuid())
        {
            Guid User = new Guid(LoginID);
            MembershipUser CurrentUser = Membership.GetUser(User);
            if (CurrentUser == null)
                return "anonymous";
            return CurrentUser.UserName;
        }
        else
            return null;
    }
    /// <summary>
    /// Caputes any data needed to a table.
    /// </summary>
    /// <param name="Data"></param>
    /// <returns></returns>
    public static int DataCapture(string Data)
    {
        SupportFramework.GlobalDataContext db = new SupportFramework.GlobalDataContext();
        SupportFramework.Global_Data_Capture gdc = new SupportFramework.Global_Data_Capture();
        gdc.Data = Data;
        gdc.DateTime_Updated = DateTime.UtcNow;
        db.Global_Data_Captures.InsertOnSubmit(gdc);
        db.SubmitChanges();
        return gdc.uid;
    }

    public static void setError(string LastException, string ErrorURLPrevious, string ErrorURL, string ErrorMessage, string ErrorTrace, string ErrorTarget, string ErrorSource, string TraceError, string UserName, string EmailofUser, DateTime LoadDate)
    {
        SupportFramework.GlobalDataContext db = new SupportFramework.GlobalDataContext();
        SupportFramework.Global_Errors_Log UEL = new SupportFramework.Global_Errors_Log();
        UEL.Date_Time = DateTime.UtcNow;
        UEL.Error_Message = ErrorMessage;
        UEL.Error_Previous_URL = ErrorURLPrevious;
        UEL.Error_Source = ErrorSource;
        UEL.Error_Target = ErrorTarget;
        UEL.Error_Trace = ErrorTrace;
        UEL.Error_URL = ErrorURL;
        UEL.Last_Exception = LastException;
        UEL.Load_Date = LoadDate.ToString();
        UEL.Trace_Error = TraceError;
        UEL.User_Email = EmailofUser;
        UEL.User_Name = UserName;
        UEL.Version = AssemblyID.GetVersion();
        //if (Session["SubmittedData"] != null)
        //{
        //    UEL.Session_Data = Server.HtmlEncode(Session["SubmittedData"].ToString().Replace(Environment.NewLine, " "));
        //}
        db.Global_Errors_Logs.InsertOnSubmit(UEL);
        db.SubmitChanges();
    }
}
