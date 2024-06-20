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
            string key = "userList";
            List<User>? userList = new List<User>();


            CancellationToken cancellationToken = default;            
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            var s = ConvertToRedisKey("user","list");

            var cacheMember = await distributedCache.GetStringAsync(s, cancellationToken);
            
            var db = redis.GetDatabase();


            if (cacheMember.IsNullOrEmpty())
            {
                userList = await FindAllAsync();

                if (userList.IsNullOrEmpty())
                {
                    return userList;
                }

                foreach (var user in userList)
                {
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    await db.ListRightPushAsync(key, JsonConvert.SerializeObject(user, settings));
                }

                return userList.Skip((pageNumber - 1) * rowsPerPage)
                            .Take(rowsPerPage)
                            .ToList();
            }

            userList = JsonConvert.DeserializeObject<List<User>>(cacheMember);
            if (userList.IsNullOrEmpty())
            {
                return userList;
            }
            else
            {
                userList.Skip((pageNumber - 1) * rowsPerPage)
                            .Take(rowsPerPage)
                            .ToList();
            }
            return userList;
        }

        public string getUserName(string Id)
        {
            var userId = Guid.Parse(Id);
            return _repositoryContext.Users
        .Where(user => user.UserId == userId)
        .Select(u => u.UserName)
        .FirstOrDefault();
        }

        public async Task<string> DeleteCache(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return "Empty key";
            }
            else
            {
                CancellationToken cancellationToken = default;
                string? checkCache = await distributedCache.GetStringAsync(key, cancellationToken);

                if (checkCache.IsNullOrEmpty())
                {
                    return "No value with this key";
                }

                await distributedCache.RemoveAsync(key);
                return "Remove key successfully!";
            }
        }

        public string ConvertToRedisKey(string prefix, string identifier)
        {
            return $"{prefix}:{identifier}"; 
        }
    }
}
