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
using Android.Gms.Common.Apis;
using Android.Support.V7.App;
using Android.Gms.Drive;
using System.IO;
using CharacterSheet.Helpers;
using CharacterSheet.Pathfinder;
using CharacterSheet.Pathfinder.XMLReader;
using System.IO.Compression;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.GoogleDrive
{
    public class MetadataCallback : Java.Lang.Object, IResultCallback
    {
        private MainActivity _activity;
        private Stream _stream;

        public MetadataCallback(Activity activity, Stream stream)
        {
            Check.ForNullArgument(activity, "activity");
            Check.ForNullArgument(stream, "stream");

            _activity = (MainActivity)activity;
            _stream = stream;
        }

        public void ShowMessage(string message)
        {
            Toast.MakeText(_activity, message, ToastLength.Long).Show();
        }

        public void OnResult(Java.Lang.Object result)
        {

            var metadataResult = Java.Lang.Object.GetObject<IDriveResourceMetadataResult>(result.Handle, JniHandleOwnership.DoNotTransfer);

            if (!metadataResult.Status.IsSuccess)
            {
                ShowMessage("Problem while trying to fetch metadata");
                return;
            }

            string filename = metadataResult.Metadata.Title;
            
            if (filename.Contains(".xml"))
            {
                var reader = new PcGenXMLReader();

                var character = reader.parseCharacterXML(_stream);

                if (character != null)
                {
                    _activity.AddCharacterToNavigationDrawer(character);
                }
            }
            else if (filename.Contains(".por"))
            {
                Dictionary<string, Stream> streams = new Dictionary<string, Stream>();

                using (var ms = new MemoryStream())
                {
                    // Google Drive can garbage collect the stream whenever it feels like,
                    // so make a copy first
                    _stream.CopyTo(ms);
                    ms.Position = 0;

                    var reader = new HeroLabXMLReader();

                    using (ZipArchive archive = new ZipArchive(ms))
                    {
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
                            }
                        }

                        _activity.AddCharactersToNavigationDrawer(characters);
                    }
                }
            }
        }
    }
}