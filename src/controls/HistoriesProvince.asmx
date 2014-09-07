<%@ WebService Language="C#" Class="HistoriesProvince" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using SupportFramework.Data;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class HistoriesProvince : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string SelectSOSItem(string provID, string uid)
    {
        Session["SubmittedData"] = provID + ":" + uid;
        try
        {
            return ProvinceDetails.PopulateSOS(new Guid(provID), Convert.ToInt32(uid));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SelectSOMItem(string provID, string uid)
    {
        Session["SubmittedData"] = provID + ":" + uid;
        try
        {
            return ProvinceDetails.PopulateSOM(new Guid(provID), Convert.ToInt32(uid));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SelectSurveyItem(string provID, string uid)
    {
        Session["SubmittedData"] = provID + ":" + uid;
        try
        {
            return ProvinceDetails.PopulateSurvey(new Guid(provID), Convert.ToInt32(uid));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SelectAttack(string provID, string uid, string ownerKingdomID, string currentUserID)
    {
        Session["SubmittedData"] = provID + ":" + uid;
        try
        {
            return ProvinceDetails.PopulateAttack(new Guid(provID), Convert.ToInt32(uid), new Guid(ownerKingdomID), new Guid(currentUserID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }

    [WebMethod(EnableSession = true)]
    public string GetLastSOS(string provID, string ownerKingdomID, string secret)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(ownerKingdomID))
            ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>", "");
        
        Session["SubmittedData"] = provID + ":" + ownerKingdomID + ":" + secret;
        try
        {
                return ProvinceDetails.PopulateSOS(new Guid(provID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
            
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string GetLastSOM(string provID, string ownerKingdomID, string secret)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(ownerKingdomID))
            ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>", "");
        Session["SubmittedData"] = provID + ":" + ownerKingdomID + ":" + secret;
        try
        {
            
                return ProvinceDetails.PopulateSOM(new Guid(provID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string GetLastSurvey(string provID, string ownerKingdomID, string secret)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(ownerKingdomID))
            ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>", "");
        Session["SubmittedData"] = provID + ":" + ownerKingdomID + ":" + secret;
        try
        {
        
                return ProvinceDetails.PopulateSurvey(new Guid(provID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
       
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string GetLastCB(string provID, string ownerKingdomID, string secret)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(ownerKingdomID))
            ownerKingdomID = teleCheck.Replace(ownerKingdomID, "").Replace("</a>", "");
        Session["SubmittedData"] = provID + ":" + ownerKingdomID + ":" + secret;
        try
        {
      
                return ProvinceDetails.PopulateCB(new Guid(provID), new Guid(ownerKingdomID), KingdomCache.getKingdom(new Guid(ownerKingdomID)));
           
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string GetProvinceHistory(string provID, string secret)
    {
        System.Text.RegularExpressions.Regex teleCheck = new System.Text.RegularExpressions.Regex("<a href=\"tel:[0-9\\-]+\">", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (teleCheck.IsMatch(secret))
            secret = teleCheck.Replace(secret, "").Replace("</a>", "");
        
        Session["SubmittedData"] = provID + ":" + secret;
        try
        {
                return ProvinceDetails.PopulateHistory(new Guid(provID), new Guid(secret));
                    }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
}