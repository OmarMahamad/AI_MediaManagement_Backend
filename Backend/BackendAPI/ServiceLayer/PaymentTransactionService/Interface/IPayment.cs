using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Subscriptions;
using RepositoryLayer.Entitys.SubscriptionEntity;
using ServiceLayer.Dtos;
using ServiceLayer.Dtos.PaymentMethodDto;


namespace ServiceLayer.PaymentTransactionService.Interface
{
    public interface IPayment
    {
        Task<CreatePlanResponeDto> PreparePlanAsync(string ProductId);
        Task<Link> PrepareSubscrASync(string PlanId, int userid, int SubscriptionID);
        Task<bool> CheakPaymentProsessingAsync(string SubscriptionID);
        Task<bool> EditPaymentAsync(string SubscriptionID,PaymentEditRequestDto request);

    }
}
