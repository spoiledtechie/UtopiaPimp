using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Boomers.Utilities.Web
{
    public static class ActionValidator
    {
        private const int DURATION = 10; // 10 min period

        public enum ActionTypeEnum
        {
            FirstVisit = 100, // The most expensive one, choose the value wisely. 
            ReVisit = 1000,  // Welcome to revisit as many times as user likes
            Postback = 5000,    // Not must of a problem for us
            AddNewWidget = 100,
            AddNewPage = 100,
        }
        public static bool IsValid(ActionTypeEnum actionType)
        {
            HttpContext context = HttpContext.Current;
            if (context.Request.Browser.Crawler) return false;

            string key = actionType.ToString() + context.Request.UserHostAddress;
            var hit = (HitInfo)(context.Cache[key] ?? new HitInfo());

            if (hit.Hits > (int)actionType) return false;
            else hit.Hits++;

            if (hit.Hits == 1)
                context.Cache.Add(key, hit, null, DateTime.Now.AddMinutes(10),
                   System.Web.Caching.Cache.NoSlidingExpiration,
                   System.Web.Caching.CacheItemPriority.Normal, null);
            return true;
        }
    }
}
public class HitInfo
{
    public int Hits;
}