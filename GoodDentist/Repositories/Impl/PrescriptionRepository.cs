using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;

namespace Repositories.Impl
{
    public class PrescriptionRepository : RepositoryBase<Prescription>, IPrescriptionRepository
	{
        public PrescriptionRepository(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        { 
        }
		public async Task<List<Prescription>?> GetAllPrescription(int pageNumber, int pageSize)
		{
			List<Prescription> prescriptions = await Paging(pageNumber, pageSize);
			return prescriptions;
		}
	}
}
