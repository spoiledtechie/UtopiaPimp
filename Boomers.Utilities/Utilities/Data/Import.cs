using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Boomers.Utilities.Data;

namespace Boomers.Utilities.Data
{
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
            catch 
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
                SqlConnection cts = Access.ConnectionStringID;
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
                SqlConnection cts = Access.ConnectionStringID;
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
}
