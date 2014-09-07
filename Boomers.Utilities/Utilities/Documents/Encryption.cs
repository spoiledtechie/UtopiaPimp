using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Boomers.Utilities.Documents
{
  public   class Encryption
    {
        private const string passKey = "c0ntr0l1";

        /// <summary>   
        /// Decrypts the input file (strInputFileName) and creates a new decrypted file (strOutputFileName)   
        /// </summary>   
        /// <param name="strInputFileName">input file name</param>   
        /// <param name="strOutputFileName">output file name</param>   
        public static void DecryptFiletoFile(string strInputFileName, string strOutputFileName)
        {
            string strFileData = "";
            using (FileStream inputStream = new FileStream(strInputFileName, FileMode.Open, FileAccess.Read))
            {
                DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

                cryptic.Key = ASCIIEncoding.ASCII.GetBytes(passKey);
                cryptic.IV = ASCIIEncoding.ASCII.GetBytes(passKey);

                CryptoStream crStream = new CryptoStream(inputStream, cryptic.CreateDecryptor(), CryptoStreamMode.Read);

                StreamReader reader = new StreamReader(crStream);

                strFileData = reader.ReadToEnd();

                reader.Close();
                inputStream.Close();
            }

            if (File.Exists(strOutputFileName))
            {
                File.Delete(strOutputFileName);
            }
            using (StreamWriter outputStream = new StreamWriter(strOutputFileName))
            {
                outputStream.Write(strFileData, 0, strFileData.Length);

                outputStream.Close();
            }

        }
        /// <summary>   
        /// Encrypts the input file(strInputFileName) and creates a new encrypted file(strOutputFileName)   
        /// </summary>   
        /// <param name="strInputFileName">input file name</param>   
        /// <param name="strOutputFileName">output file name</param>   
        public static void EncryptFiletoFile(string strInputFileName, string strOutputFileName)
        {
            byte[] fileBuffer;

            using (FileStream inputStream = new FileStream(strInputFileName, FileMode.Open, FileAccess.Read))
            {
                fileBuffer = new byte[inputStream.Length];

                inputStream.Read(fileBuffer, 0, fileBuffer.GetLength(0));


                inputStream.Close();
            }
            if (File.Exists(strOutputFileName))
            {
                File.Delete(strOutputFileName);
            }
            using (FileStream outputStream = new FileStream(strOutputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

                cryptic.Key = ASCIIEncoding.ASCII.GetBytes(passKey);
                cryptic.IV = ASCIIEncoding.ASCII.GetBytes(passKey);

                CryptoStream crStream = new CryptoStream(outputStream, cryptic.CreateEncryptor(), CryptoStreamMode.Write);

                crStream.Write(fileBuffer, 0, fileBuffer.Length);

                crStream.Close();
            }
        }
        /// <summary>   
        /// Encrypts the input string and creates a new encrypted file(strOutputFileName)   
        /// </summary>   
        /// <param name="strInputString">input string name</param>   
        /// <param name="strOutputFileName">output file name</param>   
        public static void EncryptStringtoFile(string strInputString, string strOutputFileName)
        {
            if (File.Exists(strOutputFileName))
            {
                File.Delete(strOutputFileName);
            }
            using (FileStream outputStream = new FileStream(strOutputFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

                cryptic.Key = ASCIIEncoding.ASCII.GetBytes(passKey);
                cryptic.IV = ASCIIEncoding.ASCII.GetBytes(passKey);

                CryptoStream crStream = new CryptoStream(outputStream, cryptic.CreateEncryptor(), CryptoStreamMode.Write);

                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(strInputString);

                crStream.Write(buffer, 0, buffer.Length);

                crStream.Close();
            }
        }
        /// <summary>   
        /// Decrypts the input file (strInputFileName) and creates a new decrypted file (strOutputFileName)   
        /// </summary>   
        /// <param name="strInputFileName">input file name</param>   
        public static string DecryptFiletoString(string strInputFileName)
        {
            string strFileData = "";
            using (FileStream inputStream = new FileStream(strInputFileName, FileMode.Open, FileAccess.Read))
            {
                DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

                cryptic.Key = ASCIIEncoding.ASCII.GetBytes(passKey);
                cryptic.IV = ASCIIEncoding.ASCII.GetBytes(passKey);

                CryptoStream crStream = new CryptoStream(inputStream, cryptic.CreateDecryptor(), CryptoStreamMode.Read);

                StreamReader reader = new StreamReader(crStream);

                strFileData = reader.ReadToEnd();

                reader.Close();
                inputStream.Close();
            }

            return strFileData;
        }   

    }
}
