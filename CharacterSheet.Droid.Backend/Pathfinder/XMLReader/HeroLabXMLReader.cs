using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.Droid.Backend.Pathfinder.Equipment;
using System.Linq.Expressions;
using CharacterSheet.Droid.Backend;

namespace CharacterSheet.Pathfinder.XMLReader
{

    public class HeroLabXMLReader : ICharacterParser
    {
        private enum Region
        {
            AllSaves,
            AnimalTricks,
            Attack,
            Attributes,
            Auras,
            DamageReduction,
            Defenses,
            Defensive,
            EquipmentSets,
            Feats,
            Gear,
            Immunities,
            MagicItems,
            Maneuvers,
            Melee,
            OtherSpecials,
            Personal,
            Ranged,
            Resistances,
            Saves,
            Senses,
            SkillAbilities,
            Skills,
            SpellClasses,
            SpellLike,
            SpellsKnown,
            SpellsMemorized,
            SpellBook,
            TrackedResources,
            Traits
        }

        // Null namespace value for retrieving attributes by name
        private const string NAMESPACE = null;

        private const string UNARMED_ATTACK_WEAPON_CATEGORY = "unarmed attack";
        private const string SHIELD_BASH_WEAPON_CATEGORY = "shield bash";

        // Tags
        private const string PROGRAM_TAG = "program";
        private const string CHARACTER_TAG = "character";
        private const string RACE_TAG = "race";
        private const string CLASSES_TAG = "classes";
        private const string ALIGNMENT_TAG = "alignment";
        private const string DEITY_TAG = "deity";
        private const string XP_TAG = "xp";
        private const string MONEY_TAG = "money";
        private const string PERSONAL_TAG = "personal";
        private const string CHARACTER_HEIGHT_TAG = "charheight";
        private const string LANGUAGE_TAG = "language";
        private const string SPEED_TAG = "speed";
        private const string BASE_SPEED_TAG = "basespeed";
        private const string SIZE_TAG = "size";
        private const string REACH_TAG = "reach";
        private const string SENSES_TAG = "senses";
        private const string MAGIC_ITEMS_TAG = "magicitems";
        private const string CHARACTER_WEIGHT_TAG = "charweight";
        private const string ATTRIBUTES_TAG = "attributes";
        private const string ATTRIBUTE_TAG = "attribute";
        private const string ATTRIBUTE_VALUE_TAG = "attrvalue";
        private const string ATTRIBUTE_BONUS_TAG = "attrbonus";
        private const string HEALTH_TAG = "health";
        private const string ARMOR_CLASS_TAG = "armorclass";
        private const string PENALTY_TAG = "penalty";
        private const string INITIATIVE_TAG = "initiative";
        private const string SKILLS_TAG = "skills";
        private const string SAVES_TAG = "saves";
        private const string SAVE_TAG = "save";
        private const string ALL_SAVES_TAG = "allsaves";
        private const string SITUATIONAL_MODIFIERS_TAG = "situationalmodifiers";
        private const string ATTACK_TAG = "attack";
        private const string MANEUVERS_TAG = "maneuvers";
        private const string MANEUVER_TYPE_TAG = "maneuvertype";
        private const string DEFENSES_TAG = "defenses";
        private const string EQUIPMENT_SETS_TAG = "equipmentsets";
        private const string SPELL_LIKE_TAG = "spelllike";
        private const string SPECIAL_TAG = "special";
        private const string SPELLS_KNOWN_TAG = "spellsknown";
        private const string SPELLS_MEMORIZED_TAG = "spellsmemorized";
        private const string SPELL_BOOK_TAG = "spellbook";
        private const string SPELL_TAG = "spell";
        private const string SPELL_LEVEL_TAG = "spelllevel";
        private const string FEATS_TAG = "feats";
        private const string ANIMAL_TRICKS_TAG = "animal_tricks";
        private const string TRAITS_TAG = "traits";
        private const string CLASS_TAG = "class";
        private const string SKILL_TAG = "skill";
        private const string WEAPON_TAG = "weapon";
        private const string MELEE_TAG = "melee";
        private const string RANGED_ATTACK_TAG = "rangedattack";
        private const string ARMOR_TAG = "armor";
        private const string ITEM_TAG = "item";
        private const string WEIGHT_TAG = "weight";
        private const string COST_TAG = "cost";
        private const string FEAT_TAG = "feat";
        private const string ANIMAL_TRICK_TAG = "animaltrick";
        private const string TRAIT_TAG = "trait";
        private const string CHALLENGE_RATING_TAG = "challengerating";
        private const string XP_AWARD_TAG = "xpaward";
        private const string ARCANE_SPELL_FAILURE_TAG = "arcanespellfailure";
        private const string TYPE_TAG = "type";
        private const string SUBTYPE_TAG = "subtype";
        private const string HERO_POINTS_TAG = "heropoints";
        private const string AURAS_TAG = "auras";
        private const string DESCRIPTION_TAG = "description";
        private const string DEFENSIVE_TAG = "defensive";
        private const string DAMAGE_REDUCTION_TAG = "damagereduction";
        private const string SKILL_ABILITIES_TAG = "skillabilities";
        private const string RANGED_TAG = "ranged";
        private const string TRACKED_RESOURCES_TAG = "trackedresources";
        private const string TRACKED_RESOURCE_TAG = "trackedresource";
        private const string OTHER_SPECIALS_TAG = "otherspecials";
        private const string SPELL_CLASSES_TAG = "spellclasses";
        private const string SPELL_CLASS_TAG = "spellclass";
        private const string MINIONS_TAG = "minions";
        private const string RESISTANCES_TAG = "resistances";
        private const string ENCUMBRANCE_TAG = "encumbrance";
        private const string PROGRAM_INFO_TAG = "programinfo";
        // new tags need adding to list

