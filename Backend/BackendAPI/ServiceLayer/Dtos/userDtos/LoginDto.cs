using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.userDtos
{
    public class LoginDto
    {
        [Required,EmailAddress]
        public string Email { get; set; }

        [Required,PasswordPropertyText]
        public string Password { get; set; }
    }
}
