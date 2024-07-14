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

		public async Task<Prescription> UpdatePrescription(Prescription prescription)
		{
			var model = await _repositoryContext.Prescriptions
				.Include(x => x.MedicinePrescriptions)
				.ThenInclude(x => x.Medicine)
				.FirstOrDefaultAsync(x => x.PrescriptionId == prescription.PrescriptionId);
			if (model != null)
			{
				
				_repositoryContext.Entry(model).CurrentValues.SetValues(prescription);
				_repositoryContext.MedicinePrescriptions.RemoveRange(model.MedicinePrescriptions);
				//await _repositoryContext.SaveChangesAsync();

				foreach (MedicinePrescription? prescriptionMedicine in prescription.MedicinePrescriptions.ToList())
				{
					prescriptionMedicine.Status = prescription.Status;
					model.MedicinePrescriptions.Add(prescriptionMedicine);
				}
				
				//await _repositoryContext.SaveChangesAsync();
			}
			return model;
		}

		public async Task<Prescription> CreatePrescription(Prescription prescription)
		{
			_repositoryContext.Prescriptions.Add(prescription);
			 await _repositoryContext.SaveChangesAsync();
			 return prescription;
		}

		public async Task<Prescription> DeletePrescription(int prescriptionId)
		{
			var model = await _repositoryContext.Prescriptions
				.Include(x => x.MedicinePrescriptions)
				.ThenInclude(x => x.Medicine)
				.FirstOrDefaultAsync(x => x.PrescriptionId == prescriptionId);
			if (model != null)
			{
				model.Status = false;
				foreach (var medicinePrescription in model.MedicinePrescriptions)
				{
					medicinePrescription.Status = false;
				}

				await _repositoryContext.SaveChangesAsync();
			}
			return model;
		}

		public async Task<Prescription?> GetPrescriptionById(int prescriptionId)
		{
			return await _repositoryContext.Prescriptions
				.Include(p => p.MedicinePrescriptions).ThenInclude(mp => mp.Medicine)
				.FirstOrDefaultAsync(p => p.PrescriptionId == prescriptionId);
		}
	}
}
