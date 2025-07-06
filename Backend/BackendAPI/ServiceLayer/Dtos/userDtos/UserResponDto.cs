using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.userDtos
{
    public class UserResponDto
    {

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string ImagePath { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
