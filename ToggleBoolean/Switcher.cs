using System;
using System.Globalization;
using System.Linq;

namespace ToggleBoolean
{
    public static class Switcher
    {
        private static string[][] Items = new string[][]
        {
            new string[] { "true", "false" },
            new string[] { "private", "internal", "protected", "public" },
            new string[] { "class", "struct", "enum", "record" },
            new string[] { "on", "off" },
            new string[] { "yes", "no" },
            new string[] { "0", "1" },
            new string[] { "&&", "||" },
        };

        public static string Switch(string input, bool reverse)
        {
            foreach (var item in Items)
            {
                for (var i = item.Length - 1; i >= 0; i--)
                {
                    var word = item[i];
                    if (word.Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        var next = (reverse ? (i - 1 + item.Length) : (i + 1)) % item.Length;
                        var output = item[next];

                        if (char.IsUpper(input[0]))
                        {
                            if (input.All(char.IsUpper))
                            {
                                return output.ToUpperInvariant();
                            }

                            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(output);
                        }

                        return output;
                    }
                }
            }

            return null;
        }
    }
}
