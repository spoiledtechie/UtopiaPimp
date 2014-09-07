using System;
using System.Collections;
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
using Boomers.Utilities.Services;

public partial class _Default : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (User.Identity.IsAuthenticated)
        {
            case true:
                Response.Redirect("~/members/Default.aspx");
                break;
        }
        ltHeader.Text = "<a href=\"http://blog.utopiapimp.com\">UtopiaPimp Blog</a>";
    }
}
