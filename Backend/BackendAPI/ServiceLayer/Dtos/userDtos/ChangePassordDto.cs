using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.userDtos
{
    public class ChangePassordDto
    {
        [EmailAddress,Required]
        public string email { get; set; }
        [Required,PasswordPropertyText]
        public string NewPassword { get; set; }
    }
}
