using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CharacterSheet.Helpers
{
    public class Prepare
    {
        public static string ForStringToEnumConversion(string value)
        {
            Check.ForNullArgument(value, "value");

            StringBuilder resultBuilder = new System.Text.StringBuilder();

            foreach (char c in value)
            {
                // Replace anything, but letters and digits, with space
                if (!Char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString().ToLower();

            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            result = myTI.ToTitleCase(result).Replace(" ", String.Empty);

            return result;
        }



        public static string EnumConvertedToString(string value)
        {
            Check.ForNullArgument(value, "value");

            return Regex.Replace(value, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        }
    }
}
