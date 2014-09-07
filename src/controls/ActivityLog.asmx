<%@ WebService Language="C#" Class="ActivityLog" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Boomers.Utilities.DatesTimes;

using Pimp.UParser;
using Pimp.UCache;
using SupportFramework.Data;

[WebService(Namespace = "http://www.codingforcharity.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ActivityLog : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string GetOps(string provId, string kingID, string ownerKingdomID)
    {
        Session["SubmittedData"] = provId + ":" + kingID + ":";
        try
        {
            Pimp.UData.PimpUserWrapper  pimpUser = new Pimp.UData.PimpUserWrapper ();
            return UtopiaParser.GetActivityLogOps(provId, kingID, new Guid(ownerKingdomID), pimpUser,            KingdomCache.getKingdom(new Guid(ownerKingdomID)));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        } Session.Remove("SubmittedData");
        return string.Empty;
    }
}