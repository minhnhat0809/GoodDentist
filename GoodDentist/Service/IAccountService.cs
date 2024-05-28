using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccountService
    {
        string hashPassword(string password);

        bool verifyPassword(string inputPassword, string hashedPassword);


    }
}
