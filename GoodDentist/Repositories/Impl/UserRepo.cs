﻿using BusinessObject;
using BusinessObject.Entity;
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
                return _repositoryContext.Users.Include(u => u.ClinicUsers).FirstOrDefault(user => user.UserName == userName);           
        }       

        public async Task<List<User>> GetAllUsers(int pageNumber, int rowsPerPage)
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
        
    }
}
