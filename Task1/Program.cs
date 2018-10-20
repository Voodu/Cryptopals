using System;
using Utilities;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var expected = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";
            //var hexString =
            //    new
            //        Hex("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d");
            //Console.WriteLine(hexString.ToB64().String);
            //Console.WriteLine(expected);

            for (int i = 0; i < 256; i++)
            {
                //for (byte j = 0; j <= 16; j++)
                //{
                    Console.Write($"0x{GMul((byte) i,3), -3:x2}, ");
                if (i % 16 == 15)
                    Console.WriteLine();
                //}
                //Console.WriteLine();
            }
        }

        private static byte GMul(byte a, byte b)
        { // Galois Field (256) Multiplication of two Bytes
            byte p = 0;

            for (int counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }

                bool hi_bit_set = (a & 0x80) != 0;
                a <<= 1;
                if (hi_bit_set)
                {
                    a ^= 0x1B; /* x^8 + x^4 + x^3 + x + 1 */
                }
                b >>= 1;
            }

            return p;
        }
    }
}