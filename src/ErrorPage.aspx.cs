using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Pimp.UCache;
using Pimp.UData;

public partial class ErrorPage : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();

        if (pimpUser.PimpUser.IsUserAdmin)
        {
            hlErrors.Visible = true;
        }
    }
}