        // Attributes
        private const string RACE_TEXT_ATTRIBUTE = "racetext";
        private const string SUMMARY_ATTRIBUTE = "summary";
        private const string TOTAL_ATTRIBUTE = "total";
        private const string COUNT_ATTRIBUTE = "count";
        private const string PLATINUM_PIECES_ATTRIBUTE = "pp";
        private const string GOLD_PIECES_ATTRIBUTE = "gp";
        private const string SILVER_PIECES_ATTRIBUTE = "sp";
        private const string COPPER_PIECES_ATTRIBUTE = "cp";
        private const string VALUABLES_ATTRIBUTE = "valuables";
        private const string GENDER_ATTRIBUTE = "gender";
        private const string AGE_ATTRIBUTE = "age";
        private const string HAIR_ATTRIBUTE = "hair";
        private const string EYES_ATTRIBUTE = "eyes";
        private const string SKIN_ATTRIBUTE = "skin";
        private const string TEXT_ATTRIBUTE = "text";
        private const string VALUE_ATTRIBUTE = "value";
        private const string SHORT_NAME_ATTRIBUTE = "shortname";
        private const string BASE_ATTRIBUTE = "base";
        private const string MODIFIED_ATTRIBUTE = "modified";
        private const string HIT_POINTS_ATTRIBUTE = "hitpoints";
        private const string CURRENT_HIT_POINTS_ATTRIBUTE = "currenthp";
        private const string NON_LETHAL_ATTRIBUTE = "nonlethal";
        private const string AC_ATTRIBUTE = "ac";
        private const string TOUCH_ATTRIBUTE = "touch";
        private const string FLAT_FOOTED_ATTRIBUTE = "flatfooted";
        private const string FROM_ARMOR_ATTRIBUTE = "fromarmor";
        private const string FROM_SHIELD_ATTRIBUTE = "fromshield";
        private const string FROM_DEXTERITY_ATTRIBUTE = "fromdexterity";
        private const string FROM_SIZE_ATTRIBUTE = "fromsize";
        private const string FROM_NATURAL_ATTRIBUTE = "fromnatural";
        private const string FROM_DEFLECT_ATTRIBUTE = "fromdeflect";
        private const string FROM_DODGE_ATTRIBUTE = "fromdodge";
        private const string FROM_MISC_ATTRIBUTE = "frommisc";
        private const string FROM_ATTRIBUTE_ATTRIBUTE = "fromattr";
        private const string ATTRIBUTE_TEXT_ATTRIBUTE = "attrtext";
        private const string MISC_TEXT_ATTRIBUTE = "misctext";
        private const string BASE_ATTACK_ATTRIBUTE = "baseattack";
        private const string CMD_ATTRIBUTE = "cmb";
        private const string CLASS_ATTRIBUTE = "class";
        private const string NAME_ATTRIBUTE = "name";
        private const string LEVEL_ATTRIBUTE = "level";
        private const string CARRIED_ATTRIBUTE = "carried";
        private const string CAST_TIME_ATTRIBUTE = "casttime";
        private const string RANGE_ATTRIBUTE = "range";
        private const string TARGET_ATTRIBUTE = "target";
        private const string AREA_ATTRIBUTE = "area";
        private const string EFFECT_ATTRIBUTE = "effect";
        private const string DURATION_ATTRIBUTE = "duration";
        private const string SAVE_ATTRIBUTE = "save";
        private const string DC_ATTRIBUTE = "dc";
        private const string CASTER_LEVEL_ATTRIBUTE = "casterlevel";
        private const string COMPONENT_TEXT_ATTRIBUTE = "componenttext";
        private const string SCHOOL_TEXT_ATTRIBUTE = "schooltext";
        private const string RESIST_ATTRIBUTE = "resist";
        private const string CASTS_LEFT_ATTRIBUTE = "castsleft";
        private const string MAX_CASTS_ATTRIBUTE = "maxcasts";
        private const string UNLIMITED_ATTRIBUTE = "unlimited";
        private const string RANKS_ATTRIBUTE = "ranks";
        private const string TRAINED_ONLY_ATTRIBUTE = "trainedonly";
        private const string ATTRIBUTE_BONUS_ATTRIBUTE = "attrbonus";
        private const string ATTRIBUTE_NAME_ATTRIBUTE = "attrname";
        private const string CATEGORY_TEXT_ATTRIBUTE = "categorytext";
        private const string TYPE_TEXT_ATTRIBUTE = "typetext";
        private const string SIZE_ATTRIBUTE = "size";
        private const string ATTACK_ATTRIBUTE = "attack";
        private const string FLURRY_ATTACK_ATTRIBUTE = "flurryattack";
        private const string DAMAGE_ATTRIBUTE = "damage";
        private const string CRIT_ATTRIBUTE = "crit";
        private const string EQUIPPED_ATTRIBUTE = "equipped";
        private const string RANGE_INCREMENT_VALUE_ATTRIBUTE = "rangeincvalue";
        private const string CMB_ATTRIBUTE = "cmb";
        private const string ROLE_ATTRIBUTE = "role";
        private const string PLAYER_NAME_ATTRIBUTE = "playername";
        private const string TYPE_ATTRIBUTE = "type";
        private const string RELATIONSHIP_ATTRIBUTE = "relationship";
        private const string NATURE_ATTRIBUTE = "nature";
        private const string CASTER_SOURCE_ATTRIBUTE = "castersource";
        private const string BASE_SPELL_DC_ATTRIBUTE = "basespelldc";
        private const string SPELL_SAVE_DC_ATTRIBUTE = "spellsavedc";
        private const string OVERCOME_SPELL_RESISTANCE_ATTRIBUTE = "overcomespellresistance";
        private const string CONCENTRATION_CHECK_ATTRIBUTE = "concentrationcheck";
        private const string SPELLS_ATTRIBUTE = "spells";
        private const string ENABLED_ATTRIBUTE = "enabled";
        private const string SOURCE_TEXT_ATTRIBUTE = "sourcetext";
        private const string QUANTITY_ATTRIBUTE = "quantity";
        private const string MAX_ATTRIBUTE = "max";
        private const string MIN_ATTRIBUTE = "min";
        private const string LEFT_ATTRIBUTE = "left";
        private const string USED_ATTRIBUTE = "used";
        private const string MAX_SPELL_LEVEL_ATTRIBUTE = "maxspelllevel";

        // Values
        private const string PROGRAM_NAME_VALUE = "hero lab";
        private const string ARMOR_CHECK_PENALTY_VALUE = "armor check penalty";
        private const string MAX_DEX_BONUS_VALUE = "max dex bonus";
        private const string FORTITUDE_SAVE_VALUE = "fortitude save";
        private const string REFLEX_SAVE_VALUE = "reflex save";
        private const string WILL_SAVE_VALUE = "will save";
        private const string PATHFINDER_VALUE = "pathfinder";
        private const string D20_35E_VALUE = "'d20 system'";
        private const string D20_5E_VALUE = "system reference document 5.0";

        // Important feats
        private const string POINT_BLANK_SHOT_VALUE = "point-blank shot";
        private const string FAR_SHOT_VALUE = "far shot";
        private const string TWO_WEAPON_FIGHTING_VALUE = "two-weapon fighting";
        private const string GREATER_TWO_WEAPON_FIGHTING = "greater two-weapon fighting";
        private const string IMPROVED_TWO_WEAPON_FIGHTING = "improved two-weapon fighting";

        private readonly List<string> _tags = new List<string>
        {
            PROGRAM_TAG,
            CHARACTER_TAG,
            RACE_TAG,
            CLASSES_TAG,
            ALIGNMENT_TAG,
            DEITY_TAG,
            XP_TAG,
            MONEY_TAG,
            PERSONAL_TAG,
            CHARACTER_HEIGHT_TAG,
            LANGUAGE_TAG,
            SPEED_TAG,
            BASE_SPEED_TAG,
            SIZE_TAG,
            REACH_TAG,
            SENSES_TAG,
            MAGIC_ITEMS_TAG,
            CHARACTER_WEIGHT_TAG,
            ATTRIBUTES_TAG,
            ATTRIBUTE_TAG,
            ATTRIBUTE_VALUE_TAG,
            ATTRIBUTE_BONUS_TAG,
            HEALTH_TAG,
            ARMOR_CLASS_TAG,
            PENALTY_TAG,
            INITIATIVE_TAG,
            SKILLS_TAG,
            SAVES_TAG,
            SAVE_TAG,
            ALL_SAVES_TAG,
            SITUATIONAL_MODIFIERS_TAG,
            ATTACK_TAG,
            MANEUVERS_TAG,
            MANEUVER_TYPE_TAG,
            DEFENSES_TAG,
            EQUIPMENT_SETS_TAG,
            SPELL_LIKE_TAG,
            SPECIAL_TAG,
            SPELLS_KNOWN_TAG,
            SPELLS_MEMORIZED_TAG,
            SPELL_BOOK_TAG,
            SPELL_TAG,
            SPELL_LEVEL_TAG,
            FEATS_TAG,
            ANIMAL_TRICKS_TAG,
            TRAITS_TAG,
            CLASS_TAG,
            SKILL_TAG,
            WEAPON_TAG,
            MELEE_TAG,
            RANGED_ATTACK_TAG,
            ARMOR_TAG,
            ITEM_TAG,
            WEIGHT_TAG,
            COST_TAG,
            FEAT_TAG,
            ANIMAL_TRICK_TAG,
            TRAIT_TAG,
            CHALLENGE_RATING_TAG,
            XP_AWARD_TAG,
            ARCANE_SPELL_FAILURE_TAG,
            TYPE_TAG,
            SUBTYPE_TAG,
            HERO_POINTS_TAG,
            AURAS_TAG,
            DESCRIPTION_TAG,
            DEFENSIVE_TAG,
            DAMAGE_REDUCTION_TAG,
            SKILL_ABILITIES_TAG,
            RANGED_TAG,
            TRACKED_RESOURCES_TAG,
            TRACKED_RESOURCE_TAG,
            OTHER_SPECIALS_TAG,
            SPELL_CLASSES_TAG,
            SPELL_CLASS_TAG,
            MINIONS_TAG,
            RESISTANCES_TAG,
            ENCUMBRANCE_TAG,
            PROGRAM_INFO_TAG
        };


        private Dictionary<AttackType, string> _meleeAttackTypeTextMap = new Dictionary<AttackType, string>
        {
            [AttackType.Melee1HP] = "1H-P",
            [AttackType.Melee1HO] = "1H-O",
            [AttackType.Melee2H] = "2H",
            [AttackType.Melee2WPOH] = "2W-P-(OH)",
            [AttackType.Melee2WPOL] = "2W-P-(OL)",
            [AttackType.Melee2WOH] = "2W-OH"
        };

        private readonly List<string> _untrainedSkillsList = new List<string>
        {
            "acrobatics",
            "appraise",
            "bluff",
            "climb",
            "craft",
            "diplomacy",
            "disguise",
            "escape artist",
            "fly",
            "heal",
            "intimidate",
            "perception",
            "perform",
            "ride",
            "sense motive",
            "stealth",
            "survival",
            "swim",
    };

