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

namespace CharacterSheet.UI
{
    public sealed class OnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
    {
        private readonly Action _action;

        public OnDismissListener(Action action)
        {
            _action = action;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            _action();
        }
    }
}