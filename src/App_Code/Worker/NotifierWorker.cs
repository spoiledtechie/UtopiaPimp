using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boomers.Utilities.Communications;
using System.Text;
using CS_Code;

using System.Data.Linq;

using Pimp.Users;
using PimpLibrary.Static.Enums;
using PimpLibrary.Communications;
using Pimp;


namespace App_Code.CS_Code.Worker
{
    /// <summary>
    /// Summary description for NotifierWorker
    /// </summary>
    public static class NotifierWorker
    {
        /// <summary>
        /// used to lock the thread to load the user cache.
        /// </summary>
        private static readonly object ThisLock = new object();
        /// <summary>
        /// private variable that contains the user information for notifications.
        /// </summary>
        static Dictionary<Guid, EmailNotificationWrapper> _userNotifications; // UserId and email settings                
        /// <summary>
        /// gets the messages from the DB to be sent.
        /// </summary>
        /// <returns></returns>
        public static List<MailMessage> GetEmailMessages()
        {
            var messages = new List<MailMessage>();
            UtopiaDataContext ctx = new UtopiaDataContext();

            //gets all the records that havent been sent yet.
            var records = ctx.Utopia_Province_Notifiers.Where(x => x.Sent == false && DateTime.Now >= x.SendTime);
            //updates records for each item about to be sent.
            foreach (var data in records)
            {
                var message = new MailMessage();
                message.CompanyName = "UtopiaPimp";
                message.ToName = data.UserName;
                message.ToEmail = data.Email;
                message.Subject = "Report from UtopiaPimp";
                message.Message = GenerateReport(data.Utopia_Province_Notifier_Datas, ref ctx);
                messages.Add(message);
                data.Sent = true;
            }
            ctx.SubmitChanges();
            return messages;
        }
        /// <summary>
        /// Generates the report getting ready to be sent to the user.
        /// </summary>
        /// <param name="details"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        static string GenerateReport(EntitySet<Utopia_Province_Notifier_Data> details, ref UtopiaDataContext ctx)
        {
            StringBuilder output = new StringBuilder();
            foreach (var detail in details)
            {
                output.AppendLine(detail.EventText + "<br />");
                ctx.Utopia_Province_Notifier_Datas.DeleteOnSubmit(detail);
            }
            return output.ToString();
        }
        /// <summary>
        /// gets the notification type to send to the user.
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        static string GenerateNotificationDetail(NotificationDetail detail)
        {
            var output = new StringBuilder();
            output.Append(Boomers.Utilities.DatesTimes.Formatting.Month(detail.Date.Month) + " " + detail.Date.Day.ToString() + " of YR" + detail.Date.Year.ToString() + " ");

            switch (detail.EventType)
            {
                case CeTypeEnum.ArmyReturns:
                    output.AppendLine("Your army has returned sire.");
                    break;
                case CeTypeEnum.CaputeredLand:
                case CeTypeEnum.CaputeredLandIntraKingdom:
                case CeTypeEnum.AttackedAndStole:
                case CeTypeEnum.Massacred:
                case CeTypeEnum.RazedProvince:
                case CeTypeEnum.TheyDeclaredWar:
                case CeTypeEnum.WeDeclaredWar:

                case CeTypeEnum.EmeraldDragonRavagingLands:
                case CeTypeEnum.RubyDragonProjectAgainstUs:
                case CeTypeEnum.RubyDragonRavagingLands:
                case CeTypeEnum.StartedEmeraldDragonProjectAgainstUs:
                case CeTypeEnum.DragonProjectStartedEmerald:
                case CeTypeEnum.DragonProjectStartedGold:
                case CeTypeEnum.DragonProjectStartedRuby:
                case CeTypeEnum.DragonProjectStartedSapphire:
                case CeTypeEnum.DragonRavagingLands:

                case CeTypeEnum.OpsEnding:
                case CeTypeEnum.TrackedProvinceUpdated:
                    output.AppendLine(detail.EventText);
                    break;
            }
            return output.ToString();
        }

