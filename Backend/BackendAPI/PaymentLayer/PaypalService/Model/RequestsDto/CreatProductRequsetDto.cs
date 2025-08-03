using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PaymentLayer.PaypalService.Model.RequestsDto
{
    public class CreatProductRequsetDto
    {
        [Required]
        public string name { get; set; }
        //[Required, EnumDataType(typeof(ProuductType))]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public string type { get; set; } = "DIGITAL";
        public string description { get; set; }
        public string category { get; set; } = "SOFTWARE";
       
    }
}
