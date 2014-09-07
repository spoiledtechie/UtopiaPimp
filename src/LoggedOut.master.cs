using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Web;

public partial class LoggedOut : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind(); 

        if(!IsPostBack)
        ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";

    }
    protected void LoginStatus1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();

    }

}
