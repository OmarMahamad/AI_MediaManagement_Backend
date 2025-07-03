using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entitys.ContantEntity;

namespace RepositoryLayer.Entitys.UserEntity
{
    [Index(nameof(Email))]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public string ImagePath { get; set; }

        public bool IsEmailVerified { get; set; } = false;

        [EnumDataType(typeof(RoleType))]
        public RoleType Role { get; set; }
    }


}
