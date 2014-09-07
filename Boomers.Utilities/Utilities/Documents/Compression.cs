using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Boomers.Utilities.Documents
{
 public    class Compression
    {
        /// <summary>
        /// returns the file path string of the encrypted file
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static string Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    // Create the compressed file.
                    FileStream outFile = File.Create(fi.FullName + ".gz");
                    using (GZipStream Compress = new GZipStream(outFile, CompressionMode.Compress))
                    {
                        inFile.CopyTo(Compress);
                    }
                    outFile.Close();
                    outFile.Dispose();
                    return fi.FullName + ".gz";
                }
            }
            return null;
        }

        public static string Decompress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Get original file extension, for example
                // "doc" from report.doc.gz.
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                //Create the decompressed file.
                FileStream outFile = File.Create(origName);
                using (GZipStream Decompress = new GZipStream(inFile, CompressionMode.Decompress))
                {
                    // Copy the decompression stream 
                    // into the output file.
                    Decompress.CopyTo(outFile);
                }
                outFile.Close();
                outFile.Dispose();
                return origName;
            }

        }
    }
}
