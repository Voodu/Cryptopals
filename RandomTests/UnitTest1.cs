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
    }
}
