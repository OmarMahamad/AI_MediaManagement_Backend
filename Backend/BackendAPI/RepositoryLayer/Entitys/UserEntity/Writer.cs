using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.ContantEntity;
using RepositoryLayer.Entitys.SubscriptionEntity;

namespace RepositoryLayer.Entitys.UserEntity
{
    public class Writer:User
    {
        [ForeignKey(nameof(ContentId))]
        public int? ContentId { get; set; }
        public List<Content>? Contents { get; set; }

        public int? PaymentTransactionId { get; set; }

        [ForeignKey(nameof(PaymentTransactionId))]
        public PaymentTransaction? PaymentTransaction { get; set; }
    }
}
