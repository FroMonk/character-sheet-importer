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
using CharacterSheet.UI.Fragments.Spells.Segments;
using CharacterSheet.UI.Fragments.Items.Segments;

namespace CharacterSheet.UI.Dialogs
{
    public class ItemDialog : ImmersiveAlertDialog
    { 
        public ItemDialog(Activity activity, LayoutInflater inflater, Item item, GenericListAdapter<Item> adapter) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(item, nameof(item));
                        
            this.SetTitle(item.Name);

            var layout = (ViewGroup)inflater.Inflate(Resource.Layout.dialog_item, null);

            var closeButton = (Button)layout.FindViewById(Resource.Id.item_close_button);

            new ItemSummarySegment(activity, layout, item, adapter, this);

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