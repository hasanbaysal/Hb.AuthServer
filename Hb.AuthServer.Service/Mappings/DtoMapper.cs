using AutoMapper;
using Hb.AuthServer.Core.Dtos;
using Hb.AuthServer.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hb.AuthServer.Service.Mappings
{
    public class DtoMapper:Profile
    {

        public DtoMapper()
        {

            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();

        }
            
    }
}
