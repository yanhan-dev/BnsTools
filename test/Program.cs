using Newtonsoft.Json;

using System;


namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = "123_qwer_123_bm".TrimStart("123".ToCharArray());
            Console.WriteLine(s);

            Console.WriteLine($"[{null}-123-{""}]");
        }
    }
}
