using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entitys.UserEntity;
using ServiceLayer.Dtos.userDtos;

namespace ServiceLayer.UserStatus.Interface
{
    public interface IUsre
    {
        Task<string>LoginAsync(LoginDto loginDto);
        //Task LogoutAsync();
        Task DeleteUserAsync(int id);
        Task<string> RegisterUserAsync(RegisterDto registerDto, string Pathfile);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> CheckOtpCode(string code);

        Task<bool> EmailVerified(string email);
        Task<bool> IsEmailVerified(string email);
        Task  SandVerifiedTokenToEmail(User user);
        
        

    }
}
