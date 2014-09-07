<%@ WebService Language="C#" Class="admin" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Linq;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class admin : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public void ConfirmTruncateTable(string tableName, string databaseName)
    {
        if (databaseName == "site")
        {
            SupportFramework.AdminData.Data.truncateTable(tableName, CS_Code.UtopiaDataContext.Get());
        }
        else if (databaseName == "membership")
        {
            SupportFramework.AdminData.Data.truncateTable(tableName, CS_Code.AdminDataContext.Get());
        }
    }
    [WebMethod(EnableSession = true)]
    public bool ResetUserAccountSettings(string userName, string appID)
    {
        return System.Web.Profile.ProfileManager.DeleteProfile(userName);
    }
    [WebMethod(EnableSession = true)]
    public bool DisconnectProvinceFromUser(string provinceID)
    {
        Pimp.UData.UsersData.disconnectProvinceFromUser(new Guid(provinceID));
        return true;
    }
}

