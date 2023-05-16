using BusinessEntity.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources
{
    public class MobileAppHelper
    {
        public MobileAppHelper()
        { }

        public void SerializePool(int poolIndex, List<AtletaEntity> atleti, List<MatchEntity> matchs)
        {

            MobileAppEntity entityToSerialize = new MobileAppEntity();

            entityToSerialize.numeroGirone = poolIndex.ToString();

            SetAthletesList(atleti, entityToSerialize);

            SetMatchsList(matchs, entityToSerialize);


            File.WriteAllText(@".\Mobile\Girone" + poolIndex + ".json", JsonConvert.SerializeObject(entityToSerialize, Formatting.Indented));


        }

        private static void SetMatchsList(List<MatchEntity> matchs, MobileAppEntity entityToSerialize)
        {
            foreach (var m in matchs)
            {
                entityToSerialize.incontri.Add(m.NomeRosso + " " + m.CognomeRosso);
                entityToSerialize.incontri.Add(m.NomeBlu + " " + m.CognomeBlu);
            }
        }

        private static void SetAthletesList(List<AtletaEntity> atleti, MobileAppEntity entityToSerialize)
        {
            foreach (var a in atleti)
                entityToSerialize.atleti.Add(a.Nome + " " + a.Cognome);
        }
    }
}
