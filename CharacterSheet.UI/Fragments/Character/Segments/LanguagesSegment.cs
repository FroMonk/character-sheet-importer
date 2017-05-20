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
    public class LanguagesSegment : BaseCharacterSegement
    {
        private ScaledTextView _languages;

        public LanguagesSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        public override void AssignValues()
        {
            _languages.SetText(string.Join(", ", _selectedCharacter.Bio.Languages));
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _languages = (ScaledTextView)root.FindViewById(Resource.Id.languages);
        }
    }
}