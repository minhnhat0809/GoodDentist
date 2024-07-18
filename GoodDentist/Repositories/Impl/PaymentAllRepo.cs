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
                .Include(p => p.PaymentOrder)
                    .ThenInclude(po => po.Order)
                        .ThenInclude(o => o.Examination)
                            .ThenInclude(e => e.ExaminationProfile)
                .Include(p => p.PaymentOrder)
                    .ThenInclude(po => po.Order)
                        .ThenInclude(o => o.Examination)
                            .ThenInclude(e => e.Orders)
                .Include(p => p.PaymentOrder)
                    .ThenInclude(po => po.Order)
                        .ThenInclude(o => o.Examination)
                            .ThenInclude(e => e.Prescriptions)
                .Include(p => p.PaymentPrescription)
                    .ThenInclude(pp => pp.Prescription)
                        .ThenInclude(pr => pr.Examination)
                            .ThenInclude(e => e.ExaminationProfile)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
        }
public async Task<List<PaymentAll>> GetAllPaymentsForCustomer(Guid customerId, int pageNumber, int rowsPerPage, string? sortField, string? sortOrder)
    {
        IQueryable<PaymentAll> paymentsQuery = _repositoryContext.PaymentAlls
            .Include(p => p.PaymentOrder)
                .ThenInclude(po => po.Order)
                    .ThenInclude(o => o.Examination)
                        .ThenInclude(e => e.ExaminationProfile)
            .Include(p => p.PaymentPrescription)
                .ThenInclude(pp => pp.Prescription)
                    .ThenInclude(p => p.Examination)
                        .ThenInclude(e => e.ExaminationProfile)
            .Where(p =>
                (p.PaymentOrder != null && p.PaymentOrder.Order != null && p.PaymentOrder.Order.Examination != null && p.PaymentOrder.Order.Examination.ExaminationProfile != null && p.PaymentOrder.Order.Examination.ExaminationProfile.CustomerId == customerId) ||
                (p.PaymentPrescription != null && p.PaymentPrescription.Prescription != null && p.PaymentPrescription.Prescription.Examination != null && p.PaymentPrescription.Prescription.Examination.ExaminationProfile != null && p.PaymentPrescription.Prescription.Examination.ExaminationProfile.CustomerId == customerId))
            .AsQueryable();

        // Sorting
        if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
        {
            bool isAscending = sortOrder.ToLower() == "asc";
            paymentsQuery = sortField.ToLower() switch
            {
                "date" => isAscending ? paymentsQuery.OrderBy(u => u.Date) : paymentsQuery.OrderByDescending(u => u.Date),
                "detail" => isAscending ? paymentsQuery.OrderBy(u => u.PaymentDetail) : paymentsQuery.OrderByDescending(u => u.PaymentDetail),
                "total" => isAscending ? paymentsQuery.OrderBy(u => u.Total) : paymentsQuery.OrderByDescending(u => u.Total),
                "status" => isAscending ? paymentsQuery.OrderBy(u => u.Status) : paymentsQuery.OrderByDescending(u => u.Status),
                _ => paymentsQuery
            };
        }

        // Pagination
        return await paymentsQuery.Skip((pageNumber - 1) * rowsPerPage).Take(rowsPerPage).ToListAsync();
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

        public async Task<List<PaymentAll>> GetPaymentsInRange(DateTime DateStart, DateTime DateEnd)
        {
            return await _repositoryContext.PaymentAlls.
                Where(pa => pa.Date.Value.Date >= DateStart.Date && pa.Date.Value.Date <= DateEnd.Date)
                .ToListAsync();

        }
    }
}
