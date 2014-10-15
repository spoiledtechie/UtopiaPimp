using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UParser;
using PimpLibrary.Static.Enums;
using MvcMiniProfiler;

using Pimp.Utopia;
using Pimp.Users;

using PimpLibrary.UI;
using Boomers.UserUtil;
using SupportFramework.Users;
using System.Threading.Tasks;

using SupportFramework.Data;


namespace Pimp.UData
{
    /// <summary>
    /// Summary description for PimpUserWrapper 
    /// </summary>
    public class PimpUserWrapper
    {
        private static string USER_CACHE_NAME = "UserCache";

        public PimpUser PimpUser { get; set; }

        /// <summary>
        /// gets the cached user in the session.
        /// </summary>
        /// <returns></returns>
        public PimpUserWrapper()
        {
            PimpUser = new PimpUser();
            if (HttpContext.Current.User != null && !HttpContext.Current.User.Identity.IsAuthenticated)
            {
                PimpUser = new PimpUser(SupportFramework.Users.Memberships.getUserID());
            }
            else if (HttpContext.Current.Session[USER_CACHE_NAME] == null)
            {
                getUserObject(SupportFramework.Users.Memberships.getUserID());
                updateListOfUsers(PimpUser);
                updateUserCache();
            }
            else
            {
                PimpUser = (PimpUser)HttpContext.Current.Session[USER_CACHE_NAME];
            }
        }

        public PimpUserWrapper(string userName)
        {
            getUserObject(userName);
        }

        /// <summary>
        /// Checks if the Currently logged in user is the monarch. 1-OwnerOfKingdom, 2-sub, 3-admin, 4-MoarchForKingdom, 0-non
        /// OwnerofKingdom and SubMonarch Supercedes MonarchforKingdom since MonarchForKingdom is JUST for display purposes.
        /// </summary>
        /// <returns></returns>
        private static MonarchType getMonarchType(PimpUser currentUser, OwnedKingdomProvinces cachedKingdom)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                return MonarchType.none;

            if (currentUser.StartingKingdom == new Guid())
                return MonarchType.none;

            var prov = cachedKingdom.Provinces.Where(x => x.Province_ID == currentUser.CurrentActiveProvince).FirstOrDefault();
            if (prov == null)
                return MonarchType.none;

            if (prov.Owner.GetValueOrDefault() == 1)
                return MonarchType.owner;
            if (prov.Sub_Monarch.GetValueOrDefault() == 1)
                return MonarchType.sub;

            if (currentUser.IsUserAdmin)
                return MonarchType.admin;

            return MonarchType.none;
        }
        /// <summary>
        /// gets the target finder settings from the DB
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static CS_Code.Utopia_Target_Finder_Setting getTargetFinderSettings(CS_Code.UtopiaDataContext db, Guid userId)
        {
            var settings = (from xx in db.Utopia_Target_Finder_Settings
                            where xx.User_ID == userId
                            select xx).FirstOrDefault();

            if (settings == null)
            {
                CS_Code.Utopia_Target_Finder_Setting setting = new CS_Code.Utopia_Target_Finder_Setting();
                setting.Current_Provinces_Submitted = 0;
                setting.Last_Submission = DateTime.UtcNow;
                setting.Total_Provinces_Submitted = 0;
                setting.User_ID = userId;
                db.Utopia_Target_Finder_Settings.InsertOnSubmit(setting);
                db.SubmitChanges();
                return setting;
            }
            return settings;
        }
        public void changeCurrentActiveProvinceId(Guid provinceId, string provinceName)
        {
            var b = System.Web.Profile.ProfileBase.Create(PimpUser.UserName);
            b.SetPropertyValue("StartingProvince", provinceId.ToString());
            b.Save();

            PimpUser.CurrentActiveProvince = provinceId;
            PimpUser.CurrentActiveProvinceName = provinceName;
            PimpUser.MonarchType = MonarchType.NOTSELECTED;

            var provinceOwned = PimpUser.ProvincesOwned.Where(x => x.Province_ID == PimpUser.CurrentActiveProvince).FirstOrDefault();
            if (provinceOwned != null)
                PimpUser.StartingKingdom = (Guid)provinceOwned.Owner_Kingdom_ID;

            updateListOfUsers(PimpUser);
            updateUserCache();

        }

        /// <summary>
        /// gets the provinces owned by the user
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static List<ProvinceClass> getProvincesOwned(CS_Code.UtopiaDataContext db, Guid userId)
        {
            return (from xx in db.Utopia_Province_Data_Captured_Gens
                    where xx.Owner_User_ID == userId
                    where xx.Province_Name != string.Empty
                    where xx.Owner_Kingdom_ID != new Guid()
                    select new ProvinceClass
                    {
                        Province_ID = xx.Province_ID,
                        Province_Name = xx.Province_Name,
                        Owner_Kingdom_ID = xx.Owner_Kingdom_ID,
                        Owner = xx.Owner,
                        Sub_Monarch = xx.Sub_Monarch
                    }).ToList();
        }
        public void addProvinceToProvincesOwned(CS_Code.Utopia_Province_Data_Captured_Gen gen)
        {
            ProvinceClass prov = new ProvinceClass();

            prov.Province_ID = gen.Province_ID;
            prov.Province_Name = gen.Province_Name;
            prov.Owner_Kingdom_ID = gen.Owner_Kingdom_ID;
            prov.Owner = gen.Owner;
            prov.Sub_Monarch = gen.Sub_Monarch;

            PimpUser.ProvincesOwned.Add(prov);
            updateUserCache();
            updateListOfUsers(PimpUser);
        }

        /// <summary>
        /// gets users online
        /// </summary>
        /// <returns></returns>
        public static List<PimpUser> getUsersOnline()
        {
            //var count = getListOfUsers();
            //PimpUser[] users =new Utopia.PimpUser[count.Count];
            //count.CopyTo(users);
            try
            {
                var users = getListOfUsers();
                         return users.ToList().Where(x => x.LastUpdated > DateTime.Now.AddMinutes(-5)).ToList();
            }
            catch (Exception e)
            {
                Errors.logError(e);
                return new List<Utopia.PimpUser>();
            }
        }

        public static List<PimpUser> getListOfUsers()
        {
            var users = (List<PimpUser>)HttpRuntime.Cache["ListOfUsers"];
            if (users == null)
            {
                users = new List<PimpUser>();
                setListOfUsers(users);
            }
            return users;
        }
        public static void setListOfUsers(List<PimpUser> users)
        {
            HttpRuntime.Cache.Add("ListOfUsers", users, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(72, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
        }
        public static void updateListOfUsers(PimpUser user)
        {
            var users = getListOfUsers();
            var userDelete = users.Where(x => x.UserID == user.UserID).FirstOrDefault();
            if (userDelete != null)
                users.Remove(userDelete);
            users.Add(user);
            HttpRuntime.Cache.Add("ListOfUsers", users, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(72, 0, 0), System.Web.Caching.CacheItemPriority.High, null);
        }
        private void getUserObject(string userName)
        {
            var users = getListOfUsers();
            var list = new List<Utopia.PimpUser>(users);
            var user = list.Where(x => x.UserName == userName.ToLower()).FirstOrDefault();
            if (user == null)
            {
                CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
                var userTemp = (from xx in adb.vw_aspnet_MembershipUsers
                                where xx.UserName == userName
                                select new { UserID = xx.UserId, UserName = xx.UserName.ToLower(), LastUpdated = xx.LastActivityDate }).FirstOrDefault();
                PimpUser = new Utopia.PimpUser();
                PimpUser.UserName = userTemp.UserName;
                PimpUser.UserID = userTemp.UserID;
                PimpUser.LastUpdated = userTemp.LastUpdated;

                getUserObject();
            }
            else
                PimpUser = user;


        }
        private void getUserObject(Guid userId)
        {
            var users = getListOfUsers();
            var list = new List<Utopia.PimpUser>(users);
            var user = list.Where(x => x.UserID == userId).FirstOrDefault();
            if (user == null)
            {
                CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
                var userTemp = (from xx in adb.aspnet_Users
                                where xx.UserId == userId
                                select new { UserID = xx.UserId, UserName = xx.UserName.ToLower(), LastUpdated = xx.LastActivityDate }).FirstOrDefault();

                PimpUser.UserName = userTemp.UserName;
                PimpUser.UserID = userTemp.UserID;
                PimpUser.LastUpdated = userTemp.LastUpdated;
                getUserObject();
            }
            else
                PimpUser = user;
        }




        /// <summary>
        /// gets all the needs of a user when its not cached.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private void getUserObject()
        {

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            PimpUser.TargetFinderSettings = getTargetFinderSettings(db, PimpUser.UserID);

            var b = System.Web.Profile.ProfileBase.Create(PimpUser.UserName);

            try
            { PimpUser.CurrentActiveProvince = new Guid(b.GetPropertyValue("StartingProvince").ToString()); }
            catch
            { PimpUser.CurrentActiveProvince = new Guid(); }

            PimpUser.ProvincesOwned = getProvincesOwned(db, PimpUser.UserID);

            if (PimpUser.ProvincesOwned != null && PimpUser.CurrentActiveProvince != new Guid())
            {
                var activeProvince = PimpUser.ProvincesOwned.Where(x => x.Province_ID == PimpUser.CurrentActiveProvince).FirstOrDefault();
                if (activeProvince != null)
                    PimpUser.CurrentActiveProvinceName = activeProvince.Province_Name;

                var provinceOwned = PimpUser.ProvincesOwned.Where(x => x.Province_ID == PimpUser.CurrentActiveProvince).FirstOrDefault();
                if (provinceOwned != null)
                    PimpUser.StartingKingdom = (Guid)provinceOwned.Owner_Kingdom_ID;
            }
            //if the GetOwnerKingdomID returns a new Guid, then there is no province with the current starting province.
            //So we need to remove the owner kingdom ID from the session.
            if (PimpUser.StartingKingdom == new Guid())
            {
                b.SetPropertyValue("StartingProvince", new Guid().ToString());
                PimpUser.CurrentActiveProvince = new Guid();
                PimpUser.CurrentActiveProvinceName = string.Empty;
            }
            PimpUser.IMInformation = Memberships.getUsersIMInformation(PimpUser.UserID);
            PimpUser.IsUserAdmin = SupportFramework.Users.Memberships.isUserAdmin(PimpUser.UserName);
            PimpUser.UserColumns = Column.getColumnSets(PimpUser.UserID, db);
            PimpUser.NickName = Memberships.getUsersNickName(PimpUser.UserID);

        }
        /// <summary>
        /// clears the starting kingdom.  So meaning we can't find the starting kingdom in the DB.
        /// comes from deleting old ages I think.
        /// </summary>
        public  void clearStartingKingdom()
        {
            var b = System.Web.Profile.ProfileBase.Create(PimpUser.UserName);
            b.SetPropertyValue("StartingProvince", new Guid().ToString());
            PimpUser.CurrentActiveProvince = new Guid();
            PimpUser.CurrentActiveProvinceName = string.Empty;
            PimpUser.StartingKingdom = new Guid();
            updateListOfUsers(PimpUser);
            updateUserCache();
        }

        /// <summary>
        /// updates the target finder settings for the current user
        /// </summary>
        /// <param name="currentProvincesSubmitted"></param>
        public void updateTargetFinderSettings(int currentProvincesSubmitted, int totalProvincesSubmitted, DateTime lastSubmission)
        {
            PimpUser.TargetFinderSettings.Current_Provinces_Submitted = currentProvincesSubmitted;
            PimpUser.TargetFinderSettings.Total_Provinces_Submitted = totalProvincesSubmitted;
            PimpUser.TargetFinderSettings.Last_Submission = lastSubmission;
            updateListOfUsers(PimpUser);
            updateUserCache();

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var targetSettings = (from xx in db.Utopia_Target_Finder_Settings
                                  where xx.User_ID == PimpUser.UserID
                                  select xx).FirstOrDefault();
            if (targetSettings != null)
            {
                targetSettings.Current_Provinces_Submitted = currentProvincesSubmitted;
                targetSettings.Total_Provinces_Submitted = totalProvincesSubmitted;
                targetSettings.Last_Submission = lastSubmission;
            }
            else
            {
                CS_Code.Utopia_Target_Finder_Setting ts = new CS_Code.Utopia_Target_Finder_Setting();
                ts.User_ID = PimpUser.UserID;
                ts.Total_Provinces_Submitted = totalProvincesSubmitted;
                ts.Last_Submission = lastSubmission;
                ts.Current_Provinces_Submitted = currentProvincesSubmitted;
                db.Utopia_Target_Finder_Settings.InsertOnSubmit(ts);
            }
            db.SubmitChanges();

        }

        public void RemoveSessionProvinceInfo()
        {
            HttpContext.Current.Session.Clear();
            removeUserCache();
        }



        public void updateIMInformation(IMType imType)
        {

            var set = PimpUser.IMInformation.Where(x => x.uid == imType.uid).FirstOrDefault();
            if (set != null)
                PimpUser.IMInformation.Remove(set);

            PimpUser.IMInformation.Add(imType);

            updateListOfUsers(PimpUser);
            updateUserCache();
        }
        public void deleteColumnSetsForUser(int columnSetId)
        {
            var set = PimpUser.UserColumns.Where(x => x.setUid == columnSetId).FirstOrDefault();
            if (set != null)
                PimpUser.UserColumns.Remove(set);

            updateListOfUsers(PimpUser);
            updateUserCache();
        }

        public void updateColumnSetsForUser(string setName, CS_Code.Utopia_Column_Name tec)
        {
            ColumnSet set = new ColumnSet();
            set.setName = setName;
            set.setUid = (int)tec.Data_Type_ID;
            set.columnNameUid = tec.uid;
            set.columnTypeID = (int)tec.Data_Type_ID;
            set.columnIDs = tec.Column_IDs;

            updateColumnSetsForUser(set);
        }
        public void updateColumnSetsForUser(ColumnSet columnSet)
        {
            var set = PimpUser.UserColumns.Where(x => x.setUid == columnSet.setUid).FirstOrDefault();
            if (set != null)
                PimpUser.UserColumns.Remove(set);

            PimpUser.UserColumns.Add(columnSet);
            updateListOfUsers(PimpUser);
            updateUserCache();
        }
        public void SetStartingKingdom(Guid startingKingdom)
        {
            PimpUser.StartingKingdom = startingKingdom;
            updateListOfUsers(PimpUser);
            updateUserCache();
        }
        public MonarchType getMonarchType(OwnedKingdomProvinces cachedKingdom)
        {
            if (PimpUser.MonarchType != MonarchType.NOTSELECTED)
                return PimpUser.MonarchType;

            PimpUser.MonarchType = getMonarchType(PimpUser, cachedKingdom);
            updateListOfUsers(PimpUser);
            updateUserCache();
            return PimpUser.MonarchType;
        }

        public void removeUserCache()
        {
            HttpContext.Current.Session.Remove(USER_CACHE_NAME);
        }
        public void updateUserCache()
        {
            HttpContext.Current.Session.Add(USER_CACHE_NAME, PimpUser);
        }
    }
}