using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Extensions;
using Boomers.Utilities.Services;

public partial class controls_reusable_Tumblr : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("http://blog.utopiapimp.com/api/read");
                Tumblr tm = Tumblr.GetTumblr("http://blog.utopiapimp.com/api/read");
                tm.Posts.Count = "3";
                StringBuilder sb = new StringBuilder();
                     //sb.Append("<div>");
                      //sb1.Append("<br />" + tm.Feed.Description);
                for (int i = 0; i < 2; i++)
                {
                    sb.Append("<div class=\"post\">");
                    sb.Append("<a class=\"blogLink\" href=\"" + tm.Posts.Post[i].UrlSlug + "\">" + tm.Posts.Post[i].Title + "</a><!--Intense Debate--> <span style=\"display:none\" id=\"ID" + tm.Posts.Post[i].UrlSlug + "\">" + tm.Posts.Post[i].Title + "</span> <!--/Intense Debate--> - ");
                    sb.Append(Convert.ToDateTime(tm.Posts.Post[i].DateGMT).ToShortDateString());
                    sb.Append("<div>");
                    sb.Append(tm.Posts.Post[i].Body);
                    sb.Append("</div>");
                    sb.Append("<a href=\"" + tm.Posts.Post[i].UrlSlug + "\" class=\"IDCommentsReplace\">Comments</a>");
                    sb.Append("</div>");
                    sb.Append("<hr />");
                }
                sb.Append("<!--IntenseDebate--> <script src=\"http://www.intensedebate.com/js/tumblrWrapper.php?acct=576b8b93a53a1c25b9a3ad7439a399a0\" type=\"text/javascript\"></script>  <!--/IntenseDebate-->");

                //sb.Append("</div>");
                ltBlog.Text = sb.ToString();
            }
            catch { }
        }
    }
}
