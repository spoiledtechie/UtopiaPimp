using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;


namespace SupportFramework.AdminData
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    public class Data
    {
        public static void truncateTable(string tableName, DataContext db)
        {
            db.ExecuteCommand("truncate table " + tableName);
        }
    }
}