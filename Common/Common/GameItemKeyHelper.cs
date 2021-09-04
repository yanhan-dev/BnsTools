using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class GameItemKeyHelper
    {
        public static string Get(int id)
        {
            byte[] bytes = BitConverter.GetBytes(id).Reverse().ToArray();
            string gameItemKey = Convert.ToBase64String(bytes);
            return gameItemKey;
        }
    }
}
