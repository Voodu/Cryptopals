using System;
using Utilities;
using Xunit;

namespace RandomTests
{
    public class UnitTest1
    {
        [Fact]
        public void ShouldGive37AsHammingDistance()
        {
            var first = new Readable("this is a test");
            var second = new Readable("wokka wokka!!!");
            var expected = 37;

            var answer = Task6.Program.HammingDistance(first.GetASCIIBytes(), second.GetASCIIBytes());

            Assert.Equal(expected, answer);
        }

        [Fact]
        public void ShouldSth()
        {
            //var result = GM
            //Console.WriteLine(result);
        }

        private byte GMul(byte a, byte b)
        { // Galois Field (256) Multiplication of two Bytes
            byte p = 0;

            for (int counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }

                bool hi_bit_set = (a & 0x80) != 0;
                a <<= 1;
                if (hi_bit_set)
                {
                    a ^= 0x1B; /* x^8 + x^4 + x^3 + x + 1 */
                }
                b >>= 1;
            }

            return p;
        }
    }
}
