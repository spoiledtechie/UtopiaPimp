using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CS_Code;

using PimpLibrary.Static.Enums;
using Pimp.UParser;
using PimpLibrary.Utopia;
using PimpLibrary.Communications;
using SupportFramework.Data;
using Pimp.UData;

namespace App_Code.CS_Code.Worker
{
    public static class GatherExpiredOps
    {
        public static void GatherDataForNotifier()
        {
            Notifier notifier = new Notifier();
            try
            {                
                UtopiaDataContext ctx = new UtopiaDataContext();
                DateTime now = DateTime.UtcNow; //Always use GMT with inserting into DBs...  All other Dates are GMT's...
                DateTime startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                DateTime endTime = new DateTime(now.Year, now.Month, now.Day, now.AddHours(1).Hour, 0, 0);
                var results = from data in ctx.Utopia_Province_Ops
                              where data.Expiration_Date.HasValue && data.Expiration_Date.Value >= startTime && data.Expiration_Date <= endTime
                              join data2 in ctx.Utopia_Province_Data_Captured_Gens on data.Added_By_Province_ID equals data2.Province_ID
                              where data2.Owner_User_ID.HasValue
                              select new
                              {
                                  data.Op_ID,
                                  data.OP_Text,
                                  data.Expiration_Date,
                                  data.negated,
                                  data2.Owner_User_ID,
                                  data2.Province_ID
                              };

                var milResults = from xx in ctx.Utopia_Province_Data_Captured_Type_Militaries // Find all Returning Armies
                                 from yy in ctx.Utopia_Province_Data_Captured_Gens
                                 where xx.Military_Location == 2 //2==Away 1==Home
                                 where xx.Time_To_Return.HasValue && xx.Time_To_Return.Value >= startTime && xx.Time_To_Return <= endTime
                                 where xx.Province_ID_Added == yy.Province_ID
                                 where xx.Province_ID == yy.Province_ID
                                 select new
                                 {
                                     xx.uid,
                                     xx.CapturedLand,
                                     xx.DateTime_Added,
                                     xx.Elites,
                                     xx.Regs_Off,
                                     xx.Regs_Def,
                                     xx.Soldiers,
                                     xx.Province_ID,
                                     xx.Province_ID_Added,
                                     yy.Owner_User_ID,
                                     xx.Time_To_Return
                                 };

                foreach (var result in results)
                {
                    Notification note = new Notification();
                    NotificationDetail detail = new NotificationDetail();
                    if (result.Expiration_Date.HasValue)
                        detail.Date = UtopiaParser.getUtopiaDateTime2(result.Expiration_Date.Value);
                    else
                        detail.Date = new UtopiaDate { Day = 0, Month = 0, Year = 0 };

                    var op =UtopiaHelper.Instance.Ops.First(x => x.uid == result.Op_ID);
                    detail.EventText = "Your spell '" + op.OP_Name + "' is about to expire." + (result.Expiration_Date.HasValue ? " Expiration time is " + result.Expiration_Date.Value.ToString("yyyy-MM-dd HH:mm") + " GMT" : "");
                    detail.EventType = CeTypeEnum.OpsEnding;
                    note.Details.Add(detail);

                    note.ProvinceId = result.Province_ID;
                    note.UserId = result.Owner_User_ID.GetValueOrDefault();

                    notifier.SendNotification(note);
                }
                foreach (var result in milResults) //Military Returns
                {
                    Notification note = new Notification();
                    NotificationDetail detail = new NotificationDetail();
                    if (result.Time_To_Return.HasValue)
                        detail.Date = UtopiaParser.getUtopiaDateTime2(result.Time_To_Return.Value);
                    else
                        detail.Date = new UtopiaDate { Day = 0, Month = 0, Year = 0 };
                    detail.EventText = "Your Military with '" + (result.Elites.HasValue ? result.Elites.Value.ToString() : "0") + " Elites, " + (result.Regs_Off.HasValue ? result.Regs_Off.Value.ToString() : "0") + " Offense, " + (result.Soldiers.HasValue ? result.Soldiers.Value.ToString() : "0") + " Soldiers' is about to return." + (result.Time_To_Return.HasValue ? " Expiration time is " + result.Time_To_Return.Value.ToString("yyyy-MM-dd HH:mm") + " GMT" : "");
                    detail.EventType = CeTypeEnum.ArmyReturns;

                    note.ProvinceId = result.Province_ID;
                    note.UserId = result.Owner_User_ID.GetValueOrDefault();
                    note.Details.Add(detail);

                    notifier.SendNotification(note);
                }
                notifier.Commit();
            }
            catch (Exception e)
            {
                Errors.logError(e, notifier.ToString());
            }                
        }
    }
}