using BusinessEntity.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public static class PhasesManager
    {

        private const string Qualificati_64 = "Qualificati64";
        private const string Qualificati_32 = "Qualificati32";
        private const string Qualificati_16 = "Qualificati16";
        private const string Qualificati_8 = "Qualificati8";
        private const string Semifinali = "Semifinali";
        private const string Finali = "Finali";


        public static string Decode(PhasesType phase)
        {
            switch(phase)
            {
                case PhasesType.Finals_32:
                    return Qualificati_64;
                case PhasesType.Finals_16:
                    return Qualificati_32;
                case PhasesType.Finals_8:
                    return Qualificati_16;
                case PhasesType.Finals_4:
                    return Qualificati_8;
                case PhasesType.SemiFinals:
                    return Semifinali;
                case PhasesType.Finals:
                    return Finali;
                default:
                    return "";
            };
        }

        public static PhasesType Encode(string phase)
        {
            switch (phase)
            {
                case Qualificati_64:
                    return PhasesType.Finals_32;
                case Qualificati_32:
                    return PhasesType.Finals_16;
                case Qualificati_16:
                    return PhasesType.Finals_8;
                case Qualificati_8:
                    return PhasesType.Finals_4;
                case Semifinali:
                    return PhasesType.SemiFinals;
                case Finali:
                    return PhasesType.Finals;
                default:
                    return PhasesType.None;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="atletiAmmessiEliminatorie"></param> 
        ///  0 = 1/16
        ///  1 = 1/8
        ///  2 = 1/4
        ///  3 = semifinals
        ///  4 = finals
        /// <returns></returns>
        public static int GetStartingPhase(int atletiAmmessiEliminatorie)
        {
            switch (atletiAmmessiEliminatorie)
            {
                case 32:
                    return (int)PhasesType.Finals_16;
                case 16:
                    return (int)PhasesType.Finals_8;
                case 8:
                    return (int)PhasesType.Finals_4;
                case 4:
                    return (int)PhasesType.SemiFinals;
                default: 
                    return (int)PhasesType.Finals;
            }
        }
    }
}
