using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Boomers.Utilities.Communications;
using Boomers.Utilities.Guids;
using System.Threading.Tasks;
using MvcMiniProfiler;


namespace CS_Code
{
    public partial class UtopiaDataContext : System.Data.Linq.DataContext
    {
        partial void OnCreated()
        {
            this.CommandTimeout = int.MaxValue;
        }
        public static UtopiaDataContext Get()
        {
            var conn = new MvcMiniProfiler.Data.ProfiledDbConnection(new  SqlConnection(ConfigurationManager.ConnectionStrings["UPConnectionString"].ConnectionString), MiniProfiler.Current);
            return new UtopiaDataContext(conn);
        }
    }
    public partial class AdminDataContext : System.Data.Linq.DataContext
    {
        partial void OnCreated()
        {
            this.CommandTimeout = int.MaxValue;
        }
        public static AdminDataContext Get()
        {
            var conn = new MvcMiniProfiler.Data.ProfiledDbConnection(new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString), MiniProfiler.Current);
            return new AdminDataContext(conn);
        }
    }
    public partial class GlobalDataContext : System.Data.Linq.DataContext
    {
        partial void OnCreated()
        {
            this.CommandTimeout = int.MaxValue;
        }
        public static GlobalDataContext Get()
        {
            var conn = new MvcMiniProfiler.Data.ProfiledDbConnection(new SqlConnection(ConfigurationManager.ConnectionStrings["UPConnectionString"].ConnectionString), MiniProfiler.Current);
            return new GlobalDataContext(conn);
        }
    }
}

namespace SupportFramework.Data
{
    /// <summary>
    /// Summary description for SQLStatementsCS
    /// </summary>
    public class AccessCapture
    {
        /// <summary>
        /// The connection string of the system.
        /// </summary>
        /// <returns></returns>
        public static string ConnectionStringID()
        { return ConfigurationManager.ConnectionStrings["UPConnectionString"].ConnectionString; }

        /// <summary>
        /// Caputes any data needed to a table.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static void captureData(string data, Guid userID)
        {
        
           CS_Code.GlobalDataContext db =  CS_Code.GlobalDataContext.Get();
           CS_Code.Global_Data_Capture gdc = new CS_Code.Global_Data_Capture();
           gdc.Raw_Data = data;
           gdc.date_time = DateTime.UtcNow;
           gdc.User_ID = userID;
           gdc.Application_Id = Applications.Instance.ApplicationId;
           db.Global_Data_Captures.InsertOnSubmit(gdc);
           db.SubmitChanges();
       
        }
    }
}