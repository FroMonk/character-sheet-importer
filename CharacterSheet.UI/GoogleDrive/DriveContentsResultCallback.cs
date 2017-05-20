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
using CharacterSheet.Helpers;
using CharacterSheet.UI.Activities;

namespace CharacterSheet.UI.GoogleDrive
{
    public class DriveContentsResultCallback : Java.Lang.Object, IResultCallback
    {
        private Activity _activity;
        private ProgressDialog _progressDialog;
        private GoogleApiClient _mGoogleApiClient;

        public DriveContentsResultCallback(Activity activity, ProgressDialog pDialog, GoogleApiClient apiClient)
        {
            Check.ForNullArgument(activity, "activity");
            Check.ForNullArgument(pDialog, "pDialog");
            Check.ForNullArgument(apiClient, "apiClient");

            _activity = activity;
            _progressDialog = pDialog;
            _mGoogleApiClient = apiClient;
        }

        public void ShowMessage(string message)
        {
            Toast.MakeText(_activity, message, ToastLength.Long).Show();
        }

        public void OnResult(Java.Lang.Object result)
        {
            var driveContentsResult = Java.Lang.Object.GetObject<IDriveApiDriveContentsResult>(result.Handle, JniHandleOwnership.DoNotTransfer);

            _progressDialog.Dismiss();
            if (!driveContentsResult.Status.IsSuccess)
            {
                ShowMessage("Error while opening the file contents");
                _mGoogleApiClient.Connect();
                return;
            }

            DriveId driveId = driveContentsResult.DriveContents.DriveId;
            IDriveFile file = driveId.AsDriveFile();
            file.GetMetadata(_mGoogleApiClient)
                    .SetResultCallback(new MetadataCallback(_activity, driveContentsResult.DriveContents.InputStream));            
        }
    }
}