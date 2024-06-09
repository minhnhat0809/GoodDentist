using BusinessObject;
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
        IClinicRepository ClinicRepository { get; }
        Task<int> CompleteAsync();
    }
}
