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
using Java.IO;
using CharacterSheet.UI.Activities;
using CharacterSheet.Pathfinder.XMLReader;
using System.IO.Compression;
using CharacterSheet.Helpers;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI.Utilities
{
    public class FileUtil
    {
        private List<string> _files = new List<string>();
        private MainActivity _activity;

        public FileUtil(Activity activity)
        {
            Check.ForNullArgument(activity, "activity");

            _activity = (MainActivity)activity;
        }


        public List<string> FindLocalFiles()
        {
            var downloadsDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);

            if (downloadsDir != null)
            {
                FindFiles(downloadsDir);
            }

            return _files;
        }

        private void FindFiles(File file)
        {
            File[] files = file.ListFiles();
            if (files != null)
            {
                foreach (File tempFile in files)
                {
                    if (!tempFile.IsDirectory)
                    {
                        string tempFileName = tempFile.Path;

                        if (tempFileName.Contains(".por") || tempFileName.Contains(".xml"))
                        {
                            _files.Add(tempFileName);
                        }
                    }
                    else
                    {
                        FindFiles(tempFile);
                    }
                }
            }
        }

        public void LoadFile(string filename)
        {
            CharacterStats characterToSwitchTo = null;

            if (filename.Contains(".xml"))
            {
                var reader = new PcGenXMLReader();
                using (var inputStream = System.IO.File.OpenRead(filename))
                {
                    characterToSwitchTo = reader.parseCharacterXML(inputStream);

                    _activity.AddCharacterToNavigationDrawer(characterToSwitchTo);
                }
            }
            else if (filename.Contains(".por"))
            {
                using (ZipArchive archive = ZipFile.OpenRead(filename))
                {
                    var reader = new HeroLabXMLReader();
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.ToLower().Contains("statblocks_xml/"))
                        {
                            var file = entry.FullName.Substring(entry.FullName.LastIndexOf("/") + 1);
                            file = file.Substring(file.IndexOf("_"));
                            file = file.Substring(0, file.LastIndexOf(".xml"));
                            file = file.Replace("_", " ");

                            var character = reader.parseCharacterXML(entry.Open());

                            _activity.AddCharacterToNavigationDrawer(character); 
                            
                            if (characterToSwitchTo == null)
                            {
                                characterToSwitchTo = character;
                            }  
                        }
                    }
                }
            }

            _activity.Game.SelectedCharacter = characterToSwitchTo;

            _activity.LoadCharacterFragment();
        }
    }
}