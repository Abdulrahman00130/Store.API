using AutoMapper;
using Store.API.Domain.Entities.Identity;
using Store.API.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Mapping.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
