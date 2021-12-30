using System;
using APIWebRESTful.Models;
using AutoMapper;

namespace APIWebRESTful
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hero, HeroDTO>();
            CreateMap<HeroDTO, Hero>();
        }
    }
}
