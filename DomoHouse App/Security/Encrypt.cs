using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Mica.Droid.Security
{
    public static class Encrypt
    {
        private static string key = "EncryptionKeyForDomoHouse";

        public static string EncryptString(string password)
        {
            byte[] pssBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(pssBytes, 0, pssBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        public static string DecryptString(string encryptedPassword)
        {
            encryptedPassword = encryptedPassword.Replace(" ", "+");
            byte[] pssBytes = Convert.FromBase64String(encryptedPassword);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(pssBytes, 0, pssBytes.Length);
                        cs.Close();
                    }
                    encryptedPassword = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return encryptedPassword;
        }
    }
}