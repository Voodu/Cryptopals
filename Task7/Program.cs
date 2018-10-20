using System;
using System.IO;
using System.Security.Cryptography;
using Utilities;

namespace Task7
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cipherText = new B64(File.ReadAllText(@"D:\VS17 projects\Cryptopals\Task7\7.txt")).GetBytes();
            var key = new Readable("YELLOW SUBMARINE").GetASCIIBytes();

            using (var aes = new AesManaged())
            {
                aes.Key = key;
                string plaintext = null;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.Zeros;

                // Create a decryptor to perform the stream transform.
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // Create the streams used for decryption.
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

                Console.WriteLine(plaintext);
            }
        }
    }
}