using BusinessEntity.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class QualificationManager
    {
       

        public QualificationManager()
        {

        }


        public static int GetMinFightersFor16th()
        {
            return SqlDal_MasterDB.GetQualificationCap(PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_16));
        }

        public static int GetMinFightersFor8th()
        {
            return SqlDal_MasterDB.GetQualificationCap(PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_8));
        }

        public static int GetMinFightersFor4th()
        {
            return SqlDal_MasterDB.GetQualificationCap(PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_4));
        }


        public static int GetAdmittedFightersNumber(int totalPartecipantsNumber)
        {
            if (totalPartecipantsNumber >= SqlDal_MasterDB.GetQualificationCap(PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_16)))
                return PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_16);
            else if (totalPartecipantsNumber >= SqlDal_MasterDB.GetQualificationCap(PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_8)))
                return PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_8);
            else if (totalPartecipantsNumber >= SqlDal_MasterDB.GetQualificationCap(PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_4)))
                return PhasesManager.GetQualificationCapFromPhase(PhasesType.Finals_4);
            else
                return PhasesManager.GetQualificationCapFromPhase(PhasesType.SemiFinals);
        }
    }
}
