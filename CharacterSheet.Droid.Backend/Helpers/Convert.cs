using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Helpers
{
    public class Convert
    {
        public static string StatToString(int statValue)
        {
            if (statValue >= 0)
            {
                return string.Format("+{0}", statValue);
            }
            else
            {
                return statValue.ToString();
            }
        }

        public static int StatToInt(string statValue)
        {
            Check.ForNullArgument(statValue, "statValue");

            if (string.IsNullOrEmpty(statValue))
            {
                return 0;
            }

            if (statValue.Substring(0, 1).Equals("+", StringComparison.OrdinalIgnoreCase))
            {
                return int.Parse(statValue.Substring(1));
            }
            else
            {
                return int.Parse(statValue);
            }
        }

        public static int[] AttackToIntArray(string attack)
        {
            Check.ForNullArgument(attack, "attack");

            String[] babAsStrings = attack.Split('/');
            int[] babAsInts = new int[babAsStrings.Length];

            for (int i = 0; i < babAsStrings.Length; i++)
            {
                babAsInts[i] = Helpers.Convert.StatToInt(babAsStrings[i]);
            }

            return babAsInts;
        }

        public static string AttackToString(int[] attackValues)
        {
            Check.ForNullArgument(attackValues, "attackValues");

            string attack = "";

            for (int i = 0; i < attackValues.Length; i++)
            {
                attack += Helpers.Convert.StatToString(attackValues[i]);
                if (i != attackValues.Length - 1)
                {
                    attack += "/";
                }
            }

            return attack;
        }
    }
}
