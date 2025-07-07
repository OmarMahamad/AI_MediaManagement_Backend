using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.UserEntity;
using ServiceLayer.Dtos.userDtos;

namespace ServiceLayer.AuthorizationStatus.Interface
{
    public interface IAuthStatus
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto registerDto, string Pathfile);
        Task<AuthResponseDto> ForgotPasswordAsync(string email);
        Task<AuthResponseDto> CheckOtpCode(string code);
        Task<bool> EmailVerified(string email);
        Task<bool> IsEmailVerified(string email);
        Task SandVerifiedTokenToEmail(User user);
        Task<AuthResponseDto> LogoutAsync(string token);
        Task<AuthResponseDto> ChangePasswordAsync(ChangePassordDto passordDto);

    }
}
