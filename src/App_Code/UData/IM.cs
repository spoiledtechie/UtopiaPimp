using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text;

using Pimp.UCache;
using Pimp.UData;
using Pimp;
using Pimp.Utopia;
using Pimp.Users;
using System.Threading.Tasks;
using Pimp.Chat;
using Boomers.Utilities.Web;
using Boomers.UserUtil;
using SupportFramework;

namespace Pimp.UData
{   /// <summary>
    /// Summary description for IM
    /// </summary>
    public class IM
    {
        public static bool AddIM(string im, string type, int passwordBool, Guid userID, Guid ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
            var getOldIMs = (from xx in adb.user_IMs
                             where xx.User_ID == userID
                             select xx);
            int dirtyBit = 0;
            foreach (var item in getOldIMs)
                if (item.IM_Name == im)
                    dirtyBit = 1;

            if (dirtyBit == 0)
            {
                CS_Code.user_IM pn = new CS_Code.user_IM();
                pn.IM_Name = im;
                pn.IM_Type = GetIMType(type);
                pn.User_ID = userID;
                pn.Application_ID = Applications.Instance.ApplicationId;
                pn.IM_Password_Bool = passwordBool;
                adb.user_IMs.InsertOnSubmit(pn);
                adb.SubmitChanges();
                UsersCache.updateContactForUser(ownerKingdomID, userID, (Guid)pn.Application_ID, cachedKingdom);
            }
            return true;
        }
   

        private static int GetIMType(string type)
        {
            var getType = GetIMTypes.Where(x => x.IM_Type == type).Select(x => x.uid).FirstOrDefault();
            if (getType == 0)
            {
                CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
                CS_Code.user_IM_Type_Pull pp = new CS_Code.user_IM_Type_Pull();
                pp.IM_Type = type;
                adb.user_IM_Type_Pulls.InsertOnSubmit(pp);
                adb.SubmitChanges();
                return pp.uid;
            }
            return getType;
        }
 public static bool DeleteIM(int uid, Guid ownerKingdomID, Guid userID, OwnedKingdomProvinces cachedKingdom)
    {
        CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
        var getOldNumbers = (from xx in adb.user_IMs
                             where xx.User_ID == userID
                             where xx.uid == uid
                             select xx).FirstOrDefault();
        adb.user_IMs.DeleteOnSubmit(getOldNumbers);
        adb.SubmitChanges();
        UsersCache.updateContactForUser(ownerKingdomID, userID, Applications.Instance.ApplicationId, cachedKingdom);
        return true;
    }

   

        static List<IMType> _getIMTypes;
        public static List<IMType> GetIMTypes
        {
            get
            {
                if (_getIMTypes == null)
                {
                    if (HttpRuntime.Cache["IMTypes"] == null)
                    {
                        CS_Code.AdminDataContext adb = CS_Code.AdminDataContext.Get();
                        List<IMType> imTypes = (from imp in adb.user_IM_Type_Pulls
                                                orderby imp.IM_Type ascending
                                                select new IMType { uid = imp.uid, IM_Type = imp.IM_Type }).ToList();
                        HttpRuntime.Cache["IMTypes"] = imTypes;
                        _getIMTypes = imTypes;
                    }
                    else
                    {
                        try
                        {
                            _getIMTypes = (List<IMType>)HttpRuntime.Cache["IMTypes"];
                        }
                        catch
                        {
                            HttpRuntime.Cache.Remove("IMTypes");
                            return GetIMTypes;
                        }
                    }
                }
                return _getIMTypes;
            }
        }


        /// <summary>
        /// Inserts a message.
        /// </summary>
        /// <param name="message">message to input.</param>
        /// <param name="recipient">recipient of message</param>
        /// <param name="sender">sender of message</param>
        public static void insertChatMessage(string message, Guid messageTo, Guid messageFrom)
        {
           
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                CS_Code.Global_Chat im = new CS_Code.Global_Chat();
                im.Message = message;
                im.Message_To = messageTo;
                im.Message_From = messageFrom;
                im.Sent = DateTime.UtcNow;
                im.Application_ID = SupportFramework.Applications.Instance.ApplicationId;
                db.Global_Chats.InsertOnSubmit(im);
                db.SubmitChanges();
            
        }
        /// <summary>
        /// gets any group messages that have been stored.
        /// </summary>
        /// <param name="sender">sender of the messages</param>
        /// <param name="recipient">receiver of the messages</param>
        /// <param name="MaxUid">the last id of the messages received.</param>
        /// <returns>The formatted messages.</returns>
        public static string[] getGroupChatMessagesStartChat(Guid messageFrom, Guid messageTo)
        {
            string[] messagesRecipient = new string[2];
            try
            {
                CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                List<Message> messages = (from im in db.Global_Chats
                                          where (im.Message_To == messageTo)
                                          where (im.Sent > DateTime.UtcNow.AddMinutes(-10))
                                          select new Message
                                          {
                                              FromGuid = im.Message_From,
                                              messageFrom = SupportFramework.Users.Memberships.getUserName(im.Message_From),
                                              timeStamp = im.Sent,
                                              message = im.Message,
                                              uid = im.uid
                                          }).ToList();
                if (messages.Count == 0)
                    messagesRecipient[1] = (from im in db.Global_Chats where im.Message_To == messageTo select im.uid).Take(1).FirstOrDefault().ToString();
                else
                    messagesRecipient[1] = messages.Last().uid.ToString();
                StringBuilder sb = buildMessages(messageFrom, messages);
                messagesRecipient[0] = sb.ToString();

                return messagesRecipient;
            }
            catch
            { }
            return messagesRecipient;
        }
        /// <summary>
        /// gets the group chat messages
        /// </summary>
        /// <param name="messageFrom"></param>
        /// <param name="messageTo"></param>
        /// <param name="lastChecked"></param>
        /// <returns></returns>
        public static string[] getGroupChatMessagesChatting(Guid messageFrom, Guid messageTo, int lastChecked)
        {
            string[] messagesRecipient = new string[2];

            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            List<Message> messages = (from im in db.Global_Chats

                                      where im.Message_To == messageTo
                                      where im.uid > lastChecked
                                      where im.Message_From != messageFrom
                                      select new Message
                                      {
                                          FromGuid = im.Message_From,
                                          timeStamp = im.Sent,
                                          message = im.Message,
                                          uid = im.uid
                                      }).ToList();

            StringBuilder sb = buildMessages(messageFrom, messages);
            messagesRecipient[0] = sb.ToString();
            if (messages.Count == 0)
                messagesRecipient[1] = string.Empty;
            else
                messagesRecipient[1] = messages.Last().uid.ToString();
            return messagesRecipient;
        }
        /// <summary>
        /// clears old chat messages
        /// </summary>
        public static void clearOldMessages()
        {
          
                  CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                  var mess = (from im in db.Global_Chats
                              where (im.Sent < DateTime.UtcNow.AddMinutes(-15))
                              select im);
                  db.Global_Chats.DeleteAllOnSubmit(mess);
                  db.SubmitChanges();
             
        }
        /// <summary>
        /// builds the chat message string
        /// </summary>
        /// <param name="messageFrom"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static StringBuilder buildMessages(Guid messageFrom, List<Message> messages)
        {
            List<PimpUser> users = PimpUserWrapper .getUsersOnline();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < messages.Count; i++)
            {
                if (messageFrom == messages[i].FromGuid)
                    sb.Append("<div class=\"spanReceiver\">" + users.Where(x => x.UserID == messages[i].FromGuid).FirstOrDefault().NickName + ": " + messages[i].message + "</div>");
                else
                    sb.Append("<div class=\"spanSender\">" + users.Where(x => x.UserID == messages[i].FromGuid).FirstOrDefault().NickName + ": " + messages[i].message + "</div>");
            }
            return sb;
        }

        /// <summary>
        /// Returns the web address and application path.  It operates between a secure port SSL encryption or just a regular web address.
        /// </summary>
        /// <returns>the web address of the current address in the domain bar.</returns>
        public static string Server_Port_Secure()
        {
            string ApplicationPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (!Network.IsSSL)
                return "http://" + Network.ServerName + ApplicationPath;
            else
                return "https://" + Network.ServerName + ApplicationPath;
        }
        /// <summary>
        /// gets the users online
        /// </summary>
        /// <param name="ownerKingdomID"></param>
        /// <param name="cachedKingdom"></param>
        /// <returns></returns>
        public static string GetUsersOnline(string ownerKingdomID, OwnedKingdomProvinces cachedKingdom)
        {
            List<PimpUser> list = new List<PimpUser>();
            if (ownerKingdomID == new Guid().ToString())
                list = PimpUserWrapper .getUsersOnline();
            else
                list = UsersData.getUsersOnline(new Guid(ownerKingdomID), cachedKingdom).ToList();

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"ulChatUsers\">");
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append("<li>");
                sb.Append(list[i].NickName);
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

    }

}