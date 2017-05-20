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
using CharacterSheet.Pathfinder;
using CharacterSheet.UI.Views;

namespace CharacterSheet.UI.Fragments.Character.Segments
{
    public class WeaponProficienciesSegment : BaseCharacterSegement
    {
        private LinearLayout _layout;
        private ScaledTextView _proficiencies;

        public WeaponProficienciesSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            var profs = _selectedCharacter.Bio.WeaponProficiencies;

            if (profs.Any())
            {
                _proficiencies.SetText(_selectedCharacter.Bio.WeaponProficiencies);
            }
            else
            {
                _layout.Visibility = ViewStates.Gone;
            }
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _layout = (LinearLayout)root.FindViewById(Resource.Id.weapon_proficiencies_layout);
            _proficiencies = (ScaledTextView)root.FindViewById(Resource.Id.weapon_proficiencies);
        }
    }
}