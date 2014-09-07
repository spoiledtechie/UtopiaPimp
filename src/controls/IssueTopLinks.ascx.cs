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

public partial class controls_IssueTopLinks : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.RawUrl.Contains("AdvancedList.aspx"))
            {
                hlBasicView.Visible = true;
                lblBasicView.Visible = false;
                hlAdvancedView.Visible = false;
                lblAdvancedView.Visible = true;
                hlCreateItem.Visible = true;
                lblCreateItem.Visible = false;
            }
            else if (Request.RawUrl.Contains("CreateItem.aspx"))
            {
                hlBasicView.Visible = true;
                lblBasicView.Visible = false;
                hlAdvancedView.Visible = true;
                lblAdvancedView.Visible = false;
                hlCreateItem.Visible = false;
                lblCreateItem.Visible = true;
            }
            else if (Request.RawUrl.Contains(@"/List.aspx"))
            {
                hlBasicView.Visible = false;
                lblBasicView.Visible = true;
                hlAdvancedView.Visible = true;
                lblAdvancedView.Visible = false;
                hlCreateItem.Visible = true;
                lblCreateItem.Visible = false;
            }
            else
            {
                hlBasicView.Visible = true;
                lblBasicView.Visible = false;
                hlAdvancedView.Visible = true;
                lblAdvancedView.Visible = false;
                hlCreateItem.Visible = true;
                lblCreateItem.Visible = false;
            }
        }
    }
}
