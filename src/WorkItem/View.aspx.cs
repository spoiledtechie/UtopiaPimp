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
using System.Text.RegularExpressions;

public partial class members_WorkItem_View : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    if (Request.QueryString["item"] == null || !Static.rgxNumber.Match(Request.QueryString["item"].ToString()).Success)
        //        Response.Redirect("List.aspx");

        //    int itemID = Convert.ToInt32(Request.QueryString["item"].ToString());
        //    hfItemID.Value = itemID.ToString();

        //    CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //    var GetInfo = (from II in db.Issues_Infos
        //                   join ISP in db.Issues_Status_Pulls on II.Status_ID equals ISP.uid
        //                   join IT in db.Issues_Type_Pulls on II.Type equals IT.uid
        //                   join IIP in db.Issues_Impact_Pulls on II.Impact equals IIP.uid
        //                   where II.uid == itemID
        //                   select new
        //                   {
        //                       ISP.Status,
        //                       II.Status_ID,
        //                       trueorfalseassignedto = II.Assigned_To.HasValue,
        //                       II.Assigned_To,
        //                       trueorfalseClosedBy = II.Closed_By.HasValue,
        //                       II.Closed_By,
        //                       II.Closed_On,
        //                       II.Description,
        //                       IIP.Impact_Level,
        //                       II.Planned_Release,
        //                       II.Release,
        //                       II.Reported_By,
        //                       II.Reported_On,
        //                       II.Title,
        //                       IT.Type_Pull,
        //                       II.Updated_By,
        //                       II.Updated_On,
        //                       II.Vote_Count_Yes
        //                   }).FirstOrDefault();


        //    if (GetInfo.trueorfalseClosedBy)
        //    {
        //        lblClosedByInsert.Text = SupportFramework.DataAccessGlobal.UserName(GetInfo.Closed_By.ToString()).ToString();
        //        lblClosedOnInsert.Text = GetInfo.Closed_On.ToString();
        //    }
        //    else
        //    {
        //        lblClosedByInsert.Text = "N/A";
        //    }

        //    litDescriptionInsert.Text = GetInfo.Description;

        //    lblReportedByInsert.Text = SupportFramework.DataAccessGlobal.UserName(GetInfo.Reported_By.ToString()).ToString();
        //    lblReportedOnInsert.Text = GetInfo.Reported_On.ToString();
        //    lblTitle.Text = GetInfo.Title;
        //    lblUpdatedByInsert.Text = SupportFramework.DataAccessGlobal.UserName(GetInfo.Updated_By.ToString()).ToString();
        //    lblUpdatedOnInsert.Text = GetInfo.Updated_On.ToString();

        //    IssueVotedBox.IssueID = itemID;
        //    IssueVotedBox.VoteCount = GetInfo.Vote_Count_Yes;
        //    IssueVotedBox.Status = GetInfo.Status_ID;

        //    var GetComments = (from IC in db.Issues_Comments
        //                       where IC.Issue_ID == itemID
        //                       select new
        //                       {
        //                           IC.Comment,
        //                           DateTime = IC.Added_DateTime,
        //                           UserName = SupportFramework.DataAccessGlobal.UserName(IC.Added_By_User_ID.ToString())
        //                       });

        //    dlComments.DataSource = GetComments;
        //    dlComments.DataBind();
        //}
    }
    protected void btnSubmitComment_Click(object sender, EventArgs e)
    {
        //CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //CS_Code.Issues_Comment IC = new CS_Code.Issues_Comment();
        //IC.Issue_ID = Convert.ToInt32(hfItemID.Value);
        //IC.Comment = txtbxAddComment.Text;
        //IC.Added_DateTime = DateTime.UtcNow;
        //IC.Added_By_User_ID = userID;
        //db.Issues_Comments.InsertOnSubmit(IC);

        //var updatecomment = (from ii in db.Issues_Infos
        //                     where ii.uid == Convert.ToInt32(hfItemID.Value)
        //                     select ii).FirstOrDefault();
        //updatecomment.Comment_Count = updatecomment.Comment_Count + 1;
        //db.SubmitChanges();

        //Response.Redirect(Request.Url.ToString());
    }
}
