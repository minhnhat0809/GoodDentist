using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace Repositories.Impl
{
    public class AccountRepo : RepositoryBase<User>, IAccountRepo
    {
        private readonly IDistributedCache distributedCache;
        public AccountRepo(GoodDentistDbContext goodDentistDbContext, IDistributedCache distributedCache) : base(goodDentistDbContext)
        {
            this.distributedCache = distributedCache;
        }      

        public bool checkExistUser(string userName)
        {
            try
            {
                return _repositoryContext.Users.Any(user => user.UserName == userName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void CreateUser(User user)
        {
            try
            {
                CreateAsync(user).Wait();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);               
            }
            
        }

        public async Task<List<User>> GetAllUsers()
        {
            string key = "userList";
            List<User>? userList = new List<User>();
            CancellationToken cancellationToken = default;
            string? cacheMember = await distributedCache.GetStringAsync(key, cancellationToken);

            if (cacheMember.IsNullOrEmpty())
            {
                userList = await FindAllAsync();

                if (userList.IsNullOrEmpty())
                {
                    return userList;
                }

                await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(userList), cancellationToken);

                return userList;
            }

            userList = JsonConvert.DeserializeObject<List<User>>(cacheMember);                      
            return userList;
        }

    }
}
