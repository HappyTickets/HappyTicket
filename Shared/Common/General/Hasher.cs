using System.Security.Cryptography;
using System.Text;

namespace Shared.Common.General
{

    public static class Hasher
    {

        private const string EncryptionKey = "8947az34awl34kjq8947az34awl34kjq";
        public static string EncryptSalary(this decimal salary)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16];

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(salary.ToString());
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static decimal DecryptSalary(string encryptedSalary)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.IV = new byte[16];

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedSalary)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            string salaryString = srDecrypt.ReadToEnd();
                            return decimal.Parse(salaryString);
                        }
                    }
                }
            }
        }

        public static string HashSalary(this decimal salary)
        {
            var encryptedSalary = salary.EncryptSalary();
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptedSalary));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
                return hash;
            }
        }

        public static bool VerifySalary(string hashedSalary, decimal salary)
        {
            var computedHash = salary.HashSalary();
            return hashedSalary == computedHash;
        }

    }
}
