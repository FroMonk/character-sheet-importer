using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using CharacterSheet.Helpers;
using System.Runtime.Serialization;
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Droid.Backend.Pathfinder.Notes;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class CharacterStats
    {


        public string Name { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public Bio Bio { get; set; } = new Bio();
        public AbilityScores AbilityScores { get; set; } = new AbilityScores();
        public AttackBonuses AttackBonuses { get; set; } 
        public CombatManeuvers CombatManeuvers { get; set; } = new CombatManeuvers();
        public Conditions Conditions { get; set; } = new Conditions();
        public Equipment Equipment { get; set; } = new Equipment();
        public Feats Feats { get; set; } = new Feats();
        public Gear Gear { get; set; } = new Gear();
        public SavingThrows SavingThrows { get; set; } = new SavingThrows();
        public Skills Skills { get; set; } = new Skills();
        public Spells Spells { get; set; } = new Spells();
        public Health Health { get; set; }
        public SubdualDamage SubdualDamage { get; set; }
        public Speed Speed { get; set; } = new Speed();
        public AC AC { get; set; }
        public Initiative Initiative { get; set; }
        public Dictionary<string, TrackedResource> TrackedResources { get; set; } = new Dictionary<string, TrackedResource>();
        public List<CharacterStats> Minions { get; set; } = new List<CharacterStats>();
        public List<Note> Notes { get; set; } = new List<Note>();
        public GameSystem GameSystem { get; set; } = GameSystem.Unknown;

        public int Proficiency
        {
            get
            {
                return (Bio.Level / 5) + 2;
            }
        }

        public CharacterStats(string name)
        {
            Check.ForNullArgument(name, "name");

            Name = name;

            AttackBonuses = new AttackBonuses(this);
            Initiative = new Initiative(this);
            Health = new Health(this);
            SubdualDamage = new SubdualDamage(this);
            AC = new AC(this);
        }        
    }
}
