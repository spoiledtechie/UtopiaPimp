<%@ WebService Language="C#" Class="CEChooser" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

using Pimp.UCache;
using Pimp.UIBuilder;
using SupportFramework.Data;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class CEChooser : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string SetCE(string month, string year, string kingdomIL, string CEID, string OwnerkingdomID, string currentUserID)
    {
        Session["SubmittedData"] = year + ":" + kingdomIL + ":" + CEID + ":" + OwnerkingdomID;
        try
        {
            return CE.BuildCE(Convert.ToInt32(month), Convert.ToInt32(year), kingdomIL, new Guid(CEID), new Guid(OwnerkingdomID), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(OwnerkingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SetCEPersonal(string month, string year, string kingdomIL, string CEID, string provinceName, string OwnerkingdomID, string currentUserID)
    {
        Session["SubmittedData"] = year + ":" + kingdomIL + ":" + CEID + ":" + OwnerkingdomID + ":" + month + ":" + provinceName;
        try
        {
            return CE.BuildCEPersonal(Convert.ToInt32(month), Convert.ToInt32(year), kingdomIL, CEID, provinceName, OwnerkingdomID, new Guid(currentUserID));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public string SetCEModalInfo(string location, string month, string year, string kingdomIL, string CEID, string kingdomID, string currentUserID)
    {
        Session["SubmittedData"] = year + ":" + kingdomIL + ":" + CEID + ":" + location + ":" + month + ":" + kingdomID;
        try
        {
            return CE.BuildModalPopUp(location, Convert.ToInt32(month), Convert.ToInt32(year), kingdomIL, new Guid(CEID), new Guid(kingdomID), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(kingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
    [WebMethod(EnableSession = true)]
    public void ExportCEModalInfo(string location, string month, string year, string kingdomIL, string CEID, string kingdomID, string currentUserID)
    {
        Session["SubmittedData"] = year + ":" + kingdomIL + ":" + CEID + ":" + location + ":" + month + ":" + kingdomID;
        try
        {
            CE.ExportModalPopUp(location, Convert.ToInt32(month), Convert.ToInt32(year), kingdomIL, new Guid(CEID), new Guid(kingdomID), new Guid(currentUserID), KingdomCache.getKingdom(new Guid(kingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
    }
}