        private List<string> _lightMeleeWeaponsList = new List<string>
        {
            "atlatl dart",
            "battle aspergillum",
            "brass knuckles",
            "cestus",
            "dagger",
            "dart",
            "gauntlet",
            "javelin",
            "light mace",
            "locked gauntlet",
            "punching dagger",
            "sickle",
            "spiked gauntlet",
            "wooden stake",
            "butterfly sword",
            "dogslicer",
            "gladius",
            "handaxe",
            "iron brush",
            "jutte",
            "kerambit",
            "klar",
            "kukri",
            "light hammer",
            "light pick",
            "lungchuan tamo",
            "sap",
            "shang gou",
            "shortsword",
            "starknife",
            "throwing axe",
            "tonfa",
            "war razor",
            "wushu dart",
            "aklys",
            "dan bong",
            "emei piercer",
            "fighting fan",
            "kama",
            "knuckle axe",
            "net",
            "nunchaku",
            "pata",
            "quadrens",
            "sai",
            "scorpion whip",
            "siangham",
            "sica",
            "swordbreaker dagger",
            "tekko-kagi",
            "wakizashi"
    };

        private List<string> _twoHandedWeaponsList = new List<string>
        {
            "bayonet",
            "boar spear",
            "longspear",
            "quarterstaff",
            "spear",
            "bardiche",
            "bec de corbin",
            "bill",
            "earth breaker",
            "falchion",
            "glaive",
            "glaive-guisarme",
            "greataxe",
            "greatclub",
            "greatsword",
            "guisarme",
            "halberd",
            "heavy flail",
            "hooked lance",
            "horsechopper",
            "lance",
            "lucerne hammer",
            "mattock",
            "monk's spade",
            "naginata",
            "nodachi",
            "ogre hook",
            "ranseur",
            "rhomphaia",
            "sansetsukon",
            "scythe",
            "tepoztopilli",
            "tiger fork",
            "tri-point double edged sword",
            "bo staff",
            "chain spear",
            "dire flail",
            "double walking stick katana (x2)",
            "double-chained kama",
            "dwarven urgosh",
            "elven curve blade",
            "flying blade",
            "gnome hooked hammer",
            "harpoon",
            "kusarigama",
            "kyoketsu shoge",
            "mancatcher",
            "meteor hammer",
            "orc double axe",
            "scarf, bladed",
            "seven-branched sword",
            "snag net",
            "spiked chain",
            "taiaha",
            "tetsubo",
            "two-bladed sword"
    };

        private Dictionary<string, List<string>> _casterClassArchetypesList = new Dictionary<string, List<string>>
    {
        {
            "alchemist", new List<string>
            {
                "Alchemist",
                "Chirurgeon",
                "Clone Master",
                "Internal Alchemist",
                "Mind Chemist",
                "Preservationist",
                "Psychonaut",
                "Reanimator",
                "Vivisectionist"
            }
        },
        {
            "bard", new List<string>
            {
                "Bard",
                "Animal Speaker",
                "Celebrity",
                "Demagogue",
                "Dirge Bard",
                "Geisha",
                "Songhealer",
                "Sound Striker"
            }
        },
        {
            "cleric", new List<string>
            {
                "Cleric",
                "Cloistered Cleric",
                "Separatist",
                "Theologian",
                "Undead Lord"
            }
        },
        {
            "druid", new List<string>
            {
                "Druid",
                "Dragon Shaman",
                "Menhir Savant",
                "Mooncaller",
                "Pack Lord",
                "Reincarnated Druid",
                "Saurian Shaman",
                "Shark Shaman",
                "Storm Druid"
            }
        },
        {
            "inquisitor", new List<string>
            {
                "Inquisitor",
                "Exorcist",
                "Heretic",
                "Infiltrator",
                "Preacher",
                "Sin Eater"
            }
        },
        {
            "magus", new List<string>
            {
                "Magus",
                "Hexcrafter",
                "Spellblade",
                "Staff magus"
            }
        },
        {
            "monk", new List<string>
            {
                "Monk",
                "Qinggong Monk"
            }
        },
        {
            "oracle", new List<string>
            {
                "Oracle",
                "Dual - Cursed Oracle",
                "Enlightened Philosopher",
                "Planar Oracle",
                "Possessed Oracle",
                "Seer",
                "Stargazer"
            }
        },
        {
            "paladin", new List<string>
            {
                "Paladin"
            }
        },
        {
            "ranger", new List<string>
            {
                "Ranger",
                "Trapper"
            }
        },
        {
            "sorcerer", new List<string>
            {
                "Sorcerer",
                "Crossblooded",
                "Wildblooded"
            }
        },
        {
            "summoner", new List<string>
            {
                "Summoner",
                "Broodmaster",
                "Evolutionist",
                "Master Summoner",
                "Synthesist"
            }
        },
        {
            "witch", new List<string>
            {
                "Witch",
                "Beast - bonded",
                "Gravewalker",
                "Hedge Witch"
            }
        },
        {
            "wizard", new List<string>
            {
                "Wizard",
                "Conjurer",
                "Evoker",
                "Scrollmaster"
            }
        }
        };

        private bool _hasPointBlankShot;
        private bool _hasFarShot;
        private bool _hasTwoWeaponFighting;
        private bool _hasGreaterTwoWeaponFighting;
        private bool _hasImprovedTwoWeaponFighting;

        private int _minionsDepth;
        private GameSystem _gameSystem = GameSystem.Unknown;

        private CharacterStats _characterStats;
        private CharacterStats _tempCharacterStats;
        private AbilityScore _tempAbilityScore;
        private SavingThrow _tempSavingThrow;
        private CharacterClass _tempCharacterClass;
        private Feat _tempFeat;
        private Aura _tempAura;
        private Skill _tempSkill;
        private Weapon _tempWeapon;
        private Weapon _tempFlurryWeapon;
        private Armor _tempArmor;
        private Item _tempItem;
        private Spell _tempSpell;
        private SpellClass _tempSpellClass;
        private SpellClassLevel _tempSpellClassLevel;
        private Sense _tempSense;
        private SpellBook _unallocatedSpellBook;

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
        private SubdualDamage _subdualDamage;
        private Initiative _initiative;
        private SavingThrows _savingThrows;
        private Skills _skills;
        private Speed _speed;
        private Spells _spells;
        private Dictionary<string, TrackedResource> _trackedResources;

        private Region _region;

        private string _tagName;
        private string _retrievedText;

        private List<Spell> _tempSpellList;

        public HeroLabXMLReader()
        {
            _tempSpellList = new List<Spell>();
        }

        public CharacterStats parseCharacterXML(Stream inputStream)
        {
            Check.ForNullArgument(inputStream, "inputStream");

            _tagName = null;

            XmlNodeType eventType;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreComments = true;

            using (_xpp = XmlReader.Create(inputStream, readerSettings))
            {
                try
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
                catch (FifthEditionException)
                {
                    _characterStats = new HeroLabFifthEditionXMLReader().parseCharacterXML(_xpp);
                }
            }

            return _characterStats;
        }

