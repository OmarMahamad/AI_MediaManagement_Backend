using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.PaymentMethodDto
{
    public class PaymentEditRequestDto
    {
        [Required]
        public string op { get; set; }

    }
}
