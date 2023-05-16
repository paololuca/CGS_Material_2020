using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Entity
{
    public class MobileAppEntity
    {

        public string numeroGirone { get; set; }
        public List<String> atleti { get; set; }
        public List<string> incontri { get; set; }


        public MobileAppEntity()
        {
            numeroGirone = "";
            atleti = new List<string>();
            incontri = new List<string>();        
        }
    }
}
