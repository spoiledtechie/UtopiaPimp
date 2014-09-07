using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Conversion.Extensions
{
    public static class ByteExt
    {
        public static string ToReadable(this long bytes)
        {
            if (bytes >= 1073741824)
                return string.Format("#0.00", bytes / 1024 / 1024 / 1024) + " GB";
            else if (bytes >= 1048576)
                return string.Format("#0.00", bytes / 1024 / 1024) + " MB";
            else if (bytes >= 1024)
                return string.Format("#0.00", bytes / 1024) + " KB";
            else if (bytes > 0 & bytes < 1024)
                return bytes.ToString() + " Bytes";
            else
                return "0 Bytes";
        }
    }
}
