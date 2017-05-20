using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Spell : IHasDescription
    {
        public SpellClassLevel SpellClassLevel { get; set; }

        public string Name { get; set; }
        public string DC { get; set; } = "";
        public string Save { get; set; } = "";
        public string Duration { get; set; } = "";
        public string Range { get; set; } = "";
        public string Components { get; set; } = "";
        public string SpellResistance { get; set; } = "";
        public string School { get; set; } = "";
        public string Effect { get; set; } = "";
        public string Area { get; set; } = "";
        public string Target { get; set; } = "";
        public string CasterLevel { get; set; } = "";
        public int Level { get; set; }
        public string Description { get; set; } = "";
        public string CastTime { get; set; } = "";
        public string CasterClass { get; set; }
        public string CollectionName { get; set; }
        public int CastsLeft { get; set; } = -1;
        public int NumberPrepared { get; set; } = -1;

        public bool CanCast
        {
            get
            {
                return SpellClassLevel.CanCast && CastsLeft != 0;
            }
        }

        private readonly List<string> _unlimitedCastsTextList = new List<string>
        {
            "at will"
        };

        public Spell(string name, string level, string casterClass)
        {
            Check.ForNullArgument(name, nameof(name));
            Check.ForNullArgument(casterClass, nameof(casterClass));

            Name = name;

            if (level == null)
            {
                Level = 0;
            }
            else
            {
                Level = Helpers.Convert.StatToInt(level);
            }

            CasterClass = casterClass;
            CollectionName = casterClass;
        }

        public bool CastSpell()
        {
            var hasCast = false;

            if (CanCast)
            {
                hasCast = true;

                if (CastsLeft > 0)
                {
                    CastsLeft -= 1;
                }

                SpellClassLevel.Cast();
            }

            return hasCast;
        }

        public void SetCastsLeft(string castsLeft)
        {
            if (castsLeft == null || _unlimitedCastsTextList.Contains(castsLeft.ToLower()))
            {
                CastsLeft = -1;
                NumberPrepared = -1;
            }
            else
            {
                CastsLeft = Helpers.Convert.StatToInt(castsLeft);
                NumberPrepared = CastsLeft;
            }
        }

        public void SetComponents(string components)
        {
            string comps = "";

            foreach (string component in components.Split(','))
            {
                var trimmedComponent = component.Trim();
                comps += trimmedComponent.ToCharArray()[0];
            }

            Components = comps;
        }

        public void Reset()
        {
            if (NumberPrepared != -1 && CastsLeft != -1)
            {
                CastsLeft = NumberPrepared;
            }
        }
    }
}