        /// <summary>
        /// checks if the user wants the email to be sent to them.
        /// </summary>
        /// <param name="utopiaEvent"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        static bool CheckIfUserWantsEmail(CeTypeEnum utopiaEvent, ref EmailNotificationWrapper user)
        {
            switch (utopiaEvent)
            {
                case CeTypeEnum.ArmyReturns:
                    return user.ArmyReturns;
                case CeTypeEnum.CaputeredLandIntraKingdom:
                case CeTypeEnum.AttackedAndStole:
                case CeTypeEnum.Massacred:
                case CeTypeEnum.RazedProvince:
                case CeTypeEnum.CaputeredLand:
                    return user.AttackAgainstSelf;
                case CeTypeEnum.RubyDragonProjectAgainstUs:
                case CeTypeEnum.StartedEmeraldDragonProjectAgainstUs:
                case CeTypeEnum.DragonProjectStartedEmerald:
                case CeTypeEnum.DragonProjectStartedGold:
                case CeTypeEnum.DragonProjectStartedRuby:
                case CeTypeEnum.DragonProjectStartedSapphire:
                    return user.DragonProjectStarted;
                case CeTypeEnum.EmeraldDragonRavagingLands:
                case CeTypeEnum.DragonRavagingLands:
                case CeTypeEnum.RubyDragonRavagingLands:
                    return user.DragonRavagingLands;
                case CeTypeEnum.OpsEnding:
                    return user.OpsEnded;
                case CeTypeEnum.TrackedProvinceUpdated:
                    return user.TrackedProvinceUpdated;
                case CeTypeEnum.TheyDeclaredWar:
                case CeTypeEnum.WeDeclaredWar:
                    return user.WarAction;
            }
            return false;
        }

