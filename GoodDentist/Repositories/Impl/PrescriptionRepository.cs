using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;

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

		public async Task<List<Prescription>> GetPrescriptions(int pageNumber, int pageSize)
		{
			var models = await _repositoryContext.Prescriptions
				.Include(x => x.MedicinePrescriptions)
				.ThenInclude(x=>x.Medicine)
				.ToListAsync();
			return models;
		}

		public async Task<Prescription> UpdatePresription(Prescription prescription)
		{
			var model = await _repositoryContext.Prescriptions
				.Include(x => x.MedicinePrescriptions)
				.ThenInclude(x => x.Medicine)
				.FirstOrDefaultAsync(x => x.PrescriptionId == prescription.PrescriptionId);
			_repositoryContext.Entry(model).CurrentValues.SetValues(prescription);
			_repositoryContext.SaveChangesAsync();
			return model;
		}

		public void CreatePrescription(Prescription prescription)
		{
			_repositoryContext.Prescriptions.Add(prescription);
			_repositoryContext.SaveChangesAsync();
		}
	}
}
