using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Entitys.SubscriptionEntity
{
    [Index(nameof(Name))]
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }
        [Required]
        public string ProductId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string description { get; set; }

        [EnumDataType(typeof(VideoQuality))]
        public VideoQuality? VideoQuality { get; set; }

        //[Required]
        public string? AcousticCharacteristics { get; set; }

        //[Required]
        public int? NumberOfLetters { get; set; }

        [Required]
        [Precision(10, 2)]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public CurrencyType Currency { get; set; }

        [Required,EnumDataType(typeof(SubscriptionPeriod))]
        public SubscriptionPeriod TimeOfSubscription { get; set; } 

        public bool IsActive { get; set; }

        //public Product product { get; set; }


    }
}