        private void AllocateSpells(CharacterStats characterStats)
        {
            _unallocatedSpellBook = new SpellBook("Unallocated spells");

            var spellClassesList = _spells.SpellClasses.Select(x => x.Value).ToList();

            foreach (Spell spell in _tempSpellList)
            {
                if (!string.IsNullOrEmpty(spell.CasterClass))
                {
                    SpellClass matchingCasterClass = null;

                    var casterClassLower = spell.CasterClass.ToLower();

                    if (_casterClassArchetypesList.ContainsKey(casterClassLower))
                    {
                        var archetypes = _casterClassArchetypesList[casterClassLower].Select(x => x.ToLower()).ToList();
                        var emptyClassNames = spellClassesList.Select(x => x.Name.ToLower()).ToList();

                        var matchingClasses = archetypes.Intersect(emptyClassNames).ToList();

                        if (matchingClasses.Count() == 1)
                        {
                            matchingCasterClass = spellClassesList.FirstOrDefault(x => x.Name.ToLower() == matchingClasses.FirstOrDefault());
                        }
                    }

                    if (matchingCasterClass == null && spellClassesList.Count(x => x.CasterClass.ToLower().Contains(casterClassLower)) == 1)
                    {
                        matchingCasterClass = spellClassesList.FirstOrDefault(x => x.CasterClass.ToLower().Contains(casterClassLower));
                    }

                    if (matchingCasterClass == null &&!_spells.SpellClasses.ContainsKey(spell.CasterClass))
                    {
                        _spells.SpellClasses.Add(spell.CasterClass, new SpellClass(spell.CasterClass));
                    }
                    else if (matchingCasterClass != null)
                    {
                        spell.CasterClass = matchingCasterClass.CasterClass;
                    }
                    

                    if (!_spells.SpellClasses[spell.CasterClass].Levels.ContainsKey(spell.Level))
                    {
                        _spells.SpellClasses[spell.CasterClass]
                               .Levels.Add(spell.Level, new SpellClassLevel(spell.Level.ToString(), "0"));
                    }

                    _spells.SpellClasses[spell.CasterClass]
                               .Levels[spell.Level]
                               .AddSpell(spell);
                }
                else
                {
                    _unallocatedSpellBook.Spells.Add(spell);
                }
            }
            
            spellClassesList = spellClassesList.Where(x => !x.PopulatedLevels.Any()).ToList();

            if (spellClassesList.Count == 1)
            {
                var orphanSpellClasses = _spells.SpellClasses.Select(x => x.Value).Where(y => y.Levels.Any(z => z.Value.Max == 0)).ToList();

                if (orphanSpellClasses.Count == 1)
                {
                    var emptySpellClass = spellClassesList.FirstOrDefault();
                    var orphanSpellClass = orphanSpellClasses.FirstOrDefault();

                    for (int i = 0; i < orphanSpellClass.Levels.Count; i++)
                    {
                        emptySpellClass.Levels[i].Spells = orphanSpellClass.Levels[i].Spells;
                    }

                    _spells.SpellClasses.Remove(orphanSpellClass.Name);
                }
            }

            if (_unallocatedSpellBook.Spells.Any())
            {
                _spells.SpellBooks.Add(_unallocatedSpellBook);
            }
        }

        private void ProcessCombatManeuvers(CharacterStats characterStats)
        {
            foreach (var maneuver in characterStats.CombatManeuvers.Maneuvers)
            {
                maneuver.Value.DetermineDifferences();
            }
        }

        private void MatchUpAmmo(CharacterStats characterStats)
        {
            var rangedWeapons = characterStats.Equipment.Weapons.Where(x => x.Ranged != null);

            var ammo = rangedWeapons.SelectMany(x => x.Ranged.AmmunitionList).ToList();

            ammo.ForEach(x =>
            {
                if (characterStats.TrackedResources.Count(y => y.Value.Max == x.Remaining) == 1)
                {
                    x.Name = characterStats.TrackedResources.FirstOrDefault(y => y.Value.Max == x.Remaining).Value.Name;
                }
            });
        }

        private void ProcessText()
        {
            if (_tags.Contains(_tagName))
            {
                if (PrepareText())
                {
                    switch (_tagName)
                    {
                        case DESCRIPTION_TAG:
                            DescriptionTag();
                            break;
                        case PROGRAM_INFO_TAG:
                            ProgramInfoTag();
                            break;
                    }
                }
            }
        }

        private void ProgramInfoTag()
        {
            string programInfo = _retrievedText;

            if (!string.IsNullOrEmpty(programInfo))
            {
                programInfo = programInfo.ToLower();

                if (programInfo.Contains(PATHFINDER_VALUE))
                {
                    _gameSystem = GameSystem.Pathfinder;
                }
                if (programInfo.Contains(D20_35E_VALUE))
                {
                    _gameSystem = GameSystem.D20System;
                }
                else if (programInfo.Contains(D20_5E_VALUE))
                {
                    throw new FifthEditionException();
                }
            }
        }

        private void ProcessEndTag()
        {
            _tagName = _xpp.Name.ToLower();

            if (_tags.Contains(_tagName))
            {
                switch (_tagName)
                {
                    case ATTRIBUTE_TAG:
                        AttributeEndTag();
                        break;
                    case CLASS_TAG:
                        ClassEndTag();
                        break;
                    case FEAT_TAG:
                        FeatEndTag();
                        break;
                    case SPECIAL_TAG:
                        SpecialEndTag();
                        break;
                    case SKILL_TAG:
                        SkillEndTag();
                        break;
                    case WEAPON_TAG:
                        WeaponEndTag();
                        break;
                    case ARMOR_TAG:
                        ArmorEndTag();
                        break;
                    case ITEM_TAG:
                        ItemEndTag();
                        break;
                    case SPELL_TAG:
                        SpellEndTag();
                        break;
                    case SPELL_CLASS_TAG:
                        SpellClassEndTag();
                        break;
                    case CHARACTER_TAG:
                        CharacterEndTag();
                        break;
                    case MINIONS_TAG:
                        MinionsEndTag();
                        break;
                }
            }
        }

        private void MinionsEndTag()
        {
            _minionsDepth -= 1;
        }

        private void TidyUpCharacterStats()
        {
            AllocateSpells(_tempCharacterStats);
            ProcessCombatManeuvers(_tempCharacterStats);
            MatchUpAmmo(_tempCharacterStats);
        }

        private void CharacterEndTag()
        {
            if (_minionsDepth > 0)
            {
                TidyUpCharacterStats();
                _characterStats.Minions.Add(_tempCharacterStats);
            }
        }

        private void SpellClassEndTag()
        {
            _spells.SpellClasses.Add(_tempSpellClass.CasterClass, _tempSpellClass);
        }


        private void SpellEndTag()
        {
            _tempSpellList.Add(_tempSpell);
        }

        private void ItemEndTag()
        {
            _gear.Items.Add(_tempItem);
        }

        private void ArmorEndTag()
        {
            _equipment.Armor.Add(_tempArmor);
        }

        private void ReferenceCritical(Attack attack, Critical crit)
        {
            if (attack != null)
            {
                attack.Critical = crit;
            }
        }

        private void SetupCritAndAdd(Weapon weapon)
        {
            var crit = weapon.Critical;
            var mainAttack = weapon.Attack;
            var melee = weapon.Melee;
            var ranged = weapon.Ranged;

            if (mainAttack != null)
            {
                weapon.Attack.Critical = crit;
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
                foreach (var attack in weapon.Ranged.RangedAttacks)
                {
                    attack.Critical = crit;
                }
            }

            _equipment.Weapons.Add(weapon);
        }

        private void WeaponEndTag()
        {
            SetupCritAndAdd(_tempWeapon);

            if (_tempFlurryWeapon != null)
            {
                SetupCritAndAdd(_tempFlurryWeapon);
                _tempFlurryWeapon = null;
            }
        }

        private void FeatEndTag()
        {
            _feats.Add(_tempFeat);
        }

        private void SkillEndTag()
        {
            _skills.SkillsList.Add(_tempSkill);
        }

        private void SpecialEndTag()
        {
            switch (_region)
            {
                case Region.Defensive:
                case Region.Immunities:
                case Region.Resistances:
                case Region.SkillAbilities:
                case Region.Attack:
                case Region.SpellLike:
                case Region.OtherSpecials:
                    _feats.Add(_tempFeat);
                    break;
                case Region.Senses:
                    _bio.Senses.Add(_tempSense);
                    break;
            }
        }

        private void ClassEndTag()
        {
            _bio.CharacterClasses.Add(_tempCharacterClass);
        }

        private void AttributeEndTag()
        {
            _abilityScores.Scores[(AbilityScoreType)Enum.Parse(typeof(AbilityScoreType), Prepare.ForStringToEnumConversion(_tempAbilityScore.Name))] = _tempAbilityScore;
        }

