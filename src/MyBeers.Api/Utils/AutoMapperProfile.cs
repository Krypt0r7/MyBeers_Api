using AutoMapper;
using MyBeers.Api.Data;
using MyBeers.Api.DataSystemet;
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

            CreateMap<SystemetDto, BeerData>().ForMember(f => f.ProductName, o => o.MapFrom(x => x.ProductNameThin + " " + x.ProductNameBold));
            CreateMap<SystemetBeerIn, BeerData>().ForMember(f => f.ProductName, o => o.MapFrom(x => x.ProductNameThin + " " + x.ProductNameBold));

            CreateMap<Beer, BeerDto>();
            CreateMap<Beer, BeerQueryDto>();
            CreateMap<Beer, BeersAndRatingsQueryDto>();

            CreateMap<Rating, RatingQueryDto>();
            CreateMap<Rating, RatingAndUsersQueryDto>();

        }
    }
}
