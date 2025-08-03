using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos.PaymentMethodDto
{
    public class PaymentResponeDTo
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }

    }
}
