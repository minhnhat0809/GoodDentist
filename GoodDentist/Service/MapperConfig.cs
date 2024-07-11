using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ViewDTO;
using DentistSlotDTO = BusinessObject.DTO.DentistSlotDTO;

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
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Salt, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.ClinicUsers, opt => opt.Ignore())
            .ForMember(dest => dest.DentistSlots, opt => opt.Ignore())
            .ForMember(dest => dest.Examinations, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? new DateTime(src.Dob.Value.Year, src.Dob.Value.Month, src.Dob.Value.Day) : (DateTime?)null))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId ?? default(int)))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Customer, UserDTO>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? new DateTime(src.Dob.Value.Year, src.Dob.Value.Month, src.Dob.Value.Day) : (DateTime?)null))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Clinics, opt => opt.Ignore());

            CreateMap<DentistSlot, DentistSlotDTO>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber));

            CreateMap<DentistSlotDTO, DentistSlot>()
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => src.TimeEnd))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status))
                .ForMember(dest => dest.DentistId, otp => otp.MapFrom(src => src.DentistId))
                .ForMember(dest => dest.RoomId, otp => otp.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.DentistSlotId, otp => otp.Ignore())
                .ForMember(dest => dest.Dentist, otp => otp.Ignore())
                .ForMember(dest => dest.Examinations, otp => otp.Ignore())
                .ForMember(dest => dest.Room, otp => otp.Ignore());

            CreateMap<MedicineDTO, Medicine>()
                .ReverseMap();

            CreateMap<MedicineUpdateDTO, Medicine>()
                .ReverseMap();

            CreateMap<RecordTypeDTO, RecordType>()
                .ReverseMap();

            CreateMap<RecordTypeCreateDTO, RecordType>().ReverseMap();
            CreateMap<CreateServiceDTO, Service>().ReverseMap();
            CreateMap<MedicalRecordDTO, MedicalRecord>();
            CreateMap<MedicalRecord, MedicalRecordDTO>()
                .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.RecordType.RecordName));
            CreateMap<MedicalRecordRequestDTO, MedicalRecord>().ReverseMap();
            CreateMap<CreateRoomDTO, Room>().ReverseMap();
            CreateMap<ExaminationDTO, Examination>();

            CreateMap<Examination, ExaminationDTO>()
                .ForMember(dest => dest.ExaminationProfile, opt => opt.Ignore())
                .ForMember(dest => dest.DentistSlot, opt => opt.Ignore())
                .ForMember(dest => dest.MedicalRecords, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Prescriptions, opt => opt.Ignore());

            CreateMap<DentistSlot, DentistslotDTO>()
                .ForMember(dest => dest.Dentist, opt => opt.Ignore());
            CreateMap<ExaminationProfile, ExaminationProfileDTO>()
                .ForMember(dest => dest.Dentist, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            CreateMap<Room, RoomDTO>();


            CreateMap<ExaminationRequestDTO, ExaminationDTO>().ReverseMap();
            CreateMap<CreateRoomDTO, Room>().ReverseMap();
			CreateMap<ClinicServiceDTO, BusinessObject.Entity.ClinicService>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderCreateDTO, Order>().ReverseMap();
            CreateMap<PrescriptionDTO, Prescription>().ReverseMap();
            CreateMap<PrescriptionCreateDTO, Prescription>().ReverseMap();
            CreateMap<Clinic, ClinicDTO>().ReverseMap();
            CreateMap<ExaminationRequestDTO, Examination>();
            CreateMap<Examination, ExaminationDTO>();
        
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<NotificationDTO, Notification>().ReverseMap();
            CreateMap<NotificationRequestDTO, Notification>().ReverseMap();   
        }
    }
}
