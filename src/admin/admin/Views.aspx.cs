using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.DatesTimes;

using SupportFramework;

public partial class admin_admin_Views : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BuildBrowserTypes();
            BuildOSTypes();
        }
    }

    private void BuildOSTypes()
    {
        CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();
        var OSTypes = (from xx in db.Global_User_Audits
                       where xx.ApplicationId == Applications.Instance.ApplicationId
                       group xx by xx.Windows_Platform into gg
                       select new
                       {
                           gg.Key,
                           userCount = (from yy in gg
                                        select yy.User_ID).Distinct().Count()
                       }).OrderBy(p => p.Key).ToList();
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=\"divDetails\"><div>");
        sb.Append(OSTypes.Count);
        sb.Append(" OS Types</div>");
        sb.Append("<ul class=\"ulList\">");
        for (int i = 0; i < OSTypes.Count(); i++)
        {
            sb.Append("<li><b>");
            sb.Append(OSTypes[i].userCount);
            sb.Append("</b> users; using ");
            sb.Append(OSTypes[i].Key);
            sb.Append("</li>");
        }
        sb.Append("<ul>");
        sb.Append("</div>");
        ltUrls.Text = sb.ToString();
    }

    private void BuildBrowserTypes()
    {
        CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();
                var getBrowserTypes = (from xx in db.Global_User_Audits
                                       where xx.ApplicationId == Applications.Instance.ApplicationId
                               group xx by xx.Browser_String into gg
                               select new
                               {
                                   gg.Key,
                                   UserCount = (from yy in gg
                                                select yy.User_ID).Distinct().Count(),
                                   lastUsed = (from yy in gg
                                               orderby yy.DateTime descending
                                               select yy.DateTime).FirstOrDefault(),
                               }).OrderBy(p => p.lastUsed).ToList();
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class=\"divDetails\"><div>");
        sb.Append(getBrowserTypes.Count);
        sb.Append(" Browser Types</div>");
        sb.Append("<ul class=\"ulList\">");
        for (int i = 0; i < getBrowserTypes.Count(); i++)
        {
            sb.Append("<li>Last Used: ");
            sb.Append(getBrowserTypes[i].lastUsed.Value.ToyyyyMMdd());
            sb.Append("; <b>");
            sb.Append(getBrowserTypes[i].UserCount);
            sb.Append("</b> users; using ");
            sb.Append(getBrowserTypes[i].Key);
            sb.Append("</li>");
        }
        sb.Append("<ul>");
        sb.Append("</div>");
        ltBroswerTypes.Text = sb.ToString();
    }
}
