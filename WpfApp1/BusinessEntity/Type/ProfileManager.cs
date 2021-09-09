using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Type
{
    public class ProfileManager
    {
        public bool CanCreatePools(ProfileType profile)
        {
            bool canCreate = false;

            canCreate = ((profile == ProfileType.Admin) || (profile == ProfileType.Editor));
            
            return canCreate;
        }
    }
}
