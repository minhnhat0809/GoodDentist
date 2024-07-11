using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class CustomerRepo : RepositoryBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _repositoryContext.Customers
                .Include(c => c.CustomerClinics).ThenInclude(cc => cc.Clinic)
                .ToListAsync();
        }
    }
}
