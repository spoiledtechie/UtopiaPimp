using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Pimp.UCache;
using Pimp.UData;

public partial class admin_Errors : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void gvFailedAts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();

        switch (e.CommandName)
        {
            case "cmdReviewed":
                var Queryrev = (from UEL in db.Utopia_Distorted_Datas
                                where UEL.uid == Convert.ToInt32(gvFailedAts.DataKeys[index].Value)
                                select UEL).FirstOrDefault();
                Queryrev.Reviewed = 1;
                Queryrev.Reviewed_By_DateTime = DateTime.UtcNow;
                Queryrev.Reviewed_By_UserID = pimpUser.PimpUser.UserID;
                db.SubmitChanges();
                lblCount.Text = (from xx in db.Utopia_Distorted_Datas
                                 where xx.Reviewed == 0 || xx.Reviewed == null
                                 select xx.uid).Count().ToString();
                gvFailedAts.DataBind();
                break;
            case "cmdReviewedAll":
                var Query = (from UEL in db.Utopia_Distorted_Datas
                             where UEL.uid == Convert.ToInt32(gvFailedAts.DataKeys[index].Value)
                             select UEL.aspnet_ID).FirstOrDefault();

                var getUsers = (from xx in db.Utopia_Distorted_Datas
                                where xx.aspnet_ID == Query
                                select xx);
                foreach (var item in getUsers)
                {
                    item.Reviewed = 1;
                    item.Reviewed_By_DateTime = DateTime.UtcNow;
                    item.Reviewed_By_UserID =pimpUser.PimpUser.UserID;
                }
                db.SubmitChanges();
                lblCount.Text = (from xx in db.Utopia_Distorted_Datas
                                 where xx.Reviewed == 0 || xx.Reviewed == null
                                 select xx.uid).Count().ToString();
                gvFailedAts.DataBind();
                break;
        }
    }
    protected void btnMultipleRowDelete_Click(object sender, EventArgs e)
    {
        // Looping through all the rows in the GridView
        foreach (GridViewRow row in gvFailedAts.Rows)
        {
            CheckBox checkbox = (CheckBox)row.FindControl("chkRows");

            //Check if the checkbox is checked.
            //value in the HtmlInputCheckBox's Value property is set as the //value of the delete command's parameter.
            if (checkbox.Checked)
            {
                // Retreive the Employee ID
                int employeeID = Convert.ToInt32(gvFailedAts.DataKeys[row.RowIndex].Value);
                // Pass the value of the selected Employye ID to the Delete //command.
                sdsGetFailedPoints.DeleteParameters["uid"].DefaultValue = employeeID.ToString();
                sdsGetFailedPoints.Delete();
            }
        }
    }
    protected void btnReviewedFailers_Click(object sender, EventArgs e)
    {
        SqlConnection cts = new SqlConnection(SupportFramework.Data.AccessCapture.ConnectionStringID());
        string Update = "UPDATE Utopia_Distorted_Data SET [Reviewed] = '1' WHERE Reviewed = '0'";
        SqlCommand SqlUpdate = new SqlCommand(Update, cts);
        cts.Open();
        SqlUpdate.ExecuteNonQuery();
        cts.Close();
        cts.Dispose();
        gvFailedAts.DataBind();
    }
    protected void btnTest_Click(object sender, EventArgs e)
    {
        byte[] encbuff = Convert.FromBase64String(Server.UrlDecode(tbTesting.Text));
        lblTesting.Text = System.Text.Encoding.UTF8.GetString(encbuff);
    }
    protected void btnEncode_Click(object sender, EventArgs e)
    {
        byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(tbEncode.Text);
        lblEncode.Text = Server.UrlEncode(Convert.ToBase64String(encbuff));
    }
}
