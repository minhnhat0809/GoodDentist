using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


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
                return _repositoryContext.Users.FirstOrDefault(user => user.UserName == userName);           
        }       

        public async Task<List<User>> GetAllUsers(int pageNumber, int rowsPerPage)
        {
            //string key = "userList";
            List<User>? userList = new List<User>();
            userList = await FindAllAsync();
            userList = userList
               .Skip((pageNumber - 1) * rowsPerPage)
               .Take(rowsPerPage)
               .ToList();


            /*CancellationToken cancellationToken = default;
            string? cacheMember = await distributedCache.GetStringAsync(key, cancellationToken);*/
            /*if (cacheMember.IsNullOrEmpty())
            {
                userList = await FindAllAsync();

                if (userList.IsNullOrEmpty())
                {
                    return userList;
                }

                await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(userList), cancellationToken);

                return userList;
            }

            userList = JsonConvert.DeserializeObject<List<User>>(cacheMember);  */

            return userList;
        }

        public async Task<User> LoginAccount(string username, byte[] password)
        {
            return await _repositoryContext.Users
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
