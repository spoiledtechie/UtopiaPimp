using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Admin
{
        public class UserRoles
        {
            public string role { get; set; }
        }
        public class UserItems
        {
            public string userName { get; set; }
            public string email { get; set; }
            public string passwordQuestion { get; set; }
            public string comments { get; set; }
            public string createDate { get; set; }
            public string approved { get; set; }
            public string lastLogin { get; set; }
            public string locked { get; set; }
            public string[] roles { get; set; }
            public int userErrors { get; set; }
            public int userPageViews { get; set; }
            public bool userOnline { get; set; }
        }
}
