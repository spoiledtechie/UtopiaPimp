<%@ Application Language="C#" %>
<script RunAt="server">

    protected void Application_BeginRequest()
    {

        //if (Request.IsLocal)
        //{
        //    MvcMiniProfiler.MiniProfiler.Start();
        //}
    }
    protected void Application_EndRequest()
    {
        //MvcMiniProfiler.MiniProfiler.Stop();
    }

    void Application_Start(object sender, EventArgs e)
    {
        Boomers.Utilities.Web.Timers.RestartApplication(1000 * 60 * 60 * 24 * 7); //7 Days

        Pimp.UData.UsersData.cachePhoneTypes();
        Pimp.UCache.ApplicationCache.getApplicationSettings();
        var a = Pimp.UData.UtopiaHelper.Instance.AttackType;
        var c = Pimp.UData.UtopiaHelper.Instance.ColumnNames;
        var k = Pimp.UData.UtopiaHelper.Instance.KingdomStances;
        var o = Pimp.UData.UtopiaHelper.Instance.Ops;
        var p = Pimp.UData.UtopiaHelper.Instance.Personalities;
        var r = Pimp.UData.UtopiaHelper.Instance.Races;
        var ra = Pimp.UData.UtopiaHelper.Instance.Ranks;
        //var bl = Pimp.UData.UtopiaHelper.Instance.CeTypes;
        var provs = Pimp.UData.TargetFinder.Instance.TargetedProvinces;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        //In Application End Event
        //System.Data.SqlClient.SqlDependency.Stop(System.Configuration.ConfigurationManager.ConnectionStrings["UPConnectionString"].ConnectionString);

    }

    void Application_Error(object sender, EventArgs e)
    {
        var lastException = Server.GetLastError();
        // ---&gt; System.Web.HttpException (0x80004005): The return URL specified for request redirection is invalid. at System.Web.Security.FormsAuthentication.GetReturnUrl(Boolean useDefaultIfAbsent) at System.Web.UI.WebControls.Login.GetRedirectUrl() at System.Web.UI.WebControls.Login.AttemptLogin() at System.Web.UI.WebControls.Login.OnBubbleEvent(Object source, EventArgs e) at 
        if (lastException.Message.Contains("Validation of viewstate MAC failed") || lastException.Message.Contains("does not exist.") || lastException.Message.Contains("The client disconnected") || lastException.Message.Contains("register.aspx") || lastException.ToString().Contains("The return URL specified for request redirection is invalid"))
            Response.Redirect("~/Default.aspx");
        else
        {
            SupportFramework.Data.Errors.logError(lastException);
            Context.ClearError();
            Response.Redirect("~/ErrorPage.aspx");
        }
        return;
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    /// <summary>
    /// Customize aspects of the MiniProfiler.
    /// </summary>
    //private void InitProfilerSettings()
    //{
    //    // some things should never be seen
    //    var ignored = MvcMiniProfiler.MiniProfiler.Settings.IgnoredPaths.ToList();
    //    ignored.Add("WebResource.axd");
    //    ignored.Add("/Styles/");
    //    MvcMiniProfiler.MiniProfiler.Settings.IgnoredPaths = ignored.ToArray();

    //    MvcMiniProfiler.MiniProfiler.Settings.Storage = new SampleWeb.Helpers.SqliteMiniProfilerStorage(SampleWeb.MvcApplication.ConnectionString);
    //    MvcMiniProfiler.MiniProfiler.Settings.SqlFormatter = new MvcMiniProfiler.SqlFormatters.InlineFormatter();
    //}
       
</script>
