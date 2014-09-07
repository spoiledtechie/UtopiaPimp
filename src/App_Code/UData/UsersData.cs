using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.Utopia;
using Pimp.Users;
using Pimp.UCache;
using SupportFramework;
using System.Threading.Tasks;
using Boomers.UserUtil;
using SupportFramework.Data;

namespace Pimp.UData
{
    /// <summary>
    /// Summary description for Users
    /// </summary>
    public class UsersData
    {
        /// <summary>
        /// removes the user from province id
        /// </summary>
        /// <param name="provinceId"></param>
        public static void disconnectProvinceFromUser(Guid provinceId)
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getProvinceInfo = (from tel in db.Utopia_Province_Data_Captured_Gens
                                   where tel.Province_ID == provinceId
                                   select tel).FirstOrDefault();
            getProvinceInfo.Owner_User_ID = null;
            db.SubmitChanges();

        }

        /// <summary>
        /// updates the page load of the user.
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="currentUser"></param>
        public static void UpdateLastActivityDate(PimpUser currentUser)
        {

            try
            {
                CS_Code.AdminDataContext db = CS_Code.AdminDataContext.Get();
                var updateActivity = (from xx in db.aspnet_Users
                                      where xx.UserId == currentUser.UserID
                                      where xx.ApplicationId == Applications.Instance.ApplicationId
                                      select xx).FirstOrDefault();
                if (updateActivity != null)
                {
                    updateActivity.LastActivityDate = DateTime.UtcNow;
                    db.SubmitChanges();
                    currentUser.LastUpdated = DateTime.UtcNow;
                    PimpUserWrapper.updateListOfUsers(currentUser);
                }
            }
            catch (Exception e)
            {
                Errors.logError(e);
            }

        }


        public static bool UpdateContactInfo(string city, string country, int gmt, string nickName, string state, int day, int month, int year, string notes, Guid ownerKingdomID, PimpUserWrapper currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            var getUser = (from xx in adb.user_Informations
                           where xx.user_ID == currentUser.PimpUser.UserID
                           select xx).FirstOrDefault();
            if (getUser != null)
            {
                getUser.City = city;
                getUser.Country = country;
                if (day > 0 & month > 0 & year > 0)
                    try { getUser.DOB = new DateTime(year, month, day); }
                    catch { }
                getUser.GMT_Offset = gmt.ToString();
                getUser.Nick_Name = nickName;
                getUser.State = state;
                getUser.Notes = notes;
                if (getUser.Application_ID == null)
                    getUser.Application_ID = Applications.Instance.ApplicationId;
            }
            else
            {
                CS_Code.user_Information ui = new CS_Code.user_Information();
                ui.City = city;
                ui.Country = country;
                if (day > 0 & month > 0 & year > 0)
                    ui.DOB = new DateTime(year, month, day);
                ui.GMT_Offset = gmt.ToString();
                ui.Nick_Name = nickName;
                ui.State = state;
                ui.Notes = notes;
                ui.user_ID = currentUser.PimpUser.UserID;
                ui.Application_ID = Applications.Instance.ApplicationId;
                adb.user_Informations.InsertOnSubmit(ui);
            }
            adb.SubmitChanges();
            KingdomCache.removeContactsNotSignedUp(ownerKingdomID, cachedKingdom);
            UsersCache.updateContactForUser(ownerKingdomID, currentUser.PimpUser.UserID, Applications.Instance.ApplicationId, cachedKingdom);
            return true;
        }



        public static bool DeletePhoneNumber(int uid, Guid ownerKingdomID, Guid userID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            var getOldNumbers = (from xx in adb.user_Phone_Numbers
                                 where xx.user_ID == userID
                                 where xx.uid == uid
                                 select xx).FirstOrDefault();
            if (getOldNumbers != null)
            {
                adb.user_Phone_Numbers.DeleteOnSubmit(getOldNumbers);
                adb.SubmitChanges();
                UsersCache.updateContactForUser(ownerKingdomID, userID, Applications.Instance.ApplicationId, cachedKingdom);
            }
            return true;
        }


        /// <summary>
        /// gets users online
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static List<PimpUser> getUsersOnline(Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            var provs = cachedKingdom.Provinces.Where(x => x.Kingdom_ID == ownerKingdomID).Where(x => x.Owner_User_ID != null).ToList();
            var usersOnline = PimpUserWrapper.getUsersOnline();
            return (from xx in provs
                    from yy in usersOnline
                    where xx.Owner_User_ID == yy.UserID
                    select new PimpUser
                    {
                        CurrentActiveProvinceName = xx.Province_Name,
                        CurrentActiveProvince = xx.Province_ID,
                        StartingKingdom = ownerKingdomID,
                        UserID = yy.UserID,
                        LastUpdated = yy.LastUpdated,
                        NickName = yy.NickName
                    }).ToList();
        }
        /// <summary>
        /// get user Online
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static PimpUser getUserOnline(Guid userID)
        {
            CS_Code.AdminDataContext db = CS_Code.AdminDataContext.Get();
            return (from xx in db.vw_aspnet_Users
                    from yy in db.user_Informations
                    where xx.ApplicationId == SupportFramework.Applications.Instance.ApplicationId
                    where xx.UserId == userID
                    where xx.UserId == yy.user_ID
                    select new PimpUser
                    {
                        UserID = xx.UserId,
                        NickName = yy.Nick_Name,
                        LastUpdated = xx.LastActivityDate
                    }).FirstOrDefault();
        }


        /// <summary>
        /// adds the phone number to the specified user
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="type"></param>
        /// <param name="sms"></param>
        /// <param name="userID"></param>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static bool AddPhoneNumber(string phoneNumber, string type, int sms, Guid userID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            var getOldNumbers = (from xx in adb.user_Phone_Numbers
                                 where xx.user_ID == userID
                                 select xx).ToList();
            bool doesNumberExist = false;
            foreach (var item in getOldNumbers)
                if (item.Phone_Number == phoneNumber)
                    doesNumberExist = true;

            //if doesn't exist
            if (!doesNumberExist)
            {
                CS_Code.user_Phone_Number pn = new CS_Code.user_Phone_Number();
                pn.Phone_Number = phoneNumber;
                pn.Phone_Type = GetPhoneType(type);
                pn.SMS = sms;
                pn.user_ID = userID;
                pn.Application_ID = Applications.Instance.ApplicationId;
                adb.user_Phone_Numbers.InsertOnSubmit(pn);
                adb.SubmitChanges();

                PhoneType phone = new PhoneType();
                phone.Phone_Type = type;
                phone.PhoneNumber = phoneNumber;
                phone.SMS = sms;
                phone.uid = pn.uid;

                UsersCache.addPhoneNumberToCache(phone, userID, ownerKingdomID, cachedKingdom);

            }
            return true;
        }
        /// <summary>
        /// gets the type of phone being sms, cell, land line
        /// </summary>
        /// <param name="phoneTypeName"></param>
        /// <returns></returns>
        private static int GetPhoneType(string phoneTypeName)
        {
            var getType = GetPhoneTypes.Where(x => x.Phone_Type == phoneTypeName).Select(x => x.uid).FirstOrDefault();
            //doesnt exist
            if (getType == 0)
            {
                CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
                CS_Code.user_Phone_Number_Pull pp = new CS_Code.user_Phone_Number_Pull();
                pp.Phone_Type = phoneTypeName;
                adb.user_Phone_Number_Pulls.InsertOnSubmit(pp);
                adb.SubmitChanges();
                return pp.uid;
            }
            return getType;
        }
        /// <summary>
        /// pulls the phone types from the DB.
        /// </summary>
        /// <returns></returns>
        private static List<PhoneType> getPhoneTypePullTable()
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            return (from imp in adb.user_Phone_Number_Pulls
                    orderby imp.Phone_Type ascending
                    select new PhoneType { uid = imp.uid, Phone_Type = imp.Phone_Type }).ToList();
        }

        public static void cachePhoneTypes()
        {
            var phone = getPhoneTypePullTable();
            HttpRuntime.Cache["PhoneTypes"] = phone;
        }

        static List<PhoneType> _getPhoneTypes;
        /// <summary>
        /// sets the phones types to cache and retrieves them
        /// </summary>
        public static List<PhoneType> GetPhoneTypes
        {
            get
            {
                if (_getPhoneTypes == null)
                {
                    if (HttpRuntime.Cache["PhoneTypes"] == null)
                    {
                        var phone = getPhoneTypePullTable();
                        HttpRuntime.Cache["PhoneTypes"] = phone;
                        _getPhoneTypes = phone;
                    }
                    else
                    {
                        try
                        {
                            _getPhoneTypes = (List<PhoneType>)HttpRuntime.Cache["PhoneTypes"];
                        }
                        catch
                        {
                            HttpRuntime.Cache.Remove("PhoneTypes");
                            return GetPhoneTypes;
                        }
                    }
                }
                return _getPhoneTypes;
            }
        }

    }
}