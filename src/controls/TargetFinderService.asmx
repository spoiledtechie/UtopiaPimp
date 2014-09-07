<%@ WebService Language="C#" Class="TargetFinderService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text.RegularExpressions;

using Pimp.UParser;
using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Pimp.UIBuilder;
using Pimp.UData;
using SupportFramework.Data;

[WebService(Namespace = "http://www.utopiapimp.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class TargetFinderService : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public int AddTargetData(string rawData, string currentUserID)
    {
        Session["SubmittedData"] = rawData;
        try
        {
            return TargetFinder.Instance.parseTargets(rawData);
        }
        catch (Exception e)
        {
            Errors.logError(e);
        }

        Session.Remove("SubmittedData");
        return 0;
    }
    [WebMethod(EnableSession = true)]
    public string[] SearchTargetInfo(string networthValue, string acresValue, string lastUpdatedValue, string races, string honor, string provCount)
    {
        Session["SubmittedData"] = networthValue + ":" + acresValue + ":" + lastUpdatedValue + ":" + races + ":" + honor + ":" + provCount;
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

            races = races.Replace("DarkElf", "Dark Elf");
            honor = honor.Replace("NobleLady", "Noble Lady");
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            var collection = Province.LoadTargetFinderSearch(netMax, netMin, acresMax, acesmin, 0, days, races, honor, pimpUser);
            if (collection.Count == 0)
                return new string[] { "No Provinces Currently in the Queue", "0" };
            return TargetFinder.BuildTargetedHTML(collection, Convert.ToInt32(provCount));

        }
        catch (Exception e)
        {
            Errors.logError(e);
        }
        Session.Remove("SubmittedData");
        return new string[1];
    }
    [WebMethod(EnableSession = true)]
    public string[] SearchTargetInfoInput(string netMin, string netMax, string acreMin, string acreMax, string updateMin, string updateMax, string races, string honor, string provCount)
    {
        Session["SubmittedData"] = netMax + ":" + netMin + ":" + acreMax + ":" + acreMin + ":" + updateMax + ":" + updateMin + ":" + races + ":" + honor + ":" + provCount;
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
            
            PimpUserWrapper pimpUser = new PimpUserWrapper();
            races = races.Replace("DarkElf", "Dark Elf");
            honor = honor.Replace("NobleLady", "Noble Lady");
            var collection = Province.LoadTargetFinderSearch(Convert.ToInt32(netMax), Convert.ToInt32(netMin), Convert.ToInt32(acreMax), Convert.ToInt32(acreMin), Convert.ToInt32(updateMin), Convert.ToInt32(updateMax), races, honor, pimpUser);
            if (collection.Count == 0)
                return new string[] { "No Provinces Currently in the Queue", "0" };
            return TargetFinder.BuildTargetedHTML(collection, Convert.ToInt32(provCount));
        }
        catch (Exception e)
        {
            Errors.logError(e);
        }
        Session.Remove("SubmittedData");
        return new string[1];
    }


}

