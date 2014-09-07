using System.Web.SessionState;

/// <summary>
/// MyPageBase applies the Theme for the website.
/// </summary>
public class MyBasePageCS : System.Web.UI.Page
{
    /// <summary>
    /// Applies the Profile to every page that has the mybasepagecs.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        switch (User.Identity.IsAuthenticated)
        {
            case true:
                System.Web.Profile.ProfileBase MyProfile = System.Web.HttpContext.Current.Profile;

                try
                { Page.Theme = MyProfile.GetPropertyValue("ThemePreference").ToString(); }
                catch
                {
                    Page.Theme = "Default";
                }

                //AuditUser.Audit();
                break;
        }
    }


}
public class MyBasePageCSAds : System.Web.UI.Page
{
    /// <summary>
    /// Applies the Profile to every page that has the mybasepagecs.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        switch (User.Identity.IsAuthenticated)
        {
            case true:
                System.Web.Profile.ProfileBase MyProfile = System.Web.HttpContext.Current.Profile;

                try
                { Page.Theme = MyProfile.GetPropertyValue("ThemePreference").ToString(); }
                catch
                {
                    Page.Theme = "Default";
                }

                break;
        }
    }

}
