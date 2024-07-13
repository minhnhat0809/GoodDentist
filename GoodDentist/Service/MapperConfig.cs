using AutoMapper;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreateDentistSlotDTO = BusinessObject.DTO.DentistSlotDTOs.CreateDentistSlotDTO;
using BusinessObject.DTO.ExaminationDTOs.View;
using BusinessObject.DTO.UserDTOs.View;
using BusinessObject.DTO.RoomDTOs.View;
using BusinessObject.DTO.RoomDTOs;
using BusinessObject.DTO.UserDTOs;
using BusinessObject.DTO.ServiceDTOs;
using BusinessObject.DTO.ServiceDTOs.View;
using BusinessObject.DTO.DentistSlotDTOs.View;
using BusinessObject.DTO.CustomerDTOs;
using BusinessObject.DTO.MedicineDTOs;
using BusinessObject.DTO.OrderDTOs;
using BusinessObject.DTO.OrderDTOs.View;
using BusinessObject.DTO.MedicineDTOs.View;
using BusinessObject.DTO.ExaminationDTOs;
using BusinessObject.DTO.MedicalRecordDTOs;
using BusinessObject.DTO.NotificationDTOs;
using BusinessObject.DTO.PrescriptionDTOs.View;
using BusinessObject.DTO.PrescriptionDTOs;
using BusinessObject.DTO.RecordTypeDTOs;
using BusinessObject.DTO.MedicinePrescriptionDTOs.View;
using BusinessObject.DTO.OrderServiceDTOs.View;
using BusinessObject.DTO.ClinicServiceDTOs.View;
using BusinessObject.DTO.ClinicDTOs.View;
using BusinessObject.DTO.CustomerDTOs.View;
using BusinessObject.DTO.ExaminationProfileDTOs.View;
using BusinessObject.DTO.MedicalRecordDTOs.View;
using BusinessObject.DTO.NotificationDTOs.View;

namespace Services
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            //USER
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

            /*----------------------------------------------------*/
            //DENTIST SLOT
            CreateMap<DentistSlot, CreateDentistSlotDTO>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber));

            CreateMap<DentistSlot, DentistSlotForExamDTO>();
            
            CreateMap<CreateDentistSlotDTO, DentistSlot>()
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => src.TimeEnd))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status))
                .ForMember(dest => dest.DentistId, otp => otp.MapFrom(src => src.DentistId))
                .ForMember(dest => dest.RoomId, otp => otp.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.DentistSlotId, otp => otp.Ignore())
                .ForMember(dest => dest.Dentist, otp => otp.Ignore())
                .ForMember(dest => dest.Examinations, otp => otp.Ignore())
                .ForMember(dest => dest.Room, otp => otp.Ignore());

            /*----------------------------------------------------*/
            //MEDICINE
            CreateMap<MedicineDTO, Medicine>()
                .ReverseMap();
            
            CreateMap<MedicinePrescription, MedicinePrescriptionDTO>(); 
            
            CreateMap<MedicineUpdateDTO, Medicine>()
                .ReverseMap();

            /*----------------------------------------------------*/
            //RECORD TYPE
            CreateMap<RecordTypeCreateDTO, RecordType>().ReverseMap();
            
            /*----------------------------------------------------*/
            //SERVICE
            CreateMap<CreateServiceDTO, Service>().ReverseMap();
            
            CreateMap<Service, ServiceDTO>();
            
            /*----------------------------------------------------*/
            //MEDICAL RECORD
            CreateMap<MedicalRecordRequestDTO, MedicalRecord>().ReverseMap();
            
            CreateMap<MedicalRecord, MedicalRecordDTO>()
                .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.RecordType.RecordName));
            
            /*----------------------------------------------------*/
            //ROOM
            CreateMap<CreateRoomDTO, Room>().ReverseMap();
            
            CreateMap<Room, RoomDTO>();
            
            /*----------------------------------------------------*/
            //EXAMINATION
            CreateMap<ExaminationDTO, Examination>().ReverseMap();
            
            CreateMap<ExaminationRequestDTO, ExaminationDTO>().ReverseMap();
            
            CreateMap<ExaminationRequestDTO, Examination>().ReverseMap();

            /*----------------------------------------------------*/
            //EXAMINATION PROFILE
            CreateMap<ExaminationProfile, ExaminationProfileForExamDTO>();
            CreateMap<ExaminationProfile, ExaminationProfileDTO>();

            /*----------------------------------------------------*/
            //ORDER
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderServices, opt => opt.MapFrom(src => src.OrderServices));
            
            CreateMap<OrderService, OrderServiceDTO>();
            
            CreateMap<OrderCreateDTO, Order>().ReverseMap();
            
            /*----------------------------------------------------*/
            //PRESCRIPTION
            CreateMap<Prescription, PrescriptionDTO>()
                .ForMember(dest => dest.MedicinePrescriptions, opt => opt.MapFrom(src => src.MedicinePrescriptions));
            
            CreateMap<PrescriptionDTO, Prescription>();
            
            CreateMap<PrescriptionCreateDTO, Prescription>().ReverseMap();
            
            /*----------------------------------------------------*/
            //CLINIC SERVICE
			CreateMap<ClinicServiceDTO, BusinessObject.Entity.ClinicService>().ReverseMap();
            
            /*----------------------------------------------------*/
            //CLINIC
            CreateMap<Clinic, ClinicDTO>().ReverseMap();
            
            /*----------------------------------------------------*/
           //NOTIFICATION
            CreateMap<NotificationDTO, Notification>().ReverseMap();
            
            CreateMap<NotificationRequestDTO, Notification>().ReverseMap();
            
            /*----------------------------------------------------*/
            //CUSTOMER
            CreateMap<CustomerRequestDTO, Customer>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            
            CreateMap<CustomerDTO, Customer>().ReverseMap();
        }
    }
}
