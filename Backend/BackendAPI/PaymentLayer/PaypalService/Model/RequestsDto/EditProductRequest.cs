using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PaymentLayer.PaypalService.Model.RequestsDto
{
    public class EditProductRequest
    {
        //[EnumDataType(typeof(OpType))]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public string op { get; set; }
        public string path { get; set; } = "/description";
        public string? value { get; set; }
    }
}
