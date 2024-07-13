using BusinessObject;
using BusinessObject.Entity;

namespace Repositories.Impl
{
    public interface IOrderServiceRepository : IRepositoryBase<OrderService>
    {
        
    }

    public class OrderServiceRepository : RepositoryBase<OrderService>, IOrderServiceRepository
    {
        public OrderServiceRepository(GoodDentistDbContext repositoryContext) : base(repositoryContext)
        {
        }
        
    }

}
