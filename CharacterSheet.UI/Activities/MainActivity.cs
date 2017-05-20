using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Dropins.Chooser.Android;
using System.IO;
using Android.Content.Res;
using CharacterSheet.Pathfinder.XMLReader;
using Android.Views;
using System;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections.Generic;
using CharacterSheet.Pathfinder;
using Android.Graphics;
using Android.Text;
using CharacterSheet.UI.Fragments;
using Newtonsoft.Json;
using System.Linq;
using Android.Content.PM;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CharacterSheet.UI.GoogleDrive;
using Android.Runtime;
using CharacterSheet.UI.Fragments.Character;
using CharacterSheet.UI.Helpers;
using CharacterSheet.UI.Fragments.Skills;
using CharacterSheet.UI.Fragments.Attacks;
using CharacterSheet.UI.Fragments.Spells;
using CharacterSheet.UI.Fragments.Spells.Segments;
using static CharacterSheet.UI.Helpers.DeviceHelper;
using CharacterSheet.UI.Fragments.Feats;
using CharacterSheet.UI.Fragments.Items;
using CharacterSheet.UI.Dialogs;
using CharacterSheet.UI.Fragments.Notes;
using CharacterSheet.Droid.Backend.Pathfinder.Notes;

namespace CharacterSheet.UI.Activities
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.Orientation, HardwareAccelerated = true)]
    public class MainActivity : ActionBarActivity
    {
        private const string CHARACTER_SHEET_TITLE = "Character Sheet Reader";
        private const string SAVE_FILE_NAME = "character_importer_save";
        private const string GOOGLE_DRIVE_FRAGMENT_TAG = "google_drive_fragment";
        private const int GOOGLE_DRIVE_REQUEST_CODE = 1;

        private SupportToolbar mToolbar;
        private MyActionBarDrawerToggle _drawerToggle;
        private DrawerLayout _drawerLayout;
        private ExpandableListView _charactersDrawer;

        private IMenu _menu;

        private ListView _tabsDrawer;
        private SupportFragment mCurrentFragment = new SupportFragment();
        private Stack<SupportFragment> mStackFragments;

        private ExpandableCharacterListAdapter _charactersAdapter;
        private TabListAdapter _tabsAdapter;

        public List<ExpandableSpellsLevelListAdapter> CurrentSpellsLevelListAdapters { get; set; }
        public SpellSummarySegment CurrentSpellSummarySegment { get; set; }
        public ItemsFragment CurrentItemsFragment { get; set; }
        public NotesFragment CurrentNotesFragment { get; set; }

        public Game Game { get; private set; }

        private List<CharacterStats> _charactersDataSet;
        private Dictionary<string, TabType> _tabsDataSet;

        public FormatHelper FormatHelper { get; protected set; }

        public TabType CurrentTab { get; set; }
        public string CurrentFeatsKey { get; set; }

        private void ShowMenuItem(int resource, bool shouldBeVisible)
        {
            if (_menu != null)
            {
                var item = _menu.FindItem(resource);

                if (item != null && item.IsVisible != shouldBeVisible)
                {
                    item.SetVisible(shouldBeVisible);
                }
            }
        }

        private void ShowTabs(bool shouldBeVisible)
        {
            if (_menu != null)
            {
                var tabsItem = _menu.FindItem(Resource.Id.action_tabs);

                if (tabsItem != null && tabsItem.IsVisible != shouldBeVisible)
                {
                    tabsItem.SetVisible(shouldBeVisible);
                    if (!shouldBeVisible)
                    {
                        _drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed, _tabsDrawer);
                    }
                    else
                    {
                        _drawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked, _tabsDrawer);
                    }
                }
            }
        }

        public void AddCharacterToNavigationDrawer(CharacterStats character)
        {
            _charactersDataSet.Add(character);
            Game.SelectedCharacter = character;

            LoadCharacterFragment();
        }

        public void AddCharactersToNavigationDrawer(List<CharacterStats> characters)
        {
            _charactersDataSet.AddRange(characters);
            Game.SelectedCharacter = characters.First();

            LoadCharacterFragment();
        }

        private void DeleteCharacter()
        {
            _charactersDataSet.Remove(Game.SelectedCharacter);
            _charactersAdapter.NotifyDataSetChanged();
            Game.SelectedCharacter = _charactersDataSet.FirstOrDefault();
            
            if (Game.SelectedCharacter != null)
            {
                LoadCharacterFragment();
            }            
            else
            {
                LoadImportFragment();
            }
        }

        private void RetrieveSavedState()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = OpenFileInput(SAVE_FILE_NAME);
                Game = (Game)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception ex) when (ex is Java.IO.FileNotFoundException || ex is SerializationException)
            {
                Game = new Game();

                Game.Campaigns.Add(new Campaign("Default"));
            }
        }

        private void SaveState()
        {
            Task.Run(() =>
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = OpenFileOutput(SAVE_FILE_NAME, FileCreationMode.Private);
                formatter.Serialize(stream, Game);
                stream.Close();
            });
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
                        
            SetContentView(Resource.Layout.Main);

            FormatHelper = new FormatHelper(this, new DeviceHelper(this));
            
            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _charactersDrawer = FindViewById<ExpandableListView>(Resource.Id.characters_drawer);
            _tabsDrawer = FindViewById<ListView>(Resource.Id.tabs_drawer);
            mStackFragments = new Stack<SupportFragment>();

            SetSupportActionBar(mToolbar);

            string title = null;

            if (bundle != null)
            {
                var gameInstance = (GameInstance)bundle.GetSerializable("gameInstance");

                Game = new Game
                {
                    Campaigns = gameInstance.Campaigns,
                    SelectedCharacter = gameInstance.SelectedCharacter
                };
                
                title = bundle.GetString("title");
            }

            if (Game == null)
            {
                RetrieveSavedState();
            }

            if (_charactersDataSet == null)
            {
                _charactersDataSet = Game.Campaigns.FirstOrDefault().Characters;
            }

            Button importButton = new Button(this);
            importButton.Text = "Import";
            importButton.SetBackgroundColor(Color.ParseColor("#2196F3"));
            importButton.SetTextColor(Color.White);

            importButton.Click += delegate
            {
                LoadImportFragment();
            };

            _charactersDrawer.AddHeaderView(importButton);
            _charactersAdapter = new ExpandableCharacterListAdapter(this, _charactersDataSet);
            _charactersDrawer.SetAdapter(_charactersAdapter);
            _charactersDrawer.ChildClick += Characters_ChildClick;
            _charactersDrawer.ItemLongClick += Characters_ItemLongClick;
            _charactersDrawer.GroupClick += Characters_GroupClick;

            _tabsDrawer.ItemClick += Tab_Click;

            _charactersDrawer.HapticFeedbackEnabled = false;

            _drawerToggle = new MyActionBarDrawerToggle(
                this,                           //Host Activity
                _drawerLayout,                  //DrawerLayout
                mToolbar,                       //Toolbar
                Resource.String.openDrawer,     //Opened Message
                Resource.String.closeDrawer,    //Closed Message
                CHARACTER_SHEET_TITLE,
                _charactersDrawer,
                _tabsDrawer
            );

            _drawerLayout.SetDrawerListener(_drawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);

            _drawerToggle.SyncState();

            if (title == null)
            {
                title = Game.SelectedCharacter == null ? CHARACTER_SHEET_TITLE : Game.SelectedCharacter.Name;
            }

            SupportActionBar.TitleFormatted = new SpannableString(title);

            if (bundle == null)
            {
                if (!_charactersDataSet.Any())
                {
                    LoadImportFragment();
                }
                else
                {
                    LoadCharacterFragment();
                }
            }
            else
            {
                SetupTabs();
            }
        }

        public void LoadImportFragment()
        {
            SupportFragmentManager.BeginTransaction()
                                  .Replace(Resource.Id.main, new ImportFragment())
                                  .Commit();

            _drawerLayout.CloseDrawers();
            _drawerToggle.SyncState();

            var title = "Import from...";

            _drawerToggle.ClosedTitle = title;
            SupportActionBar.TitleFormatted = new SpannableString(title);
        }

        public void LoadCharacterFragment(bool force = true)
        {
            if (force || CurrentTab != TabType.Character)
            {
                LoadFragment(new CharacterFragment());
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadSkillsFragment()
        {
            if (CurrentTab != TabType.Skills)
            {
                LoadFragment(new SkillsFragment());
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadAttacksFragment()
        {
            if (CurrentTab != TabType.Attacks)
            {
                LoadFragment(new AttacksFragment());
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadPreparedSpellsFragment()
        {
            if (CurrentTab != TabType.PreparedSpells)
            {
                LoadFragmentForSpells(showPreparedOnly: true);
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadSpellsFragment()
        {
            if (CurrentTab != TabType.Spells)
            {
                LoadFragmentForSpells(showPreparedOnly: false);
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadFeatsFragment(string key)
        {
            if (CurrentTab != TabType.Feats || (CurrentTab == TabType.Feats && !string.Equals(CurrentFeatsKey, key)))
            {
                LoadFragmentForFeats(key);
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadItemsFragment()
        {
            if (CurrentTab != TabType.Items)
            {
                LoadFragment(new ItemsFragment());
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void LoadNotesFragment()
        {
            if (CurrentTab != TabType.Notes)
            {
                LoadFragment(new NotesFragment());
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        public void SetupTabs()
        {
            if (Game.SelectedCharacter != null)
            {
                _tabsDataSet = new Dictionary<string, TabType>()
                {
                    ["Character"] = TabType.Character
                };

                if (Game.SelectedCharacter.Skills.SkillsList.Any())
                {
                    _tabsDataSet.Add("Skills", TabType.Skills);
                }

                if (Game.SelectedCharacter.Equipment.Weapons.Any())
                {
                    _tabsDataSet.Add("Attacks", TabType.Attacks);
                }

                if (Game.SelectedCharacter.Spells.SpellClasses.Any())
                {
                    if (Game.SelectedCharacter.Spells.SpellClasses.Any(x => x.Value.Levels.Values.Any(y => y.HasPreparedSpells)))
                    {
                        _tabsDataSet.Add("Prepared Spells", TabType.PreparedSpells);
                    }

                    _tabsDataSet.Add("Spells", TabType.Spells);
                }

                foreach (var key in Game.SelectedCharacter.Feats.FeatsList.Keys)
                {
                    _tabsDataSet.Add(key, TabType.Feats);
                }

                _tabsDataSet.Add("Items", TabType.Items);

                _tabsDataSet.Add("Notes", TabType.Notes);

                _tabsAdapter = new TabListAdapter(this, _tabsDataSet);
                _tabsDrawer.Adapter = _tabsAdapter;
            }
        }

        private void LoadFragment(Android.Support.V4.App.Fragment fragment)
        {
            SetupTabs();

            SupportFragmentManager.BeginTransaction()
                                  .Replace(Resource.Id.main, fragment)
                                  .Commit();

            SyncState();
        }

        private void LoadFragmentForSpells(bool showPreparedOnly)
        {
            SetupTabs();

            SupportFragmentManager.BeginTransaction()
                                  .Replace(Resource.Id.main, SpellsFragment.NewInstance(showPreparedOnly))
                                  .Commit();

            SyncState();
        }

        private void LoadFragmentForFeats(string key)
        {
            SetupTabs();

            SupportFragmentManager.BeginTransaction()
                                  .Replace(Resource.Id.main, FeatsFragment.NewInstance(key))
                                  .Commit();

            SyncState();
        }

        private void SyncState()
        {
            _drawerLayout.CloseDrawers();
            _drawerToggle.SyncState();

            _drawerToggle.ClosedTitle = Game.SelectedCharacter.Name;
            SupportActionBar.TitleFormatted = new SpannableString(Game.SelectedCharacter.Name);

        }

        private void Characters_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            LoadClickedCharacter(_charactersDataSet[e.GroupPosition].Minions[e.ChildPosition]);
        }

        private void Characters_ItemLongClick(object sender, ExpandableListView.ItemLongClickEventArgs e)
        {
            var item = e.View as LinearLayout;

            if (item != null)
            {
                var packedPosition = _charactersDrawer.GetExpandableListPosition(e.Position);

                var itemType = ExpandableListView.GetPackedPositionType(packedPosition);
                var groupPosition = ExpandableListView.GetPackedPositionGroup(packedPosition);
                var childPosition = ExpandableListView.GetPackedPositionChild(packedPosition);


                /*  if group item clicked */
                if (itemType == PackedPositionType.Group && _charactersDataSet[groupPosition].Minions.Any())
                {
                    if (_charactersDrawer.IsGroupExpanded(groupPosition))
                    {
                        _charactersDrawer.CollapseGroup(groupPosition);
                    }
                    else
                    {
                        _charactersDrawer.ExpandGroup(groupPosition);
                    }
                }
            }
        }

        private void LoadClickedCharacter(CharacterStats characterStats)
        {
            if (Game.SelectedCharacter != characterStats)
            {
                Game.SelectedCharacter = characterStats;

                LoadCharacterFragment();
            }
            else
            {
                _drawerLayout.CloseDrawers();
            }
        }

        private void Characters_GroupClick(object sender, ExpandableListView.GroupClickEventArgs e)
        {
            LoadClickedCharacter(_charactersDataSet[e.GroupPosition]);
        }
        
        void Tab_Click(object sender, ListView.ItemClickEventArgs e)
        {
            var tab = _tabsAdapter[e.Position];

            switch (tab)
            {
                case TabType.Attacks:
                    LoadAttacksFragment();
                    break;
                case TabType.Skills:
                    LoadSkillsFragment();
                    break;
                case TabType.PreparedSpells:
                    LoadPreparedSpellsFragment();
                    break;
                case TabType.Spells:
                    LoadSpellsFragment();
                    break;
                case TabType.Feats:
                    LoadFeatsFragment(_tabsAdapter.GetKey(e.Position));
                    break;
                case TabType.Items:
                    LoadItemsFragment();
                    break;
                case TabType.Notes:
                    LoadNotesFragment();
                    break;
                case TabType.Character:
                default:
                    LoadCharacterFragment(force: false);
                    break;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                _drawerToggle.OnOptionsItemSelected(item);
                return true;
            }
            else if (item.ItemId == Resource.Id.action_email)
            {
                if (Game.SelectedCharacter != null)
                {
                    SendEmail();
                }
                else
                {
                    Toast.MakeText(this, "No character selected", ToastLength.Long).Show();
                }

                return true;
            }
            else if (item.ItemId == Resource.Id.action_delete)
            {
                if (Game.SelectedCharacter != null)
                {
                    DeleteCharacter();
                }

                return true;
            }
            else if (item.ItemId == Resource.Id.action_reset_spells)
            {
                if (Game.SelectedCharacter != null)
                {
                    Game.SelectedCharacter.Spells.Reset();

                    CurrentSpellSummarySegment.Reset();
                    CurrentSpellsLevelListAdapters.ForEach(x => x.NotifyDataSetChanged());
                }

                return true;
            }
            else if (item.ItemId == Resource.Id.action_add_item)
            {
                if (Game.SelectedCharacter != null)
                {
                    var gearItem = new Item("Unnamed item");
                    Game.SelectedCharacter.Gear.Items.Add(gearItem);
                    new EditItemDialog(this,
                                       gearItem,
                                       updateAction: () => CurrentItemsFragment.Update(),
                                       removeAction: () =>
                                       {
                                           Game.SelectedCharacter.Gear.Items.Remove(gearItem);
                                           CurrentItemsFragment.Update();
                                       },
                                       shouldRemoveOnCancel: true);
                }

                return true;
            }
            else if (item.ItemId == Resource.Id.action_add_note)
            {
                if (Game.SelectedCharacter != null)
                {
                    var note = new Note { Name = "Test1", Description = "Description test" };
                    Game.SelectedCharacter.Notes.Add(note);
                    CurrentNotesFragment.Update();
                }

                return true;
            }
            else if (item.ItemId == Resource.Id.action_tabs)
            {
                if (_tabsDrawer.IsShown)
                {
                    _drawerLayout.CloseDrawer(_tabsDrawer);
                }
                else
                {
                    _drawerLayout.OpenDrawer(_tabsDrawer);
                }

                return true;
            }
            else
            {
                return base.OnOptionsItemSelected(item);
            }
        }

        private void SendEmail()
        {
            var email = new Intent(Android.Content.Intent.ActionSend);

            email.PutExtra(Android.Content.Intent.ExtraSubject, string.Format("Character import: {0}", Game.SelectedCharacter.Name));

            var jsonString = JsonConvert.SerializeObject(
              Game.SelectedCharacter, Formatting.Indented,
              new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var uri = Android.Net.Uri.FromFile(GenerateAttachmentFromString(jsonString, Game.SelectedCharacter.Name));

            email.PutExtra(Android.Content.Intent.ExtraStream, uri);
            email.SetType("message/rfc822");
            try
            {
                StartActivity(email);
            }
            catch (ActivityNotFoundException)
            {
                var alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                alert.SetTitle("No email application found");
                alert.SetMessage("A default email application could not be found for emailing your character import.");
                alert.SetCancelable(true);
                alert.Show();
            }
        }

        public Java.IO.File GenerateAttachmentFromString(string jsonString, string characterName)
        {
            var tempDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            var filePath = System.IO.Path.Combine(tempDirectory.Path, string.Format("CharacterSheetReader_ImportAttachment_{0}.txt", characterName));

            FileStream stream = new FileStream(filePath, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(jsonString);
            writer.Flush();
            stream.Close();

            var file = new Java.IO.File(filePath);
            file.SetReadable(true, false);
            return file;
        }

        public void UpdateOptionsMenu()
        {
            if (_menu != null)
            {
                ShowTabs(true);
                ShowMenuItem(Resource.Id.action_email, false);
                ShowMenuItem(Resource.Id.action_delete, false);
                ShowMenuItem(Resource.Id.action_reset_spells, false);
                ShowMenuItem(Resource.Id.action_add_item, false);
                ShowMenuItem(Resource.Id.action_add_note, false);

                switch (CurrentTab)
                {
                    case TabType.Character:
                        ShowMenuItem(Resource.Id.action_email, true);
                        ShowMenuItem(Resource.Id.action_delete, true);
                        break;
                    case TabType.Skills:
                    case TabType.Attacks:
                    case TabType.Feats:                    
                        break;
                    case TabType.PreparedSpells:
                    case TabType.Spells:
                        ShowMenuItem(Resource.Id.action_reset_spells, true);
                        break;
                    case TabType.Items:
                        ShowMenuItem(Resource.Id.action_add_item, true);
                        break;
                    case TabType.Notes:
                        ShowMenuItem(Resource.Id.action_add_note, true);
                        break;
                    default:
                        ShowTabs(false);
                        break;
                }
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.action_menu, menu);

            _menu = menu;

            UpdateOptionsMenu();

            return base.OnCreateOptionsMenu(menu);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            var gameInstance = new GameInstance
            {
                Campaigns = Game.Campaigns,
                SelectedCharacter = Game.SelectedCharacter
            };

            outState.PutSerializable("gameInstance", gameInstance);
            outState.PutString("title", SupportActionBar.Title);

            base.OnSaveInstanceState(outState);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        protected override void OnPause()
        {
            base.OnPause();

            SaveState();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            int buildVersion;

            int.TryParse(Android.OS.Build.VERSION.Sdk, out buildVersion);

            if (FormatHelper.DeviceHelper.CanBeFullScreen)
            {
                View decorView = Window.DecorView;
                var uiOptions = (int)decorView.SystemUiVisibility;
                var newUiOptions = (int)uiOptions;

                newUiOptions |= (int)SystemUiFlags.LowProfile;
                newUiOptions |= (int)SystemUiFlags.Fullscreen;
                newUiOptions |= (int)SystemUiFlags.HideNavigation;
                newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

                decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == GOOGLE_DRIVE_REQUEST_CODE)
            {
                SupportFragmentManager.FindFragmentByTag(GOOGLE_DRIVE_FRAGMENT_TAG).OnActivityResult(requestCode, (int)resultCode, data);
            }
        }
    }
}

