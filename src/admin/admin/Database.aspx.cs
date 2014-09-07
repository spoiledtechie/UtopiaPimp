using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Boomers.Admin.MsSql;
using Boomers.Admin.UI;

public partial class admin_admin_Database : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteDb db = new SiteDb();
            db.getSiteDatabase(CS_Code.AdminDataContext.Get());
            ltMemInformation.Text += db.Ui;
            lblDBName.Text = db.DbName;
            lblDBSize.Text = db.DbSize;
            lblDBReserved.Text = db.dbReservedSpace;
            lblDBData.Text = db.dbDataSize;
            lblDBIndexSize.Text = db.dbIndexSize;
            lblDBUnUsed.Text = db.dbUnusedSpace;
            lblTotalTables.Text = db.TableCount.ToString("N0");
            lbltblRows.Text = db.RowsCount.ToString("N0");


            db.getSiteDatabase(CS_Code.UtopiaDataContext.Get());
            ltSiteInformation.Text += db.Ui;
            lblSiteDBName.Text = db.DbName;
            lblSiteDBSize.Text = db.DbSize;
            lblSiteDBReserved.Text = db.dbReservedSpace;
            lblSiteDBData.Text = db.dbDataSize;
            lblSiteDBIndexSize.Text = db.dbIndexSize;
            lblSiteDBUnUsed.Text = db.dbUnusedSpace;
            lblSiteTotalTables.Text = db.TableCount.ToString("N0");
            lblSitetblRows.Text = db.RowsCount.ToString("N0");
        }
    }
}


