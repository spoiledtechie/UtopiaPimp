<%@ WebService Language="C#" Class="ContactList" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Pimp.UCache;
using SupportFramework.Data;
using Pimp.UData;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ContactList : System.Web.Services.WebService
{

    [WebMethod(EnableSession = true)]
    public bool UpdateContact(string city, string country, int gmt, string nickName, string state, int day, int month, int year, string notes, string ownerKingdomID)
    {
        Session["SubmittedData"] = city + ":" + country + ":" + gmt + ":" + nickName + ":" + state + ":" + day + ":" + month + ":" + year + ":" + notes;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return UsersData.UpdateContactInfo(city, country, gmt, nickName, state, day, month, year, notes, new Guid(ownerKingdomID), pimpUser, KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return false;
    }

    [WebMethod(EnableSession = true)]
    public bool AddPhoneNumbers(string phoneNumber, string type, string sms, string currentUserID, string ownerKingdomID)
    {
        Session["SubmittedData"] = phoneNumber + ":" + type + ":" + sms;
        try
        {
            if (sms == "on")
                sms = "1";
            else
                sms = "0";
            return UsersData.AddPhoneNumber(phoneNumber, type, Convert.ToInt32(sms),  new Guid(currentUserID), new Guid(ownerKingdomID),           KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return false;
    }
    [WebMethod(EnableSession = true)]
    public bool AddImName(string imInfo, string imType, string passwordBool, string currentUserID, string ownerKingdomID)
    {
        Session["SubmittedData"] = imInfo + ":" + imType;
        try
        {
            if (passwordBool == "on")
                return IM.AddIM(imInfo, imType, 1, new Guid(currentUserID), new Guid(ownerKingdomID),           KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            else
                return IM.AddIM(imInfo, imType, 0, new Guid(currentUserID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return false;
    }
    [WebMethod(EnableSession = true)]
    public bool DeleteImName(int uid, string ownerKingdomID, string currentUserID)
    {
        Session["SubmittedData"] = uid + ":";
        try
        {
            return IM.DeleteIM(uid, new Guid(ownerKingdomID), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return false;
    }
    [WebMethod(EnableSession = true)]
    public bool DeletePhoneNumber(int uid, string ownerKingdomID, string currentUserID)
    {
        Session["SubmittedData"] = uid + ":";
        try
        {
            return UsersData.DeletePhoneNumber(uid, new Guid(ownerKingdomID), new Guid(currentUserID),           KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return false;
    }

}