        private void ProcessStartTag()
        {
            _tagName = _xpp.Name.ToLower();

            if (_tags.Contains(_tagName))
            {
                switch (_tagName)
                {
                    case PROGRAM_TAG:
                        ProgramTag();
                        break;
                    case CHARACTER_TAG:
                        CharacterTag();
                        break;
                    case RACE_TAG:
                        RaceTag();
                        break;
                    case CLASSES_TAG:
                        ClassesTag();
                        break;
                    case ALIGNMENT_TAG:
                        AlignmentTag();
                        break;
                    case DEITY_TAG:
                        DeityTag();
                        break;
                    case XP_TAG:
                        XpTag();
                        break;
                    case MONEY_TAG:
                        MoneyTag();
                        break;
                    case PERSONAL_TAG:
                        PersonalTag();
                        break;
                    case CHARACTER_HEIGHT_TAG:
                        CharacterHeightTag();
                        break;
                    case LANGUAGE_TAG:
                        LanguageTag();
                        break;
                    case SPEED_TAG:
                        SpeedTag();
                        break;
                    case BASE_SPEED_TAG:
                        BaseSpeedTag();
                        break;
                    case SIZE_TAG:
                        SizeTag();
                        break;
                    case REACH_TAG:
                        ReachTag();
                        break;
                    case SENSES_TAG:
                        SensesTag();
                        break;
                    case SPECIAL_TAG:
                        SpecialTag();
                        break;
                    case MAGIC_ITEMS_TAG:
                        MagicItemsTag();
                        break;
                    case CHARACTER_WEIGHT_TAG:
                        CharacterWeightTag();
                        break;
                    case ATTRIBUTES_TAG:
                        AttributesTag();
                        break;
                    case ATTRIBUTE_TAG:
                        AttributeTag();
                        break;
                    case ATTRIBUTE_VALUE_TAG:
                        AttributeValueTag();
                        break;
                    case ATTRIBUTE_BONUS_TAG:
                        AttributeBonusTag();
                        break;
                    case HEALTH_TAG:
                        HealthTag();
                        break;
                    case ARMOR_CLASS_TAG:
                        ArmorClassTag();
                        break;
                    case PENALTY_TAG:
                        PenaltyTag();
                        break;
                    case INITIATIVE_TAG:
                        InitiativeTag();
                        break;
                    case SKILLS_TAG:
                        SkillsTag();
                        break;
                    case SAVES_TAG:
                        SavesTag();
                        break;
                    case SAVE_TAG:
                        SaveTag();
                        break;
                    case SITUATIONAL_MODIFIERS_TAG:
                        SituationalModifiersTag();
                        break;
                    case ALL_SAVES_TAG:
                        AllSavesTag();
                        break;
                    case ATTACK_TAG:
                        AttackTag();
                        break;
                    case MANEUVERS_TAG:
                        ManeuversTag();
                        break;
                    case MANEUVER_TYPE_TAG:
                        ManeuverTypeTag();
                        break;
                    case DEFENSES_TAG:
                        DefensesTag();
                        break;
                    case EQUIPMENT_SETS_TAG:
                        EquipmentSetsTag();
                        break;
                    case SPELL_LIKE_TAG:
                        SpellLikeTag();
                        break;
                    case CHALLENGE_RATING_TAG:
                        ChallengeRatingTag();
                        break;
                    case XP_AWARD_TAG:
                        XpAwardTag();
                        break;
                    case CLASS_TAG:
                        ClassTag();
                        break;
                    case ARCANE_SPELL_FAILURE_TAG:
                        ArcaneSpellFailureTag();
                        break;
                    case TYPE_TAG:
                        TypeTag();
                        break;
                    case SUBTYPE_TAG:
                        SubtypeTag();
                        break;
                    case HERO_POINTS_TAG:
                        HeroPointsTag();
                        break;
                    case AURAS_TAG:
                        AurasTag();
                        break;
                    case DEFENSIVE_TAG:
                        DefensiveTag();
                        break;
                    case DAMAGE_REDUCTION_TAG:
                        DamageReductionTag();
                        break;
                    case SKILL_TAG:
                        SkillTag();
                        break;
                    case SKILL_ABILITIES_TAG:
                        SkillAbilitiesTag();
                        break;
                    case FEATS_TAG:
                        FeatsTag();
                        break;
                    case TRAIT_TAG:
                    case ANIMAL_TRICK_TAG:
                    case FEAT_TAG:
                        FeatSpecialTag();
                        break;
                    case TRAITS_TAG:
                        TraitsTag();
                        break;
                    case ANIMAL_TRICKS_TAG:
                        AnimalTricksTag();
                        break;
                    case MELEE_TAG:
                        MeleeTag();
                        break;
                    case WEAPON_TAG:
                        WeaponTag();
                        break;
                    case RANGED_ATTACK_TAG:
                        RangedAttackTag();
                        break;
                    case RANGED_TAG:
                        RangedTag();
                        break;
                    case ARMOR_TAG:
                        ArmorTag();
                        break;
                    case ITEM_TAG:
                        ItemTag();
                        break;
                    case WEIGHT_TAG:
                        WeightTag();
                        break;
                    case COST_TAG:
                        CostTag();
                        break;
                    case TRACKED_RESOURCES_TAG:
                        TrackedResourcesTag();
                        break;
                    case TRACKED_RESOURCE_TAG:
                        TrackedResourceTag();
                        break;
                    case OTHER_SPECIALS_TAG:
                        OtherSpecialsTag();
                        break;
                    case SPELLS_KNOWN_TAG:
                        SpellsKnownTag();
                        break;
                    case SPELLS_MEMORIZED_TAG:
                        SpellsMemorizedTag();
                        break;
                    case SPELL_BOOK_TAG:
                        SpellBookTag();
                        break;
                    case SPELL_TAG:
                        SpellTag();
                        break;
                    case SPELL_CLASSES_TAG:
                        SpellClassesTag();
                        break;
                    case SPELL_CLASS_TAG:
                        SpellClassTag();
                        break;
                    case SPELL_LEVEL_TAG:
                        SpellLevelTag();
                        break;
                    case MINIONS_TAG:
                        MinionsTag();
                        break;
                    case RESISTANCES_TAG:
                        ResistancesTag();
                        break;
                    case ENCUMBRANCE_TAG:
                        EncumbranceTag();
                        break;
                }
            }
        }

        private void EncumbranceTag()
        {
            _gear.EncumbranceLevel = GetValue(LEVEL_ATTRIBUTE);
        }

        private void ResistancesTag()
        {
            _region = Region.Resistances;
        }

        private void MinionsTag()
        {
            TidyUpCharacterStats();
            _tempSpellList = new List<Spell>();

            if (_minionsDepth == 0)
            {
                _characterStats = _tempCharacterStats;
            }

            if (!_xpp.IsEmptyElement)
            {
                _minionsDepth += 1;
            }
        }

        private void SpellLevelTag()
        {
            string maxCasts = GetValue(MAX_CASTS_ATTRIBUTE);

            if (maxCasts == null)
            {
                maxCasts = GetValue(UNLIMITED_ATTRIBUTE);
            }

            _tempSpellClassLevel = new SpellClassLevel(GetValue(LEVEL_ATTRIBUTE), maxCasts);
            _tempSpellClass.Levels.Add(_tempSpellClassLevel.Level, _tempSpellClassLevel);
        }

        private void SpellClassTag()
        {
            _tempSpellClass = new SpellClass(GetValue(NAME_ATTRIBUTE), GetValue(SPELLS_ATTRIBUTE), GetValue(MAX_SPELL_LEVEL_ATTRIBUTE));
        }

        private void SpellClassesTag()
        {
            _region = Region.SpellClasses;
        }

        private void SpellTag()
        {
            _tempSpell = new Spell(GetValue(NAME_ATTRIBUTE), GetValue(LEVEL_ATTRIBUTE), GetValue(CLASS_ATTRIBUTE));
            _tempSpell.CasterLevel = GetValue(CASTER_LEVEL_ATTRIBUTE);
            _tempSpell.Save = GetValue(SAVE_ATTRIBUTE);
            _tempSpell.SpellResistance = GetValue(RESIST_ATTRIBUTE);
            _tempSpell.School = GetValue(SCHOOL_TEXT_ATTRIBUTE);
            _tempSpell.Components = GetValue(COMPONENT_TEXT_ATTRIBUTE);
            _tempSpell.DC = GetValue(DC_ATTRIBUTE);
            _tempSpell.Duration = GetValue(DURATION_ATTRIBUTE);
            _tempSpell.Effect = GetValue(EFFECT_ATTRIBUTE);
            _tempSpell.Area = GetValue(AREA_ATTRIBUTE);
            _tempSpell.Target = GetValue(TARGET_ATTRIBUTE);
            _tempSpell.Range = GetValue(RANGE_ATTRIBUTE);
            _tempSpell.CastTime = GetValue(CAST_TIME_ATTRIBUTE);

            _tempSpell.SetCastsLeft(GetValue(CASTS_LEFT_ATTRIBUTE));
        }

        private void SpellBookTag()
        {
            _region = Region.SpellBook;
        }

        private void SpellsMemorizedTag()
        {
            _region = Region.SpellsMemorized;
        }

        private void SpellsKnownTag()
        {
            _region = Region.SpellsKnown;
        }

        private void OtherSpecialsTag()
        {
            _region = Region.OtherSpecials;
        }

