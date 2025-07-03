using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLayer.Intercafe
{
    public interface Isecurity
    {
        
        string HashPassword(string originalValue, out string salt);
        bool VerifyPassword(string hashedValue, string originalValue, string salt);
        string VerifyEmail(string email);
        string GenerateCode();
    }
}
