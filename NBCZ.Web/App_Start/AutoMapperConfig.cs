using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using NBCZ.Model;

namespace NBCZ.Web
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Pub_UserRole, Pub_UserRoleExt>();
            });
        }
    }
}