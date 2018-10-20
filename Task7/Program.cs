using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace Task7
{
    public class Program
    {
        private const int blockSize = 16;
        private const int numberRounds = 10;

        private static readonly byte[][] rcon = new byte[11][]
        {
            new byte[] {0x00, 0, 0, 0},
            new byte[] {0x01, 0, 0, 0},
            new byte[] {0x02, 0, 0, 0},
            new byte[] {0x04, 0, 0, 0},
            new byte[] {0x08, 0, 0, 0},
            new byte[] {0x10, 0, 0, 0},
            new byte[] {0x20, 0, 0, 0},
            new byte[] {0x40, 0, 0, 0},
            new byte[] {0x80, 0, 0, 0},
            new byte[] {0x1b, 0, 0, 0},
            new byte[] {0x36, 0, 0, 0}
        };

        private static readonly byte[] sBox = new byte[256]
        {
            //0     1    2      3     4    5     6     7      8    9     A      B    C     D     E     F
            0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76, //0
            0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, //1
            0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15, //2
            0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75, //3
            0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, //4
            0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf, //5
            0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8, //6
            0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, //7
            0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73, //8
            0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb, //9
            0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, //A
            0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08, //B
            0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a, //C
            0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, //D
            0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf, //E
            0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16  //F
        };

        private static void Main(string[] args)
        {
            //var text = new B64
            //    (File.ReadAllText(@"D:\VS17 projects\Cryptopals\Task7\data.txt")).GetBytes();
            var text = new Readable("0123456789ABCDEF").GetASCIIBytes();
            var key = KeyExpansion(new Readable("ABCDEFGHIJKLMNOP").GetASCIIBytes());
            //expected: 5A D4 2F 60 07 46 B1 01 FA 1C 7C 23 18 54 2C D6 CE 48 C2 1E D3 5A 40 FA CD 92 9A 1B F2 5F C5 95
            PadInput(ref text);
            var output = new List<byte>();
            for (var i = 0; i * blockSize < text.Length; i += blockSize)
            {
                var block = text.AsSpan(i * blockSize, blockSize).ToArray();
                output.AddRange(AES(block, key));
            }

            Console.WriteLine(string.Join(' ', output.ToArray().Select(x => x.ToString("x2"))));
        }

        private static byte[] GetColumnMajor(byte[] b) //???
        {
            return new byte[16]
            {
                b[0x00], b[0x04], b[0x08], b[0x0c],
                b[0x01], b[0x05], b[0x09], b[0x0d],
                b[0x02], b[0x06], b[0x0a], b[0x0e],
                b[0x03], b[0x07], b[0x0b], b[0x0f]

            };
        }

        private static void PadInput(ref byte[] text) //???
        {
            var rem = 16 - text.Length % 16;
            if (rem == 16)
            {
                return;
            }

            var padded = new byte[text.Length + rem];
            for (var i = 0; i < text.Length; i++)
            {
                padded[i] = text[i];
            }

            for (var i = text.Length; i < padded.Length; i++)
            {
                padded[i] = 0;
            }

            text = padded;
        }

        public static byte[] AES(byte[] plaintext, List<byte[]> expandedKey) //???
        {
            plaintext = AddRoundKey(plaintext, expandedKey[0]);
            for (var roundIndex = 0; roundIndex < numberRounds; roundIndex++)
            {
                plaintext = Round(plaintext, roundIndex, expandedKey);
            }

            return plaintext;
        }

        public static List<byte[]> KeyExpansion(byte[] key)
        {
            key = GetColumnMajor(key); //???
            var N = 4;
            var K = new List<byte[]>();
            for (var i = 0; i < N; i++)
            {
                K.Add(new byte[4]
                {
                    key[i * 4 + 0], key[i * 4 + 1],
                    key[i * 4 + 2], key[i * 4 + 3]
                });
            }

            var R = 11;
            var W = new List<byte[]>(); //4-byte words, 176 bytes in total
            for (var i = 0; i < 44; i++)
            {
                W.Add(new byte[4]);
            }

            byte[] RotWord(byte[] arr) => new byte[4] { arr[1], arr[2], arr[3], arr[0] };
            byte[] SubWord(byte[] arr) => new byte[4] { sBox[arr[0]], sBox[arr[1]], sBox[arr[2]], sBox[arr[3]] };

            for (var i = 0; i < 4 * R; i++)
            {
                if (i < N)
                {
                    W[i] = K[i];
                }
                else if (i >= N && i % N == 0)
                {
                    W[i] = W[i - N].XOr(RotWord(SubWord(W[i - 1]))).XOr(rcon[i / N]);
                }
                else if (i >= N && N > 6 && i % N == 4)
                {
                    W[i] = W[i - N].XOr(SubWord(W[i - 1]));
                }
                else
                {
                    W[i] = W[i - N].XOr(W[i - 1]);
                }
            }

            var flattened = W.SelectMany(x => x).ToArray();
            var resized = new List<byte[]>(); //each byte[] is 16 elements
            for (var i = 0; i < R; i++)
            {
                resized.Add(new byte[16]);
            }

            for (var i = 0; i < flattened.Length; i++)
            {
                resized[i / 16][i % 16] = flattened[i];
            }

            return resized;
        }

        private static byte[] AddRoundKey(byte[] text, byte[] roundKey) //???
        {
            return text.XOr(roundKey);
        }

        private static byte[] Round(byte[] plaintext, int roundIndex, List<byte[]> expandedKey) //???
        {
            plaintext = SubBytes(plaintext);
            plaintext = ShiftRows(plaintext);
            if (roundIndex != 9)
            {
                plaintext = MixColumns(plaintext);
            }

            return AddRoundKey(plaintext, expandedKey[roundIndex + 1]);
        } 

        public static byte[] SubBytes(byte[] text)
        {
            return text.Select(x => sBox[x]).ToArray();
        }

        public static byte[] ShiftRows(byte[] text)
        {
            if (text.Length != 16)
            {
                throw new ArgumentException("Array of text must be exactly 16 bytes longs");
            }
            /* Input
             * B[00] B[01] B[02] B[03]
             * B[04] B[05] B[06] B[07]
             * B[08] B[09] B[10] B[11]
             * B[12] B[13] B[14] B[15]
             * Output
             * B[00] B[01] B[02] B[03]
             * B[05] B[06] B[07] B[04]
             * B[10] B[11] B[08] B[09]
             * B[15] B[12] B[13] B[14]
             */

            return new byte[16]
            {
                text[00], text[01], text[02], text[03],
                text[05], text[06], text[07], text[04],
                text[10], text[11], text[08], text[09],
                text[15], text[12], text[13], text[14]
            };
        }

        public static byte[] MixColumns(byte[] input) //input is 16 bytes //???
        {
            var columns = new List<byte[]>();
            for (int i = 0; i < 4; i++)
            {
                columns.Add(new byte[4] { input[00 + i], input[04 + i], input[08 + i], input[12 + i] });
                //columns.Add(new byte[4] { input[00 + i * 4], input[01 + i * 4], input[02 + i * 4], input[03 + i * 4] });
            }

            for (int i = 0; i < columns.Count; i++)
            {
                columns[i] = MixColumnsCore(columns[i]);
            }

            return columns.SelectMany(x => x).ToArray();
        }

        public static byte[] MixColumnsCore(byte[] input)
        {
            if (input.Length != 4)
            {
                throw new ArgumentException("Input array must be exactly 4 bytes long");
            }

            var output = new byte[4];
            output[0] = (byte)(GalMul(input[0], 2) ^ GalMul(input[3], 1) ^ GalMul(input[2], 1) ^ GalMul(input[1], 3));
            output[1] = (byte)(GalMul(input[1], 2) ^ GalMul(input[0], 1) ^ GalMul(input[3], 1) ^ GalMul(input[2], 3));
            output[2] = (byte)(GalMul(input[2], 2) ^ GalMul(input[1], 1) ^ GalMul(input[0], 1) ^ GalMul(input[3], 3));
            output[3] = (byte)(GalMul(input[3], 2) ^ GalMul(input[2], 1) ^ GalMul(input[1], 1) ^ GalMul(input[0], 3));
            return output;
        }

        public static byte[] MixColumnsCoreInverse(byte[] input)
        {
            if (input.Length != 4)
            {
                throw new ArgumentException("Input array must be exactly 4 bytes long");
            }

            var output = new byte[4];
            output[0] = (byte)(GalMul(input[0], 14) ^ GalMul(input[3], 9) ^ GalMul(input[2], 13) ^ GalMul(input[1], 11));
            output[1] = (byte)(GalMul(input[1], 14) ^ GalMul(input[0], 9) ^ GalMul(input[3], 13) ^ GalMul(input[2], 11));
            output[2] = (byte)(GalMul(input[2], 14) ^ GalMul(input[1], 9) ^ GalMul(input[0], 13) ^ GalMul(input[3], 11));
            output[3] = (byte)(GalMul(input[3], 14) ^ GalMul(input[2], 9) ^ GalMul(input[1], 13) ^ GalMul(input[0], 11));
            return output;
        }

        private static byte GalMul(byte a, byte b)
        {
            byte result = 0;
            for (var i = 0; i < 8; i++)
            {
                if ((b & 1) == 1)
                {
                    result ^= a;
                }

                var hiBitSet = (byte)(a & 0x80);
                a <<= 1;
                if (hiBitSet == 0x80)
                {
                    a ^= 0x1b;
                }

                b >>= 1;
            }

            return result;
        }
    }
}