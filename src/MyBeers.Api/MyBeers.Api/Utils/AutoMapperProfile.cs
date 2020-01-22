using AutoMapper;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserAuthenticateDto, User>();
            CreateMap<UserRegisterDto, User>();
        }
    }
}