        public static void Submit(List<Notification> input)
        {
            if (input.Count == 0)
                return;

            LoadCache();

            List<Guid> provincesWithoutInfo = (from data in input
                                               where !_userNotifications.ContainsKey(data.UserId)
                                               select data.UserId).ToList();

            if (provincesWithoutInfo.Count > 0)
            {
                var cx1 = new UtopiaDataContext();
                var cx2 = new AdminDataContext();

                var provincesWithADataEntry = (from data in cx1.Utopia_User_Notifier_Settings
                                               where provincesWithoutInfo.Contains(data.User_ID)
                                               select data).ToList();

                if (provincesWithoutInfo.Count > provincesWithADataEntry.Count)
                {
                    foreach (var province in provincesWithoutInfo)
                        if (provincesWithADataEntry.Count(x => x.User_ID == province) == 0)
                            cx1.Utopia_User_Notifier_Settings.InsertOnSubmit(new Utopia_User_Notifier_Setting { User_ID = province });

                    cx1.SubmitChanges();
                }

                //var usersToInsert = provincesWithoutInfo.Where(user => !cx1.Utopia_User_Notifier_Settings.Contains(user)).ToList();

                var userNotificationData = (from data in cx1.Utopia_User_Notifier_Settings
                                            where provincesWithoutInfo.Contains(data.User_ID)
                                            select new
                                                       {
                                                           ArmyReturns = data.ArmyReturns,
                                                           AttackOnSelf = data.AttacksOnSelf,
                                                           DragonProjectStarted = data.DragonProjectStarted,
                                                           DragonRavagingLands = data.DragonRavagingLands,
                                                           OpsEnding = data.OpsEnding,
                                                           TrackedProvinceUpdated = data.TrackedProvinceUpdated,
                                                           WarAction = data.WarAction,
                                                           UserId = data.User_ID,
                                                           data.FrequencyInMinutes,
                                                           DeliveryMethod = (UserPreferences.NotificationSettings.NotificationType)Enum.Parse(typeof(UserPreferences.NotificationSettings.NotificationType), data.SendMethod.ToString())
                                                       }).ToList();

                var userEmailData = (from data in cx2.vw_aspnet_MembershipUsers
                                     where provincesWithoutInfo.Contains(data.UserId)
                                     select new
                                                {
                                                    data.UserId,
                                                    Username = data.UserName,
                                                    data.Email,
                                                }).ToList();

                var userData = from data in userNotificationData
                               join data2 in userEmailData on data.UserId equals data2.UserId
                               join data3 in input on data.UserId equals data3.UserId
                               select new
                                          {
                                              data.ArmyReturns,
                                              data.AttackOnSelf,
                                              data.DragonProjectStarted,
                                              data.DragonRavagingLands,
                                              data.OpsEnding,
                                              data.TrackedProvinceUpdated,
                                              data.WarAction,
                                              data.UserId,
                                              data.FrequencyInMinutes,
                                              data.DeliveryMethod,
                                              data2.Email,
                                              data2.Username,
                                              data3.ProvinceId,
                                              data3.ProvinceName
                                          };

                foreach (var userDetails in userData)
                {
                    if (_userNotifications.ContainsKey(userDetails.UserId))
                        continue;

                    var ue = new EmailNotificationWrapper
                                 {
                                     ArmyReturns = userDetails.ArmyReturns,
                                     AttackAgainstSelf = userDetails.AttackOnSelf,
                                     DragonProjectStarted = userDetails.DragonProjectStarted,
                                     DragonRavagingLands = userDetails.DragonRavagingLands,
                                     Email = userDetails.Email,
                                     SendFrequency = new TimeSpan(0, 0, userDetails.FrequencyInMinutes, 0),
                                     LastEmailSent = DateTime.Now.AddYears(-1),
                                     OpsEnded = userDetails.OpsEnding,
                                     TrackedProvinceUpdated = userDetails.TrackedProvinceUpdated,
                                     UserId = userDetails.UserId,
                                     UserName = userDetails.Username,
                                     WarAction = userDetails.WarAction,
                                     DeliveryMethod = userDetails.DeliveryMethod
                                 };
                    _userNotifications.Add(userDetails.UserId, ue);
                }

                SaveCache();
            }

            // Merge duplicates
            var detailsDictionary = new Dictionary<Guid, Notification>();
            foreach (var userInput in input)
            {
                if (detailsDictionary.ContainsKey(userInput.UserId))
                {
                    detailsDictionary[userInput.UserId].Details.AddRange(userInput.Details);
                }
                else
                {
                    detailsDictionary.Add(userInput.UserId, userInput);
                }
            }

            // Loop through and conver the response to emailnotification wrappers.
            foreach (var userInput in detailsDictionary)
            {
                if (userInput.Value.Details.Count == 0) // No details in the post (no events for that user)
                    continue;

                var userInfo = _userNotifications[userInput.Key];
                DateTime sendTime = userInfo.LastEmailSent.Add(userInfo.SendFrequency);

                using (UtopiaDataContext ctx = new UtopiaDataContext())
                {
                    var record = ctx.Utopia_Province_Notifiers.FirstOrDefault(x => x.Province_ID == userInput.Value.ProvinceId);
                    if (record == null) // The user doesn't have a post in the db
                    {
                        Utopia_Province_Notifier dbNotifier = new Utopia_Province_Notifier();
                        foreach (var notifierDetail in userInput.Value.Details)
                        {
                            if (CheckIfUserWantsEmail(notifierDetail.EventType, ref userInfo))
                            {
                                dbNotifier.Utopia_Province_Notifier_Datas.Add(
                                    new Utopia_Province_Notifier_Data
                                    {
                                        BelongsToProvince_ID = userInput.Value.ProvinceId,
                                        EventType = (byte)notifierDetail.EventType,
                                        EventText = GenerateNotificationDetail(notifierDetail),
                                    });
                            }
                        }
                        if (dbNotifier.Utopia_Province_Notifier_Datas.Count == 0) // The user didn't want any notifications that was posted, continue to the next user
                            continue;

                        dbNotifier.Province_ID = userInput.Value.ProvinceId;
                        dbNotifier.User_ID = userInfo.UserId;
                        dbNotifier.UserName = userInfo.UserName;
                        dbNotifier.Email = userInfo.Email;
                        dbNotifier.ProvinceName = userInput.Value.ProvinceName;
                        dbNotifier.SendTime = DateTime.Now;
                        ctx.Utopia_Province_Notifiers.InsertOnSubmit(dbNotifier); // Save upon the next submitchanges
                    }
                    else // The user has a post in the db for this province, check if the status is already sent.
                    {
                        List<Utopia_Province_Notifier_Data> notifierItems = new List<Utopia_Province_Notifier_Data>();
                        foreach (var notifierDetail in userInput.Value.Details)
                        {
                            if (CheckIfUserWantsEmail(notifierDetail.EventType, ref userInfo))
                            {
                                notifierItems.Add(
                                    new Utopia_Province_Notifier_Data
                                    {
                                        BelongsToProvince_ID = userInput.Value.ProvinceId,
                                        EventType = (byte)notifierDetail.EventType,
                                        EventText = GenerateNotificationDetail(notifierDetail),
                                    });
                            }
                        }
                        if (notifierItems.Count == 0) // The user didn't want any notifications that was posted, continue to the next user
                            continue;

                        if (record.Sent == true)
                        {
                            record.SendTime.Add(userInfo.SendFrequency);
                            record.Sent = false;
                        }
                        record.Utopia_Province_Notifier_Datas.AddRange(notifierItems);
                    }
                    ctx.SubmitChanges();
                }
            }
        }
        /// <summary>
        /// loads the cache for the user notification and their user information
        /// </summary>
        static void LoadCache()
        {
            if (_userNotifications == null)
            {
                _userNotifications = (Dictionary<Guid, EmailNotificationWrapper>)HttpRuntime.Cache["UserNotificationInformation"];
                if (_userNotifications == null)
                {
                    lock (ThisLock)  //locks the thread to load the user notification cache.
                    {
                        _userNotifications = (Dictionary<Guid, EmailNotificationWrapper>)HttpRuntime.Cache["UserNotificationInformation"];
                        if (_userNotifications == null)
                        {
                            _userNotifications = new Dictionary<Guid, EmailNotificationWrapper>();
                            HttpRuntime.Cache.Add("UserNotificationInformation", _userNotifications, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Default, null);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Inserts item into cache
        /// </summary>
        static void SaveCache()
        {
            lock (ThisLock)
            {
                HttpRuntime.Cache.Insert("UserNotificationInformation", _userNotifications);
            }
        }



        class NotificationWrapper
        {
            public NotificationWrapper()
            {
                Details = new List<NotificationDetail>();
            }

            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public List<NotificationDetail> Details { get; set; }
            public DateTime SendTime { get; set; }
            public UserPreferences.NotificationSettings.NotificationType TypeOfNotification { get; set; }
        }

        class EmailNotificationWrapper : UserPreferences.NotificationSettings
        {
            public Guid UserId { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
        }
        /// <summary>
        /// updates the user preferences to the cache.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="preferences"></param>
        public static void UpdateUserPreferences(Guid userId, UserPreferences.NotificationSettings preferences)
        {
            LoadCache();
            if (_userNotifications.ContainsKey(userId))
            {
                var userData = _userNotifications[userId];
                userData.ArmyReturns = preferences.ArmyReturns;
                userData.AttackAgainstSelf = preferences.AttackAgainstSelf;
                userData.DeliveryMethod = preferences.DeliveryMethod;
                userData.DragonProjectStarted = preferences.DragonProjectStarted;
                userData.DragonRavagingLands = preferences.DragonRavagingLands;
                userData.OpsEnded = preferences.OpsEnded;
                userData.SendFrequency = preferences.SendFrequency;
                userData.TrackedProvinceUpdated = preferences.TrackedProvinceUpdated;
                userData.WarAction = preferences.WarAction;
                SaveCache();
            }
        }
    }
}

