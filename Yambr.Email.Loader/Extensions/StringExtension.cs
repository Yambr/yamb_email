using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Yambr.Email.Loader.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// квадратные скобки
        /// </summary>
        public const string SquareBrackets = @"\[([^]]*)\]";
        /// <summary>
        /// фигурные скобки
        /// </summary>
        public const string Braces = @"\{([^]]*)\}";
        /// <summary>
        ///  хештеги
        /// </summary>
        public const string HashTags = @"#(\w+)";
        public static ICollection<string> GetAllTags(this string text)
        {
            return GetAllTags(text, out string _);
        }

        public static ICollection<string> GetAllTags(this  string text, out string withoutTags)
        {
            var tags = new List<string>();
            // скобки
            var regexs = new[]
            {
                SquareBrackets,
                Braces,
                HashTags
            };
            foreach (var regex in regexs)
            {
                var matches = Matches(text, regex);
                if (matches == null) continue;
                text = matches.Aggregate(text, (current, match) => current.Replace(match, string.Empty));
                tags.AddRange(matches);
            }
            withoutTags = text;
            return tags
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList();
        }

        public static ICollection<string> GetHashTags(this string text)
        {
            return Matches(text, HashTags)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .ToList();
        }

        public static ICollection<string> Matches(this string text, string regex)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                return Regex.Matches(text, regex)
                    .Cast<Match>()
                    .Select(x => x.Value)
                    .Where(c => c.Length > 2)//больше 2 символов в теге
                    .ToList();
            }
            return null;
        }

        public static string RemoveWhitespace(this string str)
        {
            return string.Join(" ", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
