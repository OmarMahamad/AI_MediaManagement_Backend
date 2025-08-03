using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos
{
    public class CreatePlanResponeDto
    {
        public string id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
