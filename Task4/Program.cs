using System;
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
                File.ReadAllLines(@"..\..\..\data.txt").Select(l => new Hex(l)).ToList();
            (double score, string text) best = (0D, string.Empty);
            foreach (var hexString in hexStringList)
            {
                var (decodedText, _) = Task3.Program.SolveSingleXOr(hexString.GetBytes());
                var currentMax = decodedText.Scoring;
                if (currentMax > best.score)
                {
                    best.score = currentMax;
                    best.text = decodedText.String;
                }
            }

            Console.WriteLine($"Found text:\n{best.text}");
        }
    }
}