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
        Task DeleteUserAsync(int id);
        Task <UserResponDto> GetCurantUserByIDAsync(int id);
        Task<AuthResponseDto> EditUserAsync(int id,EditUserDto editUserDto);
        Task<AuthResponseDto> EditUserAsync(int id,string pathfile);
        Task<UserResponDto> GetUserByIDAsync(int id);

    }
}
