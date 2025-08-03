using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entitys.UserEntity;

namespace RepositoryLayer.Entitys.SubscriptionEntity
{
    [Index(nameof(SubscriptionPaypalId), IsUnique = true)]
    public class PaymentTransaction
    {
        [Key]
        public int PaymentTransactionId { get; set; }

        [Required]
        public int SubscriptionId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string SubscriptionPaypalId { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethodType))]
        public PaymentMethodType PaymentMethod { get; set; }

        [Required]
        [EnumDataType(typeof(StatusType))]
        public StatusType PaymentTransactionStatus { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

    }
}
