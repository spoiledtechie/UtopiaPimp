using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class admin_Comments : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// updates the Comments to show they have been reviewed by the admin.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvViewComments_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        if (e.CommandName == "cmdReviewed")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            SqlConnection cts = new SqlConnection(SupportFramework.Data.AccessCapture.ConnectionStringID());
            string SQLUpdate = "UPDATE [Utopia_Comments] SET [Reviewed] = '1' WHERE [uid] = '" + gvViewComments.DataKeys[index].Value + "'";
            SqlCommand SQLdatasourceUpdate = new SqlCommand(SQLUpdate, cts);
            try
            {
                cts.Open();
                SQLdatasourceUpdate.ExecuteNonQuery();
            }
            finally
            {
                cts.Close();
                cts.Dispose();
            }
            gvViewComments.DataBind();
        }
    }

}
