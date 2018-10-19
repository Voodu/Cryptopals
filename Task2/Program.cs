using System;
using System.Linq;
using System.Text;
using Task1;
using Utilities;

namespace Task2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var str1 = new Hex("1c0111001f010100061a024b53535009181c");
            var str2 = new Hex("686974207468652062756c6c277320657965");
            var expected = new Hex("746865206b696420646f6e277420706c6179");

            Console.WriteLine($"Expected:\t{expected.ToReadable()}");
            Console.WriteLine($"Calculated:\t{str1.XOr(str2).ToReadable()}");
        }
    }
}