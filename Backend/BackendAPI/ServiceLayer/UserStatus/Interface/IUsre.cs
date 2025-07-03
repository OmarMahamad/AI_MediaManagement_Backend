using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ServiceLayer.Dtos.userDtos;

namespace ServiceLayer.UserStatus.Interface
{
    public interface IUsre
    {
        Task<string>LoginAsync(LoginDto loginDto);
        //Task LogoutAsync();
        Task DeleteUserAsync(int id);
        Task<string> RegisterUserAsync(RegisterDto registerDto, string Pathfile);
        Task ForgotPasswordAsync();
        
    }
}
