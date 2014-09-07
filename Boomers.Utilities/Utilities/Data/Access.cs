using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Boomers.Utilities.Guids;

namespace Boomers.Utilities.Data
{
   public class Access
    {
        /// <summary>
        /// The connection string of the system.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection ConnectionStringID
        { get { return new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString); } }
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
                System.Guid User = new System.Guid(LoginID);
                MembershipUser CurrentUser = Membership.GetUser(User);
                if (CurrentUser == null)
                    return "anonymous";
                return CurrentUser.UserName;
            }
            else
                return null;
        }
      
    }
}
