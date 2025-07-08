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

        private readonly IRepository<User> _repo;
        private readonly IRepository<AuthorizationToken> _repoAutho;
        private readonly MessageService _message;

        public UserService(IRepository<User> repo, IRepository<AuthorizationToken> repoAutho, MessageService message)
        {
            _repoAutho = repoAutho;
            _repo = repo;
            _message = message;
        }


        public async Task DeleteUserAsync(int id) => await _repo.DeleteItemAsync(id);

        public async Task<AuthResponseDto> EditUserAsync(int id, EditUserDto editUserDto)
        {
            var user = await _repo.GetItemAsync(id);
            if (user == null)
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound, Language.English)
                };
            user.FullName = editUserDto.name;
            await _repo.EditItemAsync(user.UserId, user);
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "The modification process was successful."
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
            user.ImagePath = pathfile;
            await _repo.EditItemAsync(id, user);
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "The modification process was successful."
            };
        }

        public async Task<UserResponDto> GetCurantUserByIDAsync(int id)
        {
            var user = await _repo.GetItemAsync(id);
            if (user == null)
            {
                return new UserResponDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound, Language.English),
                };
            }
            var authoentity = await _repoAutho.FirstOrderAsync(a => !a.IsRevoked);
            if (authoentity == null)
            {
                return new UserResponDto
                {
                    IsSuccess = false,
                    Message = "This Token is Revoked"
                };
            }
            if (authoentity.IsRevoked)
                return new UserResponDto
                {
                    IsSuccess = false,
                    Message = "This Token is Revoked"
                };

            return new UserResponDto
            {
                IsSuccess = true,
                Email = user.Email,
                FullName = user.FullName,
                ImagePath = user.ImagePath,
                Message = ""
            };
        }

        public async Task<UserResponDto> GetUserByIDAsync(int id)
        {
            var userEntity = await _repo.GetItemAsync(id);
            if (userEntity == null)
            {
                return new UserResponDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.UserNotFound, Language.English)
                };
            }
            return new UserResponDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English),
                Email = userEntity.Email,
                FullName = userEntity.FullName,
                ImagePath = userEntity.ImagePath,
            };
        }
    }
}
