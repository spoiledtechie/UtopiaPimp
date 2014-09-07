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

public partial class members_WorkItem_List : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            //var GetItems = (from II in db.Issues_Infos
            //                let j = 50
            //                join ISP in db.Issues_Status_Pulls on II.Status_ID equals ISP.uid
            //                where II.Status_ID != 5 && II.Status_ID != 2 && II.Status_ID != 9 && II.Status_ID != 7 && II.Status_ID != 6
            //                orderby II.Vote_Count_Yes descending
            //                select new
            //                {
            //                    id = II.uid,
            //                    NavigateURL = "View.aspx?item=" + II.uid,
            //                    Title = II.Title,
            //                    Description = II.Description,
            //                    CommentCount = II.Comment_Count + " comments",
            //                    BottomLine = " | by ",
            //                    ReportedBy = II.Reported_By,
            //                    Status = II.Status_ID,
            //                    VoteCount = II.Vote_Count_Yes
            //                });

            //dlViewItems.DataSource = GetItems;
            //dlViewItems.DataBind();

            //if (user.isUserAdmin)
            //    dlViewItems.FindControl("divAdmin");
        }
    }
}
