using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentLayer.PaypalService.Model.ResponeDto
{
    public class CreateProductResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime create_time { get; set; }
        public List<Link> links { get; set; }
    }
}
