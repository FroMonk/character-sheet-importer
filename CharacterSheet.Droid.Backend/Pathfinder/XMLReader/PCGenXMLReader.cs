using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Droid.Backend.Pathfinder.Equipment;
using CharacterSheet.Droid.Backend.Pathfinder.Notes;
using CharacterSheet.Helpers;
using CharacterSheet.Pathfinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CharacterSheet.Pathfinder.XMLReader
{
    public class PcGenXMLReader : ICharacterParser
    {
        private enum Region
        {
            Unknown,
            Basics,
            BasicsAlignment,
            BasicsClasses,
            BasicsDeity,
            BasicsExperience,
            BasicsEyes,
            BasicsHair,
            BasicsFace,
            BasicsGender,
            BasicsHeight,
            BasicsLanguages,
            BasicsMove,
            BasicsSize,
            BasicsVision,
            BasicsWeight,
            BasicsNotes,
            Abilities,
            HitPoints,
            ArmorClass,
            Initiative,
            Skills,
            SavingThrows,
            Attack,
            Weapons,
            WeaponsUnarmed,
            WeaponsNaturalAttack,
            WeaponsWeapon,
            WeaponsWeaponCritical,
            WeaponsWeaponMeleeW1H1P,
            WeaponsWeaponMeleeW1H1O,
            WeaponsWeaponMeleeW1H2,
            WeaponsWeaponMeleeW2POH,
            WeaponsWeaponMeleeW2POL,
            WeaponsWeaponMeleeW2O,
            WeaponsWeaponRanges,
            WeaponsWeaponRangesAmmunition,
            Protection,
            ClassFeatures,
            ClassFeatureKiPool,
            ClassFeaturesStunningFist,
            ClassFeaturesLayOnHands,
            ClassFeaturesPsionics,
            ClassFeaturesWildShape,
            CheckLists,
            Equipment,
            EquipmentTotal,
            Feats,
            SpecialQualities,
            SpecialAttacks,
            Traits,
            Spells,
            SpellBook,
            MemorizedSpells
        }

        private const string NON_STANDARD_MELEE = "non-standard-melee";
        private const string SLING = "sling";
        private const string FORTITUDE = "fortitude";
        private const string REFLEX = "reflex";
        private const string WILL = "will";
        private const string STRENGTH = "strength";
        private const string DEXTERITY = "dexterity";
        private const string CONSTITUTION = "constitution";
        private const string INTELLIGENCE = "intelligence";
        private const string WISDOM = "wisdom";
        private const string CHARISMA = "charisma";
        private const string TRUE = "true";

        // Tags
        private const string BASICS_TAG = "basics";
        private const string NAME_TAG = "name";
        private const string PLAYER_NAME_TAG = "playername";
        private const string CHARACTER_TYPE_TAG = "charactertype";
        private const string HERO_POINTS_TAG = "heropoints";
        private const string AGE_TAG = "age";
        private const string ALIGNMENT_TAG = "alignment";
        private const string CLASSES_TAG = "classes";
        private const string LEVELS_TOTAL_TAG = "levels_total";
        private const string DEITY_TAG = "deity";
        private const string EXPERIENCE_TAG = "experience";
        private const string NEXT_LEVEL_TAG = "next_level";
        private const string EYES_TAG = "eyes";
        private const string COLOR_TAG = "color";
        private const string HAIR_TAG = "hair";
        private const string CR_TAG = "cr";
        private const string SHORT_TAG = "short";
        private const string GENDER_TAG = "gender";
        private const string HEIGHT_TAG = "height";
        private const string TOTAL_TAG = "total";
        private const string LANGUAGE_TAG = "language";
        private const string RATE_TAG = "rate";
        private const string RACE_TAG = "race";
        private const string REACH_TAG = "reach";
        private const string LONG_TAG = "long";
        private const string VISION_TAG = "vision";
        private const string WEIGHT_UNIT_TAG = "weight_unit";
        private const string CURRENT_TAG = "current";
        private const string ABILITIES_TAG = "abilities";
        private const string SCORE_TAG = "score";
        private const string NO_EQUIP_TAG = "noequip";
        private const string NO_EQUIP_MOD_TAG = "noequip_mod";
        private const string HIT_POINTS_TAG = "hit_points";
        private const string POINTS_TAG = "points";
        private const string ARMOR_CLASS_TAG = "armor_class";
        private const string BASE_TAG = "base";
        private const string ARMOR_BONUS_TAG = "armor_bonus";
        private const string SHIELD_BONUS_TAG = "shield_bonus";
        private const string NATURAL_TAG = "natural";
        private const string DEFLECTION_TAG = "deflection";
        private const string DODGE_BONUS_TAG = "dodge_bonus";
        private const string MISS_CHANCE_TAG = "miss_chance";
        private const string SPELL_FAILURE_TAG = "spell_failure";
        private const string CHECK_PENALTY_TAG = "check_penalty";
        private const string INITIATIVE_TAG = "initiative";
        private const string SKILLS_TAG = "skills";
        private const string SKILL_TAG = "skill";
        private const string RANKS_TAG = "ranks";
        private const string SKILL_MOD_TAG = "skill_mod";
        private const string ABILITY_MOD_TAG = "ability_mod";
        private const string UNTRAINED_TAG = "untrained";
        private const string SAVING_THROWS_TAG = "saving_throws";
        private const string DESCRIPTION_TAG = "description";
        private const string MAGIC_MOD_TAG = "magic_mod";
        private const string EPIC_MOD_TAG = "epic_mod";
        private const string ATTACK_TAG = "attack";
        private const string MELEE_TAG = "melee";
        private const string BAB_TAG = "base_attack_bonus";
        private const string SIZE_MOD_TAG = "size_mod";
        private const string MISC_MOD_TAG = "misc_mod";
        private const string RANGED_TAG = "ranged";
        private const string WEAPONS_TAG = "weapons";
        private const string CRITICAL_TAG = "critical";
        private const string TYPE_TAG = "type";
        private const string WEAPON_TAG = "weapon";
        private const string CATEGORY_TAG = "category";
        private const string OUTPUT_TAG = "output";
        private const string RANGE_TAG = "range";
        private const string TOTAL_HIT_TAG = "total_hit";
        private const string HAND_TAG = "hand";
        private const string REACH_UNIT_TAG = "reachunit";
        private const string SIZE_TAG = "size";
        private const string SPECIAL_PROPERTIES_TAG = "special_properties";
        private const string WEIGHT_TAG = "weight";
        private const string IS_LIGHT_TAG = "islight";
        private const string RANGES_TAG = "ranges";
        private const string DISTANCE_TAG = "distance";
        private const string DAMAGE_TAG = "damage";
        private const string SIMPLE_TAG = "simple";
        private const string W1_H1_P_TAG = "w1_h1_p";
        private const string W1_H1_O_TAG = "w1_h1_o";
        private const string W1_H2_TAG = "w1_h2";
        private const string W2_P_OH_TAG = "w2_p_oh";
        private const string W2_P_OL_TAG = "w2_p_ol";
        private const string W2_O_TAG = "w2_o";
        private const string CMB_TAG = "cmb";
        private const string GRAPPLE_ATTACK_TAG = "grapple_attack";
        private const string TRIP_ATTACK_TAG = "trip_attack";
        private const string DISARM_ATTACK_TAG = "disarm_attack";
        private const string SUNDER_ATTACK_TAG = "sunder_attack";
        private const string BULLRUSH_ATTACK_TAG = "bullrush_attack";
        private const string OVERRUN_ATTACK_TAG = "overrun_attack";
        private const string GRAPPLE_DEFENSE_TAG = "grapple_defense";
        private const string TRIP_DEFENSE_TAG = "trip_defense";
        private const string DISARM_DEFENSE_TAG = "disarm_defense";
        private const string SUNDER_DEFENSE_TAG = "sunder_defense";
        private const string BULLRUSH_DEFENSE_TAG = "bullrush_defense";
        private const string OVERRUN_DEFENSE_TAG = "overrun_defense";
        private const string UNARMED_TAG = "unarmed";
        private const string NATURAL_ATTACK_TAG = "naturalattack";
        private const string NOTES_TAG = "notes";
        private const string AMMUNITION_TAG = "ammunition";
        private const string CLASS_TAG = "class";
        private const string PROTECTION_TAG = "protection";
        private const string ARMOR_TAG = "armor";
        private const string AC_BONUS_TAG = "acbonus";
        private const string AC_CHECK_TAG = "accheck";
        private const string MAX_DEX_TAG = "maxdex";
        private const string MOVE_TAG = "move";
        private const string SPELL_FAIL_TAG = "spellfail";
        private const string LOCATION_TAG = "location";
        private const string SHIELD_TAG = "shield";
        private const string ITEM_TAG = "item";
        private const string CLASS_FEATURES_TAG = "class_features";
        private const string USES_PER_DAY_TAG = "uses_per_day";
        private const string LEVEL_TAG = "level";
        private const string KI_POOL_TAG = "ki_pool";
        private const string STUNNING_FIST_TAG = "stunning_fist";
        private const string PSIONICS_TAG = "psionics";
        private const string WILD_SHAPE_TAG = "wildshape";
        private const string TOTAL_PP_TAG = "total_pp";
        private const string CHECKLISTS_TAG = "checklists";
        private const string CHECKLIST_TAG = "checklist";
        private const string HEADER_TAG = "header";
        private const string CHECK_COUNT_TAG = "check_count";
        private const string EQUIPMENT_TAG = "equipment";
        private const string COST_TAG = "cost";
        private const string QUANTITY_TAG = "quantity";
        private const string TOTAL_WEIGHT_TAG = "total_weight";
        private const string LOAD_TAG = "load";
        private const string VALUE_TAG = "value";
        private const string ABILITY_TAG = "ability";
        private const string FEATS_TAG = "feats";
        private const string FEAT_TAG = "feat";
        private const string BENEFIT_TAG = "benefit";
        private const string SPECIAL_QUALITIES_TAG = "special_qualities";
        private const string SPECIAL_ATTACKS_TAG = "special_attacks";
        private const string EFFECT_TAG = "effect";
        private const string TRAITS_TAG = "traits";
        private const string TRAIT_TAG = "trait";
        private const string WEAPON_PROFICIENCIES_TAG = "weapon_proficiencies";
        private const string LANGUAGES_TAG = "languages";
        private const string MISC_TAG = "misc";
        private const string GOLD_TAG = "gold";
        private const string MULTIPLIER_TAG = "multiplier";
        private const string THREAT_TAG = "threat";
        private const string DISTANCE_UNIT_TAG = "distance_unit";
        private const string TO_HIT_TAG = "to_hit";
        private const string SPELLS_TAG = "spells";
        private const string SPELLS_INNATE_TAG = "spells_innate";
        private const string TIMES_MEMORIZED_TAG = "times_memorized";
        private const string COMPONENTS_TAG = "components";
        private const string CASTING_TIME_TAG = "castingtime";
        private const string CASTER_LEVEL_TAG = "casterlevel";
        private const string TIMES_UNIT_TAG = "times_unit";
        private const string DC_TAG = "dc";
        private const string DURATION_TAG = "duration";
        private const string TARGET_TAG = "target";
        private const string SAVE_INFO_TAG = "saveinfo";
        private const string SCHOOL_TAG = "school";
        private const string SUBSCHOOL_TAG = "subschool";
        private const string FULL_SCHOOL_TAG = "fullschool";
        private const string SPELL_RESISTANCE_TAG = "spell_resistance";
        private const string SPELL_BOOK_TAG = "spellbook";
        private const string SPECIAL_ATTACK_TAG = "special_attack";
        private const string SPECIAL_QUALITY_TAG = "special_quality";
        private const string SPELL_TAG = "spell";
        private const string MEMORIZED_SPELLS_TAG = "memorized_spells";
        // new tags need adding to list

        // Attributes
        private const string NAME_ATTRIBUTE = "name";
        private const string SPELL_LIST_CLASS_ATTRIBUTE = "spelllistclass";
        private const string SPELL_CASTER_TYPE_ATTRIBUTE = "spellcastertype";
        private const string SPELL_CASTER_LEVEL_ATTRIBUTE = "spellcasterlevel";
        private const string NUMBER_ATTRIBUTE = "number";
        private const string CAST_ATTRIBUTE = "cast";
        private const string MEMORIZE_ATTRIBUTE = "memorize";

        private readonly List<string> _tags = new List<string>
        {
            BASICS_TAG,
            NAME_TAG,
            PLAYER_NAME_TAG,
            CHARACTER_TYPE_TAG,
            HERO_POINTS_TAG,
            AGE_TAG,
            ALIGNMENT_TAG,
            CLASSES_TAG,
            LEVELS_TOTAL_TAG,
            DEITY_TAG,
            EXPERIENCE_TAG,
            NEXT_LEVEL_TAG,
            EYES_TAG,
            COLOR_TAG,
            HAIR_TAG,
            CR_TAG,
            SHORT_TAG,
            GENDER_TAG,
            HEIGHT_TAG,
            TOTAL_TAG,
            LANGUAGE_TAG,
            RATE_TAG,
            RACE_TAG,
            REACH_TAG,
            LONG_TAG,
            VISION_TAG,
            WEIGHT_UNIT_TAG,
            CURRENT_TAG,
            ABILITIES_TAG,
            SCORE_TAG,
            NO_EQUIP_TAG,
            NO_EQUIP_MOD_TAG,
            HIT_POINTS_TAG,
            POINTS_TAG,
            ARMOR_CLASS_TAG,
            BASE_TAG,
            ARMOR_BONUS_TAG,
            SHIELD_BONUS_TAG,
            NATURAL_TAG,
            DEFLECTION_TAG,
            DODGE_BONUS_TAG,
            MISS_CHANCE_TAG,
            SPELL_FAILURE_TAG,
            CHECK_PENALTY_TAG,
            INITIATIVE_TAG,
            SKILLS_TAG,
            SKILL_TAG,
            RANKS_TAG,
            SKILL_MOD_TAG,
            ABILITY_MOD_TAG,
            UNTRAINED_TAG,
            SAVING_THROWS_TAG,
            DESCRIPTION_TAG,
            MAGIC_MOD_TAG,
            EPIC_MOD_TAG,
            ATTACK_TAG,
            MELEE_TAG,
            BAB_TAG,
            SIZE_MOD_TAG,
            MISC_MOD_TAG,
            RANGED_TAG,
            WEAPONS_TAG,
            CRITICAL_TAG,
            TYPE_TAG,
            WEAPON_TAG,
            CATEGORY_TAG,
            OUTPUT_TAG,
            RANGE_TAG,
            TOTAL_HIT_TAG,
            HAND_TAG,
            REACH_UNIT_TAG,
            SIZE_TAG,
            SPECIAL_PROPERTIES_TAG,
            WEIGHT_TAG,
            IS_LIGHT_TAG,
            RANGES_TAG,
            DISTANCE_TAG,
            DAMAGE_TAG,
            SIMPLE_TAG,
            W1_H1_P_TAG,
            W1_H1_O_TAG,
            W1_H2_TAG,
            W2_P_OH_TAG,
            W2_P_OL_TAG,
            W2_O_TAG,
            CMB_TAG,
            GRAPPLE_ATTACK_TAG,
            TRIP_ATTACK_TAG,
            DISARM_ATTACK_TAG,
            SUNDER_ATTACK_TAG,
            BULLRUSH_ATTACK_TAG,
            OVERRUN_ATTACK_TAG,
            GRAPPLE_DEFENSE_TAG,
            TRIP_DEFENSE_TAG,
            DISARM_DEFENSE_TAG,
            SUNDER_DEFENSE_TAG,
            BULLRUSH_DEFENSE_TAG,
            OVERRUN_DEFENSE_TAG,
            UNARMED_TAG,
            NATURAL_ATTACK_TAG,
            NOTES_TAG,
            AMMUNITION_TAG,
            CLASS_TAG,
            PROTECTION_TAG,
            ARMOR_TAG,
            AC_BONUS_TAG,
            AC_CHECK_TAG,
            MAX_DEX_TAG,
            MOVE_TAG,
            SPELL_FAIL_TAG,
            LOCATION_TAG,
            SHIELD_TAG,
            ITEM_TAG,
            CLASS_FEATURES_TAG,
            USES_PER_DAY_TAG,
            LEVEL_TAG,
            KI_POOL_TAG,
            STUNNING_FIST_TAG,
            PSIONICS_TAG,
            WILD_SHAPE_TAG,
            TOTAL_PP_TAG,
            CHECKLISTS_TAG,
            CHECKLIST_TAG,
            HEADER_TAG,
            CHECK_COUNT_TAG,
            EQUIPMENT_TAG,
            COST_TAG,
            QUANTITY_TAG,
            TOTAL_WEIGHT_TAG,
            LOAD_TAG,
            VALUE_TAG,
            ABILITY_TAG,
            FEATS_TAG,
            FEAT_TAG,
            BENEFIT_TAG,
            SPECIAL_QUALITIES_TAG,
            SPECIAL_ATTACKS_TAG,
            EFFECT_TAG,
            TRAITS_TAG,
            TRAIT_TAG,
            WEAPON_PROFICIENCIES_TAG,
            LANGUAGES_TAG,
            MISC_TAG,
            GOLD_TAG,
            MULTIPLIER_TAG,
            THREAT_TAG,
            DISTANCE_UNIT_TAG,
            TO_HIT_TAG,
            SPELLS_TAG,
            SPELLS_INNATE_TAG,
            TIMES_MEMORIZED_TAG,
            COMPONENTS_TAG,
            CASTING_TIME_TAG,
            CASTER_LEVEL_TAG,
            TIMES_UNIT_TAG,
            DC_TAG,
            DURATION_TAG,
            TARGET_TAG,
            SAVE_INFO_TAG,
            SCHOOL_TAG,
            SUBSCHOOL_TAG,
            FULL_SCHOOL_TAG,
            SPELL_RESISTANCE_TAG,
            SPELL_BOOK_TAG,
            SPECIAL_ATTACK_TAG,
            SPECIAL_QUALITY_TAG,
            SPELL_TAG,
            MEMORIZED_SPELLS_TAG
        };

        private Dictionary<string, AttackType> _meleeAttackTypeMap = new Dictionary<string, AttackType>
        {
            [W1_H1_P_TAG] = AttackType.Melee1HP,
            [W1_H1_O_TAG] = AttackType.Melee1HO,
            [W1_H2_TAG] = AttackType.Melee2H,
            [W2_P_OH_TAG] = AttackType.Melee2WPOH,
            [W2_P_OL_TAG] = AttackType.Melee2WPOL,
            [W2_O_TAG] = AttackType.Melee2WOH
        };

        private Dictionary<string, string> _meleeAttackMap = new Dictionary<string, string>
        {
            [W1_H1_P_TAG] = "1H-P",
            [W1_H1_O_TAG] = "1H-O",
            [W1_H2_TAG] = "2H",
            [W2_P_OH_TAG] = "2W-P-(OH)",
            [W2_P_OL_TAG] = "2W-P-(OL)",
            [W2_O_TAG] = "2W-OH"
        };

        private CharacterStats _tempCharacterStats;
        private AttackBonus _tempAttackBonus;
        private AbilityScore _tempAbilityScore;
        private SavingThrow _tempSavingThrow;
        private CharacterClass _tempCharacterClass;
        private Feat _tempFeat;
        private Skill _tempSkill;
        private Weapon _tempWeapon;
        private string _tempToHit;
        private Armor _tempArmor;
        private Item _tempItem;
        private Spell _tempSpell;
        private SpellClass _tempSpellClass;
        private SpellClassLevel _tempSpellClassLevel;
        private SpellBook _tempSpellBook;
        private string _tempCritRange;
        private Melee _tempMelee;
        private Ranged _tempRanged;
        private string _tempDistance;
        private string _tempSubAbilityName;
        private TrackedResource _tempTrackedResource;
        private string _tempTrackedResourceName;
        private string _tempTrackedResourceDescription;
        private string _tempSpellCollectionName;
        private string _tempSpellLevel;
        private Ammunition _tempAmmunition;

        private XmlReader _xpp;

        private AbilityScores _abilityScores;
        private AC _ac;
        private AttackBonuses _attackBonuses;
        private Bio _bio;
        private CombatManeuvers _combatManeuvers;
        private Conditions _conditions;
        private Equipment _equipment;
        private Feats _feats;
        private Gear _gear;
        private Health _health;
        private Initiative _initiative;
        private SavingThrows _savingThrows;
        private Skills _skills;
        private Speed _speed;
        private Spells _spells;
        private Dictionary<string, TrackedResource> _trackedResources;

        private Region _region;

        private string _tagName;
        private string _retrievedText;
        private bool _mustPrepareSpells;

        public CharacterStats parseCharacterXML(Stream inputStream)
        {
            Check.ForNullArgument(inputStream, "inputStream");

            _tagName = null;
            _region = Region.Unknown;

            XmlNodeType eventType;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            using (_xpp = XmlReader.Create(RemoveInvalidCharacters(inputStream), readerSettings))
            {
                while (_xpp.Read())
                {
                    eventType = _xpp.NodeType;

                    switch (eventType)
                    {
                        case XmlNodeType.Element:
                            ProcessStartTag();
                            break;
                        case XmlNodeType.EndElement:
                            ProcessEndTag();
                            break;
                        case XmlNodeType.Text:
                            ProcessText();
                            break;
                    }
                }
            }

            if (_tempCharacterStats.SavingThrows.Throws.ContainsKey(SavingThrowType.Strength))
            {
                _tempCharacterStats.GameSystem = GameSystem.D20FifthEdition;
            }
            else if (_tempCharacterStats.CombatManeuvers.Maneuvers.Any())
            {
                _tempCharacterStats.GameSystem = GameSystem.Pathfinder;
            }
            else
            {
                _tempCharacterStats.GameSystem = GameSystem.D20System;
            }

            return _tempCharacterStats;
        }

        private void ProcessText()
        {
            if (_tags.Contains(_tagName) && _region != Region.Unknown)
            {
                if (PrepareText())
                {
                    switch (_tagName)
                    {
                        case NAME_TAG:
                            NameText();
                            break;
                        case PLAYER_NAME_TAG:
                            PlayerNameText();
                            break;
                        case CHARACTER_TYPE_TAG:
                            CharacterTypeText();
                            break;
                        case HERO_POINTS_TAG:
                            HeroPointsText();
                            break;
                        case AGE_TAG:
                            AgeText();
                            break;
                        case LONG_TAG:
                            LongText();
                            break;
                        case BAB_TAG:
                            BabText();
                            break;
                        case LEVEL_TAG:
                            LevelText();
                            break;
                        case LEVELS_TOTAL_TAG:
                            LevelsTotalText();
                            break;
                        case DESCRIPTION_TAG:
                            DescriptionText();
                            break;
                        case CURRENT_TAG:
                            CurrentText();
                            break;
                        case NEXT_LEVEL_TAG:
                            NextLevelText();
                            break;
                        case COLOR_TAG:
                            ColorText();
                            break;
                        case CR_TAG:
                            CrText();
                            break;
                        case SHORT_TAG:
                            ShortText();
                            break;
                        case TOTAL_TAG:
                            TotalText();
                            break;
                        case LANGUAGE_TAG:
                            LanguageText();
                            break;
                        case RATE_TAG:
                            RateText();
                            break;
                        case RACE_TAG:
                            RaceText();
                            break;
                        case VISION_TAG:
                            VisionText();
                            break;
                        case GOLD_TAG:
                            GoldText();
                            break;
                        case WEIGHT_UNIT_TAG:
                            WeightUnitText();
                            break;
                        case VALUE_TAG:
                            ValueText();
                            break;
                        case NO_EQUIP_TAG:
                            NoEquipText();
                            break;
                        case NO_EQUIP_MOD_TAG:
                            NoEquipModText();
                            break;
                        case SCORE_TAG:
                            ScoreText();
                            break;
                        case POINTS_TAG:
                            PointsText();
                            break;
                        case BASE_TAG:
                            BaseText();
                            break;
                        case ARMOR_BONUS_TAG:
                            ArmorBonusText();
                            break;
                        case SHIELD_BONUS_TAG:
                            ShieldBonusText();
                            break;
                        case SIZE_MOD_TAG:
                            SizeModText();
                            break;
                        case NATURAL_TAG:
                            NaturalText();
                            break;
                        case DEFLECTION_TAG:
                            DeflectionText();
                            break;
                        case DODGE_BONUS_TAG:
                            DodgeBonusText();
                            break;
                        case MISC_TAG:
                            MiscText();
                            break;
                        case MISS_CHANCE_TAG:
                            MissChanceText();
                            break;
                        case MAX_DEX_TAG:
                            MaxDexText();
                            break;
                        case SPELL_FAILURE_TAG:
                            SpellFailureText();
                            break;
                        case CHECK_PENALTY_TAG:
                            CheckPenaltyText();
                            break;
                        case SPELL_RESISTANCE_TAG:
                            SpellResistanceText();
                            break;
                        case MISC_MOD_TAG:
                            MiscModText();
                            break;
                        case RANKS_TAG:
                            RanksText();
                            break;
                        case ABILITY_TAG:
                            AbilityText();
                            break;
                        case UNTRAINED_TAG:
                            UntrainedText();
                            break;
                        case MAGIC_MOD_TAG:
                            MagicModText();
                            break;
                        case EPIC_MOD_TAG:
                            EpicModText();
                            break;
                        case GRAPPLE_ATTACK_TAG:
                            CombatManeuverAttackText("Grapple");
                            break;
                        case TRIP_ATTACK_TAG:
                            CombatManeuverAttackText("Trip");
                            break;
                        case DISARM_ATTACK_TAG:
                            CombatManeuverAttackText("Disarm");
                            break;
                        case SUNDER_ATTACK_TAG:
                            CombatManeuverAttackText("Sunder");
                            break;
                        case BULLRUSH_ATTACK_TAG:
                            CombatManeuverAttackText("Bullrush");
                            break;
                        case OVERRUN_ATTACK_TAG:
                            CombatManeuverAttackText("Overrun");
                            break;
                        case GRAPPLE_DEFENSE_TAG:
                            CombatManeuverDefenseText("Grapple");
                            break;
                        case TRIP_DEFENSE_TAG:
                            CombatManeuverDefenseText("Trip");
                            break;
                        case DISARM_DEFENSE_TAG:
                            CombatManeuverDefenseText("Disarm");
                            break;
                        case SUNDER_DEFENSE_TAG:
                            CombatManeuverDefenseText("Sunder");
                            break;
                        case BULLRUSH_DEFENSE_TAG:
                            CombatManeuverDefenseText("Bullrush");
                            break;
                        case OVERRUN_DEFENSE_TAG:
                            CombatManeuverDefenseText("Overrun");
                            break;
                        case DAMAGE_TAG:
                            DamageText();
                            break;
                        case CATEGORY_TAG:
                            CategoryText();
                            break;
                        case CRITICAL_TAG:
                            CriticalText();
                            break;
                        case REACH_TAG:
                            ReachText();
                            break;
                        case TYPE_TAG:
                            TypeText();
                            break;
                        case TO_HIT_TAG:
                            ToHitText();
                            break;
                        case RANGE_TAG:
                            RangeText();
                            break;
                        case MULTIPLIER_TAG:
                            MultiplierText();
                            break;
                        case THREAT_TAG:
                            ThreatText();
                            break;
                        case REACH_UNIT_TAG:
                        case DISTANCE_UNIT_TAG:
                            DistanceUnitText();
                            break;
                        case OUTPUT_TAG:
                            OutputText();
                            break;
                        case TOTAL_HIT_TAG:
                            TotalHitText();
                            break;
                        case HAND_TAG:
                            HandText();
                            break;
                        case SIZE_TAG:
                            SizeText();
                            break;
                        case SPECIAL_PROPERTIES_TAG:
                            SpecialPropertiesText();
                            break;
                        case IS_LIGHT_TAG:
                            IsLightText();
                            break;
                        case DISTANCE_TAG:
                            DistanceText();
                            break;
                        case AC_BONUS_TAG:
                            AcBonusText();
                            break;
                        case AC_CHECK_TAG:
                            AcCheckText();
                            break;
                        case SPELL_FAIL_TAG:
                            SpellFailText();
                            break;
                        case HEADER_TAG:
                            HeaderText();
                            break;
                        case CHECK_COUNT_TAG:
                            CheckCountText();
                            break;
                        case TOTAL_PP_TAG:
                        case USES_PER_DAY_TAG:
                            UsesPerDayText();
                            break;
                        case LOAD_TAG:
                            LoadText();
                            break;
                        case COST_TAG:
                            CostText();
                            break;
                        case LOCATION_TAG:
                            LocationText();
                            break;
                        case QUANTITY_TAG:
                            QuantityText();
                            break;
                        case TOTAL_WEIGHT_TAG:
                            TotalWeightText();
                            break;
                        case BENEFIT_TAG:
                            BenefitText();
                            break;
                        case WEAPON_PROFICIENCIES_TAG:
                            WeaponProficienciesText();
                            break;
                        case TIMES_MEMORIZED_TAG:
                            TimesMemorizedText();
                            break;
                        case COMPONENTS_TAG:
                            ComponentsText();
                            break;
                        case CASTING_TIME_TAG:
                            CastingTimeText();
                            break;
                        case CASTER_LEVEL_TAG:
                            CasterLevelText();
                            break;
                        case DC_TAG:
                            DcText();
                            break;
                        case DURATION_TAG:
                            DurationText();
                            break;
                        case EFFECT_TAG:
                            EffectText();
                            break;
                        case TARGET_TAG:
                            TargetText();
                            break;
                        case SAVE_INFO_TAG:
                            SaveInfoText();
                            break;
                        case FULL_SCHOOL_TAG:
                            FullSchoolText();
                            break;
                    }
                }
            }
        }

        private void FullSchoolText()
        {
            _tempSpell.School = _retrievedText;
        }

        private void SaveInfoText()
        {
            _tempSpell.Save = _retrievedText;
        }

        private void TargetText()
        {
            switch (_region)
            {
                case Region.Spells:
                    SpellTargetText();
                    break;
            }
        }

        private void SpellTargetText()
        {
            _tempSpell.Target = _retrievedText;
        }

        private void EffectText()
        {
            switch (_region)
            {
                case Region.Spells:
                    SpellEffectText();
                    break;
            }
        }

        private void SpellEffectText()
        {
            _tempSpell.Effect = _retrievedText;
        }

        private void DurationText()
        {
            switch (_region)
            {
                case Region.Spells:
                    _tempSpell.Duration = _retrievedText;
                    break;
            }
        }

        private void DcText()
        {
            switch (_region)
            {
                case Region.Spells:
                    SpellDcText();
                    break;
            }
        }

        private void SpellDcText()
        {
            _tempSpell.DC = _retrievedText;
        }

        private void CasterLevelText()
        {
            _tempSpell.CasterLevel = _retrievedText;
        }

        private void CastingTimeText()
        {
            _tempSpell.CastTime = _retrievedText;
        }

        private void ComponentsText()
        {
            _tempSpell.Components = _retrievedText;
        }

        private void TimesMemorizedText()
        {
            var castsLeft = "0";

            if (!_mustPrepareSpells || _tempSpell.Level == 0)
            {
                castsLeft = "-1";
            }

            _tempSpell.SetCastsLeft(castsLeft);
        }

        private void WeaponProficienciesText()
        {
            _bio.WeaponProficiencies = _retrievedText;
        }

        private void BenefitText()
        {
            _tempFeat.Description = _retrievedText;
        }

        private void TotalWeightText()
        {
            _tempItem.SetWeight(_retrievedText);
        }

        private void QuantityText()
        {
            switch (_region)
            {
                case Region.Equipment:
                    ItemQuantityText();
                    break;
                case Region.WeaponsWeaponRangesAmmunition:
                    AmmunitionQuantityText();
                    break;
            }
        }

        private void AmmunitionQuantityText()
        {
            _tempAmmunition.SetRemaining(_retrievedText);
        }

        private void ItemQuantityText()
        {
            _tempItem.SetQty(_retrievedText);
        }

        private void LocationText()
        {
            switch (_region)
            {
                case Region.Equipment:
                    ItemLocationText();
                    break;
            }
        }

        private void ItemLocationText()
        {
            _tempItem.Location = _retrievedText;
        }

        private void CostText()
        {
            switch (_region)
            {
                case Region.Equipment:
                    ItemCostText();
                    break;
            }
        }

        private void ItemCostText()
        {
            _tempItem.SetCost(_retrievedText);
        }

        private void LoadText()
        {
            _gear.EncumbranceLevel = _retrievedText;
        }

        private void UsesPerDayText()
        {
            switch (_region)
            {
                case Region.ClassFeatureKiPool:
                    KiPoolUsesPerDayText();
                    break;
                case Region.ClassFeaturesStunningFist:
                    StunningFistUsesPerDayText();
                    break;
                case Region.ClassFeaturesPsionics:
                    PsionicsUsesPerDayText();
                    break;
                case Region.ClassFeaturesWildShape:
                    WildShapeUsesPerDayText();
                    break;
            }
        }

        private void WildShapeUsesPerDayText()
        {
            _tempTrackedResource = new TrackedResource("Wildshape", _retrievedText, "0");
        }

        private void PsionicsUsesPerDayText()
        {
            _tempTrackedResource = new TrackedResource("Psionics", _retrievedText, "0");
        }

        private void StunningFistUsesPerDayText()
        {
            _tempTrackedResource = new TrackedResource("Stunning Fist", _retrievedText, "0");
        }

        private void KiPoolUsesPerDayText()
        {
            _tempTrackedResource = new TrackedResource("Ki Pool", _retrievedText, "0");
        }

        private void CheckCountText()
        {
            _tempTrackedResource = new TrackedResource(_tempTrackedResourceName, _retrievedText, "0");
            _tempTrackedResource.Notes.Add(_tempTrackedResourceDescription);
        }

        private void HeaderText()
        {
            switch (_region)
            {
                case Region.CheckLists:
                    ChecklistHeaderText();
                    break;
            }
        }

        private void ChecklistHeaderText()
        {
            _tempTrackedResourceName = _retrievedText;
        }

        private void SpellFailText()
        {
            _tempArmor.SetSpellFailure(_retrievedText);
        }

        private void AcCheckText()
        {
            switch (_region)
            {
                case Region.Protection:
                    ArmorAcCheckText();
                    break;
            }
        }

        private void ArmorAcCheckText()
        {
            _tempArmor.SetCheckPenalty(_retrievedText);
        }

        private void AcBonusText()
        {
            _tempArmor.SetArmorBonus(_retrievedText);
        }

        private void DistanceText()
        {
            _tempDistance = _retrievedText;
        }

        private void IsLightText()
        {
            _tempWeapon.IsLightWeapon = bool.Parse(_retrievedText.ToLower());
        }

        private void SpecialPropertiesText()
        {
            switch (_region)
            {
                case Region.WeaponsWeapon:
                    SetDescription(_tempWeapon);
                    break;
                case Region.WeaponsWeaponRanges:
                    SetDescription(_tempAmmunition);
                    break;
                case Region.Protection:
                    SetDescription(_tempArmor);
                    break;
                case Region.Equipment:
                    SetDescription(_tempItem);
                    break;
            }
        }

        private void SetDescription(IHasDescription item)
        {
            item.Description = _retrievedText;
        }

        private void HandText()
        {
            _tempWeapon.Hand = _retrievedText;
        }

        private void TotalHitText()
        {
            switch (_region)
            {
                case Region.WeaponsWeapon:
                    WeaponHitText();
                    break;
            }
        }

        private void WeaponHitText()
        {
            _tempToHit = _retrievedText;
        }

        private void OutputText()
        {
            switch (_region)
            {
                case Region.WeaponsWeapon:
                    WeaponOutputText();
                    break;
            }
        }

        private void WeaponOutputText()
        {
            _tempWeapon = new Weapon(_retrievedText);
        }

        private void DistanceUnitText()
        {
            _tempWeapon.Reach = String.Format("{0} {1}", _tempWeapon.Reach, _retrievedText);
        }

        private void ThreatText()
        {
            _tempWeapon.Critical = new Critical(_retrievedText.Replace(" ", ""));
        }

        private void MultiplierText()
        {
            switch (_region)
            {
                case Region.WeaponsWeaponCritical:
                    CritMultiplierText();
                    break;
            }
        }

        private void CritMultiplierText()
        {
            _tempWeapon.Critical = new Critical(_tempCritRange, _retrievedText);
        }

        private void RangeText()
        {
            switch (_region)
            {
                case Region.WeaponsWeaponCritical:
                    CritRangeText();
                    break;
                case Region.Spells:
                    SpellRangeText();
                    break;
            }
        }

        private void SpellRangeText()
        {
            _tempSpell.Range = _retrievedText;
        }

        private void CritRangeText()
        {
            _tempCritRange = _retrievedText;
        }

        private void ToHitText()
        {
            switch (_region)
            {
                case Region.WeaponsWeaponMeleeW1H1P:
                case Region.WeaponsWeaponMeleeW1H1O:
                case Region.WeaponsWeaponMeleeW1H2:
                case Region.WeaponsWeaponMeleeW2POH:
                case Region.WeaponsWeaponMeleeW2POL:
                case Region.WeaponsWeaponMeleeW2O:
                case Region.WeaponsNaturalAttack:
                case Region.WeaponsWeaponRanges:
                    WeaponToHitText();
                    break;
            }
        }

        private void WeaponToHitText()
        {
            _tempToHit = _retrievedText;
        }

        private void TypeText()
        {
            switch (_region)
            {
                case Region.WeaponsNaturalAttack:
                case Region.WeaponsUnarmed:
                case Region.WeaponsWeapon:
                    WeaponTypeText();
                    break;
            }
        }

        private void WeaponTypeText()
        {
            _tempWeapon.Type = _retrievedText;
        }

        private void ReachText()
        {
            switch (_region)
            {
                case Region.WeaponsNaturalAttack:
                case Region.WeaponsUnarmed:
                case Region.WeaponsWeapon:
                    WeaponReachText();
                    break;
            }
        }

        private void WeaponReachText()
        {
            _tempWeapon.Reach = _retrievedText;
        }

        private void CriticalText()
        {
            _tempWeapon.Critical = new Critical(_retrievedText);
        }
        
        private void CategoryText()
        {
            switch (_region)
            {
                case Region.WeaponsWeapon:
                    WeaponCategoryText();
                    break;
            }
        }

        private void WeaponCategoryText()
        {
            if (string.Equals(_retrievedText, NON_STANDARD_MELEE, StringComparison.OrdinalIgnoreCase))
            {
                _tempWeapon.IsNonStandardMelee = true;
            }
        }

        private void DamageText()
        {
            switch (_region)
            {
                case Region.WeaponsUnarmed:
                case Region.WeaponsNaturalAttack:
                case Region.WeaponsWeapon:
                    WeaponDamageText();
                    break;
                case Region.WeaponsWeaponMeleeW1H1P:
                    MeleeWeaponDamageText(W1_H1_P_TAG);
                    break;
                case Region.WeaponsWeaponMeleeW1H1O:
                    MeleeWeaponDamageText(W1_H1_O_TAG);
                    break;
                case Region.WeaponsWeaponMeleeW1H2:
                    MeleeWeaponDamageText(W1_H2_TAG);
                    break;
                case Region.WeaponsWeaponMeleeW2POH:
                    MeleeWeaponDamageText(W2_P_OH_TAG);
                    break;
                case Region.WeaponsWeaponMeleeW2POL:
                    MeleeWeaponDamageText(W2_P_OL_TAG);
                    break;
                case Region.WeaponsWeaponMeleeW2O:
                    MeleeWeaponDamageText(W2_O_TAG);
                    break;
                case Region.WeaponsWeaponRanges:
                    RangedWeaponDamageText();
                    break;
            }
        }

        private void RangedWeaponDamageText()
        {
            if (_retrievedText.Contains("d"))
            {
                RangedAttack attack = new RangedAttack(string.Format("{0} | {1}", _tempWeapon.Name, _tempDistance), _tempDistance, _retrievedText, _tempToHit, _tempCharacterStats, _tempWeapon);
                _tempRanged.RangedAttacks.Add(attack);
            }
        }

        private void AssignMeleeAttack(string attackType, Melee melee, Attack attack)
        {
            switch (attackType)
            {
                case W1_H1_P_TAG:
                    melee.OneWeaponPrimaryHand = attack;
                    break;
                case W1_H1_O_TAG:
                    melee.OneWeaponOffhand = attack;
                    break;
                case W1_H2_TAG:
                    melee.TwoHands = attack;
                    break;
                case W2_P_OH_TAG:
                    melee.TwoWeaponsPrimaryHandOtherHeavy = attack;
                    break;
                case W2_P_OL_TAG:
                    melee.TwoWeaponsPrimaryHandOtherLight = attack;
                    break;
                case W2_O_TAG:
                    melee.TwoWeaponsOffhand = attack;
                    break;
                default:
                    throw new InvalidOperationException("Invalid attackType");
            }
        }

        private void MeleeWeaponDamageText(String attackType)
        {
            if (_retrievedText.Contains("d"))
            {
                Attack attack = new Attack(_meleeAttackTypeMap[attackType], string.Format("{0} | {1}", _tempWeapon.Name, _meleeAttackMap[attackType]), _retrievedText, _tempToHit, _tempCharacterStats, _tempWeapon);
                AssignMeleeAttack(attackType, _tempMelee, attack);
            }
        }

        private void WeaponDamageText()
        {
            if (_retrievedText.ToLower().Contains("d"))
            {
                _tempWeapon.Attack = new Attack(AttackType.Melee1HP, _tempWeapon.Name, _retrievedText, _tempToHit, _tempCharacterStats, _tempWeapon);
            }
        }

        private void CombatManeuverDefenseText(String combatManeuverType)
        {
            CombatManeuver combatManeuver = _combatManeuvers.Maneuvers[combatManeuverType];
            combatManeuver.SetCmd(_retrievedText);
            combatManeuver.DetermineDifferences();
        }

        private void CombatManeuverAttackText(String combatManeuverType)
        {
            CombatManeuver combatManeuver = new CombatManeuver(_tempCharacterStats, combatManeuverType);
            combatManeuver.SetCmb(_retrievedText);

            if (!_combatManeuvers.Maneuvers.ContainsKey(combatManeuverType))
            {
                _combatManeuvers.Maneuvers.Add(combatManeuverType, combatManeuver);
            }
        }

        private void EpicModText()
        {
            switch (_region)
            {
                case Region.SavingThrows:
                    SavingThrowEpicModText();
                    break;
                case Region.Attack:
                    AttackBonusEpicModText();
                    break;
            }
        }

        private void AttackBonusEpicModText()
        {
            _tempAttackBonus.SetEpic(_retrievedText);
        }

        private void SavingThrowEpicModText()
        {
            _tempSavingThrow.SetEpic(_retrievedText);
        }

        private void MagicModText()
        {
            _tempSavingThrow.SetMagic(_retrievedText);
        }

        private void UntrainedText()
        {
            _tempSkill.SetUseUntrained(_retrievedText);
        }

        private void AbilityText()
        {
            switch (_region)
            {
                case Region.Skills:
                    SkillAbilityText();
                    break;
            }
        }

        private void SkillAbilityText()
        {
            _tempSkill.Ability = _retrievedText;
        }

        private void RanksText()
        {
            _tempSkill.SetRanks(_retrievedText);
        }

        private void MiscModText()
        {
            switch (_region)
            {
                case Region.Initiative:
                    InitiativeMiscText();
                    break;
                case Region.Skills:
                    SkillMiscText();
                    break;
                case Region.SavingThrows:
                    SavingThrowMiscModText();
                    break;
                case Region.Attack:
                    AttackBonusMiscModText();
                    break;
            }
        }

        private void AttackBonusMiscModText()
        {
            _tempAttackBonus.SetMisc(_retrievedText);
        }

        private void SavingThrowMiscModText()
        {
            _tempSavingThrow.SetMisc(_retrievedText);
        }

        private void SkillMiscText()
        {
            _tempSkill.SetMiscModifier(_retrievedText);
        }

        private void InitiativeMiscText()
        {
            _initiative.SetMisc(_retrievedText);
        }

        private void SpellResistanceText()
        {
            switch (_region)
            {
                case Region.ArmorClass:
                    AcSpellResistanceText();
                    break;
                case Region.Spells:
                    SpellSpellResistanceText();
                    break;
            }
        }

        private void SpellSpellResistanceText()
        {
            _tempSpell.SpellResistance = _retrievedText;
        }

        private void AcSpellResistanceText()
        {
            _ac.SetSpellResist(_retrievedText);
        }

        private void CheckPenaltyText()
        {
            _ac.SetArmorCheck(_retrievedText);
        }

        private void SpellFailureText()
        {
            _ac.SetArcaneFailure(_retrievedText);
        }

        private void MaxDexText()
        {
            switch (_region)
            {
                case Region.ArmorClass:
                    ArmorClassMaxDexText();
                    break;
                case Region.Protection:
                    ProtectionMaxDexText();
                    break;
            }
        }

        private void ProtectionMaxDexText()
        {
            _tempArmor.SetMaxDexBonus(_retrievedText);
        }

        private void ArmorClassMaxDexText()
        {
            _ac.SetMaxDex(_retrievedText);
        }

        private void MissChanceText()
        {
            _ac.MissChance = _retrievedText;
        }

        private void MiscText()
        {
            switch (_region)
            {
                case Region.ArmorClass:
                    ArmorMiscText();
                    break;
            }
        }

        private void ArmorMiscText()
        {
            _ac.SetMiscBonus(_retrievedText);
        }

        private void DodgeBonusText()
        {
            _ac.SetDodgeBonus(_retrievedText);
        }

        private void DeflectionText()
        {
            _ac.SetDeflectionBonus(_retrievedText);
        }

        private void NaturalText()
        {
            _ac.SetNaturalArmor(_retrievedText);
        }

        private void SizeModText()
        {
            switch (_region)
            {
                case Region.ArmorClass:
                    AcSizeModText();
                    break;
            }
        }

        private void AcSizeModText()
        {
            _ac.SetSizeBonus(_retrievedText);
        }

        private void ShieldBonusText()
        {
            _ac.SetShieldBonus(_retrievedText);
        }

        private void ArmorBonusText()
        {
            _ac.SetArmorBonus(_retrievedText);
        }

        private void BaseText()
        {
            switch (_region)
            {
                case Region.ArmorClass:
                    AcBaseText();
                    break;
                case Region.SavingThrows:
                    SavingThrowBaseText();
                    break;
            }
        }

        private void SavingThrowBaseText()
        {
            _tempSavingThrow.SetBase(_retrievedText);
        }

        private void AcBaseText()
        {
            _ac.SetBase(_retrievedText);
        }

        private void PointsText()
        {
            string hp = _retrievedText;
            _health.SetMaxHp(hp);
            _health.SetCurrentHp(hp);
        }

        private void ScoreText()
        {
            _tempAbilityScore.SetTempScore(_retrievedText);
        }

        private void NoEquipModText()
        {
            _tempAbilityScore.SetModifier(_retrievedText);
        }

        private void NoEquipText()
        {
            _tempAbilityScore.SetScore(_retrievedText);
        }

        private void ValueText()
        {
            switch (_region)
            {
                case Region.BasicsNotes:
                    NotesText();
                    break;
            }
        }

        private void NotesText()
        {
            _tempCharacterStats.Notes.Add(new Note { Name = string.Format("Note {0}", _tempCharacterStats.Notes.Count + 1), Description = _retrievedText });
        }

        private void WeightUnitText()
        {
            _bio.Weight = _retrievedText;
        }

        private void GoldText()
        {
            _bio.Money.SetGP(_retrievedText);
        }

        private void VisionText()
        {
            _bio.Senses.Add(new Sense(_retrievedText));
        }

        private void RaceText()
        {
            switch (_region)
            {
                case Region.Basics:
                    CharacterRaceText();
                    break;
            }
        }

        private void CharacterRaceText()
        {
            _bio.Race = _retrievedText;
        }

        private void RateText()
        {
            switch (_region)
            {
                case Region.BasicsMove:
                    MoveRateText();
                    break;
            }
        }

        private void MoveRateText()
        {
            String[] moveRateArray = _retrievedText.Split(' ');

            _speed.SetBaseValue(moveRateArray[0]);
            _speed.SetUnit(moveRateArray[1]);
        }

        private void LanguageText()
        {
            _bio.Languages.Add(_retrievedText);
        }

        private void TotalText()
        {
            switch (_region)
            {
                case Region.BasicsHeight:
                    HeightText();
                    break;
                case Region.Weapons:
                case Region.WeaponsUnarmed:
                    WeaponsTotalText();
                    break;
            }
        }

        private void WeaponsTotalText()
        {
            _tempToHit = _retrievedText;
        }

        private void HeightText()
        {
            _bio.Height = _retrievedText;
        }

        private void ShortText()
        {
            switch (_region)
            {
                case Region.BasicsGender:
                    GenderText();
                    break;
            }
        }

        private void SizeText()
        {
            switch (_region)
            {
                case Region.BasicsSize:
                    BioSizeText();
                    break;
                case Region.WeaponsWeapon:
                    WeaponSizeText();
                    break;
            }
        }

        private void WeaponSizeText()
        {
            _tempWeapon.Size = _retrievedText;
        }

        private void BioSizeText()
        {
            _bio.SetSize(_retrievedText);
        }

        private void GenderText()
        {
            _bio.Gender = _retrievedText;
        }

        private void CrText()
        {
            _bio.ChallengeRating = _retrievedText;
        }

        private void ColorText()
        {
            switch (_region)
            {
                case Region.BasicsEyes:
                    EyesColor();
                    break;
                case Region.BasicsHair:
                    HairColor();
                    break;
            }
        }

        private void HairColor()
        {
            _bio.Hair = _retrievedText;
        }

        private void EyesColor()
        {
            _bio.Eyes = _retrievedText;
        }

        private void NextLevelText()
        {
            _bio.SetExperienceNextLevel(_retrievedText);
        }

        private void CurrentText()
        {
            switch (_region)
            {
                case Region.BasicsExperience:
                    ExperienceText();
                    break;
            }
        }


        private void ExperienceText()
        {
            _bio.SetExperience(_retrievedText);
        }

        private void DescriptionText()
        {
            switch (_region)
            {
                case Region.Basics:
                    BioDescription();
                    break;
                case Region.Skills:
                    SkillsConditionalModifiers();
                    break;
                case Region.Attack:
                    AttackConditionalModifiers();
                    break;
                case Region.CheckLists:
                case Region.ClassFeaturesStunningFist:
                    TrackedResourceDescriptionText();
                    break;
                case Region.Equipment:
                    EquipmentDescriptionText();
                    break;
                case Region.SpecialQualities:
                case Region.SpecialAttacks:
                case Region.Traits:
                    SpecialQualitiesDescriptionText();
                    break;
                case Region.Spells:
                    SpellDescriptionText();
                    break;
            }
        }

        private void SpellDescriptionText()
        {
            SetDescription(_tempSpell);
        }

        private void SpecialQualitiesDescriptionText()
        {
            _tempFeat.Description = _retrievedText;
        }

        private void EquipmentDescriptionText()
        {
            SetDescription(_tempItem);
        }

        private void TrackedResourceDescriptionText()
        {
            if (_tempTrackedResource == null)
            {
                _tempTrackedResourceDescription = _retrievedText;
            }
            else
            {
                string description;

                if (_tempSubAbilityName != null && !string.IsNullOrEmpty(_tempSubAbilityName))
                {
                    description = string.Format("{0} - {1}", _tempSubAbilityName, _retrievedText);
                }
                else
                {
                    description = _retrievedText;
                }

                _tempTrackedResource.Notes.Add(description);
            }
        }

        private void AttackConditionalModifiers()
        {
            _attackBonuses.ConditionalModifiers.Add(_retrievedText);
        }

        private void SkillsConditionalModifiers()
        {
            _skills.ConditionalModifiers = _retrievedText;
        }

        private void BioDescription()
        {
            _bio.Description = _retrievedText;
        }

        private void LevelsTotalText()
        {
            _bio.SetLevel(_retrievedText);
        }

        private void LevelText()
        {
            switch (_region)
            {
                case Region.BasicsClasses:
                    CharacterClassLevel();
                    break;
            }
        }

        private void CharacterClassLevel()
        {
            _tempCharacterClass.SetLevel(_retrievedText);
        }

        private void CharacterClassName()
        {
            _tempCharacterClass = new CharacterClass(_retrievedText);
        }

        private void BabText()
        {
            switch (_region)
            {
                case Region.Attack:
                    BaseAttackBonus();
                    break;
            }
        }

        private void BaseAttackBonus()
        {
            _attackBonuses.Bonuses[AttackBonusType.Bab].SetBase(_retrievedText);
        }

        private void LongText()
        {
            switch (_region)
            {
                case Region.BasicsAlignment:
                    _bio.Alignment = _retrievedText;
                    break;
                case Region.BasicsSize:
                    SizeText();
                    break;
                case Region.Abilities:
                    AbilityNameText();
                    break;
                case Region.SavingThrows:
                    SavingThrowNameText();
                    break;
            }
        }

        private void SavingThrowNameText()
        {
            SavingThrowType throwType = SavingThrowType.Unknown;
            string name = null;
            string statName = null;
            AbilityScoreType basedOn = AbilityScoreType.Unknown;

            switch (_retrievedText.ToLower())
            {
                case FORTITUDE:
                    throwType = SavingThrowType.Fortitude;
                    name = "Fortitude";
                    statName = "Constitution";
                    basedOn = AbilityScoreType.Constitution;
                    break;
                case REFLEX:
                    throwType = SavingThrowType.Reflex;
                    name = "Reflex";
                    statName = "Dexterity";
                    basedOn = AbilityScoreType.Dexterity;
                    break;
                case WILL:
                    throwType = SavingThrowType.Will;
                    name = "Will";
                    statName = "Wisdom";
                    basedOn = AbilityScoreType.Wisdom;
                    break;
                case STRENGTH:
                    throwType = SavingThrowType.Strength;
                    name = "Strength";
                    statName = "Strength";
                    basedOn = AbilityScoreType.Strength;
                    break;
                case DEXTERITY:
                    throwType = SavingThrowType.Dexterity;
                    name = "Dexterity";
                    statName = "Dexterity";
                    basedOn = AbilityScoreType.Dexterity;
                    break;
                case CONSTITUTION:
                    throwType = SavingThrowType.Constitution;
                    name = "Constitution";
                    statName = "Constitution";
                    basedOn = AbilityScoreType.Constitution;
                    break;
                case INTELLIGENCE:
                    throwType = SavingThrowType.Intelligence;
                    name = "Intelligence";
                    statName = "Intelligence";
                    basedOn = AbilityScoreType.Intelligence;
                    break;
                case WISDOM:
                    throwType = SavingThrowType.Wisdom;
                    name = "Wisdom";
                    statName = "Wisdom";
                    basedOn = AbilityScoreType.Wisdom;
                    break;
                case CHARISMA:
                    throwType = SavingThrowType.Charisma;
                    name = "Charisma";
                    statName = "Charisma";
                    basedOn = AbilityScoreType.Charisma;
                    break;
            }

            _tempSavingThrow = new SavingThrow(name, statName, _tempCharacterStats, basedOn);

            _savingThrows.Throws.Add(throwType, _tempSavingThrow);
        }

        private void AbilityNameText()
        {
            _tempAbilityScore = _abilityScores.Scores[(AbilityScoreType)Enum.Parse(typeof(AbilityScoreType), Prepare.ForStringToEnumConversion(_retrievedText))];
        }

        private void AgeText()
        {
            _bio.SetAge(_retrievedText);
        }

        private void HeroPointsText()
        {
            _bio.SetHeroPoints(_retrievedText);
        }

        private void CharacterTypeText()
        {
            _bio.CharacterType = _retrievedText;
        }

        private void PlayerNameText()
        {
            _tempCharacterStats.PlayerName = _retrievedText;
        }

        private void NameText()
        {
            switch (_region)
            {
                case Region.Basics:
                    CharacterTag();
                    break;
                case Region.BasicsClasses:
                    CharacterClassName();
                    break;
                case Region.BasicsDeity:
                    DeityName();
                    break;
                case Region.BasicsMove:
                    MoveNameText();
                    break;
                case Region.Skills:
                    SkillNameText();
                    break;
                case Region.WeaponsNaturalAttack:
                    NaturalAttackNameText();
                    break;
                case Region.WeaponsWeaponRangesAmmunition:
                    AmmunitionNameText();
                    break;
                case Region.Protection:
                    ProtectionNameTag();
                    break;
                case Region.CheckLists:
                    CheckListNameText();
                    break;
                case Region.Equipment:
                    EquipmentItemNameText();
                    break;
                case Region.Feats:
                    FeatNameText("Feats");
                    break;
                case Region.SpecialQualities:
                    FeatNameText("Special Qualities");
                    break;
                case Region.SpecialAttacks:
                    FeatNameText("Special Attacks");
                    break;
                case Region.Traits:
                    FeatNameText("Traits");
                    break;
                case Region.Spells:
                case Region.SpellBook:
                    SpellNameText();
                    break;
            }
        }

        private void AmmunitionNameText()
        {
            _tempAmmunition = new Ammunition(_retrievedText);
        }

        private void SpellNameText()
        {
            _tempSpell = new Spell(_retrievedText, _tempSpellLevel, _tempSpellCollectionName);
        }

        private void FeatNameText(String featType)
        {
            _tempFeat = new Feat(_retrievedText);
            _tempFeat.Type = featType;
        }

        private void EquipmentItemNameText()
        {
            _tempItem = new Item(_retrievedText);
        }

        private void CheckListNameText()
        {
            _tempSubAbilityName = _retrievedText;
        }

        private void ProtectionNameTag()
        {
            _tempArmor = new Armor(_retrievedText);
        }

        private void NaturalAttackNameText()
        {
            _tempWeapon = new Weapon(_retrievedText);
        }

        private void SkillNameText()
        {
            _tempSkill = new Skill(_retrievedText, _tempCharacterStats);
        }

        private void MoveNameText()
        {
            _speed.MovementType = _retrievedText;
        }

        private void DeityName()
        {
            _bio.Deity = _retrievedText;
        }

        private void ProcessEndTag()
        {
            _tagName = _xpp.Name.ToLower();

            if (_tags.Contains(_tagName) && _region != Region.Unknown)
            {
                switch (_tagName)
                {
                    case ALIGNMENT_TAG:
                        AlignmentEndTag();
                        break;
                    case CLASS_TAG:
                        ClassEndTag();
                        break;
                    case CLASSES_TAG:
                        ClassesEndTag();
                        break;
                    case DEITY_TAG:
                        DeityEndTag();
                        break;
                    case MOVE_TAG:
                        MoveEndTag();
                        break;
                    case UNARMED_TAG:
                    case NATURAL_ATTACK_TAG:
                    case WEAPON_TAG:
                        WeaponEndTag();
                        break;
                    case CRITICAL_TAG:
                        CriticalEndTag();
                        break;
                    case MELEE_TAG:
                    case SIMPLE_TAG:
                        MeleeEndTag();
                        break;
                    case RANGES_TAG:
                        RangesEndTag();
                        break;
                    case ARMOR_TAG:
                    case SHIELD_TAG:
                        ArmorItemEndTag();
                        break;
                    case ITEM_TAG:
                        ItemEndTag();
                        break;
                    case KI_POOL_TAG:
                    case STUNNING_FIST_TAG:
                    case PSIONICS_TAG:
                    case WILD_SHAPE_TAG:
                    case CHECKLIST_TAG:
                        TrackedResourceEndTag();
                        break;
                    case FEAT_TAG:
                    case SPECIAL_QUALITY_TAG:
                    case SPECIAL_ATTACK_TAG:
                    case TRAIT_TAG:
                        FeatEndTag();
                        break;
                    case SPELL_TAG:
                        SpellEndTag();
                        break;
                    case SKILL_TAG:
                        SkillEndTag();
                        break;
                    case AMMUNITION_TAG:
                        AmmunitionEndTag();
                        break;
                    case TOTAL_TAG:
                        TotalEndTag();
                        break;
                    case SPELL_BOOK_TAG:
                        SpellBookEndTag();
                        break;
                }
            }
        }

        private void AlignmentEndTag()
        {
            _region = Region.Basics;
        }

        private void SpellBookEndTag()
        {
            switch (_region)
            {
                case Region.SpellBook:
                    RealSpellBookEndTag();
                    break;
            }
        }

        private void RealSpellBookEndTag()
        {
            _region = Region.Spells;
            _spells.SpellBooks.Add(_tempSpellBook);
        }

        private void TotalEndTag()
        {
            switch (_region)
            {
                case Region.EquipmentTotal:
                    EquipmentTotalEndTag();
                    break;
            }
        }

        private void EquipmentTotalEndTag()
        {
            _region = Region.Equipment;
        }

        private void AmmunitionEndTag()
        {
            if (!_tempRanged.AmmunitionList.Any(x => x.Name == _tempAmmunition.Name) && _tempAmmunition.Remaining > 0)
            {
                _tempRanged.AmmunitionList.Add(_tempAmmunition);
            }

            _tempAmmunition = null;

            _region = Region.WeaponsWeaponRanges;
        }

        private void SkillEndTag()
        {
            _skills.SkillsList.Add(_tempSkill);
        }

        private void SpellEndTag()
        {
            switch (_region)
            {
                case Region.Spells:
                    SpellClassSpellEndTag();
                    break;
                case Region.SpellBook:
                    SpellBookSpellEndTag();
                    break;
            }
        }

        private void SpellBookSpellEndTag()
        {
            _tempSpellBook.Spells.Add(_tempSpell);
        }

        private void SpellClassSpellEndTag()
        {
            _tempSpellClassLevel.AddSpell(_tempSpell);
        }

        private void FeatEndTag()
        {
            switch (_region)
            {
                case Region.Feats:
                case Region.SpecialQualities:
                case Region.SpecialAttacks:
                case Region.Traits:
                    _feats.Add(_tempFeat);
                    break;
            }
        }

        private void TrackedResourceEndTag()
        {
            if (!_trackedResources.ContainsKey(_tempTrackedResource.Name))
            {
                _trackedResources.Add(_tempTrackedResource.Name, _tempTrackedResource);
            }
        }

        private void ItemEndTag()
        {
            switch (_region)
            {
                case Region.Protection:
                    ArmorItemEndTag();
                    break;
                case Region.Equipment:
                    EquipmentItemEndTag();
                    break;
            }
        }

        private void EquipmentItemEndTag()
        {
            if (!_gear.Items.Any(x => string.Equals(x.Name, _tempItem.Name)))
            {
                _gear.Items.Add(_tempItem);
            }
        }

        private void ArmorItemEndTag()
        {
            _equipment.Armor.Add(_tempArmor);
        }

        private void RangesEndTag()
        {
            switch (_region)
            {
                case Region.WeaponsWeaponRanges:
                    WeaponRangesEndTag();
                    break;
            }
        }

        private void WeaponRangesEndTag()
        {
            if (!_tempRanged.AmmunitionList.Any())
            {
                _tempRanged.AmmunitionList.Add(new Ammunition("Ammunition"));
            }

            _tempWeapon.Ranged = _tempRanged;
        }

        private void MeleeEndTag()
        {
            switch (_region)
            {
                case Region.WeaponsWeaponMeleeW1H2:
                case Region.WeaponsWeaponMeleeW2O:
                    WeaponMeleeEndTag();
                    break;
            }
        }

        private void WeaponMeleeEndTag()
        {
            _tempWeapon.Melee = _tempMelee;
        }

        private void CriticalEndTag()
        {
            switch (_region)
            {
                case Region.WeaponsWeaponCritical:
                    WeaponsCriticalEndTag();
                    break;
            }
        }

        private void WeaponsCriticalEndTag()
        {
            _region = Region.WeaponsWeapon;
        }

        private void ReferenceCritical(Attack attack, Critical crit)
        {
            if (attack != null)
            {
                attack.Critical = crit;
            }
        }

        private void WeaponEndTag()
        {
            var crit = _tempWeapon.Critical;
            var mainAttack = _tempWeapon.Attack;
            var melee = _tempWeapon.Melee;
            var ranged = _tempWeapon.Ranged;

            if (mainAttack != null)
            {
                _tempWeapon.Attack.Critical = crit;
            }

            if (melee != null)
            {
                ReferenceCritical(melee.OneWeaponPrimaryHand, crit);
                ReferenceCritical(melee.OneWeaponOffhand, crit);
                ReferenceCritical(melee.TwoHands, crit);
                ReferenceCritical(melee.TwoWeaponsPrimaryHandOtherHeavy, crit);
                ReferenceCritical(melee.TwoWeaponsPrimaryHandOtherLight, crit);
                ReferenceCritical(melee.TwoWeaponsOffhand, crit);
            }

            if (ranged != null)
            {
                foreach (var attack in _tempWeapon.Ranged.RangedAttacks)
                {
                    attack.Critical = crit;
                }
            }

            _equipment.Weapons.Add(_tempWeapon);
        }

        private void MoveEndTag()
        {
            switch (_region)
            {
                case Region.BasicsMove:
                    BasicsMoveEndTag();
                    break;
            }
        }

        private void BasicsMoveEndTag()
        {
            _region = Region.Basics;
        }

        private void DeityEndTag()
        {
            _region = Region.Basics;
        }

        private void ClassEndTag()
        {
            switch (_region)
            {
                case Region.BasicsClasses:
                    BasicsClassesClassEndTag();
                    break;
                case Region.Spells:
                    SpellClassEndTag();
                    break;
            }
        }

        private void SpellClassEndTag()
        {
            if (!_spells.SpellClasses.ContainsKey(_tempSpellClass.Name))
            {
                _spells.SpellClasses.Add(_tempSpellClass.Name, _tempSpellClass);
            }
        }

        private void BasicsClassesClassEndTag()
        {
            _bio.CharacterClasses.Add(_tempCharacterClass);
        }

        private void ClassesEndTag()
        {
            string characterClassSummaryText = "";

            foreach (CharacterClass characterClass in _bio.CharacterClasses)
            {
                characterClassSummaryText += string.Format("{0}{1} ", characterClass.Name, characterClass.Level);
            }

            _bio.ClassDescription = characterClassSummaryText.Trim();
        }


        private void ProcessStartTag()
        {
            _tagName = _xpp.Name.ToLower();

            if (_tags.Contains(_tagName))
            {
                if (_region == Region.Unknown)
                {
                    switch (_tagName)
                    {
                        case BASICS_TAG:
                            BasicsTag();
                            break;
                    }
                }
                else
                {
                    switch (_tagName)
                    {
                        case ALIGNMENT_TAG:
                            AlignmentTag();
                            break;
                        case CLASSES_TAG:
                            ClassesTag();
                            break;
                        case DEITY_TAG:
                            DeityTag();
                            break;
                        case EXPERIENCE_TAG:
                            ExperienceTag();
                            break;
                        case EYES_TAG:
                            EyesTag();
                            break;
                        case HAIR_TAG:
                            HairTag();
                            break;
                        case GENDER_TAG:
                            GenderTag();
                            break;
                        case HEIGHT_TAG:
                            HeightTag();
                            break;
                        case LANGUAGES_TAG:
                            LanguagesTag();
                            break;
                        case MOVE_TAG:
                            MoveTag();
                            break;
                        case SIZE_TAG:
                            SizeTag();
                            break;
                        case VISION_TAG:
                            VisionTag();
                            break;
                        case WEIGHT_TAG:
                            WeightTag();
                            break;
                        case NOTES_TAG:
                            NotesTag();
                            break;
                        case ABILITIES_TAG:
                            AbilitiesTag();
                            break;
                        case HIT_POINTS_TAG:
                            HitPointsTag();
                            break;
                        case ARMOR_CLASS_TAG:
                            ArmorClassTag();
                            break;
                        case INITIATIVE_TAG:
                            InitiativeTag();
                            break;
                        case SKILLS_TAG:
                            SkillsTag();
                            break;
                        case SAVING_THROWS_TAG:
                            SavingThrowsTag();
                            break;
                        case ATTACK_TAG:
                            AttackTag();
                            break;
                        case MELEE_TAG:
                            MeleeTag();
                            break;
                        case SIMPLE_TAG:
                            MeleeTag();
                            W1H2Tag();
                            break;
                        case RANGED_TAG:
                            RangedTag();
                            break;
                        case CMB_TAG:
                            CmbTag();
                            break;
                        case WEAPONS_TAG:
                            WeaponsTag();
                            break;
                        case UNARMED_TAG:
                            UnarmedTag();
                            break;
                        case NATURAL_ATTACK_TAG:
                            NaturalAttackTag();
                            break;
                        case WEAPON_TAG:
                            WeaponTag();
                            break;
                        case CRITICAL_TAG:
                            CriticalTag();
                            break;
                        case W1_H1_P_TAG:
                            W1H1PTag();
                            break;
                        case W1_H1_O_TAG:
                            W1H1OTag();
                            break;
                        case W1_H2_TAG:
                            W1H2Tag();
                            break;
                        case W2_P_OH_TAG:
                            W2POHTag();
                            break;
                        case W2_P_OL_TAG:
                            W2POLTag();
                            break;
                        case W2_O_TAG:
                            W2OTag();
                            break;
                        case RANGES_TAG:
                            RangesTag();
                            break;
                        case AMMUNITION_TAG:
                            AmmunitionTag();
                            break;
                        case PROTECTION_TAG:
                            ProtectionTag();
                            break;
                        case CLASS_FEATURES_TAG:
                            ClassFeaturesTag();
                            break;
                        case KI_POOL_TAG:
                            KiPoolTag();
                            break;
                        case STUNNING_FIST_TAG:
                            StunningFistTag();
                            break;
                        case CHECKLISTS_TAG:
                            ChecklistsTag();
                            break;
                        case CHECKLIST_TAG:
                            ChecklistTag();
                            break;
                        case PSIONICS_TAG:
                            PsionicsTag();
                            break;
                        case WILD_SHAPE_TAG:
                            WildShapeTag();
                            break;
                        case EQUIPMENT_TAG:
                            EquipmentTag();
                            break;
                        case TOTAL_TAG:
                            TotalTag();
                            break;
                        case FEATS_TAG:
                            FeatsTag();
                            break;
                        case SPECIAL_QUALITIES_TAG:
                            SpecialQualitiesTag();
                            break;
                        case SPECIAL_ATTACKS_TAG:
                            SpecialAttacksTag();
                            break;
                        case TRAITS_TAG:
                            TraitsTag();
                            break;
                        case SPELLS_TAG:
                            SpellsTag();
                            break;
                        case SPELLS_INNATE_TAG:
                            SpellsInnateTag();
                            break;
                        case SPELL_BOOK_TAG:
                            SpellBookTag();
                            break;
                        case CLASS_TAG:
                            ClassTag();
                            break;
                        case LEVEL_TAG:
                            LevelTag();
                            break;
                        case MEMORIZED_SPELLS_TAG:
                            MemorizedSpellsTag();
                            break;
                    }
                }
            }
        }

        private void MemorizedSpellsTag()
        {
            _region = Region.MemorizedSpells;
        }

        private void LevelTag()
        {
            switch (_region)
            {
                case Region.Spells:
                    SpellLevelTag();
                    break;
            }
        }

        private void SpellLevelTag()
        {
            string level = GetValue(NUMBER_ATTRIBUTE);
            _tempSpellClassLevel = new SpellClassLevel(level, GetValue(CAST_ATTRIBUTE));
            _tempSpellClass.Levels.Add(_tempSpellClassLevel.Level, _tempSpellClassLevel);
            _tempSpellLevel = level;
        }

        private void ClassTag()
        {
            switch (_region)
            {
                case Region.Spells:
                    SpellClassTag();
                    break;
            }
        }

        private void SpellClassTag()
        {
            _mustPrepareSpells = string.Equals(GetValue(MEMORIZE_ATTRIBUTE), TRUE, StringComparison.OrdinalIgnoreCase);

            string name = GetValue(SPELL_LIST_CLASS_ATTRIBUTE);
            _tempSpellCollectionName = name;
            _tempSpellClass = new SpellClass(name, "none", GetValue(SPELL_CASTER_LEVEL_ATTRIBUTE));
        }

        private void SpellBookTag()
        {
            switch (_region)
            {
                case Region.Spells:
                    NewSpellBookTag();
                    break;
            }
        }

        private void NewSpellBookTag()
        {
            _tempSpellCollectionName = GetValue(NAME_ATTRIBUTE);
            _tempSpellBook = new SpellBook(_tempSpellCollectionName);
            _tempSpellLevel = "0";
            _region = Region.SpellBook;
        }

        private void SpellsInnateTag()
        {
            _tempSpellClass = new SpellClass("Innate", "none", "0");
            _tempSpellClassLevel = new SpellClassLevel("0", "yes");
            _tempSpellClass.Levels.Add(_tempSpellClassLevel.Level, _tempSpellClassLevel);
            _tempSpellLevel = "0";
            _tempSpellCollectionName = "Innate";
        }

        private void SpellsTag()
        {
            _region = Region.Spells;
        }

        private void TraitsTag()
        {
            _region = Region.Traits;
        }

        private void SpecialAttacksTag()
        {
            _region = Region.SpecialAttacks;
        }

        private void SpecialQualitiesTag()
        {
            _region = Region.SpecialQualities;
        }

        private void FeatsTag()
        {
            switch (_region)
            {
                case Region.Equipment:
                    ActualFeatsTag();
                    break;
            }
        }

        private void ActualFeatsTag()
        {
            _region = Region.Feats;
        }

        private void TotalTag()
        {
            switch (_region)
            {
                case Region.Equipment:
                    EquipmentTotalTag();
                    break;
            }
        }

        private void EquipmentTotalTag()
        {
            _region = Region.EquipmentTotal;
        }

        private void EquipmentTag()
        {
            _region = Region.Equipment;
        }

        private void WildShapeTag()
        {
            _region = Region.ClassFeaturesWildShape;
        }

        private void PsionicsTag()
        {
            _region = Region.ClassFeaturesPsionics;
        }

        private void StunningFistTag()
        {
            _region = Region.ClassFeaturesStunningFist;
        }

        private void KiPoolTag()
        {
            _region = Region.ClassFeatureKiPool;
        }

        private void ClassFeaturesTag()
        {
            _region = Region.ClassFeatures;
        }

        private void ChecklistTag()
        {
            _tempTrackedResourceDescription = "";
            _tempSubAbilityName = "";
            _tempTrackedResource = null;
        }

        private void ChecklistsTag()
        {
            _region = Region.CheckLists;
        }

        private void ProtectionTag()
        {
            _region = Region.Protection;
        }

        private void AmmunitionTag()
        {
            _region = Region.WeaponsWeaponRangesAmmunition;
        }

        private void RangesTag()
        {
            _region = Region.WeaponsWeaponRanges;
            _tempRanged = new Ranged();

            _tempRanged.IsSling = _tempWeapon.Name.ToLower().Contains(SLING);        
        }

        private void W1H1PTag()
        {
            _region = Region.WeaponsWeaponMeleeW1H1P;
        }

        private void W1H1OTag()
        {
            _region = Region.WeaponsWeaponMeleeW1H1O;
        }

        private void W1H2Tag()
        {
            _region = Region.WeaponsWeaponMeleeW1H2;
        }

        private void W2POHTag()
        {
            _region = Region.WeaponsWeaponMeleeW2POH;
        }

        private void W2POLTag()
        {
            _region = Region.WeaponsWeaponMeleeW2POL;
        }

        private void W2OTag()
        {
            _region = Region.WeaponsWeaponMeleeW2O;
        }

        private void CriticalTag()
        {
            switch (_region)
            {
                case Region.WeaponsWeapon:
                    WeaponsCriticalTag();
                    break;
            }
        }

        private void WeaponsCriticalTag()
        {
            _region = Region.WeaponsWeaponCritical;
        }

        private void WeaponTag()
        {
            _region = Region.WeaponsWeapon;
        }

        private void NaturalAttackTag()
        {
            _region = Region.WeaponsNaturalAttack;
        }

        private void UnarmedTag()
        {
            _region = Region.WeaponsUnarmed;

            _tempWeapon = new Weapon("Unarmed");
            _tempWeapon.IsUnarmed = true;
        }

        private void WeaponsTag()
        {
            _region = Region.Weapons;
        }

        private void CmbTag()
        {
            switch (_region)
            {
                case Region.Attack:
                    AttackCmbTag();
                    break;
            }
        }

        private void AttackCmbTag()
        {
            _tempAttackBonus = _attackBonuses.Bonuses[AttackBonusType.Cmb];
        }

        private void RangedTag()
        {
            switch (_region)
            {
                case Region.Attack:
                    AttackRangedTag();
                    break;
            }
        }

        private void AttackRangedTag()
        {
            _tempAttackBonus = _attackBonuses.Bonuses[AttackBonusType.Ranged];
        }

        private void MeleeTag()
        {
            switch (_region)
            {
                case Region.Attack:
                    AttackMeleeTag();
                    break;
                case Region.WeaponsWeapon:
                    WeaponsMeleeTag();
                    break;
            }
        }

        private void WeaponsMeleeTag()
        {
            _tempMelee = new Melee();
        }

        private void AttackMeleeTag()
        {
            _tempAttackBonus = _attackBonuses.Bonuses[AttackBonusType.Melee];
        }

        private void AttackTag()
        {
            _region = Region.Attack;
        }

        private void SavingThrowsTag()
        {
            _region = Region.SavingThrows;
        }

        private void SkillsTag()
        {
            _region = Region.Skills;
        }

        private void InitiativeTag()
        {
            _region = Region.Initiative;
        }

        private void ArmorClassTag()
        {
            _region = Region.ArmorClass;
        }

        private void HitPointsTag()
        {
            _region = Region.HitPoints;
        }

        private void AbilitiesTag()
        {
            _region = Region.Abilities;
        }

        private void NotesTag()
        {
            _region = Region.BasicsNotes;
        }

        private void WeightTag()
        {
            switch (_region)
            {
                case Region.BasicsVision:
                    BasicsWeightTag();
                    break;
            }
        }

        private void BasicsWeightTag()
        {
            _region = Region.BasicsWeight;
        }

        private void VisionTag()
        {
            _region = Region.BasicsVision;
        }

        private void SizeTag()
        {
            switch (_region)
            {
                case Region.Basics:
                    BasicsSizeTag();
                    break;
            }
        }

        private void BasicsSizeTag()
        {
            _region = Region.BasicsSize;
        }

        private void MoveTag()
        {
            switch (_region)
            {
                case Region.BasicsLanguages:
                    BasicsMoveTag();
                    break;
            }
        }

        private void BasicsMoveTag()
        {
            _region = Region.BasicsMove;
        }

        private void LanguagesTag()
        {
            _region = Region.BasicsLanguages;
        }

        private void HeightTag()
        {
            _region = Region.BasicsHeight;
        }

        private void GenderTag()
        {
            _region = Region.BasicsGender;
        }

        private void FaceTag()
        {
            _region = Region.BasicsFace;
        }

        private void HairTag()
        {
            _region = Region.BasicsHair;
        }

        private void EyesTag()
        {
            _region = Region.BasicsEyes;
        }

        private void ExperienceTag()
        {
            _region = Region.BasicsExperience;
        }

        private void DeityTag()
        {
            _region = Region.BasicsDeity;
        }

        private void ClassesTag()
        {
            switch (_region)
            {
                case Region.Basics:
                    BasicsClassesTag();
                    break;
            }
        }

        private void BasicsClassesTag()
        {
            _region = Region.BasicsClasses;
        }

        private void AlignmentTag()
        {
            _region = Region.BasicsAlignment;
        }

        private void BasicsTag()
        {
            _region = Region.Basics;
        }

        private void CharacterTag()
        {
            _tempCharacterStats = new CharacterStats(_retrievedText);

            _abilityScores = _tempCharacterStats.AbilityScores;
            _ac = _tempCharacterStats.AC;
            _attackBonuses = _tempCharacterStats.AttackBonuses;
            _bio = _tempCharacterStats.Bio;
            _combatManeuvers = _tempCharacterStats.CombatManeuvers;
            _conditions = _tempCharacterStats.Conditions;
            _equipment = _tempCharacterStats.Equipment;
            _feats = _tempCharacterStats.Feats;
            _gear = _tempCharacterStats.Gear;
            _health = _tempCharacterStats.Health;
            _initiative = _tempCharacterStats.Initiative;
            _savingThrows = _tempCharacterStats.SavingThrows;
            _skills = _tempCharacterStats.Skills;
            _speed = _tempCharacterStats.Speed;
            _spells = _tempCharacterStats.Spells;
            _trackedResources = _tempCharacterStats.TrackedResources;
        }


        private string GetValue(String attributeName)
        {
            return _xpp.GetAttribute(attributeName);
        }

        private bool PrepareText()
        {
            _retrievedText = _xpp.Value.Trim();
            return !string.IsNullOrEmpty(_retrievedText);
        }

        private Stream RemoveInvalidCharacters(Stream inputStream)
        {
            string line;
            var memStream = new MemoryStream();
            inputStream.CopyTo(memStream);
            memStream.Position = 0;
            var filteredStream = new MemoryStream();
            using (var writer = new StreamWriter(filteredStream, Encoding.UTF8, 512, true))
            {
                using (var reader = new StreamReader(memStream))
                {
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        writer.WriteLine(line.Replace("&mdash;", "-"));
                    }
                }
            }

            filteredStream.Seek(0, SeekOrigin.Begin);

            return filteredStream;
        }
    }
}