        private void TrackedResourceTag()
        {
            TrackedResource resource = new TrackedResource(GetValue(NAME_ATTRIBUTE), GetValue(MAX_ATTRIBUTE), GetValue(MIN_ATTRIBUTE));
            resource.SetLeft(GetValue(LEFT_ATTRIBUTE));
            resource.SetUsed(GetValue(USED_ATTRIBUTE));

            if (!_trackedResources.Keys.Contains(resource.Name))
            {
                _trackedResources.Add(resource.Name, resource);
            }
        }

        private void TrackedResourcesTag()
        {
            _region = Region.TrackedResources;
        }

        private void GearTag()
        {
            _region = Region.Gear;
        }

        private void CostTag()
        {
            switch (_region)
            {
                case Region.Gear:
                case Region.MagicItems:
                    _tempItem.SetCost(GetValue(VALUE_ATTRIBUTE));
                    break;
            }

        }

        private void WeightTag()
        {
            switch (_region)
            {
                case Region.Gear:
                case Region.MagicItems:
                    _tempItem.SetWeight(GetValue(VALUE_ATTRIBUTE));
                    break;
            }
        }

        private void ItemTag()
        {
            _tempItem = new Item(GetValue(NAME_ATTRIBUTE));
            _tempItem.SetQty(GetValue(QUANTITY_ATTRIBUTE));
        }

        private void ArmorTag()
        {
            _tempArmor = new Armor(GetValue(NAME_ATTRIBUTE));
            _tempArmor.SetArmorBonus(GetValue(AC_ATTRIBUTE));
        }

        private void RangedTag()
        {
            _region = Region.Ranged;
        }

        private void SetupRangedAttack(Weapon weapon, string attack)
        {
            weapon.Ranged = new Ranged(
                    _hasPointBlankShot,
                    _hasFarShot,
                    GetValue(RANGE_INCREMENT_VALUE_ATTRIBUTE),
                    attack,
                    weapon.Attack.GetDamage(0),
                    _tempCharacterStats,
                    _tempWeapon);
        }

        private void RangedAttackTag()
        {
            SetupRangedAttack(_tempWeapon, GetValue(ATTACK_ATTRIBUTE));

            if (_tempFlurryWeapon != null)
            {
                SetupRangedAttack(_tempFlurryWeapon, GetValue(FLURRY_ATTACK_ATTRIBUTE));
            }
        }

        private Attack CloneAttack(AttackType attackType, Attack originalAttack)
        {
            Attack newAttack = new Attack(attackType, string.Format("{0} | {1}", _tempWeapon.Name, _meleeAttackTypeTextMap[attackType]), originalAttack.GetDamage(0), Helpers.Convert.AttackToString(originalAttack.BaseToHit), _tempCharacterStats, _tempWeapon);

            return newAttack;
        }

