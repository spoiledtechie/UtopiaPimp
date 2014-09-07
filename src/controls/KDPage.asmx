<%@ WebService Language="C#" Class="KDPage" %>

using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Pimp.UParser;
using Pimp.UIBuilder;
using Pimp.UCache;
using Pimp.UData;
using Pimp;
using Pimp.Users;
using SupportFramework.Data;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class KDPage : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string ChangeMonarchMessageKd(string message, string kdID, string ownerKingdomID)
    {
        Session["SubmittedData"] = message + ":" + kdID;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Kingdom.SetMonarchMessage(message, kdID, new Guid(ownerKingdomID), pimpUser, KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }

    [WebMethod(EnableSession = true)]
    public string GetProvinces(string kingdomID, string provinces, string ownerKingdomID)
    {
        Session["SubmittedData"] = kingdomID + ":" + provinces;
        try
        {
            return UtopiaParser.GetTargetProvinces(new Guid(kingdomID), provinces, new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    //Gets the notes for the province
    [WebMethod(EnableSession = true)]
    public string GetProvinceNote(string provId, string ownerKingdomID)
    {
        Session["SubmittedData"] = provId + ":"; try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Province.getProvinceNotes(new Guid(provId), new Guid(ownerKingdomID), pimpUser);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    //Gets the notes for the province with the delete button
    [WebMethod(EnableSession = true)]
    public string GetProvinceNoteDelete(string provId, string ownerKingdomID)
    {
        Session["SubmittedData"] = provId + ":";
        try
        {
            return Province.ProvinceNotesGetDelete(new Guid(provId), new Guid(ownerKingdomID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    /// <summary>
    /// Deletes the Province Note.
    /// </summary>
    /// <param name="noteID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DeleteProvinceNote(string noteID, string ownerKingdomID)
    {
        Session["SubmittedData"] = noteID + ":";
        try
        {
            return Province.ProvinceNoteDelete(Convert.ToInt32(noteID), new Guid(ownerKingdomID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    //adds the note created for the province.
    [WebMethod(EnableSession = true)]
    public string AddNote(string provId, string note, string ownerKingdomID, string provinceId)
    {
        Session["SubmittedData"] = provId + ":" + note;
        try
        {
            return Province.AddNote(new Guid(provId), note, new Guid(ownerKingdomID), new Guid(provinceId));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string DeleteKDLessProv(string provId, string ownerKingdomID)
    {
        Session["SubmittedData"] = provId + ":";
        try
        {
            Guid provGuid = new Guid(provId);
            return Province.DeleteProvince(provGuid, new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    /// <summary>
    /// Requests Intel for a province in the kingdom.
    /// </summary>
    /// <param name="provId"></param>
    /// <param name="type"></param>
    [WebMethod(EnableSession = true)]
    public void RequestIntelKd(string provId, string type)
    {
        Session["SubmittedData"] = provId + ":" + type; try
        {
            Guid provGuid = new Guid(provId);
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            Province.RequestIntelKDPage(provGuid, type, pimpUser);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        }
        Session.Remove("SubmittedData");
    }
    [WebMethod(EnableSession = true)]
    public bool DeleteRequestedIntelKd(string provId, string type, string ownerKingdomID)
    {
        Session["SubmittedData"] = provId + ":" + type;
        try
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            return Province.DeleteRequestedIntelKDPage(new Guid(provId), type, pimpUser.PimpUser.StartingKingdom, pimpUser);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return false;
    }
    [WebMethod(EnableSession = true)]
    public string FilterStandardInfo(string networthValue, string acresValue, string lastUpdatedValue, string setID, string ownerKingdomID)
    {
        Session["SubmittedData"] = networthValue + ":" + acresValue + ":" + lastUpdatedValue + ":" + setID;
        try
        {
            MatchCollection mc = URegEx.rgxNumber.Matches(networthValue);
            int netMax = Convert.ToInt32(mc[1].Value) * 1000;
            int netMin = Convert.ToInt32(mc[0].Value) * 1000;
            mc = URegEx.rgxNumber.Matches(acresValue);
            int acresMax = Convert.ToInt32(mc[1].Value);
            int acesmin = Convert.ToInt32(mc[0].Value);
            int days = 100;
            if (lastUpdatedValue != "0")
                days = Convert.ToInt32(lastUpdatedValue);
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
                        Province.LoadFilterStandardProvinces(netMax, netMin, acresMax, acesmin, 0, days, pimpUser.PimpUser.StartingKingdom, cachedKingdom);
                        return KdPage.loadDynamicGrid(new Guid(), "RandomFilter", UtopiaParser.GetUserColumnsSet(Convert.ToInt32(setID), pimpUser.PimpUser.UserColumns), pimpUser.PimpUser.StartingKingdom, pimpUser.PimpUser.MonarchType, pimpUser.PimpUser.UserID, cachedKingdom);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string FilterInpInfo(string netMin, string netMax, string acreMin, string acreMax, string updateMin, string updateMax, string setID, string ownerKingdomID)
    {
        Session["SubmittedData"] = netMax + ":" + netMin + ":" + acreMax + ":" + acreMin + ":" + updateMax + ":" + updateMin + ":" + setID+":"+ownerKingdomID;
        try
        {
            if (netMin == string.Empty)
                netMin = "0";
            if (netMax == string.Empty)
                netMax = "10000000";
            if (acreMin == string.Empty)
                acreMin = "0";
            if (acreMax == string.Empty)
                acreMax = "100000";
            if (updateMin == string.Empty)
                updateMin = "0";
            if (updateMax == string.Empty)
                updateMax = "100";

            netMin = URegEx.rgxNumber.Match(netMin).Value;
            netMax = URegEx.rgxNumber.Match(netMax).Value;
            acreMin = URegEx.rgxNumber.Match(acreMin).Value;
            acreMax = URegEx.rgxNumber.Match(acreMax).Value;
            updateMin = URegEx.rgxNumber.Match(updateMin).Value;
            updateMax = URegEx.rgxNumber.Match(updateMax).Value;
            
            
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();
            var cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            Province.LoadFilterStandardProvinces(Convert.ToInt32(netMax), Convert.ToInt32(netMin), Convert.ToInt32(acreMax), Convert.ToInt32(acreMin), Convert.ToInt32(updateMin), Convert.ToInt32(updateMax), new Guid(ownerKingdomID), cachedKingdom);
            return KdPage.loadDynamicGrid(new Guid(), "RandomFilter", UtopiaParser.GetUserColumnsSet(Convert.ToInt32(setID), pimpUser.PimpUser.UserColumns), pimpUser.PimpUser.StartingKingdom, pimpUser.PimpUser.MonarchType, pimpUser.PimpUser.UserID, cachedKingdom);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
}

