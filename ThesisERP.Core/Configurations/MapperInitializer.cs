using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Data;

namespace ThesisERP.Core.Configurations
{
    public class MapperInitializer : Profile
{
        public MapperInitializer()
        {           
            //CreateMap<AppUser, UserDTO>().ReverseMap();
            //CreateMap<AppUser, RegisterUserDTO>().ReverseMap();
            //CreateMap<AppUser, LoginUserDTO>().ReverseMap();
        }
    }
}
