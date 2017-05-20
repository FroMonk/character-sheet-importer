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

namespace CharacterSheet.UI.Dialogs
{
    public class AdjustmentDialog : ImmersiveAlertDialog
    {
        public AdjustmentDialog(Activity activity, IAdjustable thingToAdjust, Action action) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(thingToAdjust, "adjustable");

            this.SetTitle(string.Format("Adjust {0} value", thingToAdjust.AdjustmentName));
            
            var viewHelper = new ViewHelper(activity);

            var layout = viewHelper.CreateLinearLayout(Orientation.Vertical, width: LayoutParams.MatchParent);
            
            var numberPicker = new NumberPicker(activity);
            numberPicker.MinValue = 1;
            numberPicker.MaxValue = 99;
            numberPicker.Value = 1;

            var buttonLayout = viewHelper.CreateLinearLayout(width: LayoutParams.MatchParent);

            var plusButton = viewHelper.CreateDialogButton("+", 15, weight: 1f);
            var resetButton = viewHelper.CreateDialogButton("Reset", 15, weight: 1f);
            var minusButton = viewHelper.CreateDialogButton("–", 15, weight: 1f);

            buttonLayout.AddView(plusButton);
            buttonLayout.AddView(resetButton);
            buttonLayout.AddView(minusButton);

            layout.AddView(numberPicker);
            layout.AddView(buttonLayout);

            this.SetView(layout);

            this.Show();

            this.SetCanceledOnTouchOutside(true);

            plusButton.Click += delegate
            {
                thingToAdjust.Adjustment = thingToAdjust.Adjustment + numberPicker.Value;
                action();
                this.Dismiss();
            };

            resetButton.Click += delegate
            {
                thingToAdjust.Adjustment = 0;
                action();
                this.Dismiss();
            };

            minusButton.Click += delegate
            {
                thingToAdjust.Adjustment = thingToAdjust.Adjustment - numberPicker.Value;
                action();
                this.Dismiss();
            };
        }    
    }
}
