using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SupportFramework
{
    /// <summary>
    /// Summary description for Applications
    /// </summary>
    public class Applications
    {
        static Applications instance = new Applications();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Applications()
        {
        }

        Applications()
        {
        }

        public static Applications Instance
        {
            get
            {
                return instance;
            }
        }
        private Guid getApplicationId()
        {
            CS_Code.AdminDataContext db = CS_Code.AdminDataContext.Get();
            return (from xx in db.vw_aspnet_Applications
                    where xx.LoweredApplicationName == Membership.ApplicationName.ToLower()
                    select xx.ApplicationId).FirstOrDefault();
        }

        private Guid _applicationId;
        public Guid ApplicationId
        {
            get
            {
                if (_applicationId == null || _applicationId == new Guid())
                {
                    _applicationId = getApplicationId();
                }
                return _applicationId;
            }
            set { _applicationId = value; }
        }


    }
}