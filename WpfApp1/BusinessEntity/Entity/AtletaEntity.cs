using System;

namespace BusinessEntity.Entity
{
    public class AtletaEntity
    {
        public int IdAtleta { get; set; }
        public int IdAsd { get; set; }
        public String Asd { get; set; }
        public String Cognome { get; set; }
        public String Nome { get; set; }
        public String Sesso { get; set; }
        public Double Ranking { get; set; }
        public Int32 Posizionamento { get; set; }

        public AtletaEntity() { }

        public string FullName
        {
            get { return Cognome + " " + Nome; }
        }

    }
}
