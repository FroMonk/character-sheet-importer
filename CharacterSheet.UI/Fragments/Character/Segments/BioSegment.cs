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
    public class BioSegment : BaseCharacterSegement
    {
        private ScaledTextView _name;
        private ScaledTextView _class;
        private ScaledTextView _level;
        private ScaledTextView _experience;
        private ScaledTextView _senses;
        private ScaledTextView _alignmentDeity;
        private ScaledTextView _raceAgeGender;
        private ScaledTextView _sizeHeightWeight;
        private ScaledTextView _eyesHair;
        private ScaledTextView _money;

        public BioSegment(Activity activity, ViewGroup root, CharacterStats selectedCharacter, CharacterFragment characterFragment) : base(activity, root, selectedCharacter, characterFragment)
        {
        }

        protected override void AssociateViews(ViewGroup root)
        {
            _name = (ScaledTextView)root.FindViewById(Resource.Id.bioName);
            _class = (ScaledTextView)root.FindViewById(Resource.Id.bioClass);
            _level = (ScaledTextView)root.FindViewById(Resource.Id.bioLevel);
            _experience = (ScaledTextView)root.FindViewById(Resource.Id.bioExperience);
            _senses = (ScaledTextView)root.FindViewById(Resource.Id.bioSenses);
            _alignmentDeity = (ScaledTextView)root.FindViewById(Resource.Id.bioAlignmentDeity);
            _raceAgeGender = (ScaledTextView)root.FindViewById(Resource.Id.bioRaceAgeGender);
            _sizeHeightWeight = (ScaledTextView)root.FindViewById(Resource.Id.bioSizeFaceHeight);
            _eyesHair = (ScaledTextView)root.FindViewById(Resource.Id.bioEyesHair);
            _money = (ScaledTextView)root.FindViewById(Resource.Id.bioMoney);
        }

        public override void AssignValues()
        {
            var bio = _selectedCharacter.Bio;

            _name.SetText(_selectedCharacter.Name);
            _class.SetText(bio.ClassDescription);
            _level.SetText(bio.Level.ToString());
            _experience.SetText(string.Format("{0}xp (next level @ {1}xp)", bio.Experience, bio.ExperienceNextLevel));
            _senses.SetText(string.Join("/", bio.Senses.Select(x => x.Name)));
            _alignmentDeity.SetText(string.Format("{0}/{1}", bio.Alignment, bio.Deity));
            _raceAgeGender.SetText(string.Format("{0}/{1}/{2}", bio.Race, bio.Age, bio.Gender));
            _sizeHeightWeight.SetText(string.Format("{0}/{1}/{2}", bio.Size, bio.Height, bio.Weight));
            _eyesHair.SetText(string.Format("{0}/{1}", bio.Eyes, bio.Hair));
            _money.SetText(bio.Money.Total);
        }
    }
}