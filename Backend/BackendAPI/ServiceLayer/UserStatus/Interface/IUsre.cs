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
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task DeleteUserAsync(int id);
        Task<string> RegisterUserAsync(RegisterDto registerDto, string Pathfile);
        Task<AuthResponseDto> ForgotPasswordAsync(string email);
        Task<AuthResponseDto> CheckOtpCode(string code);
        Task <UserResponDto> GetUserByIDAsync(int id);
        Task<bool> EmailVerified(string email);
        Task<bool> IsEmailVerified(string email);
        Task  SandVerifiedTokenToEmail(User user);
        Task<AuthResponseDto> LogoutAsync(int id);

        Task<AuthResponseDto> EditUserAsync(int id,EditUserDto editUserDto);
        Task<AuthResponseDto> EditUserAsync(int id,string pathfile);
        Task<AuthResponseDto> ChangePasswordAsync(ChangePassordDto passordDto);

    }
}
