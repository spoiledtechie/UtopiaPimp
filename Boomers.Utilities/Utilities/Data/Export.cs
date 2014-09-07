using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Boomers.Utilities.DatesTimes;

namespace Boomers.Utilities.Data
{
    public class Export
    {
        public static void ExcelExport(DataTable data, String fileName, bool openAfter)
        {

            using (ExcelWriter writer = new ExcelWriter(fileName))
            {
                writer.WriteStartDocument();

                // Write the worksheet contents
                writer.WriteStartWorksheet("Sheet1");

                //Write header row
                writer.WriteStartRow();
                foreach (DataColumn col in data.Columns)
                    writer.WriteExcelUnstyledCell(col.Caption);
                writer.WriteEndRow();

                //write data
                foreach (DataRow row in data.Rows)
                {
                    writer.WriteStartRow();
                    foreach (object o in row.ItemArray)
                    {
                        writer.WriteExcelAutoStyledCell(o);
                    }
                    writer.WriteEndRow();
                }

                // Close up the document
                writer.WriteEndWorksheet();
                writer.WriteEndDocument();
                writer.Close();
                            }
                    }
        //public DataTable LINQToDataTable(System.Data.Linq.DataContext ctx, object query)
        //{
        //    if (query == null)
        //    {
        //        throw new ArgumentNullException("query");
        //    }

        //    IDbCommand cmd = ctx.GetCommand(query as IQueryable);
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = (SqlCommand)cmd;
        //    DataTable dt = new DataTable("sd");

        //    try
        //    {
        //        cmd.Connection.Open();
        //        adapter.FillSchema(dt, SchemaType.Source);
        //        adapter.Fill(dt);
        //    }
        //    finally
        //    {
        //        cmd.Connection.Close();
        //    }
        //    return dt;
        //}
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
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
                        for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
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
                        PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        PrepareControlForExport(gv.FooterRow);
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
                        PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        PrepareControlForExport(gv.FooterRow);
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
        /// exports a string or stringbuilder to a word document and generated to the user.
        /// </summary>
        /// <param name="content">String to push to a word document</param>
        /// <param name="fileName">file name of document, don't include any times stamps.</param>
        public static void StringtoWord(string content, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.AddHeader(
                "content-disposition", string.Format("attachment; filename={0}.doc", fileName + DateTime.UtcNow.ToyyyyMMdd()));
            HttpContext.Current.Response.ContentType = "application/msword";
            HttpContext.Current.Response.Write(content);
            HttpContext.Current.Response.End();
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
                    PrepareControlForExport(current);
                }
            }
        }
        /// <summary>
        /// Exports a DataTable to Excel
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="fileName"></param>
        public static void DataTabletoXLS(System.Data.DataTable DT, string fileName)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.xls", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            string tab = "";
            foreach (DataColumn dc in DT.Columns)
            {
                HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            HttpContext.Current.Response.Write("\n");

            foreach (DataRow dr in DT.Rows)
            {
                tab = "";
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }
        public static void SaveXLSFromDataTable(string locationAndFileName, System.Data.DataTable DT)
        {
            //Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
            //xla.Visible = true;
            //Microsoft.Office.Interop.Excel.Workbook wb = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
            //Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
            ////int i = 1;
            ////int j = 1;
            ////foreach (DataRow dr in DT.Rows)
            ////{
            ////    ws.Cells[i, j] = dr.Text.ToString();
            ////    //MessageBox.Show(comp.Text.ToString());
            ////    foreach (ListViewItem.ListViewSubItem drv in comp.SubItems)
            ////    {
            ////        ws.Cells[i, j] = drv.Text.ToString();
            ////        j++;
            ////    }
            ////    j = 1;
            ////    i++;
            ////}
            //int i = 1;
            //int j = 1;
            //foreach (DataRow dr in DT.Rows)
            //{
            //    for (int k = 0; k < DT.Columns.Count; k++)
            //    {
            //        ws.Cells[i, j] = dr[k].ToString();
            //        j++;
            //    }
            //    j = 1;
            //    i++;
            //}
            //wb.SaveCopyAs(locationAndFileName);
        }
    }
    public class ExcelWriter : IDisposable
    {
        private XmlWriter _writer;

        public enum CellStyle { General, Number, Currency, DateTime, ShortDate };

        public void WriteStartDocument()
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
            _writer.WriteStartElement("ss", "Workbook", "urn:schemas-microsoft-com:office:spreadsheet");
            WriteExcelStyles();
        }

        public void WriteEndDocument()
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteEndElement();
        }

        private void WriteExcelStyleElement(CellStyle style)
        {
            _writer.WriteStartElement("Style", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteAttributeString("ID", "urn:schemas-microsoft-com:office:spreadsheet", style.ToString());
            _writer.WriteEndElement();
        }

        private void WriteExcelStyleElement(CellStyle style, string NumberFormat)
        {
            _writer.WriteStartElement("Style", "urn:schemas-microsoft-com:office:spreadsheet");

            _writer.WriteAttributeString("ID", "urn:schemas-microsoft-com:office:spreadsheet", style.ToString());
            _writer.WriteStartElement("NumberFormat", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteAttributeString("Format", "urn:schemas-microsoft-com:office:spreadsheet", NumberFormat);
            _writer.WriteEndElement();

            _writer.WriteEndElement();

        }

        private void WriteExcelStyles()
        {
            _writer.WriteStartElement("Styles", "urn:schemas-microsoft-com:office:spreadsheet");

            WriteExcelStyleElement(CellStyle.General);
            WriteExcelStyleElement(CellStyle.Number, "General Number");
            WriteExcelStyleElement(CellStyle.DateTime, "General Date");
            WriteExcelStyleElement(CellStyle.Currency, "Currency");
            WriteExcelStyleElement(CellStyle.ShortDate, "Short Date");

            _writer.WriteEndElement();
        }

        public void WriteStartWorksheet(string name)
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteStartElement("Worksheet", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteAttributeString("Name", "urn:schemas-microsoft-com:office:spreadsheet", name);
            _writer.WriteStartElement("Table", "urn:schemas-microsoft-com:office:spreadsheet");
        }

        public void WriteEndWorksheet()
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public ExcelWriter(string outputFileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            _writer = XmlWriter.Create(outputFileName, settings);
        }

        public void Close()
        {
            if (_writer == null) throw new NotSupportedException("Already closed.");

            _writer.Close();
            _writer = null;
        }

        public void WriteExcelColumnDefinition(int columnWidth)
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteStartElement("Column", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteStartAttribute("Width", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteValue(columnWidth);
            _writer.WriteEndAttribute();
            _writer.WriteEndElement();
        }

        public void WriteExcelUnstyledCell(string value)
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteStartElement("Cell", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteStartElement("Data", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteAttributeString("Type", "urn:schemas-microsoft-com:office:spreadsheet", "String");
            _writer.WriteValue(value);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void WriteStartRow()
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteStartElement("Row", "urn:schemas-microsoft-com:office:spreadsheet");
        }

        public void WriteEndRow()
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteEndElement();
        }

        public void WriteExcelStyledCell(object value, CellStyle style)
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            _writer.WriteStartElement("Cell", "urn:schemas-microsoft-com:office:spreadsheet");
            _writer.WriteAttributeString("StyleID", "urn:schemas-microsoft-com:office:spreadsheet", style.ToString());
            _writer.WriteStartElement("Data", "urn:schemas-microsoft-com:office:spreadsheet");
            switch (style)
            {
                case CellStyle.General:
                    _writer.WriteAttributeString("Type", "urn:schemas-microsoft-com:office:spreadsheet", "String");
                    break;
                case CellStyle.Number:
                case CellStyle.Currency:
                    _writer.WriteAttributeString("Type", "urn:schemas-microsoft-com:office:spreadsheet", "Number");
                    break;
                case CellStyle.ShortDate:
                case CellStyle.DateTime:
                    _writer.WriteAttributeString("Type", "urn:schemas-microsoft-com:office:spreadsheet", "DateTime");
                    break;
            }
            _writer.WriteValue(value);
            //  tag += String.Format("{1}\"><ss:Data ss:Type=\"DateTime\">{0:yyyy\\-MM\\-dd\\THH\\:mm\\:ss\\.fff}</ss:Data>", value,

            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void WriteExcelAutoStyledCell(object value)
        {
            if (_writer == null) throw new NotSupportedException("Cannot write after closing.");

            //write the <ss:Cell> and <ss:Data> tags for something
            if (value is Int16 || value is Int32 || value is Int64 || value is SByte ||
                value is UInt16 || value is UInt32 || value is UInt64 || value is Byte)
            {
                WriteExcelStyledCell(value, CellStyle.Number);
            }
            else if (value is Single || value is Double || value is Decimal) //we'll assume it's a currency
            {
                WriteExcelStyledCell(value, CellStyle.Currency);
            }
            else if (value is DateTime)
            {
                //check if there's no time information and use the appropriate style
                WriteExcelStyledCell(value, ((DateTime)value).TimeOfDay.CompareTo(new TimeSpan(0, 0, 0, 0, 0)) == 0 ? CellStyle.ShortDate : CellStyle.DateTime);
            }
            else
            {
                WriteExcelStyledCell(value, CellStyle.General);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_writer == null)
                return;

            _writer.Close();
            _writer = null;
        }

        #endregion
    }
}
