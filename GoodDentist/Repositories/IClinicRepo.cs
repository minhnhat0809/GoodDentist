using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IClinicRepo : IRepositoryBase<Clinic>
    {
        Task<Clinic?> getClinicById(string id);
    }
}
