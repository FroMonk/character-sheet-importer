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
using CharacterSheet.Helpers;
using LayoutParams = Android.Views.ViewGroup.LayoutParams;
using CharacterSheet.Droid.Backend.Pathfinder;
using CharacterSheet.UI.Helpers;
using Android.Graphics;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI.Dialogs
{
    public class AmmunitionDialog : ImmersiveAlertDialog
    {
        public AmmunitionDialog(Activity activity, Ammunition ammunition, Action updateAction, Action removeAction, bool shouldRemoveOnCancel = false) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(ammunition, nameof(ammunition));
            Check.ForNullArgument(updateAction, nameof(updateAction));
            Check.ForNullArgument(removeAction, nameof(removeAction));

            var hundreds = ammunition.Remaining / 100;
            var tens = (ammunition.Remaining % 100) / 10;
            var units = (ammunition.Remaining % 100) % 10;

            this.SetTitle(string.Format("Edit {0}", ammunition.Name));

            var viewHelper = new ViewHelper(activity);

            var layout = viewHelper.CreateLinearLayout(Orientation.Vertical, width: LayoutParams.MatchParent);

            var editName = viewHelper.CreateEditText(ammunition.Name, 20, LayoutParams.MatchParent);

            var numberPickersLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent);
            numberPickersLayout.SetGravity(GravityFlags.Center);           

            var hundredsNumberPicker = new NumberPicker(activity);
            hundredsNumberPicker.MinValue = 0;
            hundredsNumberPicker.MaxValue = 3;
            hundredsNumberPicker.Value = hundreds;

            var tensNumberPicker = new NumberPicker(activity);
            tensNumberPicker.MinValue = 0;
            tensNumberPicker.MaxValue = 9;
            tensNumberPicker.Value = tens;

            var unitsNumberPicker = new NumberPicker(activity);
            unitsNumberPicker.MinValue = 0;
            unitsNumberPicker.MaxValue = 9;
            unitsNumberPicker.Value = units;

            numberPickersLayout.AddView(hundredsNumberPicker);
            numberPickersLayout.AddView(tensNumberPicker);
            numberPickersLayout.AddView(unitsNumberPicker);

            var buttonLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent);

            var okButton = viewHelper.CreateDialogButton("OK", 15, weight: 1f);
            var cancelButton = viewHelper.CreateDialogButton("Cancel", 15, weight: 1f);
            var removeButton = viewHelper.CreateDialogButton("Remove", 15, weight: 1f);

            buttonLayout.AddView(okButton);
            buttonLayout.AddView(cancelButton);
            buttonLayout.AddView(removeButton);

            layout.AddView(editName);
            layout.AddView(numberPickersLayout);
            layout.AddView(buttonLayout);

            this.SetView(layout);           

            this.Show();

            this.SetCanceledOnTouchOutside(true);

            okButton.Click += delegate
            {
                ammunition.Name = editName.Text;
                ammunition.Remaining = (hundredsNumberPicker.Value * 100) + (tensNumberPicker.Value * 10) + unitsNumberPicker.Value;
                updateAction();
                this.Dismiss();
            };

            cancelButton.Click += delegate
            {
                if (shouldRemoveOnCancel)
                {
                    removeAction();
                }
                this.Dismiss();
            };

            removeButton.Click += delegate
            {
                removeAction();
                this.Dismiss();
            };
        }    
    }
}
