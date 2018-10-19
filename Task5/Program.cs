using System;
using System.Text;
using Utilities;

namespace Task5
{
    public class Program
    {

        private static void Main(string[] args)
        {
            var key = new Readable("ICE");
            var input = new Readable("Burning \'em, if you ain\'t quick and nimble\nI go crazy when I hear a cymbal");
            var expected =
                new Hex("0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f");
            
            Console.WriteLine(EncodeMultiXOr(input.GetASCIIBytes(), key.GetASCIIBytes()));
            Console.WriteLine(expected);
        }

        public static Readable DecodeMultiXOr(byte[] input, byte[] key)
        {
            var output = new StringBuilder();
            var ix = 0;
            foreach (var c in input)
            {
                output.Append((char)(c ^ key[ix++ % key.Length]));
            }

            return new Readable(output.ToString());
        }

        public static Hex EncodeMultiXOr(byte[] input, byte[] key)
        {
            var output = new StringBuilder();
            var ix = 0;
            foreach (var c in input)
            {
                output.Append((c ^ key[ix++ % key.Length]).ToString("x2"));
            }

            return new Hex(output.ToString());
        }
    }
}