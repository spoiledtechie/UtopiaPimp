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
using Pimp;
using Pimp.Users;
using Pimp.UData;

public partial class controls_IssueVotedBox : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PimpUserWrapper  pimpUser = new PimpUserWrapper ();

            if (pimpUser.PimpUser.IsUserAdmin)
            {
                ddlStatus.Visible = true;
                if (ddlStatus.SelectedItem != null)
                    ddlStatus.SelectedItem.Text = lblStatus.Text;

            }
        }
    }
    /// <summary>
    /// gets the user vote for the Issue.
    /// </summary>
    //public int Voted
    //{
    //    get
    //    {
    //        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
    //        return (from IV in db.Issues_Votes
    //                where IV.Issues_ID == IssueID && IV.User_ID == userID
    //                select IV.Vote_Y_N).FirstOrDefault();
    //    }
    //    set
    //    {
    //        if (value == null || value == 0)
    //        {
    //            lbVote.Visible = true;
    //            lblCheckVote.Visible = true;
    //        }
    //        else
    //        {
    //            lblCheckVote.Visible = true;
    //            lbVote.Visible = false;

    //            switch (value)
    //            {
    //                case 1:
    //                    lblCheckVote.Text = "voted up";
    //                    break;
    //                case 2:
    //                    lblCheckVote.Text = "voted down";
    //                    break;
    //            }
    //        }
    //    }
    //}
    /// <summary>
    /// sets the status of the Issue.
    /// </summary>
    //public int Status
    //{
    //    get
    //    {
    //        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
    //        return (from ISP in db.Issues_Status_Pulls
    //                where ISP.Status == lblStatus.Text
    //                select ISP.uid).FirstOrDefault();
    //    }
    //    set
    //    {
    //        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
    //        string status = (from ISP in db.Issues_Status_Pulls
    //                         where ISP.uid == value
    //                         select ISP.Status).FirstOrDefault().ToLower();
    //        lblStatus.Text = status;
    //    }
    //}

    /// <summary>
    /// gets and sets the issue ID.
    /// </summary>
    public int IssueID
    {
        get
        {
            if (hfIssueID.Value.Length == 0)
                return 0;
            else
                return Convert.ToInt32(hfIssueID.Value);
        }
        set { hfIssueID.Value = value.ToString(); }
    }
    /// <summary>
    /// Sets vote count of Item.
    /// </summary>
    public int VoteCount
    {
        get { return Convert.ToInt32(lblVoteCount.Text); }
        set
        {
            if (value > 1)
                lblVountCountText.Text = "votes";
            else
                lblVountCountText.Text = "vote";

            lblVoteCount.Text = value.ToString();
        }
    }
    protected void lbVote_Click(object sender, EventArgs e)
    {
        //CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //CS_Code.Issues_Vote IV = new CS_Code.Issues_Vote();
        //IV.DateTime_Voted = DateTime.UtcNow;
        //IV.Issues_ID = IssueID;
        //IV.User_ID = userID;
        //IV.Vote_Y_N = 1;
        //db.Issues_Votes.InsertOnSubmit(IV);

        //var GetIssues = (from II in db.Issues_Infos
        //                 where II.uid == IssueID
        //                 select II).FirstOrDefault();
        //GetIssues.Vote_Count_Yes += 1;
        //db.SubmitChanges();

        //Voted = 1;
        //Status = GetIssues.Status_ID;
        //VoteCount = GetIssues.Vote_Count_Yes;
        //upUpdateVoteBox.Update();
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        //CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        //var ChangeStatus = (from ii in db.Issues_Infos
        //                    where ii.uid == Convert.ToInt32(hfIssueID.Value)
        //                    select ii).FirstOrDefault();
        //ChangeStatus.Status_ID = Convert.ToInt32(ddlStatus.SelectedValue);
        //ChangeStatus.Updated_On = DateTime.UtcNow;
        //ChangeStatus.Updated_By = userID;
        //if (ChangeStatus.Status_ID == 2)
        //{
        //    ChangeStatus.Closed_By = userID;
        //    ChangeStatus.Closed_On = DateTime.UtcNow;
        //}
        //db.SubmitChanges();
        //lblStatus.Text = ddlStatus.SelectedItem.Text;
    }
}
