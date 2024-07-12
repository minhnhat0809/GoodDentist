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

        public UserRepo(GoodDentistDbContext goodDentistDbContext) : base(goodDentistDbContext)
        {
            
        }      

        public User? getUser(string userName)
        {
                return _repositoryContext.Users.Include(u => u.ClinicUsers).FirstOrDefault(user => user.UserName == userName);           
        }       

        public async Task<List<User>> GetAllUsers(int pageNumber, int rowsPerPage)
        {
            return await _repositoryContext.Users
                .Include(x => x.ClinicUsers)
                .ThenInclude(cu => cu.Clinic)
                .Skip((pageNumber - 1) * rowsPerPage)
                .Take(rowsPerPage)
                .ToListAsync();
        }

        public string getUserName(string Id)
        {
            var userId = Guid.Parse(Id);
            return _repositoryContext.Users
        .Where(user => user.UserId == userId)
        .Select(u => u.Name)
        .FirstOrDefault();
        }

        public bool checkUniqueUserName(string userName)
        {
            return _repositoryContext.Users.Any(u => u.UserName.Equals(userName));
        }

        public bool checkUniqueEmail(string email)
        {
            return _repositoryContext.Users.Any(u => u.Email.Equals(email));
        }
    }
}
