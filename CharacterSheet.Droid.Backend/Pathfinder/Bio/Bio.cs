using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Bio : IHasDescription
    {
        public string Name
        {
            get
            {
                return string.Empty;
            }
        }

        public string Alignment { get; set; } = "";
        public string Deity { get; set; } = "";
        public int Points { get; set; }
        public string ClassDescription { get; set; } = "";
        public int Experience { get; set; }
        public string Race { get; set; } = "";
        public Size Size { get; set; } = Size.Medium;
        public string Reach { get; set; } = "";
        public string Height { get; set; } = "";
        public string Weight { get; set; } = "";
        public List<Sense> Senses { get; set; } = new List<Sense>();
        public int Level { get; set; }
        public int ExperienceNextLevel { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public string Eyes { get; set; } = "";
        public string Hair { get; set; } = "";
        public List<string> Languages { get; set; } = new List<string>();
        public Money Money { get; set; } = new Money();
        public string Role { get; set; } = "";
        public string CharacterType { get; set; } = "";
        public string Nature { get; set; } = "";
        public string Relationship { get; set; } = "";
        public string ChallengeRating { get; set; } = "";
        public string XpAward { get; set; } = "";
        public List<CharacterClass> CharacterClasses { get; set; } = new List<CharacterClass>();
        public List<string> Types { get; set; } = new List<string>();
        public List<string> Subtypes { get; set; } = new List<string>();
        public int HeroPoints { get; set; }
        public bool HeroPointsEnabled { get; set; }
        public List<Aura> Auras { get; set; } = new List<Aura>();
        public string Description { get; set; } = "";
        public string Skin { get; set; } = "";
        public string WeaponProficiencies { get; set; } = "";
                
        public void SetHeroPointsEnabled(string heroPointsEnabled)
        {
            switch (heroPointsEnabled)
            {
                case "yes":
                    HeroPointsEnabled = true;
                    break;
                default:
                    HeroPointsEnabled = false;
                    break;
            }
        }

        public void SetHeroPoints(string heroPoints)
        {
            HeroPoints = Helpers.Convert.StatToInt(heroPoints);
        }

        public void SetPoints(string points)
        {
            Points = Helpers.Convert.StatToInt(points);
        }      

        public void SetExperience(string experience)
        {
            Experience = Helpers.Convert.StatToInt(experience);
        }        

        public void SetLevel(string level)
        {
            Level = Helpers.Convert.StatToInt(level);
        }
        
        public void SetExperienceNextLevel(string experienceNextLevel)
        {
            ExperienceNextLevel = Helpers.Convert.StatToInt(experienceNextLevel);
        }

        public void SetAge(string age)
        {
            Age = Helpers.Convert.StatToInt(age);
        }

        public void SetSize(string size)
        {
            Size = (Size)Enum.Parse(typeof(Size), Prepare.ForStringToEnumConversion(size));
        }
    }
}
