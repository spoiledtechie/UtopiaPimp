using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using App_Code.CS_Code.Worker;

/// <summary>
/// Summary description for OpsGatherer
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class OpsGatherer : System.Web.Services.WebService {

    
    [WebMethod]
    public void ExpiredOpsGatherer(Guid token) 
    {
        var pimpToken = new Guid("b17ec769-4440-4b85-a628-71f46d13f6ec");
        if (pimpToken == token)
        {
            GatherExpiredOps.GatherDataForNotifier();
        }        
    }    
}
