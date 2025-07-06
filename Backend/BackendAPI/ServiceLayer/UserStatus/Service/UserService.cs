using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizationLayer.Interface;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using HelperLayer.Notifecation.Email.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using RepositoryLayer.Entitys.AuthorizationEntity;
using RepositoryLayer.Entitys.UserEntity;
using RepositoryLayer.Interface;
using SecurityLayer.Intercafe;
using ServiceLayer.Dtos.userDtos;
using ServiceLayer.UserStatus.Interface;

namespace ServiceLayer.UserStatus.Service
{
    public class UserService : IUsre
    {
        private readonly Isecurity _security;
        private readonly IAuthorization _auth;
        private readonly IEmail _email;
        private readonly IRepository<User> _repo;
        private readonly IRepository<OtpCode> _repocode;
        private readonly IRepository<EmailVerificationToken> _repoTokn;
        private readonly IRepository<AuthorizationToken> _repoAutho;
        private readonly MessageService _message;
        private string mes = "";
        public UserService(Isecurity security, IAuthorization auth, IRepository<EmailVerificationToken> repoTokn , IRepository<OtpCode> repocode, IRepository<AuthorizationToken> repoAutho , IEmail email, IRepository<User> repo, MessageService message)
        {
            _repoTokn = repoTokn;
            _repocode = repocode;
            _auth = auth;
            _repoAutho = repoAutho;
            _security = security;
            _email = email;
            _repo = repo;
            _message = message;
        }

