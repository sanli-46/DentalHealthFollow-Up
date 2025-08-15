using System.Security.Cryptography;
using System.Text;
using DentalHealthFollow_Up.API.Options;
using Microsoft.Extensions.Options;

namespace DentalHealthFollow_Up.API.Services
{
    public class EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService(IOptions<EncryptionOptions> opt)
        {
            _key = Encoding.UTF8.GetBytes(opt.Value.Key);
            _iv = Encoding.UTF8.GetBytes(opt.Value.IV);
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key; aes.IV = _iv;
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
                sw.Write(plainText);
            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherBase64)
        {
            var buffer = Convert.FromBase64String(cipherBase64);
            using var aes = Aes.Create();
            aes.Key = _key; aes.IV = _iv;
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}

