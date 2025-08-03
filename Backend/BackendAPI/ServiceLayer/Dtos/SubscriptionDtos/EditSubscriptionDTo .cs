using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entitys.SubscriptionEntity;

namespace ServiceLayer.Dtos.SubscriptionDtos
{
    public class EditSubscriptionDTo
    {
        
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [ EnumDataType(typeof(VideoQuality))]
        public VideoQuality VideoQuality { get; set; }

        //[Required]
        public string AcousticCharacteristics { get; set; }

        //[Required]
        public int NumberOfLetters { get; set; }

        [Required]
        [Precision(10, 2)]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public CurrencyType Currency { get; set; }

        [Required, EnumDataType(typeof(SubscriptionPeriod))]
        public SubscriptionPeriod TimeOfSubscription { get; set; }
    }
}
