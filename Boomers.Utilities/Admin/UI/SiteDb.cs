using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Text.RegularExpressions;

using Boomers.Admin.MsSql;



namespace Boomers.Admin.UI
{
    public class SiteDb
    {
                /// <summary>
        /// gets the ui to build the site db for any site tables.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public SiteDb getSiteDatabase(DataContext db)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table id=\"tableInfoSite\" class=\"tbl\">");
            sb.Append("<thead><tr>");
            sb.Append("<th>Table Name</th><th class=\"{sorter: 'fancyNumber'}\">Rows</th><th class=\"{sorter: 'fancyNumber'}\">Reserved Space</th><th class=\"{sorter: 'fancyNumber'}\">Data</th><th class=\"{sorter: 'fancyNumber'}\">Index Size</th><th class=\"{sorter: 'fancyNumber'}\">Unused Space</th><th>Truncate Table</th>");
            sb.Append("</tr></thead>");

            IEnumerable<DataTables> query = db.ExecuteQuery<DataTables>("SELECT * FROM sysobjects WHERE type = 'U'");
            List<DataTables> tables = query.OrderBy(t => t.name).ToList();
            int counter = 0;
            int rowsTotal = 0;
            int dbDataSize = 0;
            int dbReserved = 0;
            int dbIndexed = 0;
            int dbUnusued = 0;
            Regex numbers = new Regex(@"[\d,]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            for (int i = 0; i < tables.Count; i++)
            {
                try
                {
                    switch (i % 2)
                    {
                        case 1:
                            sb.Append("<tr class=\"d0\">");
                            break;
                        case 0:
                            sb.Append("<tr class=\"d1\">");
                            break;
                    }

                    IEnumerable<TableInfo> queryTables = db.ExecuteQuery<TableInfo>("sp_spaceused '" + tables[i].name + "'");
                    foreach (var table in queryTables)
                    {
                        sb.Append("<td>" + table.name + "</td>");
                        sb.Append("<td>" + table.rows + "</td>");
                        sb.Append("<td>" + table.reserved + "</td>");
                        sb.Append("<td>" + table.data + "</td>");
                        sb.Append("<td>" + table.index_size + "</td>");
                        sb.Append("<td>" + table.unused + "</td>");
                        sb.Append("<td><input id=\"truncate" + counter + "\" type=\"button\" value=\"Truncate\" onclick=\"return Confirm_Truncate_Table('" + table.name + "','site')\" /></td>");
                        rowsTotal += Convert.ToInt32(table.rows);
                        dbDataSize += Convert.ToInt32(numbers.Match(table.data).Value);
                        dbReserved += Convert.ToInt32(numbers.Match(table.reserved).Value);
                        dbIndexed += Convert.ToInt32(numbers.Match(table.index_size).Value);
                        dbUnusued += Convert.ToInt32(numbers.Match(table.unused).Value);
                    }
                    sb.Append("</tr>");
                    counter += 1;
                }
                catch (Exception e)
                { //The object &#39;ofRoster&#39; does not exist in database 
               
                }
            }
            sb.Append("</table>");
            this.Ui += sb.ToString();

            //Database View
            IEnumerable<DatabaseInfo> queryDB = db.ExecuteQuery<DatabaseInfo>("sp_spaceused");
            foreach (var item in queryDB)
            {
                this.DbName = item.database_name;
                this.DbSize = item.database_size + " -- " + ((dbReserved + dbDataSize) * .0009).ToString("N2") + " MB";
                this.dbReservedSpace = item.reserved + " -- " + dbReserved.ToString("N0");
                this.dbDataSize = dbDataSize.ToString("N0");
                this.dbIndexSize = item.index_size + " -- " + dbIndexed.ToString("N0");
                this.dbUnusedSpace = item.unused + " -- " + dbUnusued.ToString("N0");
            }
            this.TableCount = tables.Count;
            this.RowsCount = rowsTotal;

            return this;
        }

        public string Ui { get; set; }
        public string DbName { get; set; }
        public string DbSize { get; set; }
        public string dbReservedSpace { get; set; }
        public string dbDataSize { get; set; }
        public string dbIndexSize { get; set; }
        public string dbUnusedSpace { get; set; }
        public int TableCount { get; set; }
        public int RowsCount { get; set; }

    }
}
