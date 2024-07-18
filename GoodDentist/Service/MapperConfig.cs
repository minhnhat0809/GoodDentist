using AutoMapper;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTO.ClinicDTOs;
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
using BusinessObject.DTO.OrderServiceDTOs;
using BusinessObject.DTO.DentistSlotDTOs;
using BusinessObject.DTO.PaymentDTOs;
using BusinessObject.DTO.PaymentDTOs.View;

namespace Services
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {

            /*----------------------------------------------------*/
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
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Clinics, opt => opt.MapFrom(src => src.ClinicUsers.Select(cu => cu.Clinic)));
           
            CreateMap<User, UserForExamDTO>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? new DateTime(src.Dob.Value.Year, src.Dob.Value.Month, src.Dob.Value.Day) : (DateTime?)null))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId ?? default(int)))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar));

            /*----------------------------------------------------*/
            //DENTIST SLOT
            CreateMap<DentistSlot, UpdateDentistSlotDTO>();
                
            CreateMap<DentistSlot, DentistSlotForExamDTO>();

            CreateMap<DentistSlot, DentistSlotDTO>();

            CreateMap<DentistSlot, DentistAndSlotDTO>();

            CreateMap<UpdateDentistSlotDTO, DentistSlot>()
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => src.TimeStart))
                .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => src.TimeEnd))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status))
                .ForMember(dest => dest.DentistId, otp => otp.MapFrom(src => src.DentistId))
                .ForMember(dest => dest.RoomId, otp => otp.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.DentistSlotId, otp => otp.Ignore())
                .ForMember(dest => dest.Dentist, otp => otp.Ignore())
                .ForMember(dest => dest.Examinations, otp => otp.Ignore())
                .ForMember(dest => dest.Room, otp => otp.Ignore());

            CreateMap<CreateDentistSlotDTO, UpdateDentistSlotDTO>();

            CreateMap<CreateDentistSlotDTO, DentistSlot>();
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
            CreateMap<Examination, ExaminationDTO>()
                .ForMember(dest => dest.ExaminationProfile, opt => opt.MapFrom(src => src.ExaminationProfile));
            
            CreateMap<ExaminationRequestDTO, ExaminationDTO>().ReverseMap();
            
            CreateMap<ExaminationRequestDTO, Examination>().ReverseMap();

            CreateMap<Examination, ExaminationForDentistSlotDTO>();

            /*----------------------------------------------------*/
            //EXAMINATION PROFILE
            CreateMap<ExaminationProfile, ExaminationProfileForExamDTO>()
                //.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                //.ForMember(dest => dest.Dentist, opt => opt.MapFrom(src => src.Dentist))
                ;

            CreateMap<ExaminationProfile, ExaminationProfileDTO>();

            CreateMap<ExaminationProfile, ExaminationProfileForCustomerDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));

            /*----------------------------------------------------*/
            //ORDER
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderServices, opt => opt.MapFrom(src => src.OrderServices));
            // order - create
            CreateMap<OrderServiceDTO, OrderService>().ReverseMap();
            CreateMap<OrderServiceCreateDTO, OrderService>().ReverseMap();
            CreateMap<OrderCreateDTO, Order>().ReverseMap();

            // order - update
            CreateMap<ServiceToOrderDTO, Service>().ReverseMap();
            CreateMap<OrderUpdateDTO, Order>().ReverseMap();

            CreateMap<OrderDTO, Order>().ReverseMap();

            /*----------------------------------------------------*/
            //PRESCRIPTION
            CreateMap<Prescription, PrescriptionDTO>()
                .ForMember(dest => dest.MedicinePrescriptions, opt => opt.MapFrom(src => src.MedicinePrescriptions));
            
            CreateMap<PrescriptionDTO, Prescription>();
            
            CreateMap<PrescriptionCreateDTO, Prescription>().ReverseMap();
            CreateMap<PrescriptionUpdateDTO, Prescription>().ReverseMap();

            /*----------------------------------------------------*/
            //CLINIC SERVICE
			CreateMap<ClinicServiceDTO, BusinessObject.Entity.ClinicService>().ReverseMap();
            
            /*----------------------------------------------------*/
            //CLINIC
            CreateMap<Clinic, ClinicDTO>().ReverseMap();
            CreateMap<Clinic, ClinicCreateDTO>().ReverseMap();
            CreateMap<Clinic, ClinicUpdateDTO>().ReverseMap();
            
            /*----------------------------------------------------*/
           //NOTIFICATION
            CreateMap<NotificationDTO, Notification>().ReverseMap();
            
            CreateMap<NotificationRequestDTO, Notification>().ReverseMap();
            
            /*----------------------------------------------------*/
            //CUSTOMER
            CreateMap<CustomerUpdateRequestDTO, CustomerRequestDTO>();
            
            CreateMap<CustomerRequestDTO, Customer>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Dob.Value)))
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Clinics, opt => opt.MapFrom(src => src.CustomerClinics.Select(cc => cc.Clinic)));


            CreateMap<Customer, CustomerDTOForPhuc>()
                .ForMember(dest => dest.Clinics, opt => opt.MapFrom(src => src.CustomerClinics.Select(cc => cc.Clinic)));

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

            CreateMap<Customer, CustomerForExamDTO>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob.HasValue ? new DateTime(src.Dob.Value.Year, src.Dob.Value.Month, src.Dob.Value.Day) : (DateTime?)null))            
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId));

            CreateMap<Customer, CustomerDTOForLoc>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Clinics, opt => opt.MapFrom(src => src.CustomerClinics.Select(cc => cc.Clinic)))
                .ForMember(dest => dest.ExaminationProfiles, opt => opt.MapFrom(src => src.ExaminationProfiles.Select(ep => ep.ExaminationProfileId)));
            
            /*----------------------------------------------------*/
            // PAYMENT
            CreateMap<PaymentAll, PaymentAllDTO>().ReverseMap();
            
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<PaymentPrescription, PaymentPrescriptionDTO>().ReverseMap();
            
            CreateMap<PaymentAllCreateDTO, PaymentAll>().ReverseMap();
            CreateMap<PaymentAllUpdateDTO, PaymentAll>().ReverseMap();
            
            CreateMap<Order, ServiceToOrderDTO>().ReverseMap();
            /*----------------------------------------------------*/
        }
    }
}
