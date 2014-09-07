using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Reflection;

namespace Boomers.Utilities.Web
{
  public   class WebResourceEncoder
    {
        public static string decodeWebResourceString(string urlEncoded)
        {
            byte[] encryptedData = HttpServerUtility.UrlTokenDecode(urlEncoded);

            Type machineKeySection = typeof(MachineKeySection);
            Type[] paramTypes = new Type[] { typeof(bool), typeof(byte[]), typeof(byte[]), typeof(int), typeof(int) };
            MethodInfo encryptOrDecryptData = machineKeySection.GetMethod("EncryptOrDecryptData", BindingFlags.Static | BindingFlags.NonPublic, null, paramTypes, null);

            try
            {
                byte[] decryptedData = (byte[])encryptOrDecryptData.Invoke(null, new object[] { false, encryptedData, null, 0, encryptedData.Length });
                string decrypted = Encoding.UTF8.GetString(decryptedData);

                return decrypted;
            }
            catch (TargetInvocationException)
            {
                return "Error decrypting data. Are you running your page on the same server and inside the same application as the web resource URL that was generated?";
            }
        }

    }
}
