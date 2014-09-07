using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Pimp.UCache;
using Boomers.Utilities.DatesTimes;
using Boomers.Utilities.Guids;
using Pimp.UParser;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_ProvinceDetail : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();

        OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);

        if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().IsValidGuid())
            hfProvinceID.Value = Request.QueryString["ID"].ToString();
        else
            Response.Redirect("../Default.aspx");
        
        var ProvinceOwnerIDCheck = cachedKingdom.Provinces.Where(x => x.Province_ID == new Guid(hfProvinceID.Value)).FirstOrDefault();
        if (ProvinceOwnerIDCheck == null & !pimpUser.PimpUser.IsUserAdmin)
            Response.Redirect("../Default.aspx");        
    }
}






//private void PopulateSOMSOM(CS_Code.UtopiaDataContext db)
//{
//    StringBuilder sb = new StringBuilder();

//    var ProvinceSOMHome = (from uu in db.Utopia_Province_Data_Captured_Type_Militaries
//                           //join UPI in db.Utopia_Province_Identifiers on uu.Province_ID equals UPI.Province_ID
//                           //join UPI1 in db.Utopia_Province_Identifiers on uu.Province_ID_Added equals UPI1.Province_ID
//                           join UPDCG in db.Utopia_Province_Data_Captured_Gens on uu.Province_ID equals UPDCG.Province_ID
//                           join UPRMN in db.Utopia_Province_Race_Military_Names on UPDCG.Race_ID equals UPRMN.Race_ID
//                           where uu.Province_ID == new System.Guid(hfProvinceID.Value)
//                           where uu.DateTime_Added >= DateTime.UtcNow.CurrentTimeToHour()
//                           select new
//                           {
//                               OffName = UPRMN.Soldier_Off_Name,
//                               DefName = UPRMN.Soldier_Def_Name,
//                               EliteName = UPRMN.Elite_Name,
//                               eliteOffMulit = UPRMN.Elite_Off_Multiplier,
//                               eliteDefMulit = UPRMN.Elite_Def_Multiplier,
//                               NetOff = uu.Net_Offense_Pts_Home,
//                               NetDef = uu.Net_Defense_Pts_Home,
//                               rawEff = uu.Efficiency_Raw,
//                               offEff = uu.Efficiency_Off,
//                               defEff = uu.Efficiency_Def,
//                               uu.Military_Location,
//                               uu.Soldiers,
//                               uu.Regs_Off,
//                               uu.Regs_Def,
//                               uu.Elites,
//                               uu.Horses,
//                               uu.Generals,
//                               uu.Time_To_Return,
//                               uu.Export_Line,
//                               uu.DateTime_Added,
//                               uu.Owner_Kingdom_ID,
//                               UpdateProvinceName = UPDCG.Province_Name
//                           }).ToList();
//    if (ProvinceSOMHome.FirstOrDefault() != null)
//    {
//        if (ProvinceSOMHome.Count() > 1)
//        {
//            List<decimal> soldiers = new List<decimal>();
//            List<decimal> regsOff = new List<decimal>();
//            List<decimal> regsDef = new List<decimal>();
//            List<decimal> elites = new List<decimal>();
//            List<decimal> horses = new List<decimal>();
//            List<decimal> netOff = new List<decimal>();
//            List<decimal> netDef = new List<decimal>();
//            List<decimal> rawEff = new List<decimal>();
//            List<decimal> OffEff = new List<decimal>();
//            List<decimal> defEff = new List<decimal>();

