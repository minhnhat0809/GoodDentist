using BusinessObject.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPaymentAllRepo
{
    Task<List<PaymentAll>> GetAllPayment(int pageNumber, int rowsPerPage);
    Task<PaymentAll> GetPaymentById(int id);
    Task CreatePayment(PaymentAll paymentAll);
    Task UpdatePayment(PaymentAll paymentAll);
    Task DeletePayment(int id);
    Task<List<PaymentAll>> GetPaymentsPerYear(int year);
    
    Task<List<PaymentAll>> GetPaymentsInRange(DateOnly DateStart, DateOnly DateEnd);
}