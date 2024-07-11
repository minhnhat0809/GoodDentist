using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICustomerRepo : IRepositoryBase<Customer>
    {
        Task<List<Customer>> GetAllCustomers(int pageNumber, int rowsPerPage);
    }
}
