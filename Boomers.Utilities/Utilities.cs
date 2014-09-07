using System;
using System.Configuration;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SpoiledTechie.Utilities
{
    /// <summary>
    /// Handles all Database Functions
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Database Connection String.
        /// </summary>
        /// <returns></returns>
        public static SqlConnection ConnectionStringID()
        { return new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString); }
    }
    /// <summary>
    /// Imports All type of Files
    /// </summary>
    public class Import
    {
        /// <summary>
        /// Exports an Excel Workbook sheet to a Datatable.
        /// </summary>
        /// <param name="strFilePath">File path of Excel Spreadsheet.</param>
        /// <param name="SheetName">Sheet name inside the excel workbook to export.</param>
        /// <returns></returns>
        public static DataTable ExceltoDataTable(string strFilePath, string SheetName)
        {
            try
            {
                string strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strFilePath + "; Jet OLEDB:Engine Type=5;" + "Extended Properties=Excel 8.0;";
                OleDbConnection cnCSV = new OleDbConnection(strConnectionString);
                cnCSV.Open();
                OleDbCommand cmdSelect = new OleDbCommand(@"SELECT * FROM [" + SheetName + "]", cnCSV);
                OleDbDataAdapter daCSV = new OleDbDataAdapter(); daCSV.SelectCommand = cmdSelect;
                DataTable dtCSV = new DataTable();
                daCSV.Fill(dtCSV);
                cnCSV.Close();
                daCSV = null;
                return dtCSV;
            }
            catch (Exception ex)
            { return null; }
        }
        /// <summary>
        /// Runs the Query given and exports to a DataTable.
        /// </summary>
        /// <param name="queryString">Requires it to be a Select Statement</param>
        /// <returns>Returns Datatable</returns>
        /// <remarks>If query statement doesn't contain a "select" keyword, it returns null.</remarks>
        public static DataTable QueryToDataTable(string queryString)
        {
            if (queryString.ToLower().Contains("select"))
            {
                // Retrieve the connection string stored in the Web.config file.
                SqlConnection cts = Database.ConnectionStringID();
                DataTable DT = new DataTable();
                // Connect to the database and run the query.
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, cts);
                // Fill the DataSet.
                adapter.Fill(DT);
                if (DT.Rows.Count == 0)
                    return null;
                else
                    return DT;
            }
            else
                return null;
        }
        /// <summary>
        /// Returns a Dataset from a query string.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static DataSet QueryToDataSet(string queryString)
        {
            if (queryString.ToLower().Contains("select"))
            {
                // Retrieve the connection string stored in the Web.config file.
                SqlConnection cts = Database.ConnectionStringID();
                DataSet DS = new DataSet();
                // Connect to the database and run the query.
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, cts);
                // Fill the DataSet.
                adapter.Fill(DS);
                return DS;
            }
            else
                return null;
        }
    }
    /// <summary>
    /// Exports all type of Files.
    /// </summary>
    public class Export
    {
        public DataTable LINQToDataTable(System.Data.Linq.DataContext ctx, object query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            IDbCommand cmd = ctx.GetCommand(query as IQueryable);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = (SqlCommand)cmd;
            DataTable dt = new DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }
        /// <summary>
        /// Retruns a DataTable in Short form.
        /// </summary>
        /// <typeparam name="T">From statement.</typeparam>
        /// <param name="varlist">Anonymous List.</param>
        /// <param name="dtReturn">Datable to Add rows to.</param>
        /// <returns></returns>
        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist, DataTable dtReturn)
        {
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                            colType = colType.GetGenericArguments()[0];


                        if (dtReturn.Columns[pi.Name] == null)
                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        /// <summary>
        /// Retruns a DataTable in Short form.
        /// </summary>
        /// <typeparam name="T">From statement.</typeparam>
        /// <param name="varlist">Anonymous List.</param>
        /// <returns></returns>
        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others         will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                            colType = colType.GetGenericArguments()[0];

                        if (dtReturn.Columns[pi.Name] == null)
                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        /// <summary>
        /// Exports Gridviews to XLS Format
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="gv"></param>
        /// <remarks>Exports Entire Gridview even if it is Paged.</remarks>
        public static void GridviewtoXLS(GridView gv, string fileName)
        {
            int DirtyBit = 0;
            int PageSize = 0;
            if (gv.AllowPaging == true)
            {
                DirtyBit = 1;
                PageSize = gv.PageSize;
                gv.AllowPaging = false;
                gv.DataBind();
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}.xls", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a table to contain the grid
                    Table table = new Table();

                    //  include the gridline settings
                    table.GridLines = gv.GridLines;

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        Utilities.Export.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        Utilities.Export.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        Utilities.Export.PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString().Replace("£", ""));
                    HttpContext.Current.Response.End();
                }
            }
            if (DirtyBit == 1)
            {
                gv.PageSize = PageSize;
                gv.AllowPaging = true;
                gv.DataBind();
            }
        }
        /// <summary>
        /// Exports Gridviews to Word Format
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="gv"></param>
        /// <remarks>Exports Entire Gridview even if it is Paged.</remarks>
        public static void GridviewtoWORD(GridView gv, string fileName)
        {
            int DirtyBit = 0;
            int PageSize = 0;
            if (gv.AllowPaging == true)
            {
                DirtyBit = 1;
                PageSize = gv.PageSize;
                gv.AllowPaging = false;
                gv.DataBind();
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}.doc", fileName));
            HttpContext.Current.Response.ContentType = "application/msword";

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    //  Create a table to contain the grid
                    Table table = new Table();

                    //  include the gridline settings
                    table.GridLines = gv.GridLines;

                    //  add the header row to the table
                    if (gv.HeaderRow != null)
                    {
                        Utilities.Export.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        Utilities.Export.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        Utilities.Export.PrepareControlForExport(gv.FooterRow);
                        table.Rows.Add(gv.FooterRow);
                    }

                    //  render the table into the htmlwriter
                    table.RenderControl(htw);

                    //  render the htmlwriter into the response
                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
            if (DirtyBit == 1)
            {
                gv.PageSize = PageSize;
                gv.AllowPaging = true;
                gv.DataBind();
            }
        }
        /// <summary>
        /// Replace any of the contained controls with literals
        /// </summary>
        /// <param name="control"></param>
        private static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    Utilities.Export.PrepareControlForExport(current);
                }
            }
        }
        /// <summary>
        /// Exports a DataTable to Excel
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="fileName"></param>
        public static void DataTabletoXLS(DataTable DT, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-16";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            string tab = "";
            foreach (DataColumn dc in DT.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName.Replace("\n", "").Replace("\t", ""));
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");

            int i;
            foreach (DataRow dr in DT.Rows)
            {
                tab = "";
                for (i = 0; i < DT.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(tab + dr[i].ToString().Replace("\n", "").Replace("\t", ""));
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }
    }
    /// <summary>
    /// Executes all Excel Procedures.
    /// </summary>
    public class Excel
    {
        /// <summary>
        /// This mehtod retrieves the excel sheet names from 
        /// an excel workbook.
        /// </summary>
        /// <param name="strFilePath">The excel file.</param>
        /// <returns>String[]</returns>
        public static String[] GetExcelSheetNames(string strFilePath)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;
            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strFilePath + ";Extended Properties=Excel 8.0;";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                return excelSheets;
            }
            catch (Exception ex)
            { return null; }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
    }
    /// <summary>
    /// Handles Formatting for DateTimes.
    /// </summary>
    public class DateTimeFormatting
    {
        /// <summary>
        /// Converts DateTime to yyyyMMdd Format
        /// </summary>
        /// <returns></returns>
        public static string ConvertToyyyyMMdd()
        {
            return DateTime.UtcNow.ToString("yyyyMMdd");
        }
        /// <summary>
        /// Converts DateTime to yyyyMMdd Format
        /// </summary>
        /// <param name="datetime">Date and Time  to convert.</param>
        /// <returns></returns>
        public static string ConvertToyyyyMMdd(DateTime datetime)
        {
            return datetime.ToString("yyyyMMdd");
        }
        /// <summary>
        /// gets the relative time from date and time entered.
        /// </summary>
        /// <param name="date">Date to format.</param>
        /// <returns>about date days/hours/minutes ago.</returns>
        public static String GetRelativeDate(DateTime date)
        {
            DateTime now = DateTime.Now;
            TimeSpan span = now - date;
            if (span <= TimeSpan.FromSeconds(60))
            {
                return span.Seconds + " seconds ago";
            }
            else if (span <= TimeSpan.FromMinutes(60))
            {
                if (span.Minutes > 1)
                {
                    return "about " + span.Minutes + " minutes ago";
                }
                else
                {
                    return "about a minute ago";
                }
            }
            else if (span <= TimeSpan.FromHours(24))
            {
                if (span.Hours > 1)
                {
                    return "about " + span.Hours + " hours ago";
                }
                else
                {
                    return "about an hour ago";
                }
            }
            else
            {
                if (span.Days > 1)
                {
                    return "about " + span.Days + " days ago";
                }
                else
                {
                    return "about a day ago";
                }
            }
        }
    }
    /// <summary>
    /// Deals with the culture of the user.
    /// </summary>
    public class Culture
    {
        /// <summary>
        /// Gets the Culture info of current Browser settings
        /// </summary>
        /// <returns></returns>
        public static CultureInfo ResolveCulture()
        {
            string[] languages = HttpContext.Current.Request.UserLanguages;
            if (languages == null || languages.Length == 0)
                return null;
            try
            {
                string language = languages[0].ToLowerInvariant().Trim();
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            { return null; }
        }
        /// <summary>
        /// Gets the region info of current culture settings.
        /// </summary>
        /// <returns></returns>
        public static RegionInfo ResolveCountry()
        {
            CultureInfo culture = ResolveCulture();
            if (culture != null)
                return new RegionInfo(culture.LCID);
            return null;
        }
    }
    /// <summary>
    /// Removes the www.Subdomain info.
    /// </summary>
    public class wwwSubDomain
    {
        //<httpModules>
        //<add type="WwwSubDomainModule" name="WwwSubDomainModule" />
        //</httpModules>
        //You also need to add an appSetting like so:
        //<appSettings>
        //<!-- Values can be 'add' or 'remove' -->
        //<add key="WwwRule" value="add"/>
        //</appSettings> 
        /// <summary>

        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            string rule = ConfigurationManager.AppSettings.Get("WwwRule");
            HttpContext context = (sender as HttpApplication).Context;
            if (context.Request.HttpMethod != "GET" || context.Request.IsLocal)
                return;
            if (context.Request.PhysicalPath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                string url = context.Request.Url.ToString();
                if (url.Contains("://www.") && rule == "remove")
                    RemoveWww(context);

                if (!url.Contains("://www.") && rule == "add")
                    AddWww(context);
            }
        }
        /// <summary>
        /// Adds the www subdomain to the request and redirects.
        /// </summary>
        private static void AddWww(HttpContext context)
        {
            string url = context.Request.Url.ToString().Replace("://", "://www.");
            PermanentRedirect(url, context);
        }
        private static readonly Regex _Regex = new Regex("(http|https)://www\\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// Removes the www subdomain from the request and redirects.
        /// </summary>
        private static void RemoveWww(HttpContext context)
        {
            string url = context.Request.Url.ToString();
            if (_Regex.IsMatch(url))
            {
                url = _Regex.Replace(url, "$1://");
                PermanentRedirect(url, context);
            }
        }
        /// <summary>
        /// Sends permanent redirection headers (301)
        /// </summary>
        private static void PermanentRedirect(string url, HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 301;
            context.Response.AppendHeader("location", url);
            context.Response.End();
        }
    }
    /// <summary>
    /// Misc. Items.
    /// </summary>
    public class Generic
    {
        /// <summary> 
        /// Checks      the specified value to see if it can be 
        /// converted      into the specified type. 
        /// <remarks> 
        /// The      method supports all the primitive types of the CLR 
        /// such      as int, boolean, double, guid etc. as well as other 
        /// simple      types like Color and Unit and custom enum types. 
        /// </remarks>
        /// 
        /// </summary> 
        /// <param name="value">The value to check.</param> 
        /// <param      name="type">The type that the value will be      checked against.</param> 
        /// <returns>True      if the value can convert to the given type, otherwise false.</returns> 
        public static bool CanConvert(string value, Type type)
        {
            if (string.IsNullOrEmpty(value) || type == null)
                return false;
            System.ComponentModel.TypeConverter conv = System.ComponentModel.TypeDescriptor.GetConverter(type);
            if (conv.CanConvertFrom(typeof(string)))
            {
                try
                { conv.ConvertFrom(value); return true; }
                catch
                { }
            }
            return false;
        }
    }
    /// <summary>
    /// Encodes Guids, Decodes Guids
    ///  c9a646d3-9c61-4cb7-bfcd-ee2522c8f633
    ///And converts it into this smaller string:
    ///00amyWGct0y_ze4lIsj2Mw 
    /// </summary>
    public static class GuidEncoder
    {
        /// <summary>
        /// Encodes the Guid from text.
        /// </summary>
        /// <param name="guidText"></param>
        /// <returns></returns>
        public static string Encode(string guidText)
        {
            Guid guid = new Guid(guidText);
            return Encode(guid);
        }
        /// <summary>
        /// Encodes the Guid from Guid.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string Encode(Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_").Replace("+", "-");
            return enc.Substring(0, 22);
        }
        /// <summary>
        /// Decodes the Guid.
        /// </summary>
        /// <param name="encoded"></param>
        /// <returns></returns>
        public static Guid Decode(string encoded)
        {
            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Guid(buffer);
        }
        /// <summary>
        /// Encodes the Guid from text.
        /// </summary>
        /// <param name="guidText"></param>
        /// <returns></returns>
        public static string EncodeURLs(string guidText)
        {
            Guid guid = new Guid(guidText);
            return EncodeURLs(guid);
        }
        /// <summary>
        /// Encodes the Guid from Guid.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string EncodeURLs(Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "-").Replace("+", "_");
            return enc.Substring(0, 22);
        }
        /// <summary>
        /// Decodes the Guid.
        /// </summary>
        /// <param name="encoded"></param>
        /// <returns></returns>
        public static Guid DecodeURLs(string encoded)
        {
            encoded = encoded.Replace("-", "/");
            encoded = encoded.Replace("_", "+");
            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Guid(buffer);
        }
    }
    /// <summary>
    /// Handles Mobile Browsing.
    /// </summary>
    public static class MobileBrowsing
    {
        private static readonly Regex MOBILE_REGEX = new Regex(@"(nokia|sonyericsson|blackberry|samsung|sec-|windows ce|motorola|mot-|up.b)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// checks if the item is a mobile device.
        /// </summary>
        public static bool IsMobile
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    HttpRequest request = context.Request;
                    if (request.Browser.IsMobileDevice)
                        return true;

                    if (!string.IsNullOrEmpty(request.UserAgent) && MOBILE_REGEX.IsMatch(request.UserAgent))
                        return true;
                }
                return false;
            }
        }
    }
    public static class Images
    {
        /// <summary>
        /// Exports a Shrunken image.
        /// </summary>
        /// <param name="TargetH">Height for exported Image.</param>
        /// <param name="TargetW">Width for Exported Image.</param>
        /// <param name="Extension">Extension of Image.</param>
        /// <param name="FullSizeImg">The Image to export.</param>
        /// <remarks>Exports the image and outputs it to the user.</remarks>
        public static void ImageShrink(int TargetH, int TargetW, string Extension, System.Drawing.Image FullSizeImg)
        {
            System.Drawing.Image
                original = FullSizeImg;
            if (original.Height > original.Width)
                TargetW = Convert.ToInt32(original.Width * (Convert.ToDouble(TargetH) / (Convert.ToDouble(original.Height))));
            else
                TargetH = Convert.ToInt32(original.Height * (Convert.ToDouble(TargetW) / (Convert.ToDouble(original.Width))));

            System.Drawing.Image imgPhoto = FullSizeImg;
            Bitmap bmPhoto = new Bitmap(TargetW, TargetH, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(72F, 72F);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, TargetW, TargetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
            switch (Extension)
            {
                case ".gif": //checks for .gif
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".png":
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".tiff":
                    bmPhoto.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Tiff);
                    break;
            }
            original.Dispose();
            imgPhoto.Dispose();
            grPhoto.Dispose();
            bmPhoto.Dispose();
            HttpContext.Current.Response.End();
        }
    }
    public static class UrlEncoderDecoder
    {
        public static string TamperProofStringEncode(string value, string key)
        {
            System.Security.Cryptography.MACTripleDES mac3des = new System.Security.Cryptography.MACTripleDES();
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            mac3des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value)) + '-' + Convert.ToBase64String(mac3des.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value)));
        }
        public static string TamperProofStringDecode(string value, string key)
        {
            string dataValue = "";
            string calcHash = "";
            string storedHash = "";
            System.Security.Cryptography.MACTripleDES mac3des = new System.Security.Cryptography.MACTripleDES();
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            mac3des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
            try
            {
                dataValue = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value.Split('-')[0]));
                storedHash = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value.Split('-')[1]));
                calcHash = System.Text.Encoding.UTF8.GetString(mac3des.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataValue)));
                if (storedHash != calcHash)
                {
                    throw new ArgumentException("Hash value does not match");
                }
            }
            catch
            {
                throw new ArgumentException("Invalid TamperProofString");
            }
            return dataValue;
        }
    }
}
/// <summary>
/// Guid Extensions.
/// </summary>
public static class GuidExtensions
{
    private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$");
    /// <summary>
    /// Checks if it is a valid GUID.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>true of false.</returns>
    public static Boolean IsValidGuid(this string guid)
    {
        if (guid != null)
        {
            if (isGuid.IsMatch(guid))
                return true;
        }
        return false;
    }
}
/// <summary>
/// Deals with any DateTime Problems.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the week number of the current Date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static int WeekNumber(this DateTime date)
    {
        GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
        return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
    /// <summary>
    /// Gets the weeks in the current year.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static int WeeksInYear(int year)
    {
        GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
        return cal.GetWeekOfYear(new DateTime(year, 12, 28), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
    /// <summary>
    /// Gets the number of weeks between each date.
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    public static int NumberOfWeeks(DateTime dateFrom, DateTime dateTo)
    {
        TimeSpan Span = dateTo.Subtract(dateFrom);
        if (Span.Days <= 7)
        {
            if (dateFrom.DayOfWeek > dateTo.DayOfWeek)
                return 2;
            return 1;
        }

        int Days = Span.Days - 7 + (int)dateFrom.DayOfWeek;
        int WeekCount = 1;
        int DayCount = 0;

        for (WeekCount = 1; DayCount < Days; WeekCount++)
        { DayCount += 7; }

        return WeekCount;
    }
}
public static class StringExtensions
{
    /// <summary>
    /// Returns a Lower case value of the string.
    /// </summary>
    /// <returns></returns>
    public static String[] ToLower(this String[] s)
    {
        int i = 0;
        String[] stringinterate = s;
        foreach (string item in s)
        {
            stringinterate[i] = item.ToLower();
            i += 1;
        }
        return stringinterate;
    }
    /// <summary>
    /// Encodes the String to certain value.
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string Encode(this string Text)
    {
        int c = 0;
        string Temp = "";
        for (int i = Text.Length; i >= 1; i--)
        {
            c = System.Convert.ToInt32(Text[i - 1]);
            c = (c - 26) - i;
            if (c < 0)
                c = 256 + c;

            Temp += (char)(c);
        }
        return Temp;
    }
    /// <summary>
    /// Decodes the string to certain value.
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string Decode(this string Text)
    {
        int c = 0;
        string Temp = "";
        for (int i = 1; i <= Text.Length; i++)
        {
            c = System.Convert.ToInt32(Text[i - 1]);
            c = (c + 26) + i;
            if (c > 255)
                c = c - 256;

            Temp += (char)(c);
        }
        return Temp;
    }
    /// <summary>
    /// Removes all HTML attributes in the text.
    /// </summary>
    /// <param name="text">HTML Text.</param>
    /// <returns>No HTML Text</returns>
    public static string htmlDecode(this string text)
    {
        System.Text.RegularExpressions.Regex regex = new Regex("<[^>]*>");
        return regex.Replace(text, " ");
    }
}
public static class DataSetLinqOperators
{
    public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
    {
        return new ObjectShredder<T>().Shred(source, null, null);
    }

    public static DataTable CopyToDataTable<T>(this IEnumerable<T> source,
                                                DataTable table, LoadOption? options)
    {
        return new ObjectShredder<T>().Shred(source, table, options);
    }

}
public class ObjectShredder<T>
{
    private FieldInfo[] _fi;
    private PropertyInfo[] _pi;
    private Dictionary<string, int> _ordinalMap;
    private Type _type;

    public ObjectShredder()
    {
        _type = typeof(T);
        _fi = _type.GetFields();
        _pi = _type.GetProperties();
        _ordinalMap = new Dictionary<string, int>();
    }

    public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
    {
        if (typeof(T).IsPrimitive)
        {
            return ShredPrimitive(source, table, options);
        }


        if (table == null)
        {
            table = new DataTable(typeof(T).Name);
        }

        // now see if need to extend datatable base on the type T + build ordinal map
        table = ExtendTable(table, typeof(T));

        table.BeginLoadData();
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (options != null)
                {
                    table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                }
                else
                {
                    table.LoadDataRow(ShredObject(table, e.Current), true);
                }
            }
        }
        table.EndLoadData();
        return table;
    }

    public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
    {
        if (table == null)
        {
            table = new DataTable(typeof(T).Name);
        }

        if (!table.Columns.Contains("Value"))
        {
            table.Columns.Add("Value", typeof(T));
        }

        table.BeginLoadData();
        using (IEnumerator<T> e = source.GetEnumerator())
        {
            Object[] values = new object[table.Columns.Count];
            while (e.MoveNext())
            {
                values[table.Columns["Value"].Ordinal] = e.Current;

                if (options != null)
                {
                    table.LoadDataRow(values, (LoadOption)options);
                }
                else
                {
                    table.LoadDataRow(values, true);
                }
            }
        }
        table.EndLoadData();
        return table;
    }

    public DataTable ExtendTable(DataTable table, Type type)
    {
        // value is type derived from T, may need to extend table.
        foreach (FieldInfo f in type.GetFields())
        {
            if (!_ordinalMap.ContainsKey(f.Name))
            {
                DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                    : table.Columns.Add(f.Name, f.FieldType);
                _ordinalMap.Add(f.Name, dc.Ordinal);
            }
        }
        foreach (PropertyInfo p in type.GetProperties())
        {
            if (!_ordinalMap.ContainsKey(p.Name))
            {
                DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                    : table.Columns.Add(p.Name, p.PropertyType);
                _ordinalMap.Add(p.Name, dc.Ordinal);
            }
        }
        return table;
    }

    public object[] ShredObject(DataTable table, T instance)
    {

        FieldInfo[] fi = _fi;
        PropertyInfo[] pi = _pi;

        if (instance.GetType() != typeof(T))
        {
            ExtendTable(table, instance.GetType());
            fi = instance.GetType().GetFields();
            pi = instance.GetType().GetProperties();
        }

        Object[] values = new object[table.Columns.Count];
        foreach (FieldInfo f in fi)
        {
            values[_ordinalMap[f.Name]] = f.GetValue(instance);
        }

        foreach (PropertyInfo p in pi)
        {
            values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
        }
        return values;
    }
}
public static class ConvertDataTable
{
    /// <summary>
    /// rec => new object[] { query }
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="varlist"></param>
    /// <param name="fn"></param>
    /// <returns></returns>
    public static DataTable ToADOTable<T>(this IEnumerable<T> varlist, CreateRowDelegate<T> fn)
    {
        DataTable dtReturn = new DataTable();
        // column names
        PropertyInfo[] oProps = null;
        // Could add a check to verify that there is an element 0
        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    { colType = colType.GetGenericArguments()[0]; }
                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }
            DataRow dr = dtReturn.NewRow();
            foreach (PropertyInfo pi in oProps)
            { dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null); }
            dtReturn.Rows.Add(dr);
        }
        return (dtReturn);
    }
    public delegate object[] CreateRowDelegate<T>(T t);
}