//            decimal attPointsHome = 0;
//            decimal defPointsHome = 0;
//            foreach (var item in ProvinceSOMHome.Where(t => t.Military_Location == 1))
//            {
//                if (item.Soldiers.GetValueOrDefault(0) != 0)
//                    soldiers.Add((decimal)item.Soldiers.GetValueOrDefault(0));
//                if (item.Regs_Off.GetValueOrDefault(0) != 0)
//                    regsOff.Add((decimal)item.Regs_Off.GetValueOrDefault(0));
//                if (item.Regs_Def.GetValueOrDefault(0) != 0)
//                    regsDef.Add((decimal)item.Regs_Def.GetValueOrDefault(0));
//                if (item.Elites.GetValueOrDefault(0) != 0)
//                    elites.Add((decimal)item.Elites.GetValueOrDefault(0));
//                if (item.Horses.GetValueOrDefault(0) != 0)
//                    horses.Add((decimal)item.Horses.GetValueOrDefault(0));
//                if (item.NetOff.GetValueOrDefault(0) != 0)
//                    netOff.Add((decimal)item.NetOff.GetValueOrDefault(0));
//                if (item.NetDef.GetValueOrDefault(0) != 0)
//                    netDef.Add((decimal)item.NetDef.GetValueOrDefault(0));
//                if (item.rawEff.GetValueOrDefault(0) != 0)
//                    rawEff.Add((decimal)item.rawEff.GetValueOrDefault(0));
//                if (item.offEff.GetValueOrDefault(0) != 0)
//                    OffEff.Add((decimal)item.offEff.GetValueOrDefault(0));
//                if (item.defEff.GetValueOrDefault(0) != 0)
//                    defEff.Add((decimal)item.defEff.GetValueOrDefault(0));
//            }
//            sb.Append("<ul class=\"ulProvinceDetails\">");
//            sb.Append("<li>Recommended to Send 2% OVER (Net Def. at Home * (Def. Efficiency/100))</li>");
//            sb.Append("<li><br /></li>");
//            sb.Append("<li> ** Summary ** </li>");

//            if (OffEff.Count > 1)
//            {
//                SOMTotals OffEffTotals = SOMSOM.Values(OffEff);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Off Efficiency: ");
//                DisplaySOMSOMUnits(sb, OffEffTotals);
//                sb.Append("</ul></li>");
//            }
//            if (rawEff.Count > 1)
//            {
//                SOMTotals DefEffTotals = SOMSOM.Values(defEff);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Def Efficiency: ");
//                DisplaySOMSOMUnits(sb, DefEffTotals);
//                sb.Append("</ul></li>");
//            }
//            if (rawEff.Count > 1)
//            {
//                SOMTotals rawEffTotals = SOMSOM.Values(rawEff);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Raw Efficiency: ");
//                DisplaySOMSOMUnits(sb, rawEffTotals);
//                sb.Append("</ul></li>");
//            }
//            if (netOff.Count > 1)
//            {
//                SOMTotals netOffTotals = SOMSOM.Values(netOff);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Net Off. at Home (from Utopia): ");
//                DisplaySOMSOMUnits(sb, netOffTotals);
//                sb.Append("</ul></li>");
//            }
//            if (netDef.Count > 1)
//            {
//                SOMTotals netDefTotals = SOMSOM.Values(netDef);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Net Def. at Home (from Utopia): ");
//                DisplaySOMSOMUnits(sb, netDefTotals);
//                sb.Append("</ul></li>");
//            }
//            sb.Append("<li><br /></li>");
//            sb.Append("<li>** Standing Army (At Home) **</li>");
//            if (soldiers.Count > 1)
//            {
//                SOMTotals solTotals = SOMSOM.Values(soldiers);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Soldiers: ");
//                DisplaySOMSOMUnits(sb, solTotals);
//                sb.Append("</ul></li>");
//                try { attPointsHome += solTotals.exactValues[0]; }
//                catch { }
//                try { defPointsHome += solTotals.exactValues[0]; }
//                catch { }
//            }
//            if (regsOff.Count > 1)
//            {
//                SOMTotals offTotals = SOMSOM.Values(regsOff);
//                sb.Append("<li><ul>");
//                sb.Append("<li>" + ProvinceSOMHome.FirstOrDefault().OffName + ": ");
//                DisplaySOMSOMUnits(sb, offTotals);
//                sb.Append("</ul></li>");
//                try { attPointsHome += offTotals.exactValues[0] * 5; }
//                catch { }
//            }
//            if (regsDef.Count > 1)
//            {
//                SOMTotals defTotals = SOMSOM.Values(regsDef);
//                sb.Append("<li><ul>");
//                sb.Append("<li>" + ProvinceSOMHome.FirstOrDefault().DefName + ": ");
//                DisplaySOMSOMUnits(sb, defTotals);
//                sb.Append("</ul></li>");
//                if (defTotals.exactValues.Count > 0)
//                    defPointsHome += defTotals.exactValues[0] * 5;
//            }
//            if (elites.Count > 1)
//            {
//                SOMTotals elitesTotals = SOMSOM.Values(elites);
//                sb.Append("<li><ul>");
//                sb.Append("<li>" + ProvinceSOMHome.FirstOrDefault().EliteName + ": ");
//                DisplaySOMSOMUnits(sb, elitesTotals);
//                sb.Append("</ul></li>");
//                try { attPointsHome += elitesTotals.exactValues[0] * (decimal)ProvinceSOMHome.FirstOrDefault().eliteOffMulit; }
//                catch { }
//                try { defPointsHome += elitesTotals.exactValues[0] * (decimal)ProvinceSOMHome.FirstOrDefault().eliteDefMulit; }
//                catch { }
//            }
//            if (horses.Count > 1)
//            {
//                SOMTotals horseTotals = SOMSOM.Values(horses);
//                sb.Append("<li><ul>");
//                sb.Append("<li>Horses: ");
//                DisplaySOMSOMUnits(sb, horseTotals);
//                sb.Append("</ul></li>");
//                try { attPointsHome += horseTotals.exactValues[0]; }
//                catch { }
//            }
//            sb.Append("<li title=\"(Soldiers + OffenseRegs*5 + Elites*" + ProvinceSOMHome.FirstOrDefault().eliteOffMulit + ")\">Total Offense Points: " + attPointsHome.ToString("N0") + " *</li>");
//            sb.Append("<li title=\"(Soldiers + OffenseRegs*5 + Elites*" + ProvinceSOMHome.FirstOrDefault().eliteDefMulit + ")\">Total Defense Points: " + defPointsHome.ToString("N0") + " *</li>");
//            sb.Append("<li><br /></li>");

