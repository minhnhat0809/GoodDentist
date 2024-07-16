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
        
                // Handle Payments
                if (model.PaymentOrder != null)
                {
                    // Remove old Payments that are in paymentAll
                    
                            //_repositoryContext.Payments.Remove(payment);
                    
                }

                // Remove existing PaymentPrescription
                if (model.PaymentPrescription != null)
                {
                    _repositoryContext.PaymentPrescriptions.Remove(model.PaymentPrescription);
                }
        
                //await _repositoryContext.SaveChangesAsync();

                // Add new Payments and update their status
                /*foreach (Payment payment in paymentAll.Payments.ToList())
                {
                    // Set the payment status to match the PaymentAll status
                    payment.Status = paymentAll.Status;
            
                    // Add the new Payment
                    model.Payments?.Add(payment);
                }*/

                // Update PaymentPrescription
                model.PaymentPrescription = paymentAll.PaymentPrescription;

               // await _repositoryContext.SaveChangesAsync();
            }
        }


        public async Task DeletePayment(int id)
        {
            PaymentAll? model =  await _repositoryContext.PaymentAlls
                //.Include(x => x.Payments)
                .Include(x => x.PaymentPrescription)
                .FirstOrDefaultAsync(x => x.PaymentAllId == id);
            if (model != null)
            {
                /*if(model.Payments != null)
                {
                    _repositoryContext.Payments.RemoveRange(model.Payments);
                }*/
                if(model.PaymentPrescription != null)
                {
                    _repositoryContext.PaymentPrescriptions.RemoveRange(model.PaymentPrescription);
                }
                await _repositoryContext.SaveChangesAsync();
                
                _repositoryContext.PaymentAlls.Remove(model);
                await _repositoryContext.SaveChangesAsync();
            }
        }
    }
}
