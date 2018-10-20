using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace Task8
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var textLines = File.ReadAllLines(@"D:\VS17 projects\Cryptopals\Task8\data.txt")
                                .Select(s => new Hex(s).GetBytes()).ToList();

            (int ix, int score) best = (0, 0);
            for (int i = 0; i < textLines.Count; i++)
            {
                var currentScore = CountDuplicatesBlocks(textLines[i]);
                if (currentScore > best.score)
                {
                    best = (i, currentScore);
                }
            }

            Console.WriteLine($"Line index: {best.ix}; maximum number of duplicates: {best.score}");
        }

        public static int CountDuplicatesBlocks(byte[] text, int blockSize = 16)
        {
            var bins = new Dictionary<byte[], int>(new ByteArrayEqualityComparer());
            var max = 0;
            for (var i = 0; i * blockSize < text.Length; i++)
            {
                var slice = text.AsSpan(i * blockSize, blockSize).ToArray();
                if (bins.ContainsKey(slice))
                {
                    var val = ++bins[slice];
                    if (val > max)
                    {
                        max = val;
                    }
                }
                else
                {
                    bins.Add(slice, 1);
                }
            }

            return max;
        }
    }
}