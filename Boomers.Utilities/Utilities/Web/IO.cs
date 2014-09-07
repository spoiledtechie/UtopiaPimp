using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;

namespace Boomers.Utilities.Web
{
    public class IO
    {
        /// <summary>
        /// Gets the text content of the requested file.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string GetFileText(string virtualPath, HttpContext context)
        {
            try
            {
                //Read from file
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(context.Server.MapPath(virtualPath));
                }
                catch
                {
                    sr = new StreamReader(virtualPath);

                }
                string strOut = sr.ReadToEnd();
                sr.Close();
                return strOut;
            }
            catch { throw; }
        }
        /// <summary>
        /// Writes out a file
        /// </summary>
        /// <param name="AbsoluteFilePath"></param>
        /// <param name="fileText"></param>
        public static void WriteToFile(string virtualPath, string fileText, HttpContext context)
        {
            try
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(context.Server.MapPath(virtualPath), false);
                }
                catch
                {
                    sw = new StreamWriter(virtualPath, false);
                }
                sw.Write(fileText);
                sw.Close();
            }
            catch { throw; }

        }
        /// <summary>
        /// Writes out an XML file so that encoding happens properly
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="fileText"></param>
        public static void WriteToXMLFile(string virtualPath, string fileText, HttpContext context)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(fileText);
                try
                {
                    xDoc.Save(context.Server.MapPath(virtualPath));
                }
                catch
                {
                    xDoc.Save(virtualPath);
                }
            }
            catch { throw; }
        }
        /// <summary>
        /// Appends _# to a filename incrementing the # until the name is unique.
        /// </summary>
        /// <param name="fname">The filename to check</param>
        /// <param name="path">The path to compare against.</param>
        /// <returns>A unique filename for the directory</returns>
        public static string EnsureUniqueFilename(string fname, string path)
        {
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(path + fname)))
                {
                    int inc = 0;
                    string pre = fname.Substring(0, fname.LastIndexOf('.')); //filename with no extension
                    string ext = fname.Substring(fname.LastIndexOf('.') + 1);//extension only

                    while (File.Exists(HttpContext.Current.Server.MapPath(path + pre + "_" + inc.ToString() + ext)))
                    {
                        inc++;
                    }
                    if (inc > 0)
                        fname = pre + "_" + inc.ToString() + ext;
                }
                return fname;
            }
            catch { throw; }
        }
    }
}
