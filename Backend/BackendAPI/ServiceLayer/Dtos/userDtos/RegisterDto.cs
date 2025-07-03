using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.UserEntity;

namespace ServiceLayer.Dtos.userDtos
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,PasswordPropertyText]
        public string Password { get; set; }

        public RoleType Role { get; set; }
    }
}
