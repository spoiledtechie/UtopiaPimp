using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Boomers.Utilities.Documents
{
 public  static   class FileExt
    {
     public static  byte[] ToByteArray(this string filename)
     {
         FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

         // Create a byte array of file stream length
         byte[] ImageData = new byte[fs.Length];

         //Read block of bytes from stream into the byte array
         fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

         //Close the File Stream
         fs.Close();
         return ImageData; //return the byte data
     }
    }
}
