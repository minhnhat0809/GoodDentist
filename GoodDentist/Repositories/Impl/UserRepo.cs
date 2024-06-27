using BusinessObject;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;


namespace Repositories.Impl
{
    public class UserRepo : RepositoryBase<User>, IUserRepo
    {
        private readonly IDistributedCache distributedCache;
        public UserRepo(GoodDentistDbContext goodDentistDbContext, IDistributedCache distributedCache) : base(goodDentistDbContext)
        {
            this.distributedCache = distributedCache;
        }      

        public User? getUser(string userName)
        {
                return _repositoryContext.Users.Include(u => u.ClinicUsers).FirstOrDefault(user => user.UserName == userName);           
        }       

        public async Task<List<User>> GetAllUsers(int pageNumber, int rowsPerPage)
        {
            return await Paging(pageNumber, rowsPerPage);                      
        }

        public string getUserName(string Id)
        {
            var userId = Guid.Parse(Id);
            return _repositoryContext.Users
        .Where(user => user.UserId == userId)
        .Select(u => u.UserName)
        .FirstOrDefault();
        }       


        public async Task<int> TotalUser()
        {
            return await _repositoryContext.Users.CountAsync();
        }
    }
}
