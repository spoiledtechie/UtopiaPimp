using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Threading.Tasks;

namespace SupportFramework.Data
{
    public class SiteMap
    {
        private enum ChangeFreq
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        /// <summary>
        /// Adds a Node to the Site Map
        /// </summary>
        /// <param name="url">URL to add to SiteMap</param>
        /// <param name="modified">true oo false if the item has just been modified.</param>
        public static void AddNode(string url, bool modified)
        {
           
                CS_Code.GlobalDataContext db = CS_Code.GlobalDataContext.Get();
                var getItem = (from glob in db.Global_Sitemaps
                               where glob.Application_Id == SupportFramework.Applications.Instance.ApplicationId
                               where glob.url == url
                               select glob).FirstOrDefault();

                if (getItem == null)
                {
                    CS_Code.Global_Sitemap globs = new CS_Code.Global_Sitemap();
                    globs.Application_Id = SupportFramework.Applications.Instance.ApplicationId;
                    globs.url = url;
                    globs.lastmod = DateTime.UtcNow;
                    globs.changefreq = ChangeFreq.monthly.ToString();
                    db.Global_Sitemaps.InsertOnSubmit(globs);
                    db.SubmitChanges();
                }
                else
                {
                    switch (modified)
                    {
                        case true:
                            getItem.lastmod = DateTime.UtcNow;

                            TimeSpan span = DateTime.UtcNow.Subtract(getItem.lastmod);
                            if (span.Days > 365)
                                getItem.changefreq = ChangeFreq.yearly.ToString();
                            else if (span.Days > 31)
                                getItem.changefreq = ChangeFreq.monthly.ToString();
                            else if (span.Days > 7)
                                getItem.changefreq = ChangeFreq.weekly.ToString();
                            else if (span.Hours > 24)
                                getItem.changefreq = ChangeFreq.daily.ToString();
                            else
                                getItem.changefreq = ChangeFreq.hourly.ToString();
                            db.SubmitChanges();
                            break;
                    }
                }
            
        }
    }
}


// An example of a Route Handler

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security;
//using System.Web;
//using System.Web.Routing;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.Compilation;


///// <summary>
///// Summary description for RouteHandler
///// </summary>
//public class GameHandler : IRouteHandler
//{
//    public IHttpHandler GetHttpHandler(RequestContext rc)
//    {

//        foreach (var item in rc.RouteData.Values)
//        {
//            rc.HttpContext.Items[item.Key] = item.Value;
//        }
//        string VirtualPath = "~/games/games.aspx";
//        if (!UrlAuthorizationModule.CheckUrlAccessForPrincipal(VirtualPath,
//            rc.HttpContext.User,
//            rc.HttpContext.Request.HttpMethod))
//            throw (new SecurityException());
//        //var page = BuildManager.CreateInstanceFromVirtualPath(VirtualPath, typeof(Page)) as IHttpHandler;
//        var routingPage = BuildManager.CreateInstanceFromVirtualPath(VirtualPath, typeof(Page)) as IHttpHandler;
//        //routingPage.ISBNNumber = rc.RouteData.Values["ISBNid"] as string;
//        return (routingPage);
//    }
//}


//Apply to Global.ASAX File

//public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
//{
//            routes.Add(new System.Web.Routing.Route("games/{game}/{name}", new GameHandler()));
//                }