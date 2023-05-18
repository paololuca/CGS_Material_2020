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
        private string disciplina;
        private object categoria;

        public MobileAppHelper(string disciplina, string category)
        {
            this.disciplina = disciplina;
            this.categoria = category;
        }

        public void SerializePool(int poolIndex, List<AtletaEntity> atleti, List<MatchEntity> matchs)
        {

            MobileAppEntity entityToSerialize = new MobileAppEntity();

            entityToSerialize.numeroGirone = poolIndex.ToString();

            SetAthletesList(atleti, entityToSerialize);

            SetMatchsList(matchs, entityToSerialize);

            File.WriteAllText(@".\Mobile\" + disciplina + @"\" + categoria + @"\Girone" + poolIndex + ".json",
                JsonConvert.SerializeObject(entityToSerialize, Formatting.Indented));


        }

        private static void SetMatchsList(List<MatchEntity> matchs, MobileAppEntity entityToSerialize)
        {
            foreach (var m in matchs)
            {
                entityToSerialize.incontri.Add(m.CognomeRosso + " " + m.NomeRosso );
                entityToSerialize.incontri.Add(m.CognomeBlu + " " + m.NomeBlu);
            }
        }

        private static void SetAthletesList(List<AtletaEntity> atleti, MobileAppEntity entityToSerialize)
        {
            foreach (var a in atleti)
                entityToSerialize.atleti.Add(a.Cognome + " " + a.Nome);
        }
    }
}
