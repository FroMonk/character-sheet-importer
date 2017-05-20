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
using Android.Support.V7.App;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Common;
using Android.Gms;
using Java.Lang;
using System.IO;
using CharacterSheet.UI.GoogleDrive;
using Android.Util;

namespace CharacterSheet.UI.Fragments
{
    public class GoogleDriveOpenerFragment : Android.Support.V4.App.Fragment, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {

        private const string TAG = "DriveOpener";
        private const int REQUEST_CODE_OPENER = 1;
        private DriveId mSelectedFileDriveId;
        private ProgressDialog _progressDialog;
        private GoogleApiClient mGoogleApiClient;

        void GoogleApiClient.IConnectionCallbacks.OnConnected(Bundle connectionHint)
        {
            if (mSelectedFileDriveId != null)
            {
                open();
                return;
            }
            IntentSender intentSender = DriveClass.DriveApi
                    .NewOpenFileActivityBuilder()
                    .SetMimeType(new string[]{
                        "application/x-zip",
                        "application/octet-stream",
                        "text/xml" })
                    .Build(mGoogleApiClient);
            try
            {
                Activity.StartIntentSenderForResult(intentSender, REQUEST_CODE_OPENER, null, 0, 0, 0);
            }
            catch (IntentSender.SendIntentException e)
            {                
               Console.WriteLine(TAG, "Unable to send intent", e);
            }
        }
        
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _progressDialog = new ProgressDialog(Activity);
            _progressDialog.Max = 100;
        }


        private void open()
        {
            _progressDialog.SetMessage("Downloading...");
            _progressDialog.Show();
            var driveFile = mSelectedFileDriveId.AsDriveFile();
            driveFile.Open(mGoogleApiClient, DriveFile.ModeReadOnly, new DriveFileDownloadProgressListener(_progressDialog))
                     .SetResultCallback(new DriveContentsResultCallback(Activity, _progressDialog, mGoogleApiClient));
            mSelectedFileDriveId = null;
        }

        
        protected const int REQUEST_CODE_RESOLUTION = 2;

        public override void OnResume()
        {
            base.OnResume();
            if (mGoogleApiClient == null)
            {
                mGoogleApiClient = new GoogleApiClient.Builder(Activity)
                        .AddApi(DriveClass.API)
                        .AddScope(DriveClass.ScopeFile)
                        .AddScope(DriveClass.ScopeAppfolder)
                        .AddConnectionCallbacks(this)
                        .AddOnConnectionFailedListener(this)
                        .Build();
            }
            mGoogleApiClient.Connect();
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == REQUEST_CODE_OPENER && resultCode == (int)Result.Ok)
            {
                mSelectedFileDriveId = (DriveId)data.GetParcelableExtra(
                        OpenFileActivityBuilder.ExtraResponseDriveId);
            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);                
                if (requestCode == REQUEST_CODE_RESOLUTION && resultCode == (int)Result.Ok)
                {

                    mGoogleApiClient.Connect();
                }
                if (resultCode == (int)Result.Canceled)
                {
                    if (mGoogleApiClient != null)
                    {
                        mGoogleApiClient.Disconnect();
                    }
                    Activity.SupportFragmentManager.PopBackStack();
                }
            }
        }

        public override void OnPause()
        {
            if (mGoogleApiClient != null)
            {
                mGoogleApiClient.Disconnect();
            }
            base.OnPause();
        }

        
        public void OnConnectionFailed(ConnectionResult result)
        {
            if (!result.HasResolution)
            {
                GoogleApiAvailability.Instance.GetErrorDialog(Activity, result.ErrorCode, 0).Show();
                return;
            }
            try
            {
                result.StartResolutionForResult(Activity, REQUEST_CODE_RESOLUTION);
            }
            catch (IntentSender.SendIntentException e)
            {
                Log.Error(TAG, "Exception while starting resolution activity", e);
            }
        }

        public void OnConnectionSuspended(int cause)
        {
        }
    }
}