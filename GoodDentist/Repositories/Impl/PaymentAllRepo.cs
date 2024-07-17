using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class PaymentAllRepo : RepositoryBase<PaymentAll>, IPaymentAllRepo
    {
        public PaymentAllRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<PaymentAll>> GetAllPayment(int pageNumber, int rowsPerPage)
        {
            return await _repositoryContext.PaymentAlls
                .Include(x => x.PaymentOrder)
                .Include(x => x.PaymentPrescription)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
        }

        public async Task<PaymentAll> GetPaymentById(int id)
        {
            PaymentAll? model =  await _repositoryContext.PaymentAlls
                .Include(x => x.PaymentOrder)
                .Include(x => x.PaymentPrescription)
                .FirstOrDefaultAsync(x => x.PaymentAllId == id);
            return model;
        }

        public async Task CreatePayment(PaymentAll paymentAll)
        {
            await _repositoryContext.PaymentAlls.AddAsync(paymentAll);
            await _repositoryContext.SaveChangesAsync();
            
        }

        public async Task UpdatePayment(PaymentAll paymentAll)
        {
            PaymentAll? model = await _repositoryContext.PaymentAlls
                .Include(x => x.PaymentOrder)
                .Include(x => x.PaymentPrescription)
                .FirstOrDefaultAsync(x => x.PaymentAllId == paymentAll.PaymentAllId);
    
            if (model != null)
            {
                // Update the PaymentAll entity
                _repositoryContext.Entry(model).CurrentValues.SetValues(paymentAll);
        
                // Handle Payment for Order's Services
                if (paymentAll.PaymentOrder != null)
                {
                    if(model.PaymentOrder!=null) _repositoryContext.Payments.Remove(model.PaymentOrder);
                    await _repositoryContext.SaveChangesAsync();
                    model.PaymentOrder = paymentAll.PaymentOrder;
                }

                //  Handle Payment for Prescription's Medicines
                if (paymentAll.PaymentPrescription != null)
                {
                    if(model.PaymentPrescription!=null) _repositoryContext.PaymentPrescriptions.Remove(model.PaymentPrescription);
                    await _repositoryContext.SaveChangesAsync();
                    model.PaymentPrescription = paymentAll.PaymentPrescription;
                }
                await _repositoryContext.SaveChangesAsync();
            }
        }


        public async Task DeletePayment(int id)
        {
            PaymentAll? model =  await _repositoryContext.PaymentAlls
                .Include(x => x.PaymentOrder)
                .Include(x => x.PaymentPrescription)
                .FirstOrDefaultAsync(x => x.PaymentAllId == id);
            if (model != null)
            {
                // delete all prescription payment
                if(model.PaymentPrescription != null)
                {
                    _repositoryContext.PaymentPrescriptions.RemoveRange(model.PaymentPrescription);
                }
                // delete all order payment
                if (model.PaymentOrder != null)
                {
                    _repositoryContext.Payments.Remove(model.PaymentOrder);
                }
                model.Status = false;
                await _repositoryContext.SaveChangesAsync();
            }
        }

        public async Task<List<PaymentAll>> GetPaymentsPerYear(int year)
        {
            return await _repositoryContext.PaymentAlls.Where(pa => pa.Date.Value.Date.Year.Equals(year)).ToListAsync();
        }
    }
}
