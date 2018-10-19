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
            var sliceSize = 5;
            var text = new Readable("blockblockblockblockblock");
            var textSpan = new Span<byte>(text.GetASCIIBytes());
            var first = textSpan.Slice(0*sliceSize, sliceSize);
            var second = textSpan.Slice(1*sliceSize, sliceSize);
            var third = textSpan.Slice(2*sliceSize, sliceSize);
            var fourth = textSpan.Slice(3*sliceSize, sliceSize);

            Console.WriteLine(first.ToString(), second.ToString());
        }
    }
}
