using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Dropins.Chooser.Android;
using System.IO;
using CharacterSheet.Pathfinder.XMLReader;
using CharacterSheet.Pathfinder;
using System.IO.Compression;
using CharacterSheet.UI.Activities;
using CharacterSheet.UI.Dialogs;

namespace CharacterSheet.UI.Fragments
{
    public class ImportFragment : Android.Support.V4.App.Fragment
    {
        private DBChooser chooser;
        private const int dropboxRequest = 0; // You can change this if needed
        private const string dropboxAppKey = "ajga1t50mqplckl";
        private MainActivity _activity;
        private const string GOOGLE_DRIVE_FRAGMENT_TAG = "google_drive_fragment";

        public ImportFragment()
        {

        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            chooser = new DBChooser(dropboxAppKey);
            _activity = (MainActivity)Activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.import_fragment, null);

            _activity.CurrentTab = TabType.Unknown;
            _activity.UpdateOptionsMenu();
            _activity.Game.SelectedCharacter = null;

            var dropbox = (ImageView)root.FindViewById(Resource.Id.dropboxImageView);
            dropbox.Click += delegate { chooser.ForResultType(DBChooser.ResultType.FileContent).Launch(this, dropboxRequest); };

            var gooogleDrive = (ImageView)root.FindViewById(Resource.Id.googleDriveImageView);
            gooogleDrive.Click += delegate
            {
                Activity.SupportFragmentManager.BeginTransaction()
                                      .Replace(Resource.Id.main, new GoogleDriveOpenerFragment(), GOOGLE_DRIVE_FRAGMENT_TAG)
                                      .Commit();
            };

            var localStorage = (ImageView)root.FindViewById(Resource.Id.androidFolderImageView);
            localStorage.Click += delegate { var filePicker = new FilePickerDialog(this.Activity); };

            if (!_activity.Game.Campaigns.FirstOrDefault().Characters.Any())
            {
                //CreateDemoCharacter();
            }


            return root;
        }

        private void CreateDemoCharacter()
        {
            var demo = new CharacterStats("Tim");

            demo.AbilityScores.Scores[AbilityScoreType.Strength] =  new AbilityScore("Strength", "STR") { Modifier = 1, Score = 2 };
            demo.AbilityScores.Scores[AbilityScoreType.Dexterity] = new AbilityScore("Dexterity", "DEX") { Modifier = 4, Score = 5 };
            demo.AbilityScores.Scores[AbilityScoreType.Constitution] = new AbilityScore("Constitution", "CON") { Modifier = 2, Score = 14 };
            demo.AbilityScores.Scores[AbilityScoreType.Intelligence] = new AbilityScore("Intelligence", "INT") { Modifier = 10, Score = 11 };
            demo.AbilityScores.Scores[AbilityScoreType.Wisdom] = new AbilityScore("Wisdom", "WIS") { Modifier = 13, Score = 14 };
            demo.AbilityScores.Scores[AbilityScoreType.Charisma] = new AbilityScore("Charisma", "CHA") { Modifier = 16, Score = 17 };
            demo.AbilityScores.Scores[AbilityScoreType.Constitution].SetTempScore("14");
            demo.AC.SetArcaneFailure("1");
            demo.AC.SetArmorCheck("3");
            demo.AC.SetArmorBonus("2");
            demo.AC.Base = 4;
            demo.AC.DeflectionBonus = 5;
            demo.AC.DodgeBonus = 6;
            demo.AC.SetMaxDex("8");
            demo.AC.MiscBonus = 9;
            demo.AC.MissChance = "foo";
            demo.AC.NaturalArmor = 10;
            demo.AC.ShieldBonus = 11;
            demo.AC.SizeBonus = 12;
            demo.AC.SpellResist = 13;

            demo.AttackBonuses.Bonuses[AttackBonusType.Melee] = new AttackBonus("Melee", AttackBonusType.Melee, demo) { Epic = 2, Misc = 3, Adjustment = 5 };
            demo.AttackBonuses.Bonuses[AttackBonusType.Ranged] = new AttackBonus("Ranged", AttackBonusType.Ranged, demo) { Epic = 10, Misc = 11, Adjustment = 13 };
            demo.AttackBonuses.Bonuses[AttackBonusType.Cmb] = new AttackBonus("Cmb", AttackBonusType.Cmb, demo) { Epic = 18, Misc = 19, Adjustment = 21 };
            demo.AttackBonuses.Bonuses[AttackBonusType.Bab] = new AttackBonus("Bab", AttackBonusType.Bab, demo);

            demo.AttackBonuses.Bonuses[AttackBonusType.Bab].SetBase("+8/+7");

            demo.AttackBonuses.ConditionalModifiers.Add("bar");
            demo.Bio.Level = 4;

            demo.Bio.Size = Size.Medium;

            demo.CombatManeuvers.Modifiers.Add("baz");

            demo.Health.SetCurrentHp("1");
            demo.Health.DamageReduction = "qux";
            demo.Health.SetMaxHp("40");
            demo.SubdualDamage.SetSubdualDamage("0");

            demo.Initiative.Misc = 2;

            demo.SavingThrows.Throws[SavingThrowType.Fortitude] = new SavingThrow("Fortitude", "Constitution", demo, AbilityScoreType.Constitution) { Base = 2, Epic = 3, Magic = 4, Misc = 5, Adjustment = 6 };
            demo.SavingThrows.Throws[SavingThrowType.Reflex] = new SavingThrow("Reflex", "Dexterity", demo, AbilityScoreType.Dexterity) { Base = 9, Epic = 10, Magic = 11, Misc = 12, Adjustment = 13 };
            demo.SavingThrows.Throws[SavingThrowType.Will] = new SavingThrow("Will", "Wisdom", demo, AbilityScoreType.Wisdom) { Base = 16, Epic = 17, Magic = 18, Misc = 19, Adjustment = 20 };
            demo.SavingThrows.ConditionalSaveModifiers = "hello";

            demo.Speed.Base = 1;
            demo.Speed.MovementType = "Walk";
            demo.Speed.Unit = "ft.";

            demo.Bio.Languages.Add("Common");
            demo.Bio.Languages.Add("Draconic");
            demo.Bio.WeaponProficiencies = "Knife and fork";
            demo.Bio.ClassDescription = "Demon";
            demo.Bio.Experience = 1000;
            demo.Bio.ExperienceNextLevel = 2000;
            demo.Bio.Senses.Add(new Sense("Dark Vision"));
            demo.Bio.Senses.Add(new Sense("Scent"));
            demo.Bio.Alignment = "Good";
            demo.Bio.Deity = "God";
            demo.Bio.Race = "Orc";
            demo.Bio.Age = 19;
            demo.Bio.Gender = "Male";
            demo.Bio.Size = Size.Medium;
            demo.Bio.Height = "6' 2\"";
            demo.Bio.Eyes = "Blue";
            demo.Bio.Hair = "Brown";
            demo.Bio.Money.CopperPieces = 10;
            demo.Bio.Money.SilverPieces = 20;
            demo.Bio.Money.GoldPieces = 30;
            demo.Bio.Money.PlatinumPieces = 40;
            demo.Bio.Money.Valuables = 50;

            demo.CombatManeuvers.Maneuvers.Add("Foo", new CombatManeuver(demo, "Foo"));
            demo.CombatManeuvers.Maneuvers.Add("Bar", new CombatManeuver(demo, "Bar"));
            demo.CombatManeuvers.Maneuvers.Add("Bull\r\nrush", new CombatManeuver(demo, "Bull rush"));

            demo.CombatManeuvers.Maneuvers["Foo"].SetCmb("+10");
            demo.CombatManeuvers.Maneuvers["Foo"].SetCmd("+20");
            demo.CombatManeuvers.Maneuvers["Foo"].DetermineDifferences();
            demo.CombatManeuvers.Maneuvers["Bar"].SetCmb("+15");
            demo.CombatManeuvers.Maneuvers["Bar"].SetCmd("+25");
            demo.CombatManeuvers.Maneuvers["Bar"].DetermineDifferences();
            demo.CombatManeuvers.Maneuvers["Bull\r\nrush"].SetCmb("+20");
            demo.CombatManeuvers.Maneuvers["Bull\r\nrush"].SetCmd("+30");
            demo.CombatManeuvers.Maneuvers["Bull\r\nrush"].DetermineDifferences();

            var armor = new Armor("Test Armor");

            armor.SetArmorBonus("99");
            armor.SetCheckPenalty("99");
            armor.SetMaxDexBonus("5");
            armor.SetSpellFailure("99");

            demo.Equipment.Armor.Add(armor);
            demo.Equipment.EquippedArmor = armor;

            demo.Minions.Add(new CharacterStats("Arnold"));
            demo.Minions.Add(new CharacterStats("Simon"));
            demo.Minions.Add(new CharacterStats("Fred"));

            var monkey = new CharacterStats("Monkey");
            monkey.Minions.Add(new CharacterStats("Barbara"));            

            _activity.AddCharacterToNavigationDrawer(demo);
            _activity.AddCharacterToNavigationDrawer(new CharacterStats("Adam"));
            _activity.AddCharacterToNavigationDrawer(monkey);
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == dropboxRequest)
            {
                if (resultCode == (int)Result.Ok)
                {
                    var result = new DBChooser.Result(data);
                    var fullFilePath = result.Link.Path;
                    var filename = result.Name;

                    if (filename.Contains(".xml"))
                    {
                        var reader = new PcGenXMLReader();
                        using (var inputStream = File.OpenRead(fullFilePath))
                        {
                            var character = reader.parseCharacterXML(inputStream);

                            if (character != null)
                            {
                                _activity.AddCharacterToNavigationDrawer(character);
                            }
                        }
                    }
                    else if (filename.Contains(".por"))
                    {
                        Dictionary<string, Stream> streams = new Dictionary<string, Stream>();

                        using (ZipArchive archive = ZipFile.OpenRead(fullFilePath))
                        {
                            var reader = new HeroLabXMLReader();

                            var characters = new List<CharacterStats>();

                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                if (entry.FullName.ToLower().Contains("statblocks_xml/"))
                                {
                                    var file = entry.FullName.Substring(entry.FullName.LastIndexOf("/") + 1);
                                    file = file.Substring(file.IndexOf("_"));
                                    file = file.Substring(0, file.LastIndexOf(".xml"));
                                    file = file.Replace("_", " ");

                                    var character = reader.parseCharacterXML(entry.Open());

                                    if (character != null)
                                    {
                                        characters.Add(character);
                                    }

                                    _activity.AddCharactersToNavigationDrawer(characters);
                                }
                            }
                        }
                    }

                    _activity.LoadCharacterFragment();
                }
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }
    }
}