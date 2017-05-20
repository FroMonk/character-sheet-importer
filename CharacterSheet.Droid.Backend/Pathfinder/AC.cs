using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class AC : IAdjustable
    {
        public CharacterStats CharacterStats { get; set; }

        public string AdjustmentName { get; } = "AC Temp";

        public int Total
        {
            get
            {
                return Base + ArmorBonus + ShieldBonus + StatBonus + SizeBonus + NaturalArmor + DodgeBonus + DeflectionBonus + MiscBonus + Adjustment;
            }
        }

        public int Touch
        {
            get
            {
                return Base + StatBonus + SizeBonus + DodgeBonus + DeflectionBonus + MiscBonus + Adjustment;
            }
        }

        public int Flat
        {
            get
            {
                return Base + ArmorBonus + ShieldBonus + SizeBonus + NaturalArmor + DodgeBonus + DeflectionBonus + MiscBonus + Adjustment;
            }
        }

        public int Base { get; set; } = 10;

        public int ArmorBonusBase { get; set; }
        public int ArmorBonus
        {
            get
            {
                return GetEquippedArmor() != null ? GetEquippedArmor().ArmorBonus : ArmorBonusBase;
            }

            set
            {
                ArmorBonusBase = value;
            }
        }
        public int ShieldBonus { get; set; }

        public int StatBonus
        {
            get
            {
                return Math.Min(CharacterStats.AbilityScores.Scores[AbilityScoreType.Dexterity].TempModifier, MaxDex);
            }
        }

        public int SizeBonus { get; set; }
        public int NaturalArmor { get; set; }
        public int DodgeBonus { get; set; }
        public int DeflectionBonus { get; set; }
        public int MiscBonus { get; set; }
        public string MissChance { get; set; } = "";

        public int ArcaneFailureBase { get; set; }
        public int ArcaneFailure
        {
            get
            {
                return GetEquippedArmor() != null ? GetEquippedArmor().SpellFailure : ArcaneFailureBase;
            }
        }

        public int ArmorCheckBase { get; set; }
        public int ArmorCheck
        {
            get
            {
                return GetEquippedArmor() != null ? GetEquippedArmor().CheckPenalty : ArmorCheckBase;
            }
        }

        public int MaxDexBase { get; set; }
        public int MaxDex
        {
            get
            {
                return GetEquippedArmor() != null ? GetEquippedArmor().MaxDexBonus : MaxDexBase;
            }
        }

        public int SpellResist { get; set; }
        
        public int Adjustment { get; set; }

        public AC(CharacterStats characterStats)
        {
            Check.ForNullArgument(characterStats, "characterStats");

            CharacterStats = characterStats;
        }

        public void SetBase(string baseValue)
        {
            Base = Helpers.Convert.StatToInt(baseValue);
        }

        public void SetArmorBonus(string armorBonus)
        {
            ArmorBonusBase = Helpers.Convert.StatToInt(armorBonus);
        }

        public void SetShieldBonus(string shieldBonus)
        {
            ShieldBonus = Helpers.Convert.StatToInt(shieldBonus);
        }

        public void SetSizeBonus(string sizeBonus)
        {
            SizeBonus = Helpers.Convert.StatToInt(sizeBonus);
        }

        public void SetNaturalArmor(string naturalArmor)
        {
            NaturalArmor = Helpers.Convert.StatToInt(naturalArmor);
        }

        public void SetDodgeBonus(string dodgeBonus)
        {
            DodgeBonus = Helpers.Convert.StatToInt(dodgeBonus);
        }

        public void SetDeflectionBonus(string deflectionBonus)
        {
            DeflectionBonus = Helpers.Convert.StatToInt(deflectionBonus);
        }

        public void SetMiscBonus(string miscBonus)
        {
            MiscBonus = Helpers.Convert.StatToInt(miscBonus);
        }

        public void SetArcaneFailure(string arcaneFailure)
        {
            ArcaneFailureBase = Helpers.Convert.StatToInt(arcaneFailure);
        }

        public void SetArmorCheck(string armorCheck)
        {
            ArmorCheckBase = Helpers.Convert.StatToInt(armorCheck);
        }

        public void SetMaxDex(string maxDex)
        {
            MaxDexBase = Helpers.Convert.StatToInt(maxDex);
        }

        public void SetSpellResist(string spellResist)
        {
            SpellResist = Helpers.Convert.StatToInt(spellResist);
        }

        private Armor GetEquippedArmor()
        {
            return CharacterStats.Equipment.EquippedArmor;
        }
    }
}
