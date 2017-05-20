using CharacterSheet.Helpers;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Conditions
    {
        public List<Condition> ConditionsList { get; set; } = new List<Condition>();
        
        public List<Condition> ActiveConditions
        {
            get
            {
                return ConditionsList.Where(x => x.IsActive).ToList();
            }
        }

        public List<Condition> GetConditionsOfType(ConditionType conType)
        {
            return ConditionsList.Where(x => x.ConditionType == conType).ToList();
        }

        public float GetOverallAdjustment(string resultName)
        {
            float adjustment = 0;
            float maxAdjustment = -999;
            bool cantStack = false;
            foreach (Condition condition in ActiveConditions)
            {
                ConditionResult result = condition.ConditionResults[resultName];

                if (!result.DoesStack)
                {
                    cantStack = true;
                }

                adjustment += result.Adjustment;

                if (result.Adjustment > maxAdjustment)
                {
                    maxAdjustment = result.Adjustment;
                }
            }

            if (cantStack)
            {
                if (adjustment > maxAdjustment) adjustment = maxAdjustment;
            }

            return adjustment;
        }
    }
}
