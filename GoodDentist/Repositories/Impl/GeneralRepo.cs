using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class GeneralRepo : IGeneralRepo
    {
        private readonly GoodDentistDbContext goodDentistDbContext;

        public GeneralRepo(GoodDentistDbContext goodDentistDbContext)
        {
            this.goodDentistDbContext = goodDentistDbContext;   
        }
        public async Task<int> ToTalCount(string type)
        {
            int total = 0;
            switch (type)
            {
                case "User":
                    total = await goodDentistDbContext.Users.CountAsync();
                    break;
                case "Service":
                    total = await goodDentistDbContext.Services.CountAsync();
                    break;
                case "Medicine":
                    total = await goodDentistDbContext.Medicines.CountAsync();
                    break;
                case "Clinic":
                    total = await goodDentistDbContext.Medicines.CountAsync();
                    break;
                case "Room":
                    total = await goodDentistDbContext.Medicines.CountAsync();
                    break;
                default:
                    break;
            }
            return total;
        }
    }
}
