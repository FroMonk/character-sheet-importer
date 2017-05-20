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
using CharacterSheet.UI.Views;

namespace CharacterSheet.UI.Dialogs
{
    public class DescriptionDialog : ImmersiveAlertDialog
    { 
        public DescriptionDialog(Activity activity, LayoutInflater inflater, IHasDescription thingWithDescription) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(thingWithDescription, nameof(thingWithDescription));
                        
            this.SetTitle(thingWithDescription.Name);

            var layout = (ViewGroup)inflater.Inflate(Resource.Layout.dialog_description, null);

            var descriptionView = (ScaledTextView)layout.FindViewById(Resource.Id.description_dialog_description);
            var closeButton = (Button)layout.FindViewById(Resource.Id.description_dialog_close_button);

            descriptionView.SetText(thingWithDescription.Description);

            this.SetView(layout);

            this.Show();
            
            this.SetCanceledOnTouchOutside(true);

            closeButton.Click += delegate
            {
                this.Dismiss();
            };
        }
    }
}