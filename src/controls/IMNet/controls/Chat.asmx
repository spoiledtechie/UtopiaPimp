<%@ WebService Language="C#" Class="Chat" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Security;
using System.Collections.Generic;

using Pimp.UCache;
using Pimp.UData;
using Pimp;
using SupportFramework;
using Pimp.Users;
using Pimp.Chat;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Chat : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string keepMeAlive()
    {
        //string[] validation = new string[4];
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            UsersData.UpdateLastActivityDate( pimpUser.PimpUser);
            
        }
        return PimpUserWrapper .getUsersOnline().Count.ToString();
    }
    /// </summary>
    /// <param name="sender">the sender of the message</param>
    /// <param name="recipient">the receiver of the message in which this is the Group name.</param>
    /// <param name="lastUid">the id of the last message recieved.</param>
    /// <returns>returns the formmated messages.</returns>
    [WebMethod(EnableSession = true)]
    public string[] getGroupMessagesChatting(string sender, string recipient, string lastUidChecked)
    {
        return IM.getGroupChatMessagesChatting(new Guid(sender), new Guid(recipient), Convert.ToInt32(lastUidChecked));//gets the group chat messages.
    }
    [WebMethod(EnableSession = true)]
    public string GetUsersOnline(string recipient)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();

        return IM.GetUsersOnline(recipient, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));//gets the group chat messages.
    }
    [WebMethod(EnableSession = true)]
    public string GetServerTime()
    {
        return DateTime.UtcNow.ToLongTimeString();
    }
    [WebMethod(EnableSession = true)]
    public void ClearOldMessages()
    {
        IM.clearOldMessages();
    }

    /// <summary>
    /// inserts a group messaged.
    /// </summary>
    /// <param name="message">the message to insert</param>
    /// <param name="sender">the sender of the message</param>
    /// <param name="recipient">the reciever of the message.</param>
    [WebMethod(EnableSession = true)]
    [System.Web.Script.Services.ScriptMethod()]
    public void setGroupMessage(string message, string sender, string recipient)
    {
        message = message.Replace("<p>", "").Replace("</p>", "");//strips out the paragraph text.
        IM.insertChatMessage(message, new Guid(recipient), new Guid(sender));//inserts the message.
    }



}