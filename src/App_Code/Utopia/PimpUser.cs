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


namespace Pimp.Utopia
{
    /// <summary>
    /// Summary description for AngelUser
    /// </summary>
    public class PimpUser
    {
        public PimpUser(Guid userID)
        {
            this.UserID = userID;
        }
        public PimpUser (String  userName)
        {
            this.UserName= userName;
        }
        public PimpUser()
        { }
        public PimpUser(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public Guid UserID { get; set; }
        public List<ProvinceClass> ProvincesOwned { get; set; }
        public bool IsUserAdmin { get; set; }

        public Guid StartingKingdom { get; set; }
        public Guid CurrentActiveProvince { get; set; }
        public string CurrentActiveProvinceName { get; set; }
        public CS_Code.Utopia_Target_Finder_Setting TargetFinderSettings { get; set; }
        public MonarchType MonarchType { get; set; }

        public List<ColumnSet> UserColumns { get; set; }
        public string UserName { get; set; }
        public DateTime LastUpdated { get; set; }
        public string NickName { get; set; }
        public List<IMType> IMInformation { get; set; }
        public UserPreferences.NotificationSettings NotificationSettings { get; set; }
        public string Password { get; set; }

       





    }
}