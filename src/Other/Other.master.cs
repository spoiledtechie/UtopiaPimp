using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Other_Other : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
            ltCSS.Text = "<link href=\"http://codingforcharity.org/utopiapimp/css/Default.css?v=" + SupportFramework.StaticContent.CSS.CssVersion + "\" rel='stylesheet' type='text/css' />";

    }
}
