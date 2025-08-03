using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.SubscriptionEntity;

namespace ServiceLayer.Dtos.SubscriptionDtos
{
    public class SubsResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Subscription? Subscription { get; set; }
        public List<Subscription>? Subscriptions { get; set; }
        public List<PaymentTransaction>? PaymentTransactions { get; set; }

    }
}
