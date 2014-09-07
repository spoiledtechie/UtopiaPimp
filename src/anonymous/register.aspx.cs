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
using YAF.Classes.Utils;
using YAF.Classes.Data;

public partial class anonymous_register : MyBasePageCS
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// Inserts the additional information into the UserTable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Created_User(object sender, EventArgs e)
    {
        TextBox Username, AIM, ICQ, MSN, Yahoo, Gmail;
        Username = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
        AIM = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtbxAIM");
        ICQ = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtbxICQ");
        MSN = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtbxMSN");
        Yahoo = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtbxYahoo");
        Gmail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtbxGmail");

        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var query = (from UUIMIP in db.Utopia_User_IM_Info_Pulls
                     select UUIMIP).ToArray();

        if (AIM.Text != "")
            SubmitAIM(SupportFramework.Users.Memberships.getUserID(Username.Text), AIM.Text, (from xx in query where xx.AIM_Name == "AIM" select xx.uid).FirstOrDefault());

        if (ICQ.Text != "")
            SubmitAIM(SupportFramework.Users.Memberships.getUserID(Username.Text), ICQ.Text, (from xx in query where xx.AIM_Name == "ICQ" select xx.uid).FirstOrDefault());

        if (MSN.Text != "")
            SubmitAIM(SupportFramework.Users.Memberships.getUserID(Username.Text), MSN.Text, (from xx in query where xx.AIM_Name == "MSN" select xx.uid).FirstOrDefault());

        if (Yahoo.Text != "")
            SubmitAIM(SupportFramework.Users.Memberships.getUserID(Username.Text), Yahoo.Text, (from xx in query where xx.AIM_Name == "Yahoo" select xx.uid).FirstOrDefault());

        if (Gmail.Text != "")
            SubmitAIM(SupportFramework.Users.Memberships.getUserID(Username.Text), Gmail.Text, (from xx in query where xx.AIM_Name == "Gmail" select xx.uid).FirstOrDefault());

        RoleMembershipHelper.SetupUserRoles(YafContext.Current.PageBoardID, Username.Text);

        CS_Code.Utopia_User_Notifier_Setting notifier = new CS_Code.Utopia_User_Notifier_Setting();
        notifier.User_ID = SupportFramework.Users.Memberships.getUserID(Username.Text);
        db.Utopia_User_Notifier_Settings.InsertOnSubmit(notifier);
        db.SubmitChanges();
    }
    private void SubmitAIM(System.Guid UserID, string IMText, int IMID)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_User_IM_Info UUIMI = new CS_Code.Utopia_User_IM_Info();
        UUIMI.User_ID = UserID;
        UUIMI.IM_Name = IMText;
        UUIMI.Date_Time = DateTime.UtcNow;
        UUIMI.IM_ID = IMID;
        db.Utopia_User_IM_Infos.InsertOnSubmit(UUIMI);
        db.SubmitChanges();
    }
}
