using BusinessObject.DTO;
using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IExaminationProfileService
    {
        Task<ResponseDTO> GetExaminationProfilesByCustomerId(string customerId);

    }
}
