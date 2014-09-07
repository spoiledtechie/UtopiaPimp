using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Pimp.UParser;
using Pimp.UData;

/// <summary>
/// Summary description for Nobility
/// </summary>
public class Nobility
{
    /// <summary>
    /// Gets the ID of the Nobility Name.
    /// </summary>
    /// <param name="NobilityName"></param>
    /// <returns></returns>
    public static int NobilityNamePull(string NobilityName, Guid currentUserID)
    {
        var query =UtopiaHelper.Instance.Ranks.Where(x => x.name == NobilityName.Trim()).Select(x => x.uid).FirstOrDefault();
        if (query == 0)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            CS_Code.Utopia_Province_Nobility_Pull UPNP = new CS_Code.Utopia_Province_Nobility_Pull();
            UPNP.Added_By_DateTime = DateTime.UtcNow;
            UPNP.Added_By_UserID = currentUserID;
            UPNP.Nobility = NobilityName.Trim();
            db.Utopia_Province_Nobility_Pulls.InsertOnSubmit(UPNP);
            db.SubmitChanges();
            UtopiaParser.FailedAt("'NobilityPull'", "'" + NobilityName + "'", currentUserID);
            return UPNP.uid;
        }
        else
            return query;
    }
}