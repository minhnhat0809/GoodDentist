using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GoodDentistDbContext _repositoryContext;
        public IUserRepo userRepo { get; private set; }
        public IClinicUserRepo clinicUserRepo { get; private set; }
        public IRoleRepo roleRepo { get; private set; }
        public IClinicRepo clinicRepo { get; private set; }
        public IDentistSlotRepo dentistSlotRepo { get; private set; }
        public IClinicRepository ClinicRepository { get; }
        public IMedicineRepository medicineRepo { get; private set; }
        public IRoomRepo roomRepo { get; private set; }
        public IRecordTypeRepository recordTypeRepo { get; private set; }
        public IServiceRepo serviceRepo { get; private set; }

        public IExamProfileRepo examProfileRepo { get; private set; }
        public IExaminationRepo examinationRepo { get; private set; }
        public IMedicalRecordRepository MedicalRecordRepository { get; private set; }

		public IClinicServiceRepo clinicServiceRepo { get; private set; }

        public IGeneralRepo generalRepo { get; private set; }

        public IOrderRepository orderRepo { get; private set; }

        public IPrescriptionRepository prescriptionRepo { get; private set; }

        public INotificationRepository NotificationRepository { get; private set; }

        public ICustomerRepo customerRepo { get; private set; }

        
        public UnitOfWork(GoodDentistDbContext context, IDistributedCache cache)
        {
            _repositoryContext = context;
            userRepo = new UserRepo(_repositoryContext);
            clinicUserRepo = new ClinicUserRepo(_repositoryContext); 
            roleRepo = new RoleRepo(_repositoryContext);
            clinicRepo = new ClinicRepo(_repositoryContext);
            dentistSlotRepo = new DentistSlotRepo(_repositoryContext);
            medicineRepo = new MedicineRepository(_repositoryContext);
            roomRepo = new RoomRepo(_repositoryContext);
            examinationRepo = new ExaminationRepo(_repositoryContext);
            ClinicRepository = new ClinicRepository(_repositoryContext);
            recordTypeRepo = new RecordTypeRepository(_repositoryContext);
            MedicalRecordRepository = new MedicalRecordRepository(_repositoryContext);
			serviceRepo = new ServiceRepo(_repositoryContext);
            examProfileRepo = new ExamProfileRepo(_repositoryContext);
            clinicServiceRepo = new ClinicServiceRepo(_repositoryContext);
            generalRepo = new GeneralRepo(_repositoryContext);
            orderRepo = new OrderRepository(_repositoryContext);
            prescriptionRepo = new PrescriptionRepository(_repositoryContext);
            NotificationRepository = new NotificationRepository(_repositoryContext);
            customerRepo = new CustomerRepo(_repositoryContext);

        }

        public async Task<int> CompleteAsync()
        {
            return await _repositoryContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _repositoryContext.Dispose();
        }
    }
}
