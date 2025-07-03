using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.UserEntity;

namespace AuthorizationLayer.Interface
{
    public interface IAuthorization
    {
        Task<string> GenerateTokenAsync(User user);       // توليد Access Token
        Task<string> GenerateRefreshTokenAsync();         // توليد Refresh Token
        Task<int> GetUserIdFromTokenAsync(string token);  // استخراج UserId من token
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token); // استخراج ClaimsPrincipal حتى لو الـ token منتهي
        bool ValidateToken(string token);


    }
}
