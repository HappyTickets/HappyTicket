using System.Security.Cryptography;
using System.Text;

namespace Shared.Common.General
{

    public static class LongIdEncryptionHelper
    {
        private static string LongIdEncryptionKey;

        public static void Initialize(string encryptionKey)
        {
            LongIdEncryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey), "Encryption key cannot be null");
        }



        public static string EncryptLongId(long id)
        {
            if (string.IsNullOrEmpty(LongIdEncryptionKey))
                throw new InvalidOperationException("Encryption key is not initialized.");

            var idBytes = BitConverter.GetBytes(id);

            using (var aesAlg = Aes.Create())
            {
                var key = Encoding.UTF8.GetBytes(LongIdEncryptionKey.PadRight(32));
                aesAlg.Key = key;
                aesAlg.GenerateIV();
                var iv = aesAlg.IV;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv))
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(iv, 0, iv.Length);

                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(idBytes, 0, idBytes.Length);
                    }

                    var encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static long DecryptLongId(string encryptedId)
        {
            if (string.IsNullOrEmpty(LongIdEncryptionKey))
                throw new InvalidOperationException("Encryption key is not initialized.");

            var encryptedBytes = Convert.FromBase64String(encryptedId);

            using (var aesAlg = Aes.Create())
            {
                var key = Encoding.UTF8.GetBytes(LongIdEncryptionKey.PadRight(32));
                aesAlg.Key = key;

                var iv = new byte[16];
                Array.Copy(encryptedBytes, iv, iv.Length);

                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var msDecrypt = new MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    var decryptedBytes = new byte[8];
                    csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);
                    return BitConverter.ToInt64(decryptedBytes, 0);
                }
            }
        }
    }

}