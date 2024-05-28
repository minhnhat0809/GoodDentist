using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class AccountService : IAccountService
    {
        private const int saltSize = 128 / 8;
        private const int keySize = 256 / 8;
        private const int iterations = 10000;
        private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        public string hashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(saltSize);

            var hashedPassword = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return string.Join(Convert.ToBase64String(salt), Convert.ToBase64String(hashedPassword));
        }

        public bool verifyPassword(string inputPassword, string hashedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
