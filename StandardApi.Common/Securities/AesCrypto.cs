using System.Security.Cryptography;
using System.Text;

namespace StandardApi.Common.Securities
{
    public static class AesCrypto
    {
        private static byte[] key = Encoding.UTF8.GetBytes("hN@hT_MiK-iHt-3L");

        private static Aes CreateAes()
        {
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = new byte[aes.BlockSize / 8];
            return aes;
        }

        public static string Encrypt(string plain)
        {
            Aes aes = CreateAes();
            ICryptoTransform ct = aes.CreateEncryptor();
            byte[] input = Encoding.Unicode.GetBytes(plain);
            var result = ct.TransformFinalBlock(input, 0, input.Length);
            return result.ToBase62();
        }

        public static string Decrypt(string cipherText)
        {
            byte[] b = cipherText.FromBase62();
            Aes aes = CreateAes();
            ICryptoTransform ct = aes.CreateDecryptor();
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            return Encoding.Unicode.GetString(output);
        }
    }
}

