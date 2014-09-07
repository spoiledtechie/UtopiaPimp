<%@ WebService Language="C#" Class="MonarchHelper" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Pimp.UCache;
using SupportFramework.Data;
using Pimp.UData;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MonarchHelper : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public bool AddSubMonarch(string provID, string ownerKingdomID)
    {
        Session["SubmittedData"] = provID;
        try
        {
            Session.Remove("MonarchType");
            Province.SubMonarchToggle(provID, new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            return true;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        }
        Session.Remove("SubmittedData");
        return false;
    }
    [WebMethod(EnableSession = true)]
    public string SetKdLimit(string value, string ownerKingdomID, string currentUserID)
    {
        Session["SubmittedData"] = value;
        try
        {
            if (Convert.ToInt32(value) > 0)
                value = (Convert.ToInt32(value) * -1).ToString();
            Kingdom.AddKdTimeLimit(value, new Guid(ownerKingdomID), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            return value;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    /// <summary>
    /// Adds the AttackOps time limit.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string SetKdOpAttackLimit(string value, string ownerKingdomID, string currentUserID)
    {
        Session["SubmittedData"] = value;
        try
        {
            if (value != "" && Convert.ToInt32(value) > 0)
                value = (Convert.ToInt32(value) * -1).ToString();
            Kingdom.AddKdOpAttackTimeLimit(value, new Guid(ownerKingdomID), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            return value;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string AddIRCKingdomChannel(string name, string password, string ownerKingdomID)
    {
        Session["SubmittedData"] = name + ":" + ownerKingdomID;
        try
        {
            name = name.Replace("#", "");
            Irc.AddIRCChannel(name, password, new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            return name;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string DeleteIRCKindomChannel(string name, string ownerKingdomID)
    {
        Session["SubmittedData"] = name + ":" + ownerKingdomID;
        try
        {
            Irc.DeleteIRCChannel(name, new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            return name;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
}

