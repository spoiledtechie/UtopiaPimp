using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

using Boomers.Utilities.Extensions;
using Pimp.UParser;
using Pimp.UCache;
using PimpLibrary.Static.Enums;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_Columns : MyBasePageCS
{

    PimpUserWrapper  currentUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        currentUser = new PimpUserWrapper ();


        if (!IsPostBack)
        {
            if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
                pnlMonarch.Visible = true;

            Guid userKingdomID;
            if (currentUser.PimpUser.MonarchType != MonarchType.none && currentUser.PimpUser.MonarchType != MonarchType.kdMonarch)
                userKingdomID =currentUser.PimpUser.StartingKingdom;
            else
                userKingdomID = currentUser.PimpUser.UserID;
            var cachedKingdom = KingdomCache.getKingdom(currentUser.PimpUser.StartingKingdom);
            string[] list =Column.CreateSetsList(false, currentUser,  cachedKingdom);

            divMySets.InnerHtml = list[0];

            divItem.InnerHtml += Column.ColumnList(userKingdomID, Convert.ToInt32(list[1]), currentUser);
        }
    }

    /// <summary>
    /// Adds the columns of the game.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddColumn_Click(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_Column_Name_Pull UCNP = new CS_Code.Utopia_Column_Name_Pull();
        UCNP.Alt = txtbxToolTip.Text;
        UCNP.User_ID_Added = pimpUser.PimpUser.UserID;
        UCNP.Enabled = true;
        UCNP.DateTime_Added = DateTime.UtcNow;
        UCNP.Column_Name = txtbxAddColumn.Text;
        UCNP.Category_ID = Convert.ToInt32(ddlSelectCatagory.SelectedItem.Value);
        db.Utopia_Column_Name_Pulls.InsertOnSubmit(UCNP);
        db.SubmitChanges();
        txtbxAddColumn.Text = "";
        ddlSelectCatagory.SelectedIndex = 0;
    }
    /// <summary>
    /// Deletes Columns for the game.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        var DeleteColumn = (from CN in db.Utopia_Column_Name_Pulls
                            where CN.uid == Convert.ToInt32(ddlDelete.SelectedItem.Value)
                            select CN);
        db.Utopia_Column_Name_Pulls.DeleteAllOnSubmit(DeleteColumn);
        db.SubmitChanges();
        ddlDelete.SelectedIndex = 0;

        var columnInfo = (from xx in db.Utopia_Column_Names
                          select xx);
        var Columns = (from xx in db.Utopia_Column_Name_Pulls
                       select xx.uid).ToList();
        foreach (var item in columnInfo)
        {
            string ids = string.Empty;
            MatchCollection mc = URegEx.rgxNumber.Matches(item.Column_IDs);
            foreach (Match num in mc)
                foreach (int col in Columns)
                    if (Convert.ToInt32(num.Value) == col)
                        ids += num.Value + ":";

            item.Column_IDs = ids;
        }
        db.SubmitChanges();
    }
}
