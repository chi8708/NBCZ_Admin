using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public partial  class Pub_UserRoleExt:Pub_UserRole
    {
        [JsonProperty]	
        public string RoleName { get; set; }
    }
}
