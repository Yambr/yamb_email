using System;
using System.Collections.Generic;
using System.Text;

namespace Yambr.Analyzer.Pullenti.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
