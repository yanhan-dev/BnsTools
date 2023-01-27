using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (maxLength < 5) throw new Exception("maxLength cant < 5");

            if (string.IsNullOrEmpty(value)) return value;

            if (value.Length <= maxLength) return value;

            int leftIndex = 0;

            int leftCount = (maxLength - 3) / 2;

            int rightIndex = value.Length - (maxLength - 3) / 2 - (maxLength - 3) % 2;

            int rightCount = (maxLength - 3) / 2 + (maxLength - 3) % 2;

            return $"{value.Substring(leftIndex, leftCount)}...{value.Substring(rightIndex, rightCount)}";
        }
    }
}
// 20 17/2=8 