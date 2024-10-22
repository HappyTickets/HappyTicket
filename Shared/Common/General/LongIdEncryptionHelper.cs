namespace Shared.Common.General
{
    public static class LongIdEncryptionHelper
    {
        private static string LongIdEncryptionKey;

        // Method to initialize the encryption key
        public static void Initialize(string encryptionKey)
        {
            LongIdEncryptionKey = encryptionKey ?? throw new ArgumentNullException(nameof(encryptionKey), "Encryption key cannot be null");
        }

        public static string EncryptId(long id)
        {
            if (string.IsNullOrEmpty(LongIdEncryptionKey))
                throw new InvalidOperationException("Encryption key is not initialized.");

            var idBytes = BitConverter.GetBytes(id);
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(LongIdEncryptionKey);
            var encryptedBytes = new byte[idBytes.Length];

            for (int i = 0; i < idBytes.Length; i++)
                encryptedBytes[i] = (byte)(idBytes[i] ^ keyBytes[i % keyBytes.Length]);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static long DecryptId(string encryptedId)
        {
            if (string.IsNullOrEmpty(LongIdEncryptionKey))
                throw new InvalidOperationException("Encryption key is not initialized.");

            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedId);
                var keyBytes = System.Text.Encoding.UTF8.GetBytes(LongIdEncryptionKey);
                var decryptedBytes = new byte[encryptedBytes.Length];

                for (int i = 0; i < encryptedBytes.Length; i++)
                {
                    decryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
                }

                return BitConverter.ToInt64(decryptedBytes, 0);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Invalid format for encrypted id.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during decryption.", ex);
            }
        }
    }

}
