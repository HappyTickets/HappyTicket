using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.General
{
    public static class GeneralEncryptorDecryptor
    {
        public static string EncryptString(string input)
        {
            StringBuilder encryptedString = new();

            foreach (char c in input)
            {
                encryptedString.Append((char)(c + 3));
            }

            return encryptedString.ToString();
        }

        public static string DecryptString(string encryptedInput)
        {
            StringBuilder decryptedString = new();

            foreach (char c in encryptedInput)
            {
                decryptedString.Append((char)(c - 3));
            }

            return decryptedString.ToString();
        }
    }
}
