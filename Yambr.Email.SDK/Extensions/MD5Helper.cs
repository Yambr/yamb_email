using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Yambr.SDK.Extensions
{
    public static class MD5Extensions
    {
        public static string GetHashByJson(this object sourse)
        {
            var serializeObject = JsonConvert.SerializeObject(sourse);
            return GetHash(serializeObject);
        }

        public static string GetHash(this string source)
        {
            using (var md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, source);
            }
        }

        public static string GetHash(this byte[] source)
        {
            using (var md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, source);
            }
        }

        public static bool VerifyHash(string source, string hash)
        {
            using (var md5Hash = MD5.Create())
            {
                return VerifyMd5Hash(md5Hash, source, hash);
            }
        }


        private static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return GetMd5Hash(md5Hash, bytes);
        }

        private static string GetMd5Hash(HashAlgorithm md5Hash, byte[] bytes)
        {
            // Convert the input string to a byte array and compute the hash.
            var data = md5Hash.ComputeHash(bytes);
            return Md5ToString(data);
        }

        private static string Md5ToString(IEnumerable<byte> data)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyMd5Hash(HashAlgorithm md5Hash, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetMd5Hash(md5Hash, input);
            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

    }
}
