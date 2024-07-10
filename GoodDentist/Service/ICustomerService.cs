using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICustomerService
    {
        Task<ResponseDTO> GetAllCustomerOfDentist(string dentistId);
        Task<ResponseDTO> GetAllCustomers(string search);
    }
}
