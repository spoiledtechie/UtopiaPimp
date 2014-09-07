using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Boomers.Utilities.Documents
{
    public static class Files
    {

        
        public static string SaveDocuments(HttpPostedFileBase file, string fileName, string filePath)
        {
            if (file.ContentLength > 0)
            {
                filePath = Path.Combine(filePath, fileName);
                file.SaveAs(filePath);
                return filePath;
            }
            else throw new NullReferenceException("No File Found,ContentLength < 1");
        }
        public static bool CompareFiles(string file1, string file2)
        {
            //' Compares two files, byte by byte
            //' d returns true if no differences
            //Dim blnIdentical As Boolean = True
            System.IO.FileStream objFile1 = new System.IO.FileStream(file1, System.IO.FileMode.Open);
            System.IO.FileStream objFile2 = new System.IO.FileStream(file2, System.IO.FileMode.Open);
            if (objFile1.Length != objFile2.Length)
            {
                objFile1.Close();
                objFile2.Close();
                return false;
            }
            else
            {
                int intByteF1, intByteF2;
                do
                {
                    intByteF1 = objFile1.ReadByte();
                    intByteF2 = objFile2.ReadByte();
                    if (intByteF1 != intByteF2)
                    {
                        objFile1.Close();
                        objFile2.Close();
                        return false;
                    }
                } while (intByteF1 != -1);
            }
            return true;
        }
    }
}
