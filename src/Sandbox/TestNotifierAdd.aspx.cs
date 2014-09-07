using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.CS_Code.Worker;

using PimpLibrary.Static.Enums;
using PimpLibrary.Communications;
using PimpLibrary.Utopia;
using PimpLibrary.Utopia.Kingdom;
using PimpLibrary.Utopia.Ops;

public partial class Sandbox_TestNotifierAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Notifier notf = new Notifier();
        Notification note = new Notification();

        NotificationDetail detail = new NotificationDetail();
        detail.Location = new KingdomLocation{ Island = 10, Kingdom = 20 };
        detail.EventType = CeTypeEnum.CaputeredLand;
        detail.EventText = "he ravaged your lands and took 8 acers.";
        detail.Date = new UtopiaDate { Year = 0, Month = 6, Day = 4 };
        detail.Attacker = new AttackerOp{ Location = new KingdomLocation { Island = 12, Kingdom = 18 }, Name = "Muzzli" };
        note.Details.Add(detail);

        detail = new NotificationDetail();
        detail.Location = new KingdomLocation { Island = 10, Kingdom = 20 };
        detail.EventType = CeTypeEnum.CaputeredLand;
        detail.EventText = "he ravaged your lands and took 118 acers.";
        detail.Date = new UtopiaDate { Year = 0, Month = 7, Day = 4 };
        detail.Attacker = new AttackerOp { Location = new KingdomLocation { Island = 18, Kingdom = 18 }, Name = "Pruttan" };
        note.Details.Add(detail);

        note.ProvinceName = "Heiniken served Ice Cold";
        note.UserId = new Guid("4AF13CA0-2F8E-4E13-AED8-358CEFA78ACF");
        note.ProvinceId = new Guid("FB400048-4861-4AC7-9EAD-1292A704589B");

        notf.SendNotification(note);
        notf.Commit();
    }
}