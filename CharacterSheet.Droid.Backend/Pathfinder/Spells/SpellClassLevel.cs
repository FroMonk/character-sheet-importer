using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class SpellClassLevel
    {
        public int Level { get; set; }
        public int Used { get; set; } = 0;
        public int Max { get; set; }

        public bool CanPrepareSpell
        {
            get
            {
                return Spells.Sum(x => x.NumberPrepared) < Max;
            }
        }

        public int Remaining
        {
            get
            {
                return Max - Used;
            }
        }

        public bool CanCast
        {
            get
            {
                return Remaining > 0;
            }
        }
        
        public List<Spell> Spells { get; set; } = new List<Spell>();

        public bool HasPreparedSpells
        {
            get
            {
                return Spells.Any(x => x.NumberPrepared > 0);
            }
        }

        public List<Spell> PreparedSpells
        {
            get
            {
                return Spells.Where(x => x.NumberPrepared > 0 || x.Level == 0).ToList();
            }
        }

        public SpellClassLevel(string level, string maxCasts)
        {
            Check.ForNullArgument(level, "level");
            Check.ForNullArgument(maxCasts, "maxCasts");

            Level = Helpers.Convert.StatToInt(level);

            if (maxCasts.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Max = -1;
            }
            else if (maxCasts.Contains("+"))
            {
                foreach (string value in maxCasts.Split('+'))
                {
                    Max += Helpers.Convert.StatToInt(value);
                }
            }
            else
            {
                Max = Helpers.Convert.StatToInt(maxCasts);
            }
        }

        public void Cast()
        {
            Used += 1;
        }

        public void Reset()
        {
            Used = 0;

            Spells.ForEach(x => x.Reset());
        }

        public void AddSpell(Spell spell)
        {
            spell.SpellClassLevel = this;

            Spells.Add(spell);
        }
    }
}
