using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Boomers.Utilities.Communications;
using CS_Code;
public partial class Sandbox_TestGatherOpsFromDBToNotifier : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //UtopiaDataContext ctx = new UtopiaDataContext();
        //    DateTime now = DateTime.UtcNow; //Always use GMT with inserting into DBs...  All other Dates are GMT's...
        //    DateTime startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
        //    DateTime endTime = new DateTime(now.Year, now.Month, now.Day, now.AddHours(1).Hour, 0, 0);
        //var milResults = from xx in ctx.Utopia_Province_Data_Captured_Type_Militaries // Find all Returning Armies
        //                 from yy in ctx.Utopia_Province_Data_Captured_Gens
        //                 where xx.Military_Location == 2 //2==Away 1==Home
        //                 where xx.Time_To_Return.HasValue && xx.Time_To_Return.Value >= startTime && xx.Time_To_Return <= endTime
        //                 where xx.Province_ID_Added == yy.Province_ID
        //                 where xx.Province_ID == yy.Province_ID
        //                 select new
        //                 {
        //                     xx.uid,
        //                     xx.CapturedLand,
        //                     xx.DateTime_Added,
        //                     xx.Elites,
        //                     xx.Regs_Off,
        //                     xx.Regs_Def,
        //                     xx.Soldiers,
        //                     xx.Province_ID,
        //                     xx.Province_ID_Added,
        //                     yy.Owner_User_ID,
        //                     xx.Time_To_Return
        //                 };

        //Response.Write("Correct start date: " + startTime.ToString("yyyy-MM-dd HH:mm") + " - Correct end date: " + endTime.ToString("yyyy-MM-dd HH:mm") + "<br />");
        //foreach (var result in milResults)
        //{
        //    Response.Write(string.Format("U: {0}, Land: {1}, Exp: {2}<br />", result.uid, result.CapturedLand, result.Time_To_Return.Value.ToString("yyyy-MM-dd HH:mm")));
        //}





        //OpsGatherer f = new OpsGatherer();
        //f.ExpiredOpsGatherer(new Guid("b17ec769-4440-4b85-a628-71f46d13f6ec"));

        //TestGatherOpsFromDBToNotifier
        App_Code.CS_Code.Worker.GatherExpiredOps.GatherDataForNotifier();

        //UtopiaDataContext ctx = new UtopiaDataContext();
        //DateTime now = DateTime.UtcNow; //Always use GMT with inserting into DBs...  All other Dates are GMT's...
        //DateTime startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
        //DateTime endTime = new DateTime(now.Year, now.Month, now.Day, now.AddHours(1).Hour, 0, 0);
        //var results = from data in ctx.Utopia_Province_Ops
        //              where data.Expiration_Date.HasValue && data.Expiration_Date.Value >= startTime && data.Expiration_Date <= endTime
        //              join data2 in ctx.Utopia_Province_Data_Captured_Gens on data.Added_By_Province_ID equals data2.Province_ID
        //              select new
        //              {
        //                  data.Op_ID,
        //                  data.OP_Text,
        //                  data.Expiration_Date,
        //                  data.negated,
        //                  data2.Owner_User_ID,
        //                  data2.Province_ID
        //              };

        //Response.Write("Correct start date: " + startTime.ToString("yyyy-MM-dd HH:mm") + " - Correct end date: " + endTime.ToString("yyyy-MM-dd HH:mm") + "<br />");
        ////Response.Write("total: " + results.Count + "<br />");

        //foreach (var res in results)
        //{
        //    var op = UtopiaParser.GetOps.First(x => x.uid == res.Op_ID);
            
        //    Response.Write("Op: " + op.OP_Name + " , Expires: " + res.Expiration_Date.Value.ToString("yyyy-MM-dd HH:mm") + "<br />");
        //}
    }
}