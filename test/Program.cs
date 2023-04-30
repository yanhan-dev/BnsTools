using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using System.Linq;

namespace test
{
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        //月
    //        for (int i = 1; i < 12; i++)
    //        {   //日
    //            for (int j = 1; j < 31; j++)
    //            {
    //                byte[] b = MD5.HashData(Encoding.UTF8.GetBytes($"1998{i}{j}"));
    //                if (b[0] == 0xBA && b[1] == 0x8C)
    //                {
    //                    Console.WriteLine($"1998{i}{j}");
    //                    string hexString = BitConverter.ToString(b).Replace("-", "").ToLower();
    //                    Console.WriteLine(hexString);
    //                }
    //            }
    //        }
    //    }
    //}

    class Program
    {
        static void Main(string[] args)
        {
            string cipherText = "2XU5FuJGPK3KHApSkkSeQPML8NYvTqhZeorAHivXC1jd4gqyf2fx9kfxqPCfsHBEuCibhirhYkMjfXH493WxTe9S";

            // Base58 decoding
            byte[] decodedBytes = Base58CheckDecode(cipherText);

            // Base32 decoding
            string base32Text = Encoding.ASCII.GetString(decodedBytes);
            byte[] base32Bytes = Base32Decode(base32Text);

            // Base64 decoding
            string plainText = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.ASCII.GetString(base32Bytes)));

            Console.WriteLine(plainText); // Output: flag{base64-base32-base58}
        }

        static byte[] Base58CheckDecode(string input)
        {
            var alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            var baseCount = alphabet.Length;

            // Convert the input string to an integer
            var num = 0;
            foreach (var c in input)
            {
                var p = alphabet.IndexOf(c);
                if (p == -1)
                    throw new FormatException("Invalid character found in input");

                num *= baseCount;
                num += p;
            }

            // Convert the integer to a byte array, accounting for leading zeros and checksum
            var leadingZerosCount = input.TakeWhile(c => c == '1').Count();
            var data = BitConverter.GetBytes(num).Reverse().SkipWhile(b => b == 0).ToArray();
            var checksum = new SHA256Managed().ComputeHash(new SHA256Managed().ComputeHash(data)).Take(4).ToArray();
            var output = new byte[leadingZerosCount + data.Length + checksum.Length];
            Array.Copy(data, 0, output, leadingZerosCount, data.Length);
            Array.Copy(checksum, 0, output, leadingZerosCount + data.Length, checksum.Length);

            return output;
        }

        static byte[] Base32Decode(string input)
        {
            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var baseCount = alphabet.Length;

            // Convert the input string to an integer
            var num = 0;
            foreach (var c in input)
            {
                var p = alphabet.IndexOf(char.ToUpperInvariant(c));
                if (p == -1)
                    throw new FormatException("Invalid character found in input");

                num <<= 5;
                num += p;
            }

            // Convert the integer to a byte array
            var output = new byte[(int)Math.Ceiling(BigInteger.Log(num, 256))];
            for (var i = output.Length - 1; i >= 0; i--)
            {
                output[i] = (byte)(num % 256);
                num /= 256;
            }

            return output;
        }
    }

}
