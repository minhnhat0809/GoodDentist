using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Impl;

public class PaymentRepo: RepositoryBase<Payment>, IPaymentRepo
{
    public PaymentRepo(GoodDentistDbContext repositoryContext) : base(repositoryContext)
    {
        
    }
    
}