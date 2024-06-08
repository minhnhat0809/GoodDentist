using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;

namespace Services
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            CreateMap<CreateUserDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? (DateOnly?)DateOnly.FromDateTime(src.Dob.Value) : null))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Salt, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.ClinicUsers, opt => opt.Ignore())
            .ForMember(dest => dest.DentistSlots, opt => opt.Ignore())
            .ForMember(dest => dest.Examinations, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore()); ;

            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? new DateTime(src.Dob.Value.Year, src.Dob.Value.Month, src.Dob.Value.Day) : (DateTime?)null))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId ?? default(int)))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<ClinicDTO, Clinic>().ReverseMap();
            CreateMap<DentistSlotDTO, DentistSlot>().ReverseMap();
        }
    }
}
