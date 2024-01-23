using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Web.Configuration;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class Encryption
    {
        private static int KEY_SIZE = 256;
        private static int PASSWORD_ITERATIONS = 3;

        public string Encrypt(string plainText, string salt)
        {
            string result = "";
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0) { throw new Exception("Encryption failed. Text is empty."); }
            if (salt == null || salt.Length <= 0) { throw new Exception("Encryption failed. Salt is empty."); }
            
            // Create an Rijndael object 
            // with the specified key and IV. 
            using (Rijndael rijndael = Rijndael.Create())
            {
                string passPhrase = WebConfigurationManager.AppSettings["PASS_PHRASE"];
                byte[] passPhraseBytes = Encoding.ASCII.GetBytes(passPhrase);
                byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

                Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhraseBytes, saltBytes, PASSWORD_ITERATIONS);
                
                rijndael.Key = password.GetBytes(KEY_SIZE / 8);
                rijndael.IV = password.GetBytes(rijndael.BlockSize / 8);
                rijndael.Padding = PaddingMode.Zeros;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        //result = Encoding.Unicode.GetString(msEncrypt.ToArray());
                        result = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }

            return result;
        }

        public string Decrypt(string encryptedText, string salt)
        {
            string result = "";
            // Check arguments. 
            if (encryptedText == null || encryptedText.Length <= 0) { throw new Exception("Encryption failed. Text is empty."); }
            if (salt == null || salt.Length <= 0) { throw new Exception("Encryption failed. Salt is empty."); }

            //byte[] encryptedTextBytes = Encoding.Unicode.GetBytes(encryptedText);
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

            // Create an Rijndael object 
            // with the specified key and IV. 
            using (Rijndael rijndael = Rijndael.Create())
            {
                string passPhrase = WebConfigurationManager.AppSettings["PASS_PHRASE"];
                byte[] passPhraseBytes = Encoding.ASCII.GetBytes(passPhrase);
                byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

                Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhraseBytes, saltBytes, PASSWORD_ITERATIONS);

                rijndael.Key = password.GetBytes(KEY_SIZE / 8);
                rijndael.IV = password.GetBytes(rijndael.BlockSize / 8);
                rijndael.Padding = PaddingMode.Zeros;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(encryptedTextBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            result = result.Trim(new char[] { '\0' });

            return result;
        }
    }
}