//            var captures = ProvinceSOMHome.Where(t => t.Military_Location == 2).GroupBy(x => x.DateTime_Added).ToList();
//            List<List<List<decimal>>> soms = new List<List<List<decimal>>>();
//            lblSOMSOMUpdate.Text = captures.Count() + " SOM's submitted after: " + UtopiaParser.getUtopiaDateTime(UtopiaParser.GetServerName());
//            if (captures.Count > 0)
//            {
//                foreach (var item in captures[0])
//                {
//                    List<List<decimal>> somItem = new List<List<decimal>>();
//                    List<decimal> soldiersAway = new List<decimal>();
//                    List<decimal> regsOffAway = new List<decimal>();
//                    List<decimal> regsDefAway = new List<decimal>();
//                    List<decimal> elitesAway = new List<decimal>();
//                    List<decimal> horsesAway = new List<decimal>();

//                    somItem.Add(soldiersAway);
//                    somItem.Add(regsOffAway);
//                    somItem.Add(regsDefAway);
//                    somItem.Add(elitesAway);
//                    somItem.Add(horsesAway);
//                    soms.Add(somItem);
//                }

//                for (int i = 0; i < captures.Count(); i++)
//                {
//                    int j = 0;
//                    foreach (var item in captures[i])
//                    {
//                        try
//                        {
//                            if (item.Soldiers.GetValueOrDefault(0) != 0)
//                                soms[j][0].Add((decimal)item.Soldiers.GetValueOrDefault(0));
//                            if (item.Regs_Off.GetValueOrDefault(0) != 0)
//                                soms[j][1].Add((decimal)item.Regs_Off.GetValueOrDefault(0));
//                            if (item.Regs_Def.GetValueOrDefault(0) != 0)
//                                soms[j][2].Add((decimal)item.Regs_Def.GetValueOrDefault(0));
//                            if (item.Elites.GetValueOrDefault(0) != 0)
//                                soms[j][3].Add((decimal)item.Elites.GetValueOrDefault(0));
//                            if (item.Horses.GetValueOrDefault(0) != 0)
//                                soms[j][4].Add((decimal)item.Horses.GetValueOrDefault(0));
//                        }
//                        catch { }
//                        j++;
//                    }
//                }
//                decimal attPoints = 0;
//                decimal defPoints = 0;
//                for (int i = 0; i < soms.Count(); i++)
//                {
//                    sb.Append("<li>** Armies Away ");
//                    if (captures[0].FirstOrDefault().Time_To_Return.HasValue)
//                        sb.Append("(Back in " + captures[0].FirstOrDefault().Time_To_Return.Value.Subtract(DateTime.UtcNow).Hours + ":" + captures[0].FirstOrDefault().Time_To_Return.Value.Subtract(DateTime.UtcNow).Minutes + " Hours) **</li>");
//                    else
//                        sb.Append("**</li>");
//                    if (soms[i][0].Count > 1)
//                    {
//                        SOMTotals solTotals = SOMSOM.Values(soms[i][0]);
//                        sb.Append("<li><ul>");
//                        sb.Append("<li>Soldiers: ");
//                        DisplaySOMSOMUnits(sb, solTotals);
//                        sb.Append("</ul></li>");
//                        attPoints += solTotals.exactValues[0];
//                        defPoints += solTotals.exactValues[0];
//                    }
//                    if (soms[i][1].Count > 1)
//                    {
//                        SOMTotals offTotals = SOMSOM.Values(soms[i][1]);
//                        sb.Append("<li><ul>");
//                        sb.Append("<li>" + ProvinceSOMHome.FirstOrDefault().OffName + ": ");
//                        DisplaySOMSOMUnits(sb, offTotals);
//                        sb.Append("</ul></li>");
//                        attPoints += offTotals.exactValues[0] * 5;
//                    }
//                    if (soms[i][2].Count > 1)
//                    {
//                        SOMTotals defTotals = SOMSOM.Values(soms[i][2]);
//                        sb.Append("<li><ul>");
//                        sb.Append("<li>" + ProvinceSOMHome.FirstOrDefault().DefName + ": ");
//                        DisplaySOMSOMUnits(sb, defTotals);
//                        sb.Append("</ul></li>");
//                        defPoints += defTotals.exactValues[0] * 5;
//                    }
//                    if (soms[i][3].Count > 1)
//                    {
//                        SOMTotals elitesTotals = SOMSOM.Values(soms[i][3]);
//                        sb.Append("<li><ul>");
//                        sb.Append("<li>" + ProvinceSOMHome.FirstOrDefault().EliteName + ": ");
//                        DisplaySOMSOMUnits(sb, elitesTotals);
//                        sb.Append("</ul></li>");
//                        attPoints += elitesTotals.exactValues[0] * (decimal)ProvinceSOMHome.FirstOrDefault().eliteOffMulit;
//                        defPoints += elitesTotals.exactValues[0] * (decimal)ProvinceSOMHome.FirstOrDefault().eliteDefMulit;
//                    }
//                    if (soms[i][4].Count > 1)
//                    {
//                        SOMTotals horseTotals = SOMSOM.Values(soms[i][4]);
//                        sb.Append("<li><ul>");
//                        sb.Append("<li>Horses: ");
//                        DisplaySOMSOMUnits(sb, horseTotals);
//                        sb.Append("</ul></li>");
//                        try { attPoints += horseTotals.exactValues[0]; }
//                        catch { }
//                    }
//                    sb.Append("<li>Total Offensive Points: " + attPoints.ToString("N0") + " *</li>");
//                    sb.Append("<li>Total Defensive Points: " + defPoints.ToString("N0") + " *</li>");
//                    sb.Append("<li><br /></li>");
//                }

