using BusinessObject;
using Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepo userRepo { get; }
        IClinicUserRepo clinicUserRepo { get; }
        IRoleRepo roleRepo { get; }
        IClinicRepo clinicRepo { get; }
        IServiceRepo serviceRepo { get; }
        IRoomRepo roomRepo { get; }
        IDentistSlotRepo dentistSlotRepo { get; }
        IMedicineRepository medicineRepo { get; }
        IRecordTypeRepository recordTypeRepo { get; }
        IClinicRepository ClinicRepository { get; }
        IExaminationRepo examinationRepo { get; }
        IMedicalRecordRepository MedicalRecordRepository { get; }
        IClinicServiceRepo clinicServiceRepo { get; }
        IGeneralRepo generalRepo { get; }
        IExamProfileRepo examProfileRepo { get; }
        IOrderRepository orderRepo { get; }
        IPrescriptionRepository prescriptionRepo { get; }

        ICustomerRepo customerRepo { get; }
        ICustomerClinicRepository CustomerClinicRepository { get; }
        Task<int> CompleteAsync();

        INotificationRepository NotificationRepository { get; }
    }
}
