using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.AuthorizationEntity;
using RepositoryLayer.Interface;
using SecurityLayer.Intercafe;

namespace SecurityLayer.Service
{
    public class SecurityService : Isecurity
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 100000; // قوة التكرار
        private readonly IRepository<OtpCode> _codeRepository;

        public SecurityService(IRepository<OtpCode> codeRepository)
        {
            _codeRepository = codeRepository;
        }
        public string HashPassword(string originalValue, out string salt)
        {
            // توليد Salt عشوائي
            var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            salt = Convert.ToBase64String(saltBytes);

            // تنفيذ PBKDF2
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(originalValue),
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize
            );

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string hashedValue, string originalValue, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(originalValue),
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize
            );

            var newHash = Convert.ToBase64String(hashBytes);

            return hashedValue == newHash;
        }

        public string VerifyEmail(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
                throw new ArgumentException("Invalid email format.");

            return email;
        }

        public string GenerateOtpCode(int length = 6)
        {
            const string validChars = "0123456789";
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            var result = new StringBuilder(length);
            foreach (var b in bytes)
            {
                result.Append(validChars[b % validChars.Length]);
            }

            return result.ToString();
        }

        


    }
}
