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
        Task<ResponseDTO> GetAllCustomerOfDentist(string dentistId, string search);
        Task<ResponseDTO> GetAllCustomers(string search, int pageNumber, int rowsPerPage);
    }
}
