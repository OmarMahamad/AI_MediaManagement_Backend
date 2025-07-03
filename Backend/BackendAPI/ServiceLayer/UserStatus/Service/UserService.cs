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
        private readonly IRepository<AuthorizationToken> _repoAutho;
        private readonly MessageService _message;
        public UserService(Isecurity security, IAuthorization auth, IRepository<AuthorizationToken> repoAutho , IEmail email, IRepository<User> repo, MessageService message)
        {
            _auth = auth;
            _repoAutho = repoAutho;
            _security = security;
            _email = email;
            _repo = repo;
            _message = message;
        }

        public Task DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPasswordAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _repo.FirstOrderAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return _message.GetMessage(MessageKeys.EmailNotFound, Language.English);

            var passValid = _security.VerifyPassword(user.PasswordHash, loginDto.Password, user.PasswordSalt);
            if (!passValid)
                return _message.GetMessage(MessageKeys.InvalidPassword, Language.English);

            var token = await _auth.GenerateTokenAsync(user);
            var refreshToken = await _auth.GenerateRefreshTokenAsync();

            AuthorizationToken authorizationToken = new AuthorizationToken
            {
                UserId = user.UserId,
                AccessToken = token,
                RefreshToken = refreshToken,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(5),
                IsRevoked = false,
                User = user
            };

            await _repoAutho.AddItemAsync(authorizationToken);
            return token;
        }


        //public Task LogoutAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<string> RegisterUserAsync(RegisterDto registerDto,string pathfile)
        {
            var userExit=await _repo.FirstOrderAsync(u => u.Email == registerDto.Email);
            if(userExit!=null)
                return _message.GetMessage(MessageKeys.FoundEmail, Language.English);
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
            return _message.GetMessage(MessageKeys.RegisterSuccess,Language.English);
        }
    }
}
