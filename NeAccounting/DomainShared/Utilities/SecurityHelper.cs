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
}
