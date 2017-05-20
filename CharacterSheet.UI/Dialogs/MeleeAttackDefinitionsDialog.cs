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
    public class MeleeAttackDefinitionsDialog : AlertDialog.Builder
    { 
        public MeleeAttackDefinitionsDialog(Context context, LayoutInflater inflater) : base(context, AlertDialog.ThemeHoloLight)
        {
            Check.ForNullArgument(context, nameof(context));
            Check.ForNullArgument(inflater, nameof(inflater));
                        
            this.SetTitle("Melee attack definitions");

            var layout = (ViewGroup)inflater.Inflate(Resource.Layout.dialog_melee_attack_definitions, null);                        

            this.SetView(layout);

            var dialog = this.Show();
            
            dialog.SetCanceledOnTouchOutside(true);
        }
    }
}