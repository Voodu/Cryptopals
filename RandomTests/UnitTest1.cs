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
        public void ShouldCountDuplicatesWork()
        {
            var input = new byte[]
            {
                1, 2, 3, 4, 5,
                1, 2, 3, 4, 5,
                3, 3, 1, 2, 2, 4, 5, 3, 1
            };

            var expected = 2;

            var output = Task8.Program.CountDuplicatesBlocks(input, 5);
            Assert.Equal(expected, output);
        }
    }
}
