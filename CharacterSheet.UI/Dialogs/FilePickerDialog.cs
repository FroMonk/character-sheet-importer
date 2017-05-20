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
using LayoutParams = Android.Views.ViewGroup.LayoutParams;

using CharacterSheet.Helpers;
using CharacterSheet.UI.Utilities;
using Android.Content.Res;

namespace CharacterSheet.UI.Dialogs
{
    public class FilePickerDialog : ImmersiveAlertDialog
    {
        private readonly Activity _activity;
        private readonly FileUtil _fileUtil;

        public FilePickerDialog(Activity activity) : base(activity)
        {
            Check.ForNullArgument(activity, "activity");

            _activity = activity;

            _fileUtil = new FileUtil(_activity);

            var files = _fileUtil.FindLocalFiles();

            SetTitle("Choose file to import");

            ScrollView scrollView = new ScrollView(activity);
            

            LinearLayout filesLayout = new LinearLayout(_activity);
            filesLayout.Orientation = Android.Widget.Orientation.Vertical;

            if (files.Any())
            {
                foreach (var filename in files)
                {
                    filesLayout.AddView(CreateFileTextView(filename));
                }

                scrollView.AddView(filesLayout);

                this.SetView(scrollView);

                this.Show();

                this.SetCanceledOnTouchOutside(true);
            }
            else
            {
                Toast.MakeText(_activity, "No files found", ToastLength.Long).Show();
            }   
        }

        private TextView CreateFileTextView(string filename)
        {
            var textView = new TextView(_activity);

            textView.Text = filename.Substring(filename.LastIndexOf("/") + 1);
            textView.SetPadding(20,20,20,20);
            textView.Background = this.Context.GetDrawable(Resource.Drawable.back);

            textView.Click += delegate { _fileUtil.LoadFile(filename); this.Dismiss(); };

            return textView;
        }
    }
}