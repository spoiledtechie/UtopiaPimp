using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Pimp.UParser;

public partial class admin_UAdmin_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        lblKingdomCount.Text = (from xx in db.Utopia_Kingdom_Infos
                                select xx.Owner_Kingdom_ID).Distinct().Count().ToString("N0");
        lblKingdomsUpdated.Text = (from xx in db.Utopia_Kingdom_Infos
                                   where xx.Updated_By_DateTime >= DateTime.UtcNow.AddDays(-5)
                                   select xx.Owner_Kingdom_ID).Distinct().Count().ToString("N0");
        lblUserProvinces.Text = (from xx in db.Utopia_Province_Data_Captured_Gens
                                 where xx.Owner_User_ID != null
                                 select xx.uid).Count().ToString("N0");
    }

    protected void btnDeleteOldData_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Kingdom_CE where DateTime_Added < {0}", dtStart);

        //var getKI = (from xx in db.Utopia_Kingdom_Infos
        //             where xx.Added_By_DateTime < dtStart
        //             select xx).ToList();
        //db.Utopia_Kingdom_Infos.DeleteAllOnSubmit(getKI);


        //var getGen = (from xx in db.Utopia_Province_Data_Captured_Gens
        //              where xx.Updated_By_DateTime < dtStart
        //              select xx);
        //db.Utopia_Province_Data_Captured_Gens.DeleteAllOnSubmit(getGen);

        //lblDeletedData.Text = "Deleted Data";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Kingdom_Monarch_Message where TimeStamp < {0}", dtStart);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Kingdom_Monarch_Settings where Last_Updated_DateTime < {0}", dtStart);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Kingdom_Monarch_Voting where DateTime_Updated < {0}", dtStart);
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Data_Captured_Attack where DateTime_Added < {0}", dtStart);
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Data_Captured_Science where DateTime_Added < {0}", dtStart);
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Data_Captured_Survey where DateTime_Updated < {0}", dtStart);
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Data_Captured_Type_Military where DateTime_Added < {0}", dtStart);
    }
    protected void Button8_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Memos where TimeStamp < {0}", dtStart);
    }
    protected void Button9_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Notes where Added_By_DataTime < {0}", dtStart);
    }
    protected void Button10_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Ops where TimeStamp < {0}", dtStart);
    }
    protected void Button11_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Target_Finder");
    }
    protected void ProvinceData_Click(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Province_Data_Captured_Gen where Updated_By_DateTime < {0}", dtStart);
    }
    protected void CBData_Click(object sender, EventArgs e) 
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();


        //iterates through the last age deleting 10 days worth of CBs at a time because this table can get so been and create too large of a transaction if 
        //deleted in one pass.
        for (int i = 90; i > -1; i = i - 10)
        {
            db.ExecuteCommand("delete  from Utopia_Province_Data_Captured_CB where Updated_By_DateTime < {0}", dtStart.AddDays(-1 * i));
        }
    }
    protected void DeleteKingdomData_OnClick(object sender, EventArgs e)
    {
        DateTime dtStart = UtopiaParser.WORLD_OF_LEGENDS_OLD_START_DATE.AddDays(-2);
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("delete  from Utopia_Kingdom_Info where Updated_By_DateTime < {0}", dtStart);
    }
    protected void IndexBoomersBtn_Click(object sender, EventArgs e)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("USE pimp --Enter the name of the database you want to reindex DECLARE @TableName varchar(255) DECLARE TableCursor CURSOR FOR  SELECT table_name FROM information_schema.tables  WHERE table_type = 'base table'  TableCursor  NEXT FROM TableCursor INTO @TableName  WHILE @@FETCH_STATUS = 0  BEGIN  DBCC DBREINDEX(@TableName,' ',90)  FETCH NEXT FROM TableCursor INTO @TableName  END  CLOSE TableCursor  DEALLOCATE TableCursor");
        }
    protected void btnIndexPimp_Click(object sender, EventArgs e)
    {
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        db.ExecuteCommand("USE boomersMembership--Enter the name of the database you want to reindex DECLARE @TableName varchar(255) DECLARE TableCursor CURSOR FOR SELECT table_name FROM information_schema.tables WHERE table_type = 'base table' OPEN TableCursor FETCH NEXT FROM TableCursor INTO @TableName WHILE @@FETCH_STATUS = 0 BEGIN DBCC DBREINDEX(@TableName,' ',90) FETCH NEXT FROM TableCursor INTO @TableName END CLOSE TableCursor DEALLOCATE TableCursor");
    }
}