using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Cryptography;

namespace okitoki.twitch.irc.client
{
    [Serializable]
    public class Credentials
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("oauth-token")]
        public string OauthToken { get; set; }

        public Credentials() { }

        public Credentials(string username, string token)
        {
            this.Username = username;
            this.OauthToken = token;
        }

        public static string Encrypt(string key, string value)
        {
            Aes aes = GetAes(key);
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] encryptedBytes = aes.CreateEncryptor().TransformFinalBlock(valueBytes, 0, valueBytes.Length);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string key, string value)
        {
            Aes aes = GetAes(key);
            byte[] encryptedBytes = Convert.FromBase64String(value);
            byte[] descryptedBytes = aes.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            return Encoding.UTF8.GetString(descryptedBytes);
        }

        private static Aes GetAes(string key)
        {
            byte[] aesBytes = new byte[16];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            Array.Copy(keyBytes, aesBytes, Math.Min(keyBytes.Length, 16));

            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 128;
            aes.Key = aesBytes;
            aes.IV = aesBytes;
            return aes;
        }
    }
}
