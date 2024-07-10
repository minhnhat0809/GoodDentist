using BusinessObject.DTO;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class GeneralService : IGeneralService
    {
        private readonly IUnitOfWork unitOfWork;

        public GeneralService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetTotal(string type)
        {
            ResponseDTO responseDTO = new ResponseDTO("", 200, true, null);
            int total = await unitOfWork.generalRepo.ToTalCount(type);
            responseDTO.Result = total;
            return responseDTO;
        }
    }
}