        private void MeleeWeapon(string weaponNameForLookups, Weapon weapon)
        {
            Melee melee = new Melee();

            string weaponNameLower = weaponNameForLookups.ToLower();

            bool isLightWeapon = _lightMeleeWeaponsList.Contains(weaponNameLower);

            weapon.IsLightWeapon = isLightWeapon;

            Attack attack = weapon.Attack;

            melee.OneWeaponPrimaryHand = CloneAttack(AttackType.Melee1HP, attack);
            melee.OneWeaponOffhand = CloneAttack(AttackType.Melee1HO, attack);
            melee.TwoHands = CloneAttack(AttackType.Melee2H, attack);
            melee.TwoWeaponsPrimaryHandOtherHeavy = CloneAttack(AttackType.Melee2WPOH, attack);
            melee.TwoWeaponsPrimaryHandOtherLight = CloneAttack(AttackType.Melee2WPOL, attack);
            melee.TwoWeaponsOffhand = CloneAttack(AttackType.Melee2WOH, attack);

            if (_twoHandedWeaponsList.Contains(weaponNameLower))
            {
                weapon.Hand = "Both Hands";
            }
            else
            {
                int modifierValue = melee.OneWeaponPrimaryHand.BaseModifier;
                int strengthModTempValue = _tempCharacterStats.AbilityScores.Scores[AbilityScoreType.Strength].TempModifier;
                modifierValue -= strengthModTempValue;

                int halfModifierValue = modifierValue + (int)Math.Floor(0.5 * strengthModTempValue);
                int oneAndAHalfModifierValue = modifierValue + (int)Math.Floor(1.5 * strengthModTempValue);

                melee.OneWeaponOffhand.BaseModifier = halfModifierValue;
                melee.TwoWeaponsOffhand.BaseModifier = halfModifierValue;

                if (!isLightWeapon)
                {
                    melee.TwoHands.BaseModifier = oneAndAHalfModifierValue;
                }
            }

            int[] oneHP = melee.OneWeaponPrimaryHand.BaseToHit;
            int[] oneHO = new int[oneHP.Length];
            int[] twoWPOH = new int[oneHP.Length];
            int[] twoWPOL = new int[oneHP.Length];
            int[] twoWOH = new int[1];

            if (_hasTwoWeaponFighting)
            {
                for (int i = 0; i < oneHP.Length; i++)
                {
                    oneHO[i] = oneHP[i] - 2;
                    twoWPOH[i] = oneHP[i] - 2;
                    twoWPOL[i] = oneHP[i] - 2;

                    if (i == 0)
                    {
                        if (_hasImprovedTwoWeaponFighting)
                        {
                            twoWOH[i] = oneHP[i] - 7;
                        }
                        else if (_hasGreaterTwoWeaponFighting)
                        {
                            twoWOH[i] = oneHP[i] - 12;
                        }
                        else
                        {
                            twoWOH[i] = oneHP[i] - 2;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < oneHP.Length; i++)
                {
                    oneHO[i] = oneHP[i] - 4;
                    twoWPOH[i] = oneHP[i] - 6;
                    twoWPOL[i] = oneHP[i] - 4;

                    if (i == 0)
                    {
                        twoWOH[i] = oneHP[i] - 8;
                    }
                }
            }

            melee.OneWeaponOffhand.BaseToHit = oneHO;
            melee.TwoWeaponsPrimaryHandOtherHeavy.BaseToHit = twoWPOH;
            melee.TwoWeaponsPrimaryHandOtherLight.BaseToHit = twoWPOL;
            melee.TwoWeaponsOffhand.BaseToHit = twoWOH;

            weapon.Melee = melee;
        }

        private void CreateWeapon(Weapon weapon)
        {
            weapon.Type = GetValue(TYPE_TEXT_ATTRIBUTE);

            string size = GetValue(SIZE_ATTRIBUTE);

            if (size != null)
            {
                weapon.Size = size;
            }

            weapon.Critical = new Critical(GetValue(CRIT_ATTRIBUTE));

            string hand = GetValue(EQUIPPED_ATTRIBUTE);

            if (hand != null)
            {
                weapon.Hand = hand;
            }

            string categoryText = GetValue(CATEGORY_TEXT_ATTRIBUTE).ToLower();

            if (categoryText.ToLower().Contains(UNARMED_ATTACK_WEAPON_CATEGORY))
            {
                weapon.IsUnarmed = true;
            }

            if (categoryText.ToLower().Contains(SHIELD_BASH_WEAPON_CATEGORY))
            {
                weapon.IsNonStandardMelee = true;
            }

        }

        private void WeaponTag()
        {
            var weaponName = GetValue(NAME_ATTRIBUTE);

            _tempWeapon = new Weapon(weaponName);
            _tempWeapon.Attack = new Attack(AttackType.Melee1HP, weaponName, GetValue(DAMAGE_ATTRIBUTE), GetValue(ATTACK_ATTRIBUTE), _tempCharacterStats, _tempWeapon);

            CreateWeapon(_tempWeapon);

            if (_region == Region.Melee)
            {
                MeleeWeapon(weaponName, _tempWeapon);
            }

            if (GetValue(FLURRY_ATTACK_ATTRIBUTE) != null)
            {
                _tempFlurryWeapon = new Weapon(string.Format("{0} (Flurry)", weaponName));
                _tempFlurryWeapon.Attack = new Attack(AttackType.Melee1HP, _tempFlurryWeapon.Name, GetValue(DAMAGE_ATTRIBUTE), GetValue(FLURRY_ATTACK_ATTRIBUTE), _tempCharacterStats, _tempWeapon);

                CreateWeapon(_tempFlurryWeapon);

                if (_region == Region.Melee)
                {
                    MeleeWeapon(weaponName, _tempFlurryWeapon);
                }
            }

        }

        private void MeleeTag()
        {
            _region = Region.Melee;
        }

        private void AnimalTricksTag()
        {
            _region = Region.AnimalTricks;
        }

        private void TraitsTag()
        {
            _region = Region.Traits;
        }

        private void FeatsTag()
        {
            _region = Region.Feats;
        }

        private void SkillAbilitiesTag()
        {
            _region = Region.SkillAbilities;
        }

        private void SkillTag()
        {
            _tempSkill = new Skill(GetValue(NAME_ATTRIBUTE), _tempCharacterStats);

            string ability = GetValue(ATTRIBUTE_NAME_ATTRIBUTE);
            
            if (ability != null)
            {
                ability = ability.Length > 3 ? ability.Substring(0, 3).ToUpper() : ability;

                _tempSkill.Ability = ability;
            }

            _tempSkill.SetRanks(GetValue(RANKS_ATTRIBUTE));
            _tempSkill.SetMiscModifier((_tempSkill.SkillModifier - (_tempSkill.AbilityModifier + _tempSkill.MiscModifier + _tempSkill.Ranks)).ToString());
            _tempSkill.UseUntrained = string.Equals(GetValue(TRAINED_ONLY_ATTRIBUTE), "no", StringComparison.OrdinalIgnoreCase) || _untrainedSkillsList.Contains(_tempSkill.Name.ToLower());
        }

        private void DamageReductionTag()
        {
            _region = Region.DamageReduction;
        }

        private void DefensiveTag()
        {
            _region = Region.Defensive;
        }

        private void AurasTag()
        {
            _region = Region.Auras;
        }

        private void DescriptionTag()
        {
            switch (_region)
            {
                case Region.Auras:
                    SetDescription(_tempAura);
                    break;
                case Region.Personal:
                    SetDescription(_bio);
                    break;
                case Region.Defensive:
                case Region.Immunities:
                case Region.SkillAbilities:
                case Region.Feats:
                case Region.Attack:
                case Region.SpellLike:
                case Region.OtherSpecials:
                case Region.Resistances:
                    SetDescription(_tempFeat);
                    break;
                case Region.Skills:
                    SetDescription(_tempSkill);
                    break;
                case Region.Ranged:
                case Region.Melee:
                    SetWeaponDescription();
                    break;
                case Region.Defenses:
                    SetDescription(_tempArmor);
                    break;
                case Region.Gear:
                case Region.MagicItems:
                    SetDescription(_tempItem);
                    break;
                case Region.SpellBook:
                case Region.SpellsMemorized:
                case Region.SpellsKnown:
                    SetDescription(_tempSpell);
                    break;
                case Region.Senses:
                    SetDescription(_tempSense);
                    break;
            }
        }

        private void SetWeaponDescription()
        {
            SetDescription(_tempWeapon);

            if (_tempFlurryWeapon != null)
            {
                SetDescription(_tempFlurryWeapon);
            }
        }

        private void SetDescription(IHasDescription item)
        {
            string description = _retrievedText;

            if (!string.IsNullOrEmpty(description))
            {
                item.Description = description;
            }
        }

        private void AuraSpecialTag()
        {
            _tempAura = new Aura(GetValue(NAME_ATTRIBUTE), GetValue(SHORT_NAME_ATTRIBUTE), GetValue(TYPE_ATTRIBUTE));
            _tempAura.SourceText = GetValue(SOURCE_TEXT_ATTRIBUTE);
        }

        private void HeroPointsTag()
        {
            _bio.SetHeroPoints(GetValue(TOTAL_ATTRIBUTE));
            _bio.SetHeroPointsEnabled(GetValue(ENABLED_ATTRIBUTE));
        }

        private void SubtypeTag()
        {
            _bio.Subtypes.Add(GetValue(NAME_ATTRIBUTE));
        }

        private void TypeTag()
        {
            _bio.Types.Add(GetValue(NAME_ATTRIBUTE));
        }

        private void ArcaneSpellFailureTag()
        {
            _tempCharacterClass.ArcaneSpellFailure = GetValue(TEXT_ATTRIBUTE);
        }

        private void ClassTag()
        {
            _tempCharacterClass = new CharacterClass(GetValue(NAME_ATTRIBUTE));
            _tempCharacterClass.SetLevel(GetValue(LEVEL_ATTRIBUTE));
            _tempCharacterClass.SetCasterSource(GetValue(CASTER_SOURCE_ATTRIBUTE));

            string spellDc = GetValue(BASE_SPELL_DC_ATTRIBUTE);

            if (spellDc == null)
            {
                spellDc = GetValue(SPELL_SAVE_DC_ATTRIBUTE);
            }

            if (spellDc != null)
            {
                _tempCharacterClass.SetBaseSpellDc(spellDc);
            }

            _tempCharacterClass.OvercomeSpellResistance = GetValue(OVERCOME_SPELL_RESISTANCE_ATTRIBUTE);
            _tempCharacterClass.ConcentrationCheck = GetValue(CONCENTRATION_CHECK_ATTRIBUTE);
            _tempCharacterClass.SetCasterLevel(GetValue(CASTER_LEVEL_ATTRIBUTE));
            _tempCharacterClass.SetSpellSource(GetValue(SPELLS_ATTRIBUTE));
        }

        private void XpAwardTag()
        {
            _bio.XpAward = GetValue(TEXT_ATTRIBUTE);
        }

        private void ChallengeRatingTag()
        {
            _bio.ChallengeRating = GetValue(TEXT_ATTRIBUTE);
        }

        private void SpellLikeTag()
        {
            _region = Region.SpellLike;
        }

        private void EquipmentSetsTag()
        {
            _region = Region.EquipmentSets;
        }

        private void DefensesTag()
        {
            _region = Region.Defenses;
        }

        private void ManeuverTypeTag()
        {
            var name = GetValue(NAME_ATTRIBUTE);
            CombatManeuver maneuver = new CombatManeuver(_tempCharacterStats, name);
            maneuver.SetCmb(GetValue(CMB_ATTRIBUTE));
            maneuver.SetCmd(GetValue(CMD_ATTRIBUTE));

            _combatManeuvers.Maneuvers.Add(name, maneuver);
        }

        private void ManeuversTag()
        {
            _region = Region.Maneuvers;
        }

        private void AttackTag()
        {
            _region = Region.Attack;

            AttackBonus bab = _attackBonuses.Bonuses[AttackBonusType.Bab];
            AttackBonus ranged = _attackBonuses.Bonuses[AttackBonusType.Ranged];
            AttackBonus melee = _attackBonuses.Bonuses[AttackBonusType.Melee];
            AttackBonus cmb = _attackBonuses.Bonuses[AttackBonusType.Cmb];

            string babValue = GetValue(BASE_ATTACK_ATTRIBUTE);

            bab.SetBase(babValue);
        }

        private void AllSavesTag()
        {
            _region = Region.AllSaves;
        }

        private void SituationalModifiersTag()
        {
            var value = GetValue(TEXT_ATTRIBUTE);

            if (!string.IsNullOrWhiteSpace(value))
            {
                switch (_region)
                {
                    case Region.Saves:
                        _savingThrows.ConditionalSaveModifiers = GetValue(TEXT_ATTRIBUTE);
                        break;
                    case Region.Maneuvers:
                        _combatManeuvers.Modifiers.Add(GetValue(TEXT_ATTRIBUTE));
                        break;
                }
            }
        }

        private void SaveTag()
        {
            string throwName = GetValue(NAME_ATTRIBUTE);

            SavingThrowType throwType = SavingThrowType.Unknown;
            string name = null;
            string statName = null;
            AbilityScoreType basedOn = AbilityScoreType.Unknown;

            switch (throwName.ToLower())
            {
                case FORTITUDE_SAVE_VALUE:
                    throwType = SavingThrowType.Fortitude;
                    name = "Fortitude";
                    statName = "Constitution";
                    basedOn = AbilityScoreType.Constitution;                    
                    break;
                case REFLEX_SAVE_VALUE:
                    throwType = SavingThrowType.Reflex;
                    name = "Reflex";
                    statName = "Dexterity";
                    basedOn = AbilityScoreType.Dexterity;
                    break;
                case WILL_SAVE_VALUE:
                    throwType = SavingThrowType.Will;
                    name = "Will";
                    statName = "Wisdom";
                    basedOn = AbilityScoreType.Wisdom;
                    break;
            }

            _tempSavingThrow = new SavingThrow(name, statName, _tempCharacterStats, basedOn);

            _tempSavingThrow.SetBase(GetValue(BASE_ATTRIBUTE));
            _tempSavingThrow.SetMisc(GetValue(FROM_MISC_ATTRIBUTE));

            _savingThrows.Throws.Add(throwType, _tempSavingThrow);
        }

        private void SavesTag()
        {
            _region = Region.Saves;
        }

        private void SkillsTag()
        {
            _region = Region.Skills;
        }

        private void InitiativeTag()
        {
            int total;
            int dex;

            int.TryParse(GetValue(TOTAL_ATTRIBUTE), out total);
            int.TryParse(GetValue(ATTRIBUTE_TEXT_ATTRIBUTE), out dex);

            _initiative.Misc = total - dex;
        }

        private void PenaltyTag()
        {

            switch (GetValue(NAME_ATTRIBUTE).ToLower())
            {
                case ARMOR_CHECK_PENALTY_VALUE:
                    _ac.SetArmorCheck(GetValue(VALUE_ATTRIBUTE));
                    break;
                case MAX_DEX_BONUS_VALUE:
                    _ac.SetMaxDex(GetValue(VALUE_ATTRIBUTE));
                    break;
            }
        }

        private void ArmorClassTag()
        {
            _ac.SetArmorBonus(GetValue(FROM_ARMOR_ATTRIBUTE));
            _ac.SetShieldBonus(GetValue(FROM_SHIELD_ATTRIBUTE));
            _ac.SetSizeBonus(GetValue(FROM_SIZE_ATTRIBUTE));
            _ac.SetNaturalArmor(GetValue(FROM_NATURAL_ATTRIBUTE));
            _ac.SetDeflectionBonus(GetValue(FROM_DEFLECT_ATTRIBUTE));
            _ac.SetDodgeBonus(GetValue(FROM_DODGE_ATTRIBUTE));
            _ac.SetMiscBonus(GetValue(FROM_MISC_ATTRIBUTE));
        }

        private void HealthTag()
        {
            _health.SetCurrentHp(GetValue(CURRENT_HIT_POINTS_ATTRIBUTE));
            _health.SetMaxHp(GetValue(HIT_POINTS_ATTRIBUTE));
            _subdualDamage.SetSubdualDamage(GetValue(NON_LETHAL_ATTRIBUTE));
        }

        private void AttributeBonusTag()
        {
            _tempAbilityScore.SetModifier(GetValue(BASE_ATTRIBUTE));
        }

        private void AttributeValueTag()
        {
            _tempAbilityScore.SetScore(GetValue(BASE_ATTRIBUTE));
            _tempAbilityScore.SetTempScore(GetValue(MODIFIED_ATTRIBUTE));
        }

        private void AttributeTag()
        {
            string attributeName = GetValue(NAME_ATTRIBUTE);
            _tempAbilityScore = new AbilityScore(attributeName, attributeName.ToUpper().Substring(0, 2));
        }

        private void AttributesTag()
        {
            _region = Region.Attributes;
        }

        private void CharacterWeightTag()
        {
            _bio.Weight = GetValue(TEXT_ATTRIBUTE);
        }

        private void MagicItemsTag()
        {
            _region = Region.MagicItems;
        }

        private void SpecialTag()
        {
            switch (_region)
            {
                case Region.Auras:
                    AuraSpecialTag();
                    break;
                case Region.DamageReduction:
                    _health.DamageReduction = GetValue(SHORT_NAME_ATTRIBUTE);
                    break;
                case Region.Defensive:
                case Region.Immunities:
                case Region.SkillAbilities:
                case Region.Attack:
                case Region.SpellLike:
                case Region.OtherSpecials:
                case Region.Resistances:
                    FeatSpecialTag();
                    break;
                case Region.Senses:
                    SenseSpecialTag();
                    break;
            }
        }

        private void SenseSpecialTag()
        {
            _tempSense = new Sense(GetValue(NAME_ATTRIBUTE));
        }

        private void FeatSpecialTag()
        {
            string featName = GetValue(NAME_ATTRIBUTE);
            _tempFeat = new Feat(featName);

            string featType = GetValue(TYPE_ATTRIBUTE);

            if (featType == null)
            {
                featType = Prepare.EnumConvertedToString(_region.ToString());
            }

            _tempFeat.Type = featType;

            switch (featName.ToLower())
            {
                case POINT_BLANK_SHOT_VALUE:
                    _hasPointBlankShot = true;
                    break;
                case FAR_SHOT_VALUE:
                    _hasFarShot = true;
                    break;
                case TWO_WEAPON_FIGHTING_VALUE:
                    _hasTwoWeaponFighting = true;
                    break;
                case GREATER_TWO_WEAPON_FIGHTING:
                    _hasGreaterTwoWeaponFighting = true;
                    break;
                case IMPROVED_TWO_WEAPON_FIGHTING:
                    _hasImprovedTwoWeaponFighting = true;
                    break;
            }
        }

        private void SensesTag()
        {
            _region = Region.Senses;
        }

        private void ReachTag()
        {
            _bio.Reach = GetValue(TEXT_ATTRIBUTE);
        }

        private void SizeTag()
        {
            _bio.SetSize(GetValue(NAME_ATTRIBUTE));
        }

        private void BaseSpeedTag()
        {
            _speed.SetBaseValue(GetValue(VALUE_ATTRIBUTE));
        }

        private void SpeedTag()
        {
            _speed.SetValue(GetValue(VALUE_ATTRIBUTE));
        }

        private void LanguageTag()
        {
            _bio.Languages.Add(GetValue(NAME_ATTRIBUTE));
        }

        private void CharacterHeightTag()
        {
            _bio.Height = GetValue(TEXT_ATTRIBUTE);
        }

        private void PersonalTag()
        {
            _bio.Gender = GetValue(GENDER_ATTRIBUTE);
            _bio.SetAge(GetValue(AGE_ATTRIBUTE));
            _bio.Hair = GetValue(HAIR_ATTRIBUTE);
            _bio.Eyes = GetValue(EYES_ATTRIBUTE);
            _bio.Skin = GetValue(SKIN_ATTRIBUTE);

            _region = Region.Personal;
        }
        
        private void MoneyTag()
        {
            _bio.Money.SetPP(GetValue(PLATINUM_PIECES_ATTRIBUTE));
            _bio.Money.SetGP(GetValue(GOLD_PIECES_ATTRIBUTE));
            _bio.Money.SetSP(GetValue(SILVER_PIECES_ATTRIBUTE));
            _bio.Money.SetCP(GetValue(COPPER_PIECES_ATTRIBUTE));

            var valuables = GetValue(VALUABLES_ATTRIBUTE);

            if (valuables != null)
            {
                _bio.Money.SetValuables(valuables);
            }
        }

        private void XpTag()
        {
            _bio.SetExperience(GetValue(TOTAL_ATTRIBUTE));
        }

        private void DeityTag()
        {
            _bio.Deity = GetValue(NAME_ATTRIBUTE);
        }

        private void AlignmentTag()
        {
            _bio.Alignment = GetValue(NAME_ATTRIBUTE);
        }

        private void ClassesTag()
        {
            _bio.SetLevel(GetValue(LEVEL_ATTRIBUTE));
            _bio.ClassDescription = GetValue(SUMMARY_ATTRIBUTE);
        }

        private void RaceTag()
        {
            _bio.Race = GetValue(RACE_TEXT_ATTRIBUTE);
        }

        private void CharacterTag()
        {
            _tempSpellList = new List<Spell>();
            _tempCharacterStats = new CharacterStats(GetValue(NAME_ATTRIBUTE));
            _tempCharacterStats.GameSystem = _gameSystem;

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
            _subdualDamage = _tempCharacterStats.SubdualDamage;
            _initiative = _tempCharacterStats.Initiative;
            _savingThrows = _tempCharacterStats.SavingThrows;
            _skills = _tempCharacterStats.Skills;
            _speed = _tempCharacterStats.Speed;
            _spells = _tempCharacterStats.Spells;
            _trackedResources = _tempCharacterStats.TrackedResources;

            _tempCharacterStats.PlayerName = GetValue(PLAYER_NAME_ATTRIBUTE);
            _bio.Role = GetValue(ROLE_ATTRIBUTE);
            _bio.CharacterType = GetValue(TYPE_ATTRIBUTE);
            _bio.Nature = GetValue(NATURE_ATTRIBUTE);
            _bio.Relationship = GetValue(RELATIONSHIP_ATTRIBUTE);
        }

        private void ProgramTag()
        {
            string program = GetValue(NAME_ATTRIBUTE);

            if (!program.Equals(PROGRAM_NAME_VALUE, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("The XML file does not appear to have been created by Hero Lab");
            }
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
    }

}
