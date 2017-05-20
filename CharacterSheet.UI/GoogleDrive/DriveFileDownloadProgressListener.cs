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
using Android.Gms.Drive;
using CharacterSheet.Helpers;

namespace CharacterSheet.UI.GoogleDrive
{
    public class DriveFileDownloadProgressListener : Java.Lang.Object, IDriveFileDownloadProgressListener
    {
        private ProgressDialog _progressDialog;

        public DriveFileDownloadProgressListener(ProgressDialog pDialog)
        {
            Check.ForNullArgument(pDialog, "pDialog");

            _progressDialog = pDialog;
        }

        public void OnProgress(long bytesDownloaded, long bytesExpected)
        {
            int progress = (int)(bytesDownloaded * 100 / bytesExpected);
            _progressDialog.SetMessage(string.Format("Downloading... {0}%", progress));
        }        
    }

}