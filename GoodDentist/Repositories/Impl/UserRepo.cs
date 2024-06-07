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

        public async Task<List<User>> GetAllUsers()
        {
            //string key = "userList";
            List<User>? userList = new List<User>();
            userList = _repositoryContext.Users.ToList();
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

    }
}
