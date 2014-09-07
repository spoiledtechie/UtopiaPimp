using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boomers.Utilities.Guids
{
    public class Encoder
    {
        /// <summary>
        /// Encodes the Guid from Guid.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string Encode(System.Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
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
            return new System.Guid(buffer);
        }
    }
}
