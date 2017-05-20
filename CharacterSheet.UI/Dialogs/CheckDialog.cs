using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CharacterSheet.Helpers;
using CharacterSheet.Droid.Backend.Pathfinder;
using LayoutParams = Android.Views.ViewGroup.LayoutParams;
using Android.App;
using CharacterSheet.UI.Helpers;
using CharacterSheet.Pathfinder;

namespace CharacterSheet.UI.Dialogs
{
    public class CheckDialog : ImmersiveAlertDialog
    { 
        public CheckDialog(Activity activity, ICheckRoll thingToRoll) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(thingToRoll, "thingToRoll");

            var viewHelper = new ViewHelper(activity);

            var result = thingToRoll.Roll();

            this.SetTitle(thingToRoll.Name);

            var layout = viewHelper.CreateLinearLayout(Orientation.Vertical, LayoutParams.MatchParent, LayoutParams.WrapContent);
            
            layout.AddView(viewHelper.CreateTextView(string.Format("{0}{1}{2}", System.Environment.NewLine, result.Total, System.Environment.NewLine), 50, LayoutParams.MatchParent, LayoutParams.WrapContent, GravityFlags.CenterHorizontal));
            layout.AddView(viewHelper.CreateTextView(string.Format("D20 roll: {0}", result.Rolled), 20, LayoutParams.MatchParent, LayoutParams.WrapContent, GravityFlags.CenterHorizontal));
            layout.AddView(viewHelper.CreateTextView(string.Format("Rolled at {0}", result.RolledAt), 10, LayoutParams.MatchParent, LayoutParams.WrapContent, GravityFlags.CenterHorizontal));

            this.SetView(layout);

            this.Show();
            
            this.SetCanceledOnTouchOutside(true);
        }
    }
}