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
            _key = Convert.FromBase64String(opt.Value.Key);
            _iv = Convert.FromBase64String(opt.Value.IV);

            if (!new[] { 16, 24, 32 }.Contains(_key.Length))
                throw new CryptographicException("AES key length must be 16/24/32 bytes.");
            if (_iv.Length != 16)
                throw new CryptographicException("AES IV length must be 16 bytes.");
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs, Encoding.UTF8))
                sw.Write(plainText);

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var bytes = Convert.FromBase64String(cipherText);
            using var ms = new MemoryStream(bytes);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}
