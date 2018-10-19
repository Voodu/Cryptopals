using System;
using System.Text;

namespace Utilities
{
    public class B64 : IStringWrapper
    {
        public B64(string @string)
        {
            String = @string ?? throw new ArgumentNullException(nameof(@string));
        }

        public string String { get; set; }

        public byte[] GetBytes()
        {
            return System.Convert.FromBase64String(String);
        }

        public Readable ToReadable()
        {
            return new Readable(Encoding.UTF8.GetString(GetBytes()));
        }

        public Hex ToHex()
        {
            return ToReadable().ToHex();
        }

        public override string ToString()
        {
            return String;
        }
    }
}