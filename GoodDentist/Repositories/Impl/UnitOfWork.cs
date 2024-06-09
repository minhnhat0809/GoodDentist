using BusinessObject;
using Microsoft.EntityFrameworkCore;
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
        public IDistributedCache distributedCache { get; private set; }
        public IMedicineRepository medicineRepo { get; private set; }
        public IRoomRepo roomRepo { get; private set; }
        public IRecordTypeRepository recordTypeRepo { get; private set; }


        public UnitOfWork(GoodDentistDbContext context, IDistributedCache cache)
        {
            _repositoryContext = context;
            distributedCache = cache;
            userRepo = new UserRepo(_repositoryContext, distributedCache);
            clinicUserRepo = new ClinicUserRepo(_repositoryContext); 
            roleRepo = new RoleRepo(_repositoryContext);
            clinicRepo = new ClinicRepo(_repositoryContext);
            dentistSlotRepo = new DentistSlotRepo(_repositoryContext);
            medicineRepo = new MedicineRepository(_repositoryContext);
            roomRepo = new RoomRepo(_repositoryContext);
            recordTypeRepo = new RecordTypeRepository(_repositoryContext);
            
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
