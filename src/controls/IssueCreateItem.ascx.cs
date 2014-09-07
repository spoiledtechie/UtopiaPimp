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

public partial class controls_CreateItem : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //DateTime SubmitDate = DateTime.UtcNow;
        //CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //CS_Code.Issues_Info II = new CS_Code.Issues_Info();
        //II.Description = txtbxDescription.Text;
        //II.Reported_By = userID;
        //II.Reported_On = SubmitDate;
        //II.Status_ID = 1;
        //II.Title = txtbxTitle.Text;
        //II.Type = 2;
        //II.Impact = 4;
        //II.Updated_By = userID;
        //II.Updated_On = SubmitDate;
        //II.Vote_Count_Yes = 1;
        //II.Vote_Count_No = 0;
        //db.Issues_Infos.InsertOnSubmit(II);
        //db.SubmitChanges();

        //CS_Code.Issues_Vote IV = new CS_Code.Issues_Vote();
        //IV.DateTime_Voted = SubmitDate;
        //IV.Issues_ID = II.uid;
        //IV.User_ID = userID;
        //IV.Vote_Y_N = 1;
        //db.Issues_Votes.InsertOnSubmit(IV);
        //db.SubmitChanges();

        //Response.Redirect(Request.RawUrl);
    }
}
