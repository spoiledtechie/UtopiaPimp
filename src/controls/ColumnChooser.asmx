<%@ WebService Language="C#" Class="ColumnChooser" %>

using System;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Pimp.UParser;
using Pimp.UCache;
using Pimp.UIBuilder;
using SupportFramework.Data;
using Pimp.UData;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ColumnChooser : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string LoadColumnSets(string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = userID + ":" + kingdomID + ":" + ownerKingdomID;
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        try
        {
            
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);

            return Columns.CreateColumnSetsForDisplay(new Guid(kingdomID), null, null, pimpUser, cachedKingdom);

            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: LoadColumnSets, " + secret + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e, Boomers.Utilities.Objects.ObjectSerializer.toJson(pimpUser.PimpUser));
        }
        Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string LoadColumnsSelected(string setSelectedID, string userID, string kingdomID, string secret)
    {
        Session["SubmittedData"] = setSelectedID + ":" + userID + ":" + kingdomID + ":" + secret;
        try
        {

            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Columns.DisplayColumnsForDisplay(Convert.ToInt32(setSelectedID), new Guid(kingdomID), new Guid(userID), pimpUser, KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string AddKingdomSetName(string setName, string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = setName + ":" + userID + ":" + kingdomID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            return Column.AddSetNameToKingdom(new Guid(kingdomID), setName, pimpUser, cachedKingdom);
            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: AddKingdomSetName, " + setName + ", " + secret + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string AddSetName(string setName, string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = setName + ":" + userID + ":" + kingdomID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(new Guid(ownerKingdomID));
            return Column.AddSetNameToUser(new Guid(kingdomID), setName, pimpUser.PimpUser.StartingKingdom, pimpUser.PimpUser.UserID, cachedKingdom);
            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: AddSetName, " + setName + ", " + secret + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string DeleteSetName(string setID, string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = setID + ":" + userID + ":" + kingdomID + ":" + ownerKingdomID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            return Column.DeleteSetForUser(new Guid(kingdomID), Convert.ToInt32(setID), pimpUser.PimpUser.UserID, cachedKingdom);
            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: DeleteSetName, " + setID + ", " + ownerKindomID + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string DeleteKingdomSetName(string setID, string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = setID + ":" + userID + ":" + kingdomID + ":" + ownerKingdomID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);

            return Column.DeleteSetForKingdom(new Guid(kingdomID), Convert.ToInt32(setID), new Guid(ownerKingdomID), pimpUser, cachedKingdom);
            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: DeleteKingdomSetName, " + setID + ", " + ownerKindomID + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string AddToMySet(string setID, string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = setID + ":" + userID + ":" + kingdomID + ":" + ownerKingdomID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            return Column.AddToMyColumnSets(new Guid(kingdomID), Convert.ToInt32(setID), pimpUser, cachedKingdom);
            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: AddToMySet, " + setID + ", " + secret + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string AddToKingdomSet(string setID, string userID, string kingdomID, string ownerKingdomID)
    {
        Session["SubmittedData"] = setID + ":" + userID + ":" + kingdomID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(new Guid(ownerKingdomID));
            return Column.AddToKingdomColumnSets(new Guid(kingdomID), Convert.ToInt32(setID), new Guid(ownerKingdomID), pimpUser, cachedKingdom);
            //return "You found me in a bad spot.  I think I broke, but not that sure. [Error: AddToKingdomSet, " + setID + ", " + secret + ", " + kingdomID + ", " + userID + "]";
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SetColumnForUser(string columnID, string setID, string userID, string kingdomID, string secret)
    {
        Session["SubmittedData"] = columnID + ":" + userID + ":" + kingdomID + ":" + secret;
        try
        {
            return Column.ToggleColumnForUser(Convert.ToInt32(columnID), Convert.ToInt32(setID), new Guid(userID), new Guid(kingdomID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SetColumnForKingdom(string columnID, string setID, string userID, string kingdomID, string secret)
    {
        Session["SubmittedData"] = columnID + ":" + setID + ":" + userID + ":" + kingdomID + ":" + secret;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Column.ToggleColumnForKingdom(Convert.ToInt32(columnID), Convert.ToInt32(setID), new Guid(userID), new Guid(kingdomID), KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string UpColumnForUser(string columnID, string setID, string userID, string secret)
    {
        Session["SubmittedData"] = setID + ":" + columnID + ":" + userID + ":" + secret;
        try
        {
            return Column.UpColumnForUser(columnID, Convert.ToInt32(setID), new Guid(userID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string UpColumnForKingdom(string columnID, string setID, string kingdomID, string secret)
    {
        Session["SubmittedData"] = setID + ":" + columnID + ":" + kingdomID + ":" + secret;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Column.UpColumnForKingdom(columnID, Convert.ToInt32(setID), new Guid(kingdomID), KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string DownColumnForUser(string columnID, string setID, string userID, string secret)
    {
        Session["SubmittedData"] = setID + ":" + columnID + ":" + userID + ":" + secret;
        try
        {
            return Column.DownColumnForUser(columnID, Convert.ToInt32(setID), new Guid(userID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }

    [WebMethod(EnableSession = true)]
    public string DownColumnForKingdom(string columnID, string setID, string kingdomID, string secret)
    {
        Session["SubmittedData"] = setID + ":" + columnID + ":" + kingdomID + ":" + secret;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Column.DownColumnForKingdom(columnID, Convert.ToInt32(setID), new Guid(kingdomID), KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
}

