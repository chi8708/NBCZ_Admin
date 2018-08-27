using NBCZ.Model;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBCZ.Web.App_Start
{
    public class TinyMapperConfig
    {
        public static void Bind()
        {
            TinyMapper.Bind<Pub_UserRole, Pub_UserRoleExt>();
        }
    }
}