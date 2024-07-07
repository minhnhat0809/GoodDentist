using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Entity;

namespace Repositories
{
	public interface IPrescriptionRepository : IRepositoryBase<Prescription>
	{
		Task<List<Prescription>?> GetAllPrescription(int pageNumber, int pageSize);
	}
}
