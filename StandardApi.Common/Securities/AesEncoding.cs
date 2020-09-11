using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace StandardApi.Common.Securities
{
    public static class AesEncoding
    {
        private const string KEY = "2202199909051998";
        private const string IV = "_hn@nT_mIK_IHT_3L_";

        public static string EncryptAes(this string plainText)
        {
            Encoding byteEncoder = Encoding.UTF8;
            var key = byteEncoder.GetBytes(KEY);
            var iv = byteEncoder.GetBytes(IV);

            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

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
            var text = Convert.ToBase64String(encrypted);
            return text;
        }

        public static string DecryptAes(this string cipherText)
        {
            Encoding byteEncoder = Encoding.UTF8;
            var key = byteEncoder.GetBytes(KEY);
            var iv = byteEncoder.GetBytes(IV);

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, key, iv);
            var plainText = string.Format(decriptedFromJavascript);

            return plainText;

            //// Create an Aes object
            //// with the specified key and IV.
            //using (Aes aes = Aes.Create())
            //{


            //    using (MemoryStream ms = new MemoryStream(encrypted))
            //    {
            //        // Read the prepended IV
            //        aes.Key = key;
            //        aes.IV = iv;
            //        ms.Read(iv, 0, iv.Length);

            //        // Note that we are setting IV, Mode, Padding
            //        aes.Mode = CipherMode.CBC;
            //        aes.Padding = PaddingMode.PKCS7;

            //        // Create an encryptor to perform the stream transform.
            //        using (ICryptoTransform decrytor = aes.CreateDecryptor())
            //        using (CryptoStream cs = new CryptoStream(ms, decrytor, CryptoStreamMode.Read))
            //        // Here we are setting the Encoding
            //        using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
            //        {
            //            // Read all data from the stream.
            //            string plainText = sr.ReadToEnd();
            //            return plainText;
            //        }
            //    }
            //}
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
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

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = key;
                aes.IV = iv;

                var decrytor = aes.CreateDecryptor();
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decrytor, CryptoStreamMode.Read))
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
                catch (Exception)
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }
    }
}
