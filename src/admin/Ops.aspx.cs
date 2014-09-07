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

public partial class admin_Ops : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAddOp_Click(object sender, EventArgs e)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_Province_Ops_Pull upop = new CS_Code.Utopia_Province_Ops_Pull();
        upop.Added_By_User_ID = SupportFramework.Users.Memberships.getUserID();
        upop.OP_Name = txtbxOp.Text;
        upop.TimeStamp = DateTime.UtcNow;
        db.Utopia_Province_Ops_Pulls.InsertOnSubmit(upop);
        db.SubmitChanges();

        txtbxOp.Text = "";
        gvViewOps.DataBind();

    }
}
