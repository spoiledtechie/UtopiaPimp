using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_Voting : MyBasePageCS
{
    PimpUserWrapper  pimpUser;

    protected void Page_Load(object sender, EventArgs e)
    {
         pimpUser = new PimpUserWrapper ();

         if (pimpUser.PimpUser.StartingKingdom == new Guid())
            HttpContext.Current.Response.Redirect("Default.aspx");

        if (!IsPostBack)
        {
            OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var getVotes = (from yy in db.Utopia_Province_Data_Captured_Gens
                            from zz in db.Utopia_Province_Data_Captured_Gens
                            where yy.Monarch_Vote_Province_ID == zz.Province_ID
                            where zz.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                            where yy.Owner_Kingdom_ID == zz.Owner_Kingdom_ID
                            where zz.Kingdom_ID == zz.Owner_Kingdom_ID
                            where yy.Kingdom_ID == zz.Owner_Kingdom_ID
                            select new
                            {
                                provID = yy.Province_ID,
                                votedForProvID = yy.Monarch_Vote_Province_ID,
                                userID = yy.Owner_User_ID,
                                provName = yy.Province_Name,
                                votedForProvName = zz.Province_Name,
                            }).ToList();

            var getKingdom = cachedKingdom.Provinces.Where(x => x.Kingdom_ID == pimpUser.PimpUser.StartingKingdom).Where(x => x.Owner_User_ID != null).Where(x => x.Owner_User_ID != new Guid()).ToList();
            lblVotesNeeded.Text = ((getKingdom.Count() / 3) + 1).ToString();

            ddlVotedFor.DataSource = getKingdom;
            ddlVotedFor.DataBind();

            var vote = getVotes.Where(x => x.provID ==pimpUser.PimpUser.CurrentActiveProvince).FirstOrDefault();
            if (vote != null)
                ddlVotedFor.SelectedValue = vote.votedForProvID.ToString();

            StringBuilder sb = new StringBuilder();

            sb.Append("<table class=\"tblKingdomInfo center\">");
            sb.Append("<tr><th>Province Name</th><th>Voted</th><th>Number of Votes</th></tr>");
            for (int i = 0; i < getKingdom.Count(); i++)
            {
                switch (i % 2)
                {
                    case 0:
                        sb.Append("<tr class=\"d0\"\">");
                        break;
                    case 1:
                        sb.Append("<tr class=\"d1\"\">");
                        break;
                }

                sb.Append("<td>" + getKingdom[i].Province_Name + "</td>");
                sb.Append("<td>" + (getVotes.Where(x => x.provID == getKingdom[i].Province_ID).Count() > 0 ? "yes" : "no") + "</td>");
                sb.Append("<td>" + getVotes.Where(x => x.votedForProvID == getKingdom[i].Province_ID).Count() + "</td>");
                sb.Append("</tr>");
                if (getKingdom[i].Owner == 1)
                    lblOwner.Text = getKingdom[i].Province_Name;

            }
            sb.Append("</table>");
            divVotes.InnerHtml = sb.ToString();
        }
    }
    protected void btnVote_Click(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        OwnedKingdomProvinces cachedKingdom = KingdomCache.getKingdom(pimpUser.PimpUser.StartingKingdom);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var checkVote = (from xx in db.Utopia_Province_Data_Captured_Gens
                         where xx.Province_ID == pimpUser.PimpUser.CurrentActiveProvince
                         where xx.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                         select xx).FirstOrDefault();
        if (checkVote != null)
        {
            if (ddlVotedFor.SelectedIndex == 0)
                checkVote.Monarch_Vote_Province_ID = null;
            else
                checkVote.Monarch_Vote_Province_ID = new Guid(ddlVotedFor.SelectedValue);
            db.SubmitChanges();
        }

        int votes = Convert.ToInt32(lblVotesNeeded.Text);

        var getVotes = (from yy in db.Utopia_Province_Data_Captured_Gens
                        from zz in db.Utopia_Province_Data_Captured_Gens
                        where yy.Monarch_Vote_Province_ID == zz.Province_ID
                        where zz.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                        where yy.Owner_Kingdom_ID == zz.Owner_Kingdom_ID
                        where zz.Kingdom_ID == zz.Owner_Kingdom_ID
                        where yy.Kingdom_ID == zz.Owner_Kingdom_ID
                        select new
                        {
                            provID = yy.Province_ID,
                            votedForProvID = yy.Monarch_Vote_Province_ID,
                            userID = yy.Owner_User_ID,
                            provName = yy.Province_Name,
                            votedForProvName = zz.Province_Name,
                        }).ToList();

        var getKingdom = cachedKingdom.Provinces.Where(x => x.Kingdom_ID == pimpUser.PimpUser.StartingKingdom).ToList();
        Guid provMonarched = new Guid();
        ProvinceClass provMonarchedRemove = getKingdom.Where(x => x.Owner == 1).FirstOrDefault();
        for (int i = 0; i < getKingdom.Count(); i++)
        {
            if (getVotes.Where(x => x.votedForProvID == getKingdom[i].Province_ID).Count() >= votes)
            {
                var getProvince = (from xx in db.Utopia_Province_Data_Captured_Gens
                                   where xx.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                                   where xx.Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                                   where xx.Province_ID == getKingdom[i].Province_ID
                                   select xx).FirstOrDefault();
                getProvince.Owner = 1;
                provMonarched = getProvince.Province_ID;

                if (provMonarchedRemove != null && provMonarchedRemove.Province_ID != getProvince.Province_ID)
                {
                    var getProvinceRemove = (from xx in db.Utopia_Province_Data_Captured_Gens
                                             where xx.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                                             where xx.Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                                             where xx.Province_ID == provMonarchedRemove.Province_ID
                                             select xx).FirstOrDefault();
                    getProvinceRemove.Owner = null;
                }

                var getkd = (from xx in db.Utopia_Kingdom_Infos
                             where xx.Owner_Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                             where xx.Kingdom_ID == pimpUser.PimpUser.StartingKingdom
                             select xx).FirstOrDefault();

                getkd.Owner_User_ID = getProvince.Owner_User_ID;
            }
        }
        db.SubmitChanges();
        KingdomCache.removeKingdomFromKingdomCache(pimpUser.PimpUser.StartingKingdom, pimpUser.PimpUser.StartingKingdom, cachedKingdom);
        if (provMonarched != new Guid())
        {
            KingdomCache.removeProvinceFromKingdomCache(pimpUser.PimpUser.StartingKingdom, provMonarched, cachedKingdom);
            if (provMonarchedRemove != null && provMonarchedRemove.Province_ID != provMonarched)
                KingdomCache.removeProvinceFromKingdomCache(pimpUser.PimpUser.StartingKingdom, provMonarchedRemove.Province_ID, cachedKingdom);
        }
        Response.Redirect("Voting.aspx");
    }
}
