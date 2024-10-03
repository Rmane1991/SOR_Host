using System;
using System.Text;
using AppLogger;
using System.Data;
using System.Security.Cryptography;
using System.IO;

namespace BussinessAccessLayer
{
    public class AppSecurity
    {
        #region data table
        DataTable dtEncEmb = new DataTable();
        string[] _OperationStatus = new string[20];
        #endregion

        #region RandomStringGenerator
        public string RandomStringGenerator()
        {
            try
            {
                char[] _SpecialChars = { '@', '#', '$', '%', '^', '&', '+', '=' };
                string _SpecialSymbols = "~!@#$%^&*()_+|}{:?><`-\\=][';/.,";
                string _Alphabets = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string _Number = string.Empty;
                Random _RandomVal = new Random();
            Loop:
                _Number = _RandomVal.Next().ToString().Substring(0, 2);
                if (Convert.ToInt32(_Number) > _Alphabets.Length)
                    goto Loop;
                else
                    return DateTime.Now.ToString("yyddHHMMss") + _Number.Substring(1, 1) + _Alphabets[Convert.ToInt32(_Number)].ToString() + _SpecialSymbols.Substring(Convert.ToInt32(_Number.Substring(1, 1)), 2);
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Page : AppSecurity.cs \nFunction : RandomStringGenerator() \nException Occured\n" + Ex.Message);
                return "Record Could not be Updated. Please refer logs for detail.";
            }
        }
        #endregion

        #region Encrypt
        public string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        #endregion

        #region Decrypt
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion

        #region Encryption At Client Side & Decryption At Server Side - JavaScript To C#
        public static string UnMaskString(string objP)
        {
            string convertedObjP = string.Empty;
            try
            {
                string[] resP = objP.Split(';');
                for (int iP = 0; iP <= resP.Length - 1; iP++)
                {
                    if (resP[iP].ToString() != "")
                    {
                        int unicode = int.Parse(resP[iP].ToString());
                        char character = (char)unicode;
                        string text = character.ToString();

                        convertedObjP += text;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return convertedObjP;
        }

        public string DecryptStringAES(string cipherText)
        {

            var keybytes = Encoding.UTF8.GetBytes("POIU8S8KXRRT80DF");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        #endregion

        #region Default Password
        public static string GenerateDfPw()
        {
            return "mW4O1HWHz11TZRLF9MhK587bebnvFAgj";
                // "t4Ke4dcB6R5HeRe0xpEKgzV6CT3Yr8CG";
        }
        #endregion
    }
}
