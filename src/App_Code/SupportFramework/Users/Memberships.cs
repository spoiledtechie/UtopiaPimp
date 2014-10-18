using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Pimp;

using Boomers.UserUtil;
using Pimp.Utopia;
using Pimp.Users;
using SupportFramework.Data;

namespace SupportFramework.Users
{
    /// <summary>
    /// Summary description for Memberships
    /// </summary>
    public class Memberships
    {
        public static string USER_ID_IN_CACHE_NAME = "userID";
        //private static string IS_ADMIN_IN_CACHE_NAME = "IsAdmin";

        public static int getUserErrorCount(string userName)
        {
            CS_Code.GlobalDataContext adb = CS_Code.GlobalDataContext.Get();
            return (from tel in adb.Global_Errors_Logs
                    where tel.Application_Id == Applications.Instance.ApplicationId
                    where tel.User_ID == Memberships.getUserID(userName)
                    select tel.uid).Count();
        }

        public static int getUserPageViews(string userName)
        {
            CS_Code.GlobalDataContext adb = CS_Code.GlobalDataContext.Get();
            return (from xx in adb.Global_User_Audits
                    where xx.ApplicationId == Applications.Instance.ApplicationId
                    where xx.User_ID == Memberships.getUserID(userName)
                    select xx.uid).Count();
        }
        /// <summary>
        /// gets the last active date of the user
        /// </summary>
        /// <param name="adb"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DateTime getLastActivityDate(CS_Code.AdminDataContext adb, Guid userId)
        {
            return (from yy in adb.vw_aspnet_Users
                    where userId == yy.UserId
                    select yy.LastActivityDate).FirstOrDefault();
        }
        /// <summary>
        /// gets the UserID of the username.
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static System.Guid getUserID(string userName)
        {
            MembershipUser CurrentUser = Membership.GetUser(userName);
            return new System.Guid(CurrentUser.ProviderUserKey.ToString());
        }
        /// <summary>
        /// changes the user Logged in Users email
        /// </summary>
        /// <param name="oldEmail"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        public static void changeUserEmailByEmail(string oldEmail, string newEmail)
        {
            string user = Membership.GetUserNameByEmail(oldEmail);
            changeUserEmailByUserName(user, newEmail);
        }
        /// <summary>
        /// changes user email
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        public static void changeUserEmailByUserName(string userName, string userEmail)
        {
            MembershipUser mu = Membership.GetUser(userName);
            mu.Email = userEmail;
            Membership.UpdateUser(mu);
        }
        /// <summary>
        /// changes the users name and sets a new cookie.
        /// </summary>
        /// <param name="oldUserName"></param>
        /// <param name="newUserName"></param>
        /// <returns></returns>
        public static bool changeUserName(Guid userId, string newUserName)
        {
            CS_Code.AdminDataContext db = CS_Code.AdminDataContext.Get();
            try
            {
                var getUser = (from xx in db.aspnet_Users
                               where xx.UserId == userId
                               select xx).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.UserName = newUserName;
                    getUser.LoweredUserName = newUserName.ToLower();
                    db.SubmitChanges();
                    FormsAuthentication.SetAuthCookie(newUserName, false);
                    return true;
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
            }
            return false;
        }
        /// <summary>
        /// gets the current user id
        /// </summary>
        /// <returns></returns>
        public static System.Guid getUserID()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                MembershipUser CurrentUser = Membership.GetUser();
                return new System.Guid(CurrentUser.ProviderUserKey.ToString());
            }
            else
                return new Guid();
        }
        /// <summary>
        /// checks if user is admin.
        /// </summary>
        /// <returns></returns>
        public static bool isUserAdmin()
        {
            return Roles.IsUserInRole("admin");
        }
        public static bool isUserAdmin(string userName)
        {
            return Roles.IsUserInRole(userName, "admin");
        }
        /// <summary>
        /// gets the current users email
        /// </summary>
        /// <returns></returns>
        public static string getUserEmail()
        {
            MembershipUser CurrentUser = Membership.GetUser();
            return CurrentUser.Email;
        }
        /// <summary>
        /// gets USer Email
        /// </summary>
        /// <param name="userName">Username for user</param>
        /// <returns>User Email</returns>
        public static string getUserEmail(string userName)
        {
            MembershipUser CurrentUser = Membership.GetUser(userName);
            return CurrentUser.Email;
        }
        /// <summary>
        /// gets User Email from User ID.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User name</returns>
        public static string getUserEmail(Guid userID)
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
        public static string getUserName(Guid userID)
        {
            MembershipUser CurrentUser = Membership.GetUser(userID);
            if (CurrentUser == null)
                return "anonymous";
            return CurrentUser.UserName;
        }
        /// <summary>
        /// gets the user name if the current logged in user.
        /// </summary>
        /// <returns></returns>
        public static string getUserName()
        {
            if (HttpContext.Current.User.Identity.Name == null)
                return "anonymous";
            else
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
        /// <summary>
        /// gets the users NickName
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static string getUsersNickName(Guid userId)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            return (from yy in adb.user_Informations
                    where userId == yy.user_ID
                    select yy.Nick_Name).FirstOrDefault();
        }
        public static List<IMType> getUsersIMInformation(Guid userID)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            return (from xx in adb.user_IM_Type_Pulls
                    from yy in adb.user_IMs
                    where xx.uid == yy.IM_Type
                    where yy.Application_ID == Applications.Instance.ApplicationId
                    where yy.User_ID == userID
                    select new IMType
                    {
                        IM_Name = yy.IM_Name,
                        IM_Password = yy.IM_Password,
                        IM_Password_Bool = yy.IM_Password_Bool,
                        IM_Type = xx.IM_Type,
                        uid = yy.uid
                    }).ToList();
        }
        public static List<PhoneType> getUsersPhoneInformation(Guid userID, Guid applicationID)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            return (from xx in adb.user_Phone_Number_Pulls
                    from yy in adb.user_Phone_Numbers
                    where xx.uid == yy.Phone_Type
                    where yy.Application_ID == applicationID
                    where yy.user_ID == userID
                    select new PhoneType
                    {
                        PhoneNumber = yy.Phone_Number,
                        SMS = yy.SMS,
                        Phone_Type = xx.Phone_Type,
                        uid = yy.uid
                    }).ToList();
        }
        public static Contact getUserContact(Guid userID, Guid applicationID)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            return (from yy in adb.user_Informations
                    from zz in adb.vw_aspnet_Users
                    where zz.UserId == yy.user_ID
                    where yy.user_ID == userID
                    select new Contact
                    {
                        //zz.UserName,
                        State = yy.State,
                        Nick_Name = yy.Nick_Name,
                        GMT_Offset = yy.GMT_Offset,
                        Country = yy.Country,
                        City = yy.City,
                        DOB = yy.DOB,
                        user_ID = yy.user_ID,
                        Notes = yy.Notes,
                        phoneNumbers = (from pnp in adb.user_Phone_Number_Pulls
                                        from pn in adb.user_Phone_Numbers
                                        where pnp.uid == pn.Phone_Type
                                        where pn.user_ID == yy.user_ID
                                        select new PhoneType { PhoneNumber = pn.Phone_Number, SMS = pn.SMS, Phone_Type = pnp.Phone_Type }).ToList(),
                        imNames = (from imp in adb.user_IM_Type_Pulls
                                   from im in adb.user_IMs
                                   where imp.uid == im.IM_Type
                                   where im.User_ID == yy.user_ID
                                   select new IMType { uid = im.uid, IM_Password_Bool = im.IM_Password_Bool, IM_Name = im.IM_Name, IM_Type = imp.IM_Type }).ToList()
                    }).FirstOrDefault();
        }


    }
}