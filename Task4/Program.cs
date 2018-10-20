using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Task4
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var hexStringList =
                File.ReadAllLines(@"D:\VS17 projects\Cryptopals\Task4\data.txt").Select(l => new Hex(l)).ToList();
            var maxScore = 0D;
            var maxString = string.Empty;
            foreach (var hexString in hexStringList)
            {
                var (decodedText, _) = Task3.Program.SolveSingleXOr(hexString.GetBytes());
                var currentMax = decodedText.Scoring;
                if (currentMax > maxScore)
                {
                    maxScore = currentMax;
                    maxString = decodedText.String;
                }
            }

            Console.WriteLine(maxString);
        }
    }
}