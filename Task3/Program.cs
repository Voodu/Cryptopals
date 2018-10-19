using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Task3
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var bytes = new Hex("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736").GetBytes();
            Console.WriteLine($"Encoded: {bytes}");
            Console.WriteLine($"Decoded: {SolveSingleXOr(bytes).DecodedText}");
        }

        public static (Readable DecodedText, KeyGuess FoundKey) SolveSingleXOr(byte[] bytes)
        {
            var results = new List<Readable>();
            var keys = new List<KeyGuess>();
            for (byte testedKey = 0b0000_0000; testedKey < 0b1111_1111; testedKey++)
            {
                var testedString = bytes.XOr(testedKey).ToReadable();
                results.Add(testedString);
                keys.Add(new KeyGuess {Key = testedKey, Scoring = testedString.Scoring});
            }

            return (
                results.Aggregate((agg, next) => next.Scoring > agg.Scoring ? next : agg),
                keys.Aggregate((agg, next) => next.Scoring > agg.Scoring ? next : agg));
        }
    }
}