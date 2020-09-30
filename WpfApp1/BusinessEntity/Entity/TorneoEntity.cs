using System;

namespace BusinessEntity.Entity
{
    public class TorneoEntity
    {

        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Place { get; set; }
        public String Note { get; set; }

        public TorneoEntity() { }
    }
}
