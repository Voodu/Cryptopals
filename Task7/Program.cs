using System;
using System.IO;
using System.Security.Cryptography;
using Utilities;

namespace Task7
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var cipherText = new B64(File.ReadAllText(@"D:\VS17 projects\Cryptopals\Task7\7.txt")).GetBytes();
            var key = new Readable("YELLOW SUBMARINE").GetASCIIBytes();
            Console.WriteLine(Decrypt(cipherText, key));
        }

        public static string Decrypt(byte[] cipherText, byte[] key)
        {
            string plaintext = null;
            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.Zeros;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }
    }
}