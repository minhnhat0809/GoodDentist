using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;

namespace Utilities
{
    public class Mapper : Profile
    {
        public Mapper() 
        {
            CreateMap<User, CreateUserDTO>().ReverseMap();
        }       

    }
}
