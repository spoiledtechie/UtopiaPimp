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

public partial class admin_AdminNews : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
     protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_New UN = new CS_Code.Utopia_New();
        UN.Body = taAddInfo.InnerText;
        UN.TimeStamp = DateTime.UtcNow;
        UN.Title = txtbxTitle.Text;
        UN.User_ID_Added = pimpUser.PimpUser.UserID;
        db.Utopia_News.InsertOnSubmit(UN);
        db.SubmitChanges();
        Response.Redirect("AdminNews.aspx");
    }
}
