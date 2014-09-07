using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_Races : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnMultipleRowDelete_Click(object sender, EventArgs e)
    {
        // Looping through all the rows in the GridView
        foreach (GridViewRow row in gvViewOps.Rows)
        {
            CheckBox checkbox = (CheckBox)row.FindControl("chkRows");

            //Check if the checkbox is checked.
            //value in the HtmlInputCheckBox's Value property is set as the //value of the delete command's parameter.
            if (checkbox.Checked)
            {
                // Retreive the Employee ID
                int employeeID = Convert.ToInt32(gvViewOps.DataKeys[row.RowIndex].Value);
                // Pass the value of the selected Employye ID to the Delete //command.
                sdsViewOps.DeleteParameters["uid"].DefaultValue = employeeID.ToString();
                sdsViewOps.Delete();
            }
        }
    }
}
