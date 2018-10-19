using System;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Hex : IStringWrapper
    {
        public Hex(string @string)
        {
            String = @string ?? throw new ArgumentNullException(nameof(@string));
        }

        public string String { get; set; }

        public Readable ToReadable()
        {
            return new Readable(Encoding.UTF8.GetString(GetBytes()));
        }

        public B64 ToB64()
        {
            return ToReadable().ToB64();
        }

        public byte[] GetBytes()
        {
            if (String.Length % 2 != 0)
            {
                String += '0';
            }

            var bytes = new byte[String.Length / 2];
            for (var i = 0; i < String.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(String.Substring(i, 2), 16);
            }

            return bytes;
        }

        public Hex XOr(Hex hex2)
        {
            var str1 = String;
            var str2 = hex2.String;
            if (str1.Length != str2.Length)
            {
                throw new ArgumentException("Both strings must have the same length");
            }

            var result = new StringBuilder(str1.Length);
            foreach (var letter in str1.Zip(str2,
                                            (s1, s2) => new
                                            {
                                                StrInt1 = Convert.ToInt32(s1.ToString(), 16),
                                                StrInt2 = Convert.ToInt32(s2.ToString(), 16)
                                            }))
            {
                result.Append((letter.StrInt1 ^ letter.StrInt2).ToString("X"));
            }

            return new Hex(result.ToString());
        }

        public Hex XOr(int key)
        {
            var str = String;
            if (str.Length % 2 != 0)
            {
                str += '0';
            }

            var result = new StringBuilder();
            for (var i = 0; i < str.Length; i += 2)
            {
                result.Append((Convert.ToInt32(str.Substring(i, 2), 16) ^ key).ToString("X"));
            }

            return new Hex(result.ToString());
        }

        public override string ToString()
        {
            return String;
        }
    }
}