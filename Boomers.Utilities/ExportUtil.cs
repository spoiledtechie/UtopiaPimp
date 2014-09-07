using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
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

namespace SpoiledTechie
{
    /// <summary>
    /// Exports to all types of Files. 
    /// </summary>
    public class ExportUtil
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
                        ExportUtil.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        ExportUtil.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        ExportUtil.PrepareControlForExport(gv.FooterRow);
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
                        ExportUtil.PrepareControlForExport(gv.HeaderRow);
                        table.Rows.Add(gv.HeaderRow);
                    }

                    //  add each of the data rows to the table
                    foreach (GridViewRow row in gv.Rows)
                    {
                        ExportUtil.PrepareControlForExport(row);
                        table.Rows.Add(row);
                    }

                    //  add the footer row to the table
                    if (gv.FooterRow != null)
                    {
                        ExportUtil.PrepareControlForExport(gv.FooterRow);
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
                    ExportUtil.PrepareControlForExport(current);
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

            int i;
            foreach (DataRow dr in DT.Rows)
            {
                tab = "";
                for (i = 0; i < DT.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
        }
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
}
