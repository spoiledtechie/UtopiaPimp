using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Text;
using Boomers.Utilities.Guids;

using Pimp.UCache;
using Pimp.UParser;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_Activity : MyBasePageCS
{
    PimpUserWrapper  currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = new PimpUserWrapper ();

        if (!IsPostBack)
        {
            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);

            if (currentUser.PimpUser.StartingKingdom != null && HttpContext.Current.Request.QueryString["kdid"] != null)
            {
                if (KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom, new Guid(Request.QueryString["kdid"].ToString()), cachedKingdom) != null)
                {
                    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
                    List<ProvinceClass> provNames = new List<ProvinceClass>();

                    if (HttpContext.Current.Request.QueryString["kdty"].ToString() == "Random")
                    {
                        provNames = (from yy in cachedKingdom.Provinces
                                     where !(from xx in db.Utopia_Kingdom_Infos
                                             where xx.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                             select xx.Kingdom_ID).Contains((Guid)yy.Kingdom_ID) || yy.Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                     select yy).ToList();
                        lblActLog.Text = "Kd-Less Provinces";
                    }
                    else
                    {
                        provNames = (from xx in cachedKingdom.Provinces
                                     where xx.Kingdom_ID == currentUser.PimpUser.StartingKingdom || xx.Kingdom_ID == new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString())
                                     select xx).ToList();
                        lblActLog.Text = UtopiaParser.GetKingdomIslandLocation(currentUser.PimpUser.StartingKingdom, new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString()), cachedKingdom);
                    }
                    var provIDHome = (from xx in provNames
                                      where xx.Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                      select xx.Province_ID);

                    var provIDAway = (from xx in provNames
                                      where xx.Kingdom_ID != currentUser.PimpUser.StartingKingdom
                                      select xx.Province_ID);


                    lblActLog1.Text = lblActLog.Text;
                    DateTime dbStart = DateTime.UtcNow;
                    List<ActivityLog> list = new List<ActivityLog>();
                    if (HttpContext.Current.Request.QueryString["kdty"].ToString() != "Random")
                    {
                        //ce
                        var ce = (from yy in db.Utopia_Kingdom_CEs
                                  where yy.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                  where yy.Kingdom_ID == new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString())
                                  where provIDHome.Contains(yy.Province_ID_Added)
                                  group yy by yy.Province_ID_Added into gg
                                  select new
                                  {
                                      gg.Key,
                                      count = (from zz in gg
                                               where zz.Province_ID_Added == gg.Key
                                               select gg).Count()
                                  }).ToList();
                        for (int i = 0; i < ce.Count(); i++)
                        {
                            ActivityLog al = new ActivityLog();
                            al.ceCount = ce[i].count;
                            al.province_ID = ce[i].Key;
                            list.Add(al);
                        }
                    }
                    lblTimes.Text += "dbce: " + DateTime.UtcNow.Subtract(dbStart).TotalSeconds.ToString() + " secs ";
                    DateTime dbattack = DateTime.UtcNow;

                    //attacks
                    var attack = (from yy in db.Utopia_Province_Data_Captured_Attacks
                                  where yy.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                  where provIDHome.Contains(yy.Province_ID_Added)
                                  where provIDAway.Contains(yy.Province_ID_Attacked)
                                  group yy by yy.Province_ID_Added into gg
                                  select new
                                  {
                                      gg.Key,
                                      count = (from zz in gg
                                               where zz.Province_ID_Added == gg.Key
                                               select gg).Count()
                                  }).ToList();

                    lblTimes.Text += "dbAttack: " + DateTime.UtcNow.Subtract(dbattack).TotalSeconds.ToString() + " secs ";
                    DateTime dbScience = DateTime.UtcNow;

                    //sciences
                    var science = (from yy in db.Utopia_Province_Data_Captured_Sciences
                                   where yy.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                   where provIDHome.Contains(yy.Province_ID_Added)
                                   where provIDAway.Contains(yy.Province_ID)
                                   group yy by yy.Province_ID_Added into gg
                                   select new
                                   {
                                       gg.Key,
                                       count = (from zz in gg
                                                where zz.Province_ID_Added == gg.Key
                                                select gg).Count()
                                   }).ToList();

                    lblTimes.Text += "dbScience: " + DateTime.UtcNow.Subtract(dbScience).TotalSeconds.ToString() + " secs ";
                    DateTime dbSurvey = DateTime.UtcNow;
                    //surveys
                    var survey = (from yy in db.Utopia_Province_Data_Captured_Surveys
                                  where yy.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                                  where provIDHome.Contains(yy.Province_ID_Updated_By)
                                  where provIDAway.Contains(yy.Province_ID)
                                  group yy by yy.Province_ID_Updated_By into gg
                                  select new
                                  {
                                      gg.Key,
                                      count = (from zz in gg
                                               where zz.Province_ID_Updated_By == gg.Key
                                               select gg).Count()
                                  }).ToList();

                    lblTimes.Text += "dbSurvey: " + DateTime.UtcNow.Subtract(dbSurvey).TotalSeconds.ToString() + " secs ";
                    DateTime dbSom = DateTime.UtcNow;

                    //SOM
                    var som = (from yy in db.Utopia_Province_Data_Captured_Type_Militaries
                               where yy.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                               where provIDHome.Contains(yy.Province_ID_Added)
                               where provIDAway.Contains(yy.Province_ID)
                               group yy by yy.Province_ID_Added into gg
                               select new
                               {
                                   gg.Key,
                                   count = (from zz in gg
                                            where zz.Province_ID_Added == gg.Key
                                            select gg).Count()
                               }).ToList();

                    lblTimes.Text += "dbSom: " + DateTime.UtcNow.Subtract(dbSom).TotalSeconds.ToString() + " secs ";
                    DateTime dbOps = DateTime.UtcNow;

                    //ops
                    var ops = (from yy in db.Utopia_Province_Ops
                               where yy.Owner_Kingdom_ID == currentUser.PimpUser.StartingKingdom
                               where provIDHome.Contains(yy.Added_By_Province_ID)
                               where provIDAway.Contains(yy.Directed_To_Province_ID)
                               group yy by yy.Added_By_Province_ID into gg
                               select new
                               {
                                   gg.Key,
                                   count = (from zz in gg
                                            where zz.Added_By_Province_ID == gg.Key
                                            select gg).Count()
                               }).ToList();

                    lblTimes.Text += "dbOps: " + DateTime.UtcNow.Subtract(dbOps).TotalSeconds.ToString() + " secs ";
                    DateTime dbcb = DateTime.UtcNow;

                    //cb
                    var cb = (from yy in cachedKingdom.Provinces
                              where provIDHome.Contains(yy.CB_Updated_By_Province_ID.GetValueOrDefault())
                              where provIDAway.Contains(yy.Province_ID)
                              group yy by yy.CB_Updated_By_Province_ID into gg
                              select new
                              {
                                  gg.Key,
                                  count = (from zz in gg
                                           where zz.CB_Updated_By_Province_ID == gg.Key
                                           select gg).Count()
                              }).ToList();

                    lblTimes.Text += "dbcb: " + DateTime.UtcNow.Subtract(dbcb).TotalSeconds.ToString() + " secs ";
                    DateTime dbFor = DateTime.UtcNow;


                    for (int i = 0; i < attack.Count(); i++)
                    {
                        var checkProv = (from xx in list
                                         where xx.province_ID == attack[i].Key
                                         select xx).FirstOrDefault();
                        if (checkProv != null)
                            checkProv.attackCount = attack[i].count;
                        else
                        {
                            ActivityLog al = new ActivityLog();
                            al.attackCount = attack[i].count;
                            al.province_ID = attack[i].Key;
                            list.Add(al);
                        }
                    }
                    for (int i = 0; i < science.Count(); i++)
                    {
                        var checkProv = (from xx in list
                                         where xx.province_ID == science[i].Key
                                         select xx).FirstOrDefault();
                        if (checkProv != null)
                            checkProv.scienceCount = science[i].count;
                        else
                        {
                            ActivityLog al = new ActivityLog();
                            al.scienceCount = science[i].count;
                            al.province_ID = science[i].Key;
                            list.Add(al);
                        }
                    }
                    for (int i = 0; i < survey.Count(); i++)
                    {
                        var checkProv = (from xx in list
                                         where xx.province_ID == survey[i].Key
                                         select xx).FirstOrDefault();
                        if (checkProv != null)
                            checkProv.surveyCount = survey[i].count;
                        else
                        {
                            ActivityLog al = new ActivityLog();
                            al.surveyCount = survey[i].count;
                            al.province_ID = survey[i].Key;
                            list.Add(al);
                        }
                    }
                    for (int i = 0; i < ops.Count(); i++)
                    {
                        var checkProv = (from xx in list
                                         where xx.province_ID == ops[i].Key
                                         select xx).FirstOrDefault();
                        if (checkProv != null)
                            checkProv.opsCount = ops[i].count;
                        else
                        {
                            ActivityLog al = new ActivityLog();
                            al.opsCount = ops[i].count;
                            al.province_ID = ops[i].Key;
                            list.Add(al);
                        }
                    }
                    for (int i = 0; i < som.Count(); i++)
                    {
                        var checkProv = (from xx in list
                                         where xx.province_ID == som[i].Key
                                         select xx).FirstOrDefault();
                        if (checkProv != null)
                            checkProv.somCount = som[i].count;
                        else
                        {
                            ActivityLog al = new ActivityLog();
                            al.somCount = som[i].count;
                            al.province_ID = som[i].Key;
                            list.Add(al);
                        }
                    }
                    for (int i = 0; i < cb.Count(); i++)
                    {
                        if (cb[i].Key.HasValue)
                        {
                            var checkProv = (from xx in list
                                             where xx.province_ID == cb[i].Key
                                             select xx).FirstOrDefault();
                            if (checkProv != null)
                                checkProv.cbCount = cb[i].count;
                            else
                            {
                                ActivityLog al = new ActivityLog();
                                al.cbCount = cb[i].count;
                                al.province_ID = cb[i].Key.Value;
                                list.Add(al);
                            }
                        }
                    }
                    lblTimes.Text += "dbfors: " + DateTime.UtcNow.Subtract(dbFor).TotalSeconds.ToString() + " secs ";

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table class=\"tblKingdomInfo\" id=\"tblKingdomInfo\">");

                    sb.Append("<thead><tr>");
                    sb.Append("<th class=\"{sorter: 'text'}\">Province Name</th>");
                    sb.Append("<th class=\"{sorter: 'fancyNumber'}\">CB</th>");
                    sb.Append("<th class=\"{sorter: 'fancyNumber'}\">SoS</th>");
                    sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Survey</th>");
                    sb.Append("<th class=\"{sorter: 'fancyNumber'}\">CE</th>");
                    sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Ops</th>");
                    sb.Append("<th class=\"{sorter: 'fancyNumber'}\">Attacks</th>");
                    sb.Append("</tr></thead><tbody>");
                    for (int i = 0; i < list.Count(); i++)
                    {
                        switch (i % 2)
                        {
                            case 1:
                                sb.Append("<tr class=\"d0\">");
                                break;
                            case 0:
                                sb.Append("<tr class=\"d1\">");
                                break;
                        }
                        sb.Append("<td><a href='#' onclick=\"showOps('" + list[i].province_ID.RemoveDashes() + "', '");
                        if (HttpContext.Current.Request.QueryString["kdty"].ToString() == "Random")
                            sb.Append("Random");
                        else
                            sb.Append(new Guid(HttpContext.Current.Request.QueryString["kdid"].ToString()));
                        sb.Append("'); return false;\">" + provNames.Where(x => x.Province_ID == list[i].province_ID).FirstOrDefault().Province_Name + "</a></td>");
                        //sb.Append("<td>" + (from xx in provNames where xx.Province_ID == list[i].province_ID select xx.Province_Name).FirstOrDefault() + "</td>");
                        sb.Append("<td>" + list[i].cbCount + "</td>");
                        sb.Append("<td>" + list[i].scienceCount + "</td>");
                        sb.Append("<td>" + list[i].surveyCount + "</td>");
                        sb.Append("<td>" + list[i].ceCount + "</td>");
                        sb.Append("<td>" + list[i].opsCount + "</td>");
                        sb.Append("<td>" + list[i].attackCount + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("<ul><li>CE Counts do not exist for Kd-Less Provinces</li></ul>");
                    ltActivityLog.Text = sb.ToString();
                    lblTimes.Text += "dbFull: " + DateTime.UtcNow.Subtract(dbStart).TotalSeconds.ToString() + " secs ";
                    if (!currentUser.PimpUser.IsUserAdmin)
                        lblTimes.Visible = false;
                }
            }
            else
                ltActivityLog.Text = "You have accessed this page in the wrong way.  Sorry.";
        }
    }
}
public class ActivityLog
{
    public Guid province_ID;
    public string provinceName;
    public int ceCount;
    public int cbCount;
    public int somCount;
    public int opsCount;
    public int surveyCount;
    public int scienceCount;
    public int attackCount;
}
