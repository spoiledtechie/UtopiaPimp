using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Boomers.Utilities.Web
{
    public class Helper
    {
        #region URL and Navigation
        /// <summary>
        /// Returns the url to the root of the application for client based calls.
        /// </summary>
        /// <returns></returns>
        public static string GetClientAbsoluteURL(HttpContext context)
        {
            string appPath = context.Request.ApplicationPath;
            if (!appPath.EndsWith("/"))
                appPath += "/";

            return context.Request.IsSecureConnection ? "https://" : "http://" + context.Request.Url.Authority + appPath;
        }
        /// <summary>
        /// If this function is called, the request is forced to be an ssl secured request.
        /// </summary>
        /// <param name="context">The current context</param>
        public static void ForceSSLConnection(HttpContext context)
        {
            if (!context.Request.IsSecureConnection)
                context.Response.Redirect(context.Request.Url.ToString().Replace(context.Request.Url.Scheme, "https"));
        }
        /// <summary>
        /// Sends permanent redirection headers (301)
        /// </summary>
        public static void PermanentRedirect(string url, HttpContext context)
        {
            if (url.EndsWith("Default.aspx", StringComparison.OrdinalIgnoreCase))
                url = url.Replace("Default.aspx", string.Empty);

            context.Response.Clear();
            context.Response.StatusCode = 301;
            context.Response.AppendHeader("location", url);
            context.Response.End();
        }
        #endregion
        #region Sitemap
        /// <summary>
        /// Builds out the sitemap into html using lists and linking to each page.
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns>A string holding the html</returns>
        public static string BuildSitemapHtml(SiteMapNode rootNode)
        {
            StringBuilder sb = new StringBuilder();
            if (rootNode != null)
            {
                sb.AppendFormat("<h1><a href='{0}' runat='server'>{1}</a></h1>", rootNode.Url, rootNode.Title);

                if (rootNode.HasChildNodes)
                {
                    sb.Append("<ul>");
                    sb = BuildSitemapChildHtml(rootNode, sb);
                    sb.Append("<ul>");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Recursively build childNodes for a sitemap
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        private static StringBuilder BuildSitemapChildHtml(SiteMapNode rootNode, StringBuilder sb)
        {
            foreach (SiteMapNode node in rootNode.ChildNodes)
            {
                sb.AppendFormat("<h3><a href='{0}' runat='server'>{1}</a></h3>", node.Url, node.Title);
                if (node.HasChildNodes)
                {
                    sb.Append("<ol>");
                    foreach (SiteMapNode child in node.ChildNodes)
                    {
                        if (child.HasChildNodes)
                            sb = BuildSitemapChildHtml(child, sb);

                        sb.AppendFormat("<li><a href='{0}' runat='server'>{1}</a></li>", child.Url, child.Title);
                    }
                    sb.Append("</ol>");
                }
            }
            return sb;
        }
        #endregion
    }
}
