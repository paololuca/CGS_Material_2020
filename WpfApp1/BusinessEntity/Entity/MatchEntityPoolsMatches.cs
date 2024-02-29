using System;

namespace BusinessEntity.Entity
{
    public class MatchEntityPoolsMatches
    {
        public int IdTorneo { get; set; }
        public int IdGirone { get; set; }
        public String NomeAtletaRosso { get; set; }
        public int PuntiRosso { get; set; }
        public string NomeAtletaBlu { get; set; }
        public int PuntiBlu { get; set; }
        public bool DoppiaMorte { get; set; }


        /// <summary>
        /// Costruttore senza parametri
        /// </summary>
        public MatchEntityPoolsMatches(MatchEntity entity, int idTorneo, int idGirone)
        {
            IdTorneo = idTorneo;
            IdGirone = idGirone;

            NomeAtletaRosso = entity.CognomeRosso + " " + entity.NomeRosso;
            NomeAtletaBlu = entity.CognomeBlu+ " " + entity.NomeBlu;
            PuntiRosso = entity.PuntiRosso;
            PuntiBlu = entity.PuntiBlu;
            DoppiaMorte = entity.DoppiaMorte;
        }

    }
}
