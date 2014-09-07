using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Threading;

namespace SupportFramework.Data
{
    /// <summary>
    /// Summary description for Errors
    /// </summary>
    public class Errors
    {
        /// <summary>
        /// Inserts the data into the table where the parser failed.
        /// </summary>
        /// <param name="failedat"></param>
        /// <param name="data"></param>
        public static void failedAt(string failedat, string data, Guid userID)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Distorted_Data DD = new CS_Code.Utopia_Distorted_Data();
            DD.aspnet_ID = userID;
            DD.date_time = DateTime.UtcNow;
            DD.Raw_Data = data.Replace(Environment.NewLine, "");
            DD.Failed_At = failedat;
            DD.Version = AssemblyID.Version;
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["SubmittedData"] != null)
                    DD.rawData = HttpContext.Current.Session["SubmittedData"].ToString();
                HttpContext.Current.Session["Failed"] = 1;
            }
            db.Utopia_Distorted_Datas.InsertOnSubmit(DD);
            db.SubmitChanges();

        }

        public static void logError(Exception e)
        {
            logError(e, null);
        }
        /// <summary>
        /// logs the error into DB.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="additionalInformation"></param>
        public static void logError(Exception e, string additionalInformation)
        {

            string LastException, ErrorURLPrevious, ErrorURL, ErrorMessage, ErrorTrace, ErrorTarget, ErrorSource, TraceError;
            LastException = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Server.HtmlEncode(e.ToString().Replace(Environment.NewLine, " ")));
            Exception LastError = e;
            if (HttpContext.Current.Request.UrlReferrer != null)
                ErrorURLPrevious = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.UrlReferrer.ToString().Replace(Environment.NewLine, " "));
            else
                ErrorURLPrevious = null;



            ErrorURL = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.Url.ToString().Replace(Environment.NewLine, " "));
            ErrorMessage = HttpContext.Current.Server.HtmlEncode(LastError.Message.ToString().Replace(Environment.NewLine, " "));
            ErrorTrace = HttpContext.Current.Server.HtmlEncode(LastError.StackTrace.ToString().Replace(Environment.NewLine, " "));
            ErrorTarget = HttpContext.Current.Server.HtmlEncode(LastError.TargetSite.ToString().Replace(Environment.NewLine, " "));
            ErrorSource = HttpContext.Current.Server.HtmlEncode(LastError.Source.ToString().Replace(Environment.NewLine, " "));
            TraceError = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Trace.ToString().Replace(Environment.NewLine, " "));

            CS_Code.GlobalDataContext db = CS_Code.GlobalDataContext.Get();
            CS_Code.Global_Errors_Log UEL = new CS_Code.Global_Errors_Log();
            UEL.Application_Id = Applications.Instance.ApplicationId;
            UEL.Date_Time = DateTime.UtcNow;
            UEL.Error_Message = ErrorMessage;
            UEL.Error_Source = ErrorSource;
            UEL.Error_Target = ErrorTarget;
            UEL.Error_Trace = ErrorTrace;
            UEL.Last_Exception = LastException;
            UEL.Load_Date = DateTime.UtcNow.ToString();
            UEL.Trace_Error = TraceError;
            UEL.Error_URL = ErrorURL;

            if (ErrorURL.Contains("WebResource.axd") | ErrorURL.Contains("ScriptResource.axd"))
            {
                var url = ErrorURL.IndexOf("?d=");
                                additionalInformation = Boomers.Utilities.Web.WebResourceEncoder.decodeWebResourceString(ErrorURL.Remove(0, url + 3));
            }
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string UserName = HttpContext.Current.User.Identity.Name;
                System.Web.Security.MembershipUser mu = System.Web.Security.Membership.GetUser(UserName);
                UEL.User_Email = mu.Email.ToString();
                UEL.User_ID = SupportFramework.Users.Memberships.getUserID(UserName);
            }
            UEL.Version = AssemblyID.Version;

            UriBuilder currentUrl = new UriBuilder(ErrorURL);
            UEL.Domain = currentUrl.Host.Replace("http://", "").Replace("www.", "");
            UEL.Error_Url_Path = currentUrl.Path;
            if (currentUrl.Query != string.Empty)
                UEL.Error_Url_QS = currentUrl.Query;

            if (additionalInformation != null)
                UEL.AdditionalInformation = additionalInformation;


            if (!string.IsNullOrEmpty(ErrorURLPrevious))
            {
                UriBuilder previousUrl = new UriBuilder(ErrorURLPrevious);
                UEL.Error_Previous_Url_Path = previousUrl.Path;
                if (previousUrl.Query != string.Empty)
                    UEL.Error_Previous_Url_QS = previousUrl.Query;
            }

            if (HttpContext.Current.Session != null)
                if (HttpContext.Current.Session["SubmittedData"] != null)
                    UEL.Session_Data = HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Session["SubmittedData"].ToString().Replace(Environment.NewLine, " "));

            db.Global_Errors_Logs.InsertOnSubmit(UEL);
            db.SubmitChanges();


        }
    }
}