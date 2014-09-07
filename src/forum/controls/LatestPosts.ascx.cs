using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YAF.Classes.Utils;
using Boomers.Utilities.DatesTimes;

/// <summary>
/// This will give you the Top Posts of all time by number of posts.
/// There are TWO options for displaying.
/// You can either have a list or a table.
/// If list, leave the code alone.
/// If Table, then comment out the list REGION code and UnComment the 2 Table REGIONS of code.
/// </summary>
public partial class forum_controls_LatestPosts : System.Web.UI.UserControl
{
    private static int _defaultPostCount = 10;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            string str = "select top " + PostCount + " b.name as [BoardName], f.Name as [ForumName], t.topic as [Topic], t.TopicID, t.ForumID, t.views as [NumberofViews], t.numposts as [PostCount], t.lastposted as [LastPost] from yaf_topic t inner join yaf_forum f on t.forumid = f.forumid inner join yaf_category c on f.categoryid = c.categoryid inner join yaf_board b on c.boardid = b.boardid";
            if (BoardName != null)
                str += " where b.name = '" + BoardName + "'";
            str += " order by posted desc";
            List<TopPosts> query = db.ExecuteQuery<TopPosts>(str).ToList();


            StringBuilder sb = new StringBuilder();
            #region EnabledList
            if (Class != null)
                sb.Append("<ul class=\"" + Class + "\">");
            else
                sb.Append("<ul>");
            for (int i = 0; i < query.Count; i++)
            {
                sb.Append("<li>");
                sb.Append("<span style=\"float:right;\">" + query[i].LastPost.GetValueOrDefault().ToShortDateTimeString() + "</span>");
                sb.Append("<a href=\"" + YafBuildLink.GetLinkNotEscaped(ForumPages.posts, "t={0}", query[i].TopicID).Replace("glance", "default") + "\">" + query[i].Topic + "</a> (" + query[i].PostCount + " posts)");
                sb.Append("<br/><span style=\"margin-left: 10px; color:#E0E0E0;\">in</span> <b><a href=\"" + YafBuildLink.GetLinkNotEscaped(ForumPages.topics, "f={0}", query[i].ForumID).Replace("glance", "default") + "\">" + query[i].ForumName + "</a></b>");
                sb.Append(" <span style=\"color:#E0E0E0;\">" + query[i].NumberofViews + " views</span>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            #endregion

            #region DisabledTable
            //if (Class != null)
            //    sb.Append("<table class=\"" + Class + "\">");
            //else
            //    sb.Append("<table>");

            //if (ClassHeader != null)
            //    sb.Append("<thead class=\"" + ClassHeader + "\">");
            //else
            //    sb.Append("<thead>");

            //sb.Append("<tr><th>Post</th><th>Posts Count</th><th>Views</th><th>Last Post</th></tr></thead>");

            //for (int i = 0; i < query.Count; i++)
            //{
            //    switch (i % 2)
            //    {
            //        case 0:
            //            if (ClassRow != null)
            //                sb.Append("<tr class=\"" + ClassRow + "\">");
            //            else
            //                sb.Append("<tr>");
            //            break;
            //        case 1:
            //            if (ClassRowAlternating != null)
            //                sb.Append("<tr class=\"" + ClassRowAlternating + "\">");
            //            else
            //                sb.Append("<tr>");
            //            break;
            //    }

            //    sb.Append("<td><a href=\"" + YafBuildLink.GetLinkNotEscaped(ForumPages.posts, "t={0}", query[i].TopicID).Replace("glance", "default") + "\">" + query[i].Topic + "</a></td>");
            //    sb.Append("<td>" + query[i].PostCount + "</td>");
            //    sb.Append("<td>" + query[i].NumberofViews + "</td>");
            //    sb.Append("<td>" + query[i].LastPost + "<br/>in <a href=\"" + YafBuildLink.GetLinkNotEscaped(ForumPages.topics, "f={0}", query[i].ForumID).Replace("glance", "default") + "\">" + query[i].ForumName + "</a></td>");
            //    sb.Append("</tr>");
            //}
            //sb.Append("</table>");
            #endregion
            ltItem.Text = sb.ToString();
        }
    }
    public int PostCount
    {
        get { return _defaultPostCount; }
        set { _defaultPostCount = value; }
    }
    public string BoardName { get; set; }
    public string Class { get; set; }
    #region DisabledTable2
    //public string ClassRowAlternating { get; set; }
    //public string ClassRow { get; set; }
    //public string ClassHeader { get; set; }
    #endregion

    public class TopPosts
    {
        public int TopicID;
        public int ForumID;
        public string BoardName;
        public string ForumName;
        public string Topic;
        public int NumberofViews;
        public int PostCount;
        public DateTime? LastPost;
    }
}


