using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Readable : IStringWrapper
    {
        private static readonly Dictionary<char, double> CharacterScore = new Dictionary<char, double>()
        {
            {'a', 0.06517 },
            {'b', 0.01242},
            {'c', 0.02173},
            {'d', 0.03498},
            {'e', 0.10414},
            {'f', 0.01978},
            {'g', 0.01586},
            {'h', 0.04928},
            {'i', 0.05580},
            {'j', 0.00090},
            {'k', 0.00505},
            {'l', 0.03314},
            {'m', 0.02021},
            {'n', 0.05645},
            {'o', 0.05963},
            {'p', 0.01376},
            {'q', 0.00086},
            {'r', 0.04975},
            {'s', 0.05157},
            {'t', 0.07293},
            {'u', 0.02251},
            {'v', 0.00829},
            {'w', 0.01712},
            {'x', 0.00136},
            {'y', 0.01459},
            {'z', 0.00078},
            {' ', 0.19181}
        };

        private string _string;

        public Readable(string @string)
        {
            String = @string ?? throw new ArgumentNullException(nameof(@string));
        }

        public double Scoring { get; private set; }

        public string String
        {
            get => _string;
            set
            {
                _string = value;
                Scoring = GetScoring();
            }
        }

        public byte[] GetASCIIBytes()
        {
            return Encoding.ASCII.GetBytes(String);
        }

        public Hex ToHex()
        {
            return new Hex(string.Join("", String.Select(c => Convert.ToInt32(c).ToString("x2"))));
        }

        public B64 ToB64()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(String);
            return new B64(Convert.ToBase64String(plainTextBytes));
        }

        private double GetScoring()
        {
            var score = 0D;
            foreach (var letter in String)
            {
                var smallChar = char.ToLowerInvariant(letter);
                if (CharacterScore.ContainsKey(smallChar))
                {
                    score += CharacterScore[smallChar];
                }
                else
                {
                    score += 0.006905;
                }
            }

            return score;
        }

        public override string ToString()
        {
            return String;
        }
    }
}