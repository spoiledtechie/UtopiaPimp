using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Boomers.Utilities.DatesTimes;

namespace Boomers.Utilities.Documents
{
    public class TextLogger
    {
        /// <summary>
        /// logs text to a text file at C:/temp/[site name]
        /// </summary>
        /// <param name="site"></param>
        /// <param name="text"></param>
        public static void LogItem(string site, string text)
        {
            string fileLocation = "C:/temp/" + site + DateTime.UtcNow.ToyyyyMMdd() + ".txt";
            DirectoryInfo di = new DirectoryInfo(fileLocation);
            FileInfo fileInfo = new FileInfo(fileLocation);
            StreamWriter stream = null;
            if (!Directory.Exists("C:/temp"))
            {
                Directory.CreateDirectory("C:/temp");
            }

            if (!fileInfo.Exists)
            {
                stream = fileInfo.CreateText();
            }
            try
            {
                if (stream == null)
                {
                    stream = File.AppendText(fileLocation);
                }
                stream.WriteLine(DateTime.UtcNow + ": " + text);
                stream.WriteLine("=====================================================");
                stream.Close();
                stream.Dispose();
            }
            catch (Exception exception)
            {
            }
        }
    }
}
