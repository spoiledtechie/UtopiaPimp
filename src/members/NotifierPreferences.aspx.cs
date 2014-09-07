using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.CS_Code.Worker;
using Pimp.UCache;
using Pimp;
using Pimp.Users;
using Pimp.Utopia;
using Pimp.UData;

public partial class members_NotifierPreferences : MyBasePageCS
{
    PimpUserWrapper  pimpUser;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;

         pimpUser = new PimpUserWrapper ();

         if (pimpUser.PimpUser.NotificationSettings == null)
        {
            CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
            var dbData = getNotifications(db);

            if (dbData == null)
            {
                CS_Code.Utopia_User_Notifier_Setting notifier = new CS_Code.Utopia_User_Notifier_Setting();
                notifier.User_ID = pimpUser.PimpUser.UserID;
                db.Utopia_User_Notifier_Settings.InsertOnSubmit(notifier);
                db.SubmitChanges();
                dbData = getNotifications(db);
            }

            pimpUser.PimpUser.NotificationSettings = dbData;
        }

         cbxArmyReturns.Checked = pimpUser.PimpUser.NotificationSettings.ArmyReturns;
         cbxAttacked.Checked = pimpUser.PimpUser.NotificationSettings.AttackAgainstSelf;
         cbxDragonInitiated.Checked = pimpUser.PimpUser.NotificationSettings.DragonProjectStarted;
         cbxDragonRavaging.Checked = pimpUser.PimpUser.NotificationSettings.DragonRavagingLands;
         cbxObsExpired.Checked = pimpUser.PimpUser.NotificationSettings.OpsEnded;
         cbxTrackedProvinceUpdated.Checked = pimpUser.PimpUser.NotificationSettings.TrackedProvinceUpdated;
         cbxWarAction.Checked = pimpUser.PimpUser.NotificationSettings.WarAction;

        for (int i = 0; i < ddlDeliveryMethod.Items.Count; i++)
        {
            if (ddlDeliveryMethod.Items[i].Value == pimpUser.PimpUser.NotificationSettings.DeliveryMethod.ToString())
                ddlDeliveryMethod.Items[i].Selected = true;
            else
                ddlDeliveryMethod.Items[i].Selected = false;
        }

        int frequency = (pimpUser.PimpUser.NotificationSettings.SendFrequency.Minutes == 0 ? 60 : pimpUser.PimpUser.NotificationSettings.SendFrequency.Minutes);
        for (int i = 0; i < ddlFrequency.Items.Count; i++)
        {
            if (ddlFrequency.Items[i].Value == frequency.ToString())
                ddlFrequency.Items[i].Selected = true;
            else
                ddlFrequency.Items[i].Selected = false;
        }
    }

    private UserPreferences.NotificationSettings getNotifications(CS_Code.UtopiaDataContext db)
    {
        var dbData = (from data in db.Utopia_User_Notifier_Settings
                      where data.User_ID == pimpUser.PimpUser.UserID
                      select new UserPreferences.NotificationSettings
                      {
                          ArmyReturns = data.ArmyReturns,
                          AttackAgainstSelf = data.AttacksOnSelf,
                          DragonProjectStarted = data.DragonProjectStarted,
                          DragonRavagingLands = data.DragonRavagingLands,
                          SendFrequency = new TimeSpan(0, 0, data.FrequencyInMinutes, 0),
                          LastEmailSent = DateTime.Now.AddYears(-1),
                          OpsEnded = data.OpsEnding,
                          TrackedProvinceUpdated = data.TrackedProvinceUpdated,
                          WarAction = data.WarAction,
                          DeliveryMethod = (UserPreferences.NotificationSettings.NotificationType)Enum.Parse(typeof(UserPreferences.NotificationSettings.NotificationType), data.SendMethod.ToString())
                      }).FirstOrDefault();
        return dbData;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        PimpUserWrapper  pimpUser = new PimpUserWrapper ();
       
        var userId = SupportFramework.Users.Memberships.getUserID();
        UserPreferences.NotificationSettings preferences = new UserPreferences.NotificationSettings();
        preferences.ArmyReturns = cbxArmyReturns.Checked;
        preferences.AttackAgainstSelf = cbxAttacked.Checked;
        preferences.DeliveryMethod = UserPreferences.NotificationSettings.NotificationType.Email;
        preferences.DragonProjectStarted = cbxDragonInitiated.Checked;
        preferences.DragonRavagingLands = cbxDragonRavaging.Checked;
        preferences.SendFrequency = new TimeSpan(0, Convert.ToInt32(ddlFrequency.SelectedValue), 0);
        preferences.OpsEnded = cbxObsExpired.Checked;
        preferences.TrackedProvinceUpdated = cbxTrackedProvinceUpdated.Checked;
        preferences.WarAction = cbxWarAction.Checked;
        pimpUser.PimpUser.NotificationSettings = preferences;
        PimpUserWrapper.updateListOfUsers(pimpUser.PimpUser);
        
        CS_Code.UtopiaDataContext db = CS_Code.UtopiaDataContext.Get();
        CS_Code.Utopia_User_Notifier_Setting notifier = db.Utopia_User_Notifier_Settings.First(x => x.User_ID == userId);
        notifier.ArmyReturns = preferences.ArmyReturns;
        notifier.AttacksOnSelf = preferences.AttackAgainstSelf;
        notifier.DragonProjectStarted = preferences.DragonProjectStarted;
        notifier.DragonRavagingLands = preferences.DragonRavagingLands;
        notifier.FrequencyInMinutes = Convert.ToInt32(ddlFrequency.SelectedValue);
        notifier.OpsEnding = preferences.OpsEnded;
        notifier.SendMethod = (byte)preferences.DeliveryMethod;
        notifier.TrackedProvinceUpdated = preferences.TrackedProvinceUpdated;
        notifier.WarAction = preferences.WarAction;
        db.SubmitChanges();

        NotifierWorker.UpdateUserPreferences(userId, preferences);
        lblStatus.Text = "Your preferences has been updated";
    }
}