using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DbConnectionsManager
{
    public static class StringCipher
    {
        // ReSharper disable once InconsistentNaming
        private const string SALT_PASSWORD = @"C#1989Behzad3Kh9";

        public static string PublicPassword = @"H\,g,d@13";

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (key-size / 8).  Default key-size is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] InitVectorBytes = Encoding.ASCII.GetBytes(SALT_PASSWORD);

        // This constant is used to determine the key-size of the encryption algorithm.
        private const int KeySize = 256;

        public static string Encrypt(this string plainText, string passPhrase)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
#pragma warning disable 618
                var keyBytes = password.GetBytes(KeySize / 8);
#pragma warning restore 618
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC })
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    var cipherTextBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherTextBytes);
                }
            }
        }

        public static string Decrypt(this string cipherText, string passPhrase)
        {
            var cipherTextBytes = Convert.FromBase64String(cipherText);
            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
#pragma warning disable 618
                var keyBytes = password.GetBytes(KeySize / 8);
#pragma warning restore 618
                using (var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC })
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes))
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainTextBytes = new byte[cipherTextBytes.Length];
                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }
        }

        public static string Encrypt(this string plainText)
        {
            return plainText.Encrypt(PublicPassword);
        }

        public static string Decrypt(this string cipherText)
        {
            return cipherText.Decrypt(PublicPassword);
        }
    }
}