//                sb.Append("<li><br /></li>");
//                sb.Append("<li>* Total Points are calculated with highest matches.</li>");
//                sb.Append("<li>*** 4 or more possibilities suggest submitting another SOM</li>");
//                sb.Append("<li><br /></li>");
//                sb.Append("<li>SOM+SOM Values are meant to be exact.  These numbers are better than CB's as long as there is only one possibilty. Two or More possibilites means that you should make an educated guess on which value to use.  The highest value is recommended.</li>");
//            }
//        }
//        else
//            AccordionPane5.Visible = false;
//    }
//    else
//        AccordionPane5.Visible = false;


//    ltSOMSOM.Text = sb.ToString();

//}
//private static void DisplaySOMSOMUnits(StringBuilder sb, SOMTotals Totals)
//{
//    if (Totals.exactValues.Count > 5)
//    {
//        sb.Append(Totals.exactValues[0].ToString("N0"));
//        for (int i = 1; i < 5; i++)
//            sb.Append("; " + Totals.exactValues[i].ToString("N0"));
//    }
//    else
//    {
//        if (Totals.exactValues.Count > 0)
//        {
//            sb.Append(Totals.exactValues[0].ToString("N0"));
//            for (int i = 1; i < Totals.exactValues.Count; i++)
//                sb.Append("; " + Totals.exactValues[i].ToString("N0"));
//        }
//        else { sb.Append("None"); }
//    }
//    sb.Append(" (");
//    sb.Append(Totals.values[0].ToString("N0"));
//    for (int j = 1; j < Totals.values.Count; j++)
//        sb.Append("; " + Totals.values[j].ToString("N0"));
//    sb.Append(")");
//    if (Totals.exactValues.Count > 5)
//        sb.Append(" ***");
//    sb.Append("</li>");
//}
