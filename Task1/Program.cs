using System;
using Utilities;

namespace Task1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var expected = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";
            var hexString =
                new
                    Hex("49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d");
            Console.WriteLine(hexString.ToB64().String);
            Console.WriteLine(expected);
        }
    }
}