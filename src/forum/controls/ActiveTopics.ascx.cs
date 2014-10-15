using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YAF.Classes.Utils;

public partial class forum_controls_ActiveTopics : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {                
                DataTable dtActivePosts = YAF.Classes.Data.DB.topic_latest(1, 5, YafContext.Current.PageUserID);
                dlForumPosts.DataSource = dtActivePosts;
                dlForumPosts.DataBind();
            }
            catch 
            {
                //string test = "";
            }
        }
    }
}
