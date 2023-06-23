using AutoMapper;
using Practical19_WebApi.Entities;
using Practical19_WebApi.Models;

namespace Practical19_WebApi.Profiles;

public class AuthProfile : Profile
{
	public AuthProfile()
	{
		CreateMap<RegisterDto, User>();
        CreateMap<LoginDto, User>();
    }
}
