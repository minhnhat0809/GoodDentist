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
                .Include(x => x.Payment)
                .Include(x => x.PaymentPrescription)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
        }

        public async Task<PaymentAll> GetPaymentById(int id)
        {
            return await _repositoryContext.PaymentAlls
                .Include(x => x.Payment)
                .Include(x => x.PaymentPrescription)
                .FirstOrDefaultAsync(x => x.PaymentAllId == id);
        }

        public async Task CreatePayment(PaymentAll paymentAll)
        {
            await _repositoryContext.PaymentAlls.AddAsync(paymentAll);
            await _repositoryContext.SaveChangesAsync();
        }

        public async Task UpdatePayment(PaymentAll paymentAll)
        {
            var existingPayment = await _repositoryContext.PaymentAlls.FindAsync(paymentAll.PaymentAllId);
            if (existingPayment != null)
            {
                _repositoryContext.Entry(existingPayment).CurrentValues.SetValues(paymentAll);
                await _repositoryContext.SaveChangesAsync();
            }
        }

        public async Task DeletePayment(int id)
        {
            var payment = await _repositoryContext.PaymentAlls.FindAsync(id);
            if (payment != null)
            {
                _repositoryContext.PaymentAlls.Remove(payment);
                await _repositoryContext.SaveChangesAsync();
            }
        }
    }
}
