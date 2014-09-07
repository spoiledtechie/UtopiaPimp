<%@ WebService Language="C#" Class="MainMaster" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Pimp.UIBuilder;
using Pimp.UParser;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp.UData;
using Pimp;
using Pimp.Users;
using SupportFramework.Data;
using Boomers.Utilities.Guids;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MainMaster : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string AddItem(string data, string provinceName, string targetedGuids)
    {
        string dataAdded;
        //The Province name isn't needed in this type because I am not starting a brand new kingdom.
        Session["SubmittedData"] = data;
        try
        {
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            if (pimpUser.PimpUser.CurrentActiveProvince == null || pimpUser.PimpUser.CurrentActiveProvince == new Guid())
            {
                return UtopiaParser.ReturnErrorsToUser(ErrorTypeEnum.CurrentActiveProvinceNotFound);
            }
            dataAdded = UtopiaParser.UtopiaParsing(data, "AddData", "1", provinceName, targetedGuids, pimpUser, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
        }
        catch (Exception e)
        {
            Errors.logError(e);
            dataAdded = UtopiaParser.ReturnErrorsToUser(ErrorTypeEnum.SomethingWentWrong);
        }
        Session.Remove("SubmittedData");
        //For the Updated Message....
        Session.Remove("AddedTypeData");
        return dataAdded;
    }


    [WebMethod(EnableSession = true)]
    public bool RetireKd(string ownerKingdomID, string kdID)
    {
        Session["SubmittedData"] = ownerKingdomID + ":" + kdID;
        try
        {
            Kingdom.retireKingdom(new Guid(kdID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
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
    public bool UnRetireKd(string ownerKingdomID, string kdID)
    {
        Session["SubmittedData"] = ownerKingdomID + ":" + kdID;
        try
        {
            Kingdom.unRetireKingdom(new Guid(kdID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
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
    public bool UpdateKingdomStatus(string ownerKingdomID, string kdID, string status)
    {
        Session["SubmittedData"] = ownerKingdomID + ":" + kdID;
        try
        {
            Kingdom.updateKingdomStatus(new Guid(kdID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)), status);
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
    public String[] RefreshUtopianTime()
    {
        try
        {
            return UtopiaParser.DisplayUtopianDateTime();
        }
        catch (Exception e)
        {
            Errors.logError(e);
        }
        return new string[1];
    }
    [WebMethod(EnableSession = true)]
    public void UpdateUserActivityTime(Guid userID, Guid appID)
    {
        Session["SubmittedData"] = userID.ToString() + ":" + appID;
        try
        {
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            UsersData.UpdateLastActivityDate(pimpUser.PimpUser);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        }
        Session.Remove("SubmittedData");
    }
    [WebMethod(EnableSession = true)]
    public string RefreshAd()
    {
        try
        {
            string item = @"<map name='admap37989' id='admap37989'><area href='http://www.projectwonderful.com/out_nojs.php?r=0&amp;c=0&amp;id=37989&amp;type=5' shape='rect' coords='0,0,728,90' title='' alt='' target='_blank' /></map>";
            item += @"<table cellpadding='0' border='0' cellspacing='0' width='728' bgcolor=''><tr><td><img src='http://www.projectwonderful.com/nojs.php?id=37989&amp;type=5' width='728' height='93' usemap='#admap37989' border='0' alt='' /></td></tr><tr><td bgcolor='' colspan='1'><center><a style='font-size:10px;color:#fff;text-decoration:none;line-height:1.2;font-weight:bold;font-family:Tahoma, verdana,arial,helvetica,sans-serif;text-transform: none;letter-spacing:normal;text-shadow:none;white-space:normal;word-spacing:normal;' href='http://www.projectwonderful.com/advertisehere.php?id=37989&amp;type=5' target='_blank'>Your ad could be here, right now.</a></center></td></tr></table>";
            return item;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string LoadKingdomGrid(string kdType, string columnSetID, string addedSets, string kingdomID, string ownerKingdomID)
    {
        // Boomers.Utilities.Documents.TextLogger.LogItem("utopiapimp", "LoadKingdomGrid" + kdType + ":" + columnSetID + ":" + addedSets + ":" + kingdomID + ":" + ownerKingdomID);
        
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled|System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(ownerKingdomID))
                    ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>","");
        
        Session["SubmittedData"] = kdType + ":" + columnSetID + ":" + addedSets + ":" + kingdomID + ":" + ownerKingdomID;
        if (!Boomers.Utilities.Guids.GuidExt.IsValidGuid(kingdomID) || columnSetID.Length == 0)
        {
            return "Your URL is Malformed so that Kingdom Data cannot be retrieved.  Please Click on your Home Kingdom to Make it work.  Contact Spoiledtechie@gmail.com if this problem persists.";
        }
        PimpUserWrapper pimpUser = new PimpUserWrapper();
        try
        {
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            if (pimpUser.PimpUser.UserID != new Guid())
            {
                string columns = UtopiaParser.GetUserColumnsSet(Convert.ToInt32(columnSetID), pimpUser.PimpUser.UserColumns);
                columns += addedSets;
                return KdPage.loadDynamicGrid(new Guid(kingdomID), kdType, columns, new Guid(ownerKingdomID), pimpUser.PimpUser.MonarchType, pimpUser.PimpUser.UserID, cachedKingdom);
            }
            //Errors.failedAt("'loadingKingdomData'", Boomers.Utilities.Objects.ObjectSerializer.toJson(pimpUser.PimpUser), pimpUser.PimpUser.UserID);
            return string.Empty;
        }
        catch (Exception e)
        {
            Errors.logError(e, Boomers.Utilities.Objects.ObjectSerializer.toJson(pimpUser.PimpUser));
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string LoadOpsSummary(string kdType, string kingdomID, string ownerKingdomID)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled|System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if(teleCheck.IsMatch(ownerKingdomID))
            ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>", "");
        
        Session["SubmittedData"] = kdType + ":" + kingdomID + ":" + ownerKingdomID;
        try
        {
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            if (kingdomID.IsValidGuid())
                return KdPage.loadOpsHistory(kdType, new Guid(kingdomID), new Guid(ownerKingdomID), pimpUser, KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            else
                return null;
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string LoadKingdomSummary(string kingdomID, string ownerKingdomID, string currentProvId, string currentUserID)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(ownerKingdomID))
            ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>", "");
        if (teleCheck.IsMatch(currentUserID))
            currentUserID = teleCheck.Replace(currentUserID, "").Replace("</a>", "");
        
        Session["SubmittedData"] = kingdomID + " owner:" + ownerKingdomID + " provId:" + currentProvId + " userId" + currentUserID;
        try
        {
            return FrontPage.kdSummary(new Guid(kingdomID), new Guid(ownerKingdomID), new Guid(currentProvId), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
}

