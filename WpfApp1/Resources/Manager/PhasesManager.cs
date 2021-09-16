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
        ///  E' l'indice della fase nel transitioner
        ///  0 = 1/32
        ///  1 = 1/16
        ///  2 = 1/8
        ///  3 = 1/4
        ///  4 = semifinals
        ///  5 = finals
        /// <returns></returns>
        public static int GetStartingPhase(int atletiAmmessiEliminatorie)
        {
            switch (atletiAmmessiEliminatorie)
            {
                case 64:
                    return (int)PhasesType.Finals_32;
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

        public static int GetQualificationCapFromPhase(PhasesType phase)
        {
            switch (phase)
            {
                case PhasesType.Finals_32:
                    return 64;
                case PhasesType.Finals_16:
                    return 32;
                case PhasesType.Finals_8:
                    return 16;
                case PhasesType.Finals_4:
                    return 8;
                case PhasesType.SemiFinals:
                    return 4;
                default:
                    return 4;
            }
        }
    }
}
