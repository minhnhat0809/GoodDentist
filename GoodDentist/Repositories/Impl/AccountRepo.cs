using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class AccountRepo : RepositoryBase<User>, IAccountRepo
    {
        public AccountRepo(GoodDentistDbContext goodDentistDbContext) : base(goodDentistDbContext)
        {

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
    }
}
