using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Feats
    {
        public Dictionary<string, List<Feat>> FeatsList { get; set; } = new Dictionary<string, List<Feat>>();
        
        public void Add(Feat feat)
        {
            if (FeatsList.ContainsKey(feat.Type))
            {
                FeatsList.FirstOrDefault(x => x.Key == feat.Type).Value.Add(feat);
            }
            else
            {
                var list = new List<Feat>
                {
                    feat
                };

                FeatsList.Add(feat.Type, list);
            }
        }

    }
}