        public async Task<AuthResponseDto> ChangePasswordAsync(ChangePassordDto passordDto)
        {
            var userEntity = await _repo.FirstOrderAsync(u => u.Email == passordDto.email);
            if (userEntity == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.EmailNotFound, Language.English)
                };
            }
            var hach = _security.HashPassword(passordDto.NewPassword, out string salt);
            userEntity.PasswordSalt = salt;
            userEntity.PasswordHash = hach;
            await _repo.EditItemAsync(userEntity.UserId,userEntity);
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English)
            };

        }

        public async Task<AuthResponseDto> CheckOtpCode(string code)
        {
            var codeEntity = await _repocode.FirstOrderAsync(c => c.Code == code);
            if (codeEntity == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.CodeNotFound, Language.English)
                };

            if (codeEntity.IsUsed)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UsedCode, Language.English)
                };

            if (codeEntity.ExpiryDate <= DateTime.UtcNow)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.ExpiryDateCode, Language.English)
                };

            // الكود سليم → نعدله إنه مستخدم
            codeEntity.IsUsed = true;
            await _repocode.EditItemAsync(codeEntity.OtpCodeId, codeEntity);
            await _repocode.DeleteItemAsync(codeEntity.OtpCodeId);

            return new AuthResponseDto { IsSuccess = true, Message = _message.GetMessage(MessageKeys.AccessCode, Language.English) };
        }
        public async Task DeleteUserAsync(int id)=> await _repo.DeleteItemAsync(id);

        public async Task<AuthResponseDto> EditUserAsync(int id,EditUserDto editUserDto)
        {
            var user=await _repo.GetItemAsync(id);
            if (user == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound, Language.English)
                };
            await _repo.EditItemAsync(id, user);
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message ="The modification process was successful."
            };
        }

        public async Task<AuthResponseDto> EditUserAsync(int id, string pathfile)
        {
            var user = await _repo.GetItemAsync(id);
            if (user == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound, Language.English)
                };
            user.ImagePath= pathfile;
            await _repo.EditItemAsync(id, user);
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "The modification process was successful."
            };
        }

        public async Task<bool> EmailVerified(string email)
        {
            var user = await _repo.FirstOrderAsync(u => u.Email == email);
            if (user == null) return false;

            user.IsEmailVerified = true;
            await _repo.EditItemAsync(user.UserId, user);
            return true;
        }
        public async Task<AuthResponseDto> ForgotPasswordAsync(string email)
        {
            var user=await _repo.FirstOrderAsync(u=>u.Email == email);
            if (user == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.EmailNotFound, Language.English)
                };
            var code = _security.GenerateOtpCode();

            OtpCode otpCode = new OtpCode()
            {
                Code = code,
                UserId = user.UserId,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                User = user,
            };
            await _repocode.AddItemAsync(otpCode);
            await _email.SendEmailAsync(email, _message.GetMessage(MessageKeys.ResetPassword,Language.English), _message.GetMessage(MessageKeys.SendCode,Language.English,code));

            return new AuthResponseDto { IsSuccess = true, Message = _message.GetMessage(MessageKeys.CheckEmail, Language.English) };
        }

        public async Task<UserResponDto> GetUserByIDAsync(int id)
        {
            var user=await _repo.GetItemAsync(id);
            if (user == null)
            {
                return new UserResponDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound, Language.English),
                };
            }
            return new UserResponDto
            {
                IsSuccess = true,
                Email = user.Email,
                FullName = user.FullName,
                ImagePath = user.ImagePath,
                Message = ""
            };
        }

        public async Task<bool> IsEmailVerified(string email)
        {
            var userEntity = await _repo.FirstOrderAsync(u => u.Email == email);
            if(userEntity == null) return false;

            return userEntity.IsEmailVerified;
        }
        
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _repo.FirstOrderAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.EmailNotFound, Language.English)
                };

            if (!await IsEmailVerified(user.Email))
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.IsEmailVerified, Language.English)
                };

            var passValid = _security.VerifyPassword(user.PasswordHash, loginDto.Password, user.PasswordSalt);
            if (!passValid)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.InvalidPassword, Language.English)
                };

            var token = await _auth.GenerateTokenAsync(user);
            var refreshToken = await _auth.GenerateRefreshTokenAsync();

            var authorizationToken = new AuthorizationToken
            {
                UserId = user.UserId,
                AccessToken = token,
                RefreshToken = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(5),
                IsRevoked = false,
            };

            await _repoAutho.AddItemAsync(authorizationToken);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login Success",
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto> LogoutAsync(int id)
        {
            var Authuser=await _repoAutho.GetItemAsync(id);
            if (Authuser == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound,Language.English)
                };
            Authuser.IsRevoked = true;
            await _repoAutho.EditItemAsync(Authuser.AuthorizationId,Authuser);
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message="Logout user"
            };

        }

        public async Task<string> RegisterUserAsync(RegisterDto registerDto,string pathfile)
        {
            var userExit=await _repo.FirstOrderAsync(u => u.Email == registerDto.Email);
            if (userExit != null)
            {
                mes = _message.GetMessage(MessageKeys.FoundEmail, Language.English);
                return mes;
            }
                
            string salt;
            var hashpassword = _security.HashPassword(registerDto.Password,out salt);

            User user = new User()
            {
                FullName = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = hashpassword,
                PasswordSalt = salt,
                IsEmailVerified = false,
                Role = registerDto.Role,
                ImagePath = pathfile
            };
            await _repo.AddItemAsync(user);

            await SandVerifiedTokenToEmail(user);

            return _message.GetMessage(MessageKeys.RegisterSuccess,Language.English);
        }

        public async Task SandVerifiedTokenToEmail(User user)
        {
            var token = Guid.NewGuid().ToString();
            var tokenEntity = new EmailVerificationToken
            {
                UserId = user.UserId,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
            };
            await _repoTokn.AddItemAsync(tokenEntity);

            var verificationLink = $"https://localhost:7216/api/User/VerifyEmail?token={token}";

            var body = $@"
                <h2>Welcome {user.FullName}</h2>
                <p>Please click the link below to verify your email:</p>
                <a href='{verificationLink}'>Verify Email</a>
            ";

            await _email.SendEmailAsync(user.Email, "Email Verification", body);
        }

    }
}
