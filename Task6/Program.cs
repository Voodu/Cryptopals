using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities;

namespace Task6
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const int maxKey = 40;
            const int precision = 1;
            var text = new B64
                (File.ReadAllText(@"D:\VS17 projects\Cryptopals\Task6\data.txt")).GetBytes();
            var textSpan = new ReadOnlySpan<byte>(text);
            var keyGuesses = new List<KeySizeGuess>();
            for (var keysize = 2; keysize < maxKey + 1; keysize++)
            {
                var sum = 0D;
                for (var i = 0; i < precision * 2; i += 2)
                {
                    var first = textSpan.Slice(precision * keysize, keysize);
                    var second = textSpan.Slice((precision + 1) * keysize, keysize);
                    sum += (double)HammingDistance(first.ToArray(), second.ToArray()) / keysize;
                }

                var avg = sum / precision;
                keyGuesses.Add(new KeySizeGuess
                {
                    KeySize = keysize,
                    Scoring = avg
                });
            }

            keyGuesses.Sort((a, b) => a.Scoring.CompareTo(b.Scoring));
            var checkedKeys = keyGuesses.Take(5).ToList();
            Console.WriteLine($"Checked key sizes: {string.Join(',', checkedKeys.Select(k => k.KeySize))}");

            var readables = new List<Readable>();
            foreach (var checkedKey in checkedKeys)
            {
                readables.Add(SolveCipherForKeysize(text, checkedKey));
            }

            Console.WriteLine(readables.Aggregate((agg, next) => next.Scoring > agg.Scoring ? next : agg).String);
        }

        public static Readable SolveCipherForKeysize(byte[] text, KeySizeGuess checkedKey)
        {
            var divideText = DivideText(text, checkedKey.KeySize);
            var transposed = TransposeBlocks(divideText);
            var finalKey = new byte[checkedKey.KeySize];
            for (var i = 0; i < transposed.Count; i++)
            {
                var block = transposed[i];
                finalKey[i] = Task3.Program.SolveSingleXOr(block).FoundKey.Key;
            }

            return Task5.Program.DecodeMultiXOr(text, finalKey);
        }

        public static List<byte[]> DivideText(byte[] text, int keySize)
        {
            var blocks = new List<byte[]>();

            var blockIx = -1;
            for (var i = 0; i < text.Length; i++)
            {
                if (i % keySize == 0)
                {
                    blockIx++;
                    blocks.Add(new byte[keySize]);
                }

                blocks[blockIx][i % keySize] = text[i];
            }

            return blocks;
        }

        public static List<byte[]> TransposeBlocks(List<byte[]> blocks)
        {
            var transposedBlocks = new List<byte[]>();
            for (var i = 0; i < blocks.First().Length; i++)
            {
                transposedBlocks.Add(new byte[blocks.Count]);
            }

            for (var i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                for (var j = 0; j < block.Length; j++)
                {
                    transposedBlocks[j][i] = block[j];
                }
            }

            return transposedBlocks;
        }

        public static int HammingDistance(byte[] s1, byte[] s2)
        {
            var sum = Math.Abs(s1.Length - s2.Length);
            sum += s1.Zip(s2, HammingDistance).Sum();
            return sum;
        }

        public static int HammingDistance(byte c1, byte c2)
        {
            return (c1 ^ c2).SetBits();
        }
    }
}