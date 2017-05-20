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

namespace CharacterSheet.UI.Dialogs
{
    public class SpellDialog : ImmersiveAlertDialog
    { 
        public SpellDialog(Activity activity, LayoutInflater inflater, Spell spell, ExpandableSpellsLevelListAdapter adapter) : base(activity)
        {
            Check.ForNullArgument(activity, nameof(activity));
            Check.ForNullArgument(inflater, nameof(inflater));
            Check.ForNullArgument(spell, nameof(spell));
                        
            this.SetTitle(spell.Name);

            var layout = (ViewGroup)inflater.Inflate(Resource.Layout.dialog_spell, null);

            var closeButton = (Button)layout.FindViewById(Resource.Id.description_spell_close_button);

            new SpellSummarySegment(activity, layout, spell, adapter);

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