using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
    public static class SecurityHelper
    {
        public static string GetSha512Hash(string input)
        {
            //using (var sha256 = new SHA256CryptoServiceProvider())
            using var sha512 = SHA512.Create();
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = sha512.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
            //return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
        }

    }

    public static class StringCipher
    {
        private const string Key = "U~>PJ){uJeCtRWYr\"zvyw!l5vRy#F0o>";
        public static string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
