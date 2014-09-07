using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SupportFramework;
using SupportFramework.Users;

public partial class admin_Errors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        ltJavascriptInject.Text = "<script type=\"text/javascript\">";
        ltJavascriptInject.Text += "$(document).ready(function() {$('#" + gvErrorHandling.ClientID + "').tablesorter({widgets: ['zebra'], widgetZebra: {css: ['GridViewAlternatingRowStyle','GridviewRowStyle']}});});";
        ltJavascriptInject.Text += "</script>";
        //Cache.Remove("Errors");
        if (!IsPostBack)
        {
            CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();
            lblErrorCount.Text = (from tel in db.Global_Errors_Logs
                                  where tel.Application_Id == Applications.Instance.ApplicationId
                                  where tel.Reviewed == 0
                                  select tel.uid).Count().ToString("N0");
            PopulateErrors(SiteType.All);
        }

    }
    private enum SiteType
    {
        All,
        UtopiaPimp,
        UtopiaShrimp
    }
    private void PopulateErrors(SiteType type)
    {
        if (Cache["Errors"] != null)
        {
            var errors = Cache["Errors"] as List<Error>;

            switch (type)
            {
                case SiteType.UtopiaPimp:
                    errors = errors.Where(x => x.Error_URL.Contains("utopiapimp.com")).ToList();
                    break;
                case SiteType.UtopiaShrimp:
                    errors = errors.Where(x => x.Error_URL.Contains("utopiashrimp.com")).ToList();
                    break;
            }
            Cache.Add("Errors", errors, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
            gvErrorHandling.DataSource = errors;
            gvErrorHandling.DataBind();
        }
        else
        {


            CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();
            switch (type)
            {
                case SiteType.All:
                    var getErrors = (from xx in db.Global_Errors_Logs
                                     where xx.Application_Id == Applications.Instance.ApplicationId
                                     where xx.Reviewed == 0
                                     select new Error
                                     {
                                         uid = xx.uid,
                                         User_ID = (Guid)xx.User_ID.GetValueOrDefault(),
                                         userName =Memberships.getUserName(xx.User_ID.GetValueOrDefault()),
                                         User_Email = xx.User_Email,
                                         Load_Date = xx.Load_Date,
                                         Error_URL = xx.Error_URL,
                                         Error_Url_Path = xx.Error_Url_Path,
                                         Error_Url_QS = xx.Error_Url_QS,
                                         Error_Previous_Url_Path = xx.Error_Previous_Url_Path,
                                         Error_Previous_Url_QS = xx.Error_Previous_Url_QS,
                                         Domain = xx.Domain
                                     }).ToList();
                    Cache.Add("Errors", getErrors, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);

                    gvErrorHandling.DataSource = getErrors;
                    gvErrorHandling.DataBind();
                    break;
                case SiteType.UtopiaPimp:
                    var getErrorsPimp = (from xx in db.Global_Errors_Logs
                                         where xx.Application_Id == Applications.Instance.ApplicationId
                                         where xx.Reviewed == 0
                                         where xx.Error_URL.Contains("utopiapimp.com")
                                         select new Error
                                         {
                                             uid = xx.uid,
                                             User_ID = (Guid)xx.User_ID,
                                             userName = Memberships.getUserName(xx.User_ID.GetValueOrDefault()),
                                             User_Email = xx.User_Email,
                                             Load_Date = xx.Load_Date,
                                             Error_URL = xx.Error_URL,
                                             Error_Url_Path = xx.Error_Url_Path,
                                             Error_Url_QS = xx.Error_Url_QS,
                                             Error_Previous_Url_Path = xx.Error_Previous_Url_Path,
                                             Error_Previous_Url_QS = xx.Error_Previous_Url_QS,
                                             Domain = xx.Domain
                                         }).ToList();
                    Cache.Add("Errors", getErrorsPimp, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
                    gvErrorHandling.DataSource = getErrorsPimp;
                    gvErrorHandling.DataBind();
                    break;
                case SiteType.UtopiaShrimp:
                    var getErrorsShrimp = (from xx in db.Global_Errors_Logs
                                           where xx.Application_Id == Applications.Instance.ApplicationId
                                           where xx.Reviewed == 0
                                           where xx.Error_URL.Contains("utopiashrimp.com")
                                           select new Error
                                           {
                                               uid = xx.uid,
                                               User_ID = (Guid)xx.User_ID,
                                               userName = Memberships.getUserName(xx.User_ID.GetValueOrDefault()),
                                               User_Email = xx.User_Email,
                                               Load_Date = xx.Load_Date,
                                               Error_URL = xx.Error_URL,
                                               Error_Url_Path = xx.Error_Url_Path,
                                               Error_Url_QS = xx.Error_Url_QS,
                                               Error_Previous_Url_Path = xx.Error_Previous_Url_Path,
                                               Error_Previous_Url_QS = xx.Error_Previous_Url_QS,
                                               Domain = xx.Domain
                                           }).ToList();
                    Cache.Add("Errors", getErrorsShrimp, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
                    gvErrorHandling.DataSource = getErrorsShrimp;
                    gvErrorHandling.DataBind();
                    break;
            }
        }
    }
    /// <summary>
    /// Allows the Gridview to be sortable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_PreRender(object sender, EventArgs e)
    {
        //You only need the following 2 lines of code if you are not 
        // using an ObjectDataSource of SqlDataSource
        GridView gv = (GridView)sender;
        if (gv.Rows.Count > 0)
        {
            //This will add the <thead> and <tbody> elements
            //This adds the <tfoot> element. 
            //Remove if you don't have a footer row
            gv.UseAccessibleHeader = true;
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            //gv.FooterRow.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void btnReviewedAll_Click(object sender, EventArgs e)
    {
        CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();

        var getItems = (from tel in db.Global_Errors_Logs
                        where tel.Application_Id == Applications.Instance.ApplicationId
                        where tel.Reviewed == 0
                        select tel);

        foreach (var item in getItems)
            item.Reviewed = 1;

        db.SubmitChanges();
        lblErrorCount.Text = (from tel in db.Global_Errors_Logs
                              where tel.Application_Id == Applications.Instance.ApplicationId
                              where tel.Reviewed == 0
                              select tel.uid).Count().ToString("N0");
        gvErrorHandling.DataBind();
        Cache.Remove("Errors");
        PopulateErrors(SiteType.All);
    }
    /// <summary>
    /// When the button is pushed, it updates a row in the Errors table to show that it has been reviewed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    protected void gvErrorHandling_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        CS_Code.GlobalDataContext adb = CS_Code.GlobalDataContext.Get();

        //handles the gridview button review in error handling gridview.
        //checks to see the command name first.
        //then updates the item in the sql table which places a 1 at the item of interest.
        if (e.CommandName == "cmdReviewed")
        {

            int index = Convert.ToInt32(e.CommandArgument);

            var getError = (from tel in adb.Global_Errors_Logs
                            where tel.Application_Id == Applications.Instance.ApplicationId
                            where tel.uid == Convert.ToInt32(gvErrorHandling.DataKeys[index].Value)
                            select tel).FirstOrDefault();
            getError.Reviewed = 1;
            adb.SubmitChanges();

            if (Cache["Errors"] != null)
            {
                var errors = Cache["Errors"] as List<Error>;

                errors.Remove(errors.Where(x => x.uid == Convert.ToInt32(gvErrorHandling.DataKeys[index].Value)).FirstOrDefault());
                Cache.Add("Errors", errors, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);

                gvErrorHandling.DataSource = errors;
            }

            gvErrorHandling.DataBind();

        }
        else if (e.CommandName == "cmdReviewUsers")
        {
            int index = Convert.ToInt32(e.CommandArgument);

            var getError = (from tel in adb.Global_Errors_Logs
                            where tel.Application_Id == Applications.Instance.ApplicationId
                            where tel.uid == Convert.ToInt32(gvErrorHandling.DataKeys[index].Value)
                            select tel).FirstOrDefault();
            var getAllErrorsByUser = (from xx in adb.Global_Errors_Logs
                                      where xx.Application_Id == Applications.Instance.ApplicationId
                                      where xx.User_ID == getError.User_ID
                                      where xx.Reviewed == 0
                                      select xx);
            foreach (var item in getAllErrorsByUser)
                item.Reviewed = 1;

            adb.SubmitChanges();


            if (Cache["Errors"] != null)
            {
                var errors = Cache["Errors"] as List<Error>;

                while (errors.Where(x => x.User_ID == getError.User_ID).FirstOrDefault() != null)
                    errors.Remove(errors.Where(x => x.User_ID == getError.User_ID).FirstOrDefault());

                Cache.Add("Errors", errors, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);

                gvErrorHandling.DataSource = errors;
            }
            gvErrorHandling.DataBind();
        }
        else if (e.CommandName == "cmdDelete")
        {

            int index = Convert.ToInt32(e.CommandArgument);

            var getError = (from tel in adb.Global_Errors_Logs
                            where tel.Application_Id == Applications.Instance.ApplicationId
                            where tel.uid == Convert.ToInt32(gvErrorHandling.DataKeys[index].Value)
                            select tel).FirstOrDefault();
            adb.Global_Errors_Logs.DeleteOnSubmit(getError);
            adb.SubmitChanges();

            gvErrorHandling.DataBind();

        } 
        lblErrorCount.Text = (from tel in adb.Global_Errors_Logs
                                where tel.Application_Id == Applications.Instance.ApplicationId
                                where tel.Reviewed == 0
                                select tel.uid).Count().ToString("N0");

    }

    protected void btnUsePimp_Click(object sender, EventArgs e)
    {
        if (Cache["Errors"] != null)
            Cache.Remove("Errors");
        PopulateErrors(SiteType.UtopiaPimp);
    }
    protected void btnUseShrimp_Click(object sender, EventArgs e)
    {
        if (Cache["Errors"] != null)
            Cache.Remove("Errors");
        PopulateErrors(SiteType.UtopiaShrimp);
    }
    protected void btnClearCache_Click(object sender, EventArgs e)
    {
        if (Cache["Errors"] != null)
            Cache.Remove("Errors");
    }
}
public class Error
{
    public int uid { get; set; }
    public Guid User_ID { get; set; }
    public string userName { get; set; }
    public string User_Email { get; set; }
    public string Load_Date { get; set; }
    public string Error_URL { get; set; }
    public string Error_Url_Path { get; set; }
    public string Error_Url_QS { get; set; }
    public string Error_Previous_Url_Path { get; set; }
    public string Error_Previous_Url_QS { get; set; }
    public string Domain { get; set; }
}