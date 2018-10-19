using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public static class Extensions
    {
        public static int SetBits(this int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            if (action == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var element in source)
            {
                action(element);
            }
        }

        public static byte[] XOr(this byte[] bytes, byte key)
        {
            var retBytes = new byte[bytes.Length];
            for (var i = 0; i < bytes.Length; i++)
            {
                retBytes[i] = (byte) (bytes[i] ^ key);
            }

            return retBytes;
        }

        public static Readable ToReadable(this byte[] bytes)
        {
            return  new Readable(Encoding.ASCII.GetString(bytes));
        }
    }
}