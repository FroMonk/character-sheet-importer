using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Critical
    {
        public int RangeLower { get; set; }
        public int RangeHigher { get; set; }
        public int Multiplier { get; set; }
        public string AdditionalCritText { get; set; }

        public string Text
        {
            get
            {
                string crit = RangeLower != RangeHigher ? String.Format("{0}-{1}/*{2}", RangeLower, RangeHigher, Multiplier)
                                                        : String.Format("{0}/*{1}", RangeHigher, Multiplier);

                if (string.IsNullOrEmpty(AdditionalCritText))
                {
                    return crit;
                }
                else
                {
                    return crit + AdditionalCritText;
                }
            }
        }

        public Critical(string range, string multiplier)
        {
            Check.ForNullArgument(range, nameof(range));
            Check.ForNullArgument(multiplier, nameof(multiplier));

            if (range.Contains("-"))
            {
                RangeLower = Helpers.Convert.StatToInt(range.Substring(0, range.IndexOf("-")));
                RangeHigher = Helpers.Convert.StatToInt(range.Substring(range.IndexOf("-") + 1));
            }
            else
            {
                RangeLower = 20;
                RangeHigher = 20;
            }
            
            Multiplier = Helpers.Convert.StatToInt(multiplier);
        }

        public Critical(string critical)
        {
            Check.ForNullArgument(critical, "critical");
            AdditionalCritText = "";

            if (critical.Contains("/"))
            {
                string range = critical.Substring(0, critical.IndexOf("/"));
                if (range.Contains("-"))
                {
                    RangeLower = Helpers.Convert.StatToInt(range.Substring(0, range.IndexOf("-")));
                    RangeHigher = Helpers.Convert.StatToInt(range.Substring(range.IndexOf("-") + 1));
                }
                else
                {
                    RangeLower = 20;
                    RangeHigher = 20;
                }
            }
            else
            {
                RangeLower = 20;
                RangeHigher = 20;
            }

            critical = critical.Substring(critical.IndexOf("/") + 1);

            if (critical.Length > 2)
            {
                AdditionalCritText = critical.Substring(2);
                critical = critical.Substring(0, 2);
            }

            Multiplier = Helpers.Convert.StatToInt(critical.Substring(1));
        }

        public bool IsInRange(int toHit)
        {
            return toHit >= RangeLower && toHit <= RangeHigher;
        }
    }
}
