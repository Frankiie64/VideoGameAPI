using AutoMapper;
using PVideoGamesAPI.Models;
using PVideoGamesAPI.Models.Dtos;
using PVideoGamesAPI.Models.Tables_Complements;
using PVideoGamesAPI.Models.Tables_Complements.Complements_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PVideoGamesAPI.GamesMapper
{
    public class GamesMappers : Profile
    {
        public GamesMappers()
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Game, VideoGameDto>().ReverseMap();
            CreateMap<Game, VideoGameCreateDto>().ReverseMap();
            CreateMap<Game, VideoGameUpdateDto>().ReverseMap();
            CreateMap<Requeriments, RequirementsDto>().ReverseMap();
            CreateMap<Requeriments, RequirementsCreateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserRegisterDto>().ReverseMap();

        }
    }
}
