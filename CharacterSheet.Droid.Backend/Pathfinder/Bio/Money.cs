using CharacterSheet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSheet.Pathfinder
{
    [Serializable]
    public class Money
    {
        public int PlatinumPieces { get; set; }
        public int GoldPieces { get; set; }
        public int? ElectrumPieces { get; set; }
        public int SilverPieces { get; set; }
        public int CopperPieces { get; set; }
        public int? Valuables { get; set; }

        public string Total
        {
            get
            {
                if (ElectrumPieces.HasValue)
                {
                    return string.Format("{0}pp/{1}gp/{2}ep/{3}sp/{4}cp/{5} in valuables", PlatinumPieces,
                                                                                     GoldPieces,
                                                                                     ElectrumPieces.Value,
                                                                                     SilverPieces,
                                                                                     CopperPieces,
                                                                                     Valuables);
                }


                if (Valuables.HasValue)
                {
                    return string.Format("{0}pp/{1}gp/{2}sp/{3}cp/{4} in valuables", PlatinumPieces,
                                                                                     GoldPieces,
                                                                                     SilverPieces,
                                                                                     CopperPieces,
                                                                                     Valuables.Value);
                }

                return string.Format("{0}pp/{1}gp/{2}sp/{3}cp", PlatinumPieces,
                                                                GoldPieces,
                                                                SilverPieces,
                                                                CopperPieces);
            }
        }

        public void SetPP(string pp)
        {
            PlatinumPieces = Helpers.Convert.StatToInt(pp);
        }

        public void SetGP(string gp)
        {
            gp = gp.Replace(",", "");
            if (gp.Contains("."))
            {
                gp = gp.Substring(0, gp.IndexOf("."));
            }

            GoldPieces = Helpers.Convert.StatToInt(gp);
        }

        public void SetEP(string ep)
        {
            ElectrumPieces = Helpers.Convert.StatToInt(ep);
        }

        public void SetSP(string sp)
        {
            SilverPieces = Helpers.Convert.StatToInt(sp);
        }

        public void SetCP(string cp)
        {
            CopperPieces = Helpers.Convert.StatToInt(cp);
        }

        public void SetValuables(string valuables)
        {
            Valuables = Helpers.Convert.StatToInt(valuables);
        }
    }
}
