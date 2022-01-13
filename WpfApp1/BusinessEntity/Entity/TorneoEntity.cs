using System;

namespace BusinessEntity.Entity
{
    public class TorneoEntity
    {

        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// formatted start date
        /// </summary>
        public String Start { get
            {
                return StartDate.ToShortDateString();
            }
        }
        public DateTime EndDate { get; set; }
        /// <summary>
        /// formatted nd date
        /// </summary>
        public String End
        {
            get
            {
                return EndDate.ToShortDateString();
            }
        }
        public String Place { get; set; }
        public String Note { get; set; }

        public TorneoEntity() { }
    }
}
