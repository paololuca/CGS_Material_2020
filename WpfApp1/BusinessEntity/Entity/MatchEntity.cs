using System;

namespace BusinessEntity.Entity
{
    public class MatchEntity
    {
        /// <summary>
        /// Match's index order
        /// </summary>
        public int M { get; set; }
        public int IdRosso { get; set; }
        public String SatrapiaRosso { get; set; }
        public String CognomeRosso { get; set; }
        public String NomeRosso { get; set; }
        public int PuntiRosso { get; set; }

        public int IdBlu { get; set; }
        public String SatrapiaBlu { get; set; }
        public String CognomeBlu { get; set; }
        public String NomeBlu { get; set; }
        public int PuntiBlu { get; set; }

        //public String PrimoSangue { get; set; }
        public bool DoppiaMorte { get; set; }

        /// <summary>
        /// Costruttore con parametri
        /// </summary>
        /// <param name="red">Atleta angolo rosso</param>
        /// <param name="blu">Atleta angolo blu</param>
        public MatchEntity(AtletaEntity red, AtletaEntity blu)
        {
            IdRosso = red.IdAtleta;
            SatrapiaRosso = red.Asd;
            CognomeRosso = red.Cognome;
            NomeRosso = red.Nome;

            IdBlu = blu.IdAtleta;
            SatrapiaBlu = blu.Asd;
            CognomeBlu = blu.Cognome;
            NomeBlu = blu.Nome;

            PuntiRosso = 0;
            PuntiBlu = 0;
            //PrimoSangue = "";
            
        }

        /// <summary>
        /// Costruttore senza parametri
        /// </summary>
        public MatchEntity()
        { }

    }
}
