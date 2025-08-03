using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentLayer.PaypalService.Model.RequestsDto;
using PaymentLayer.PaypalService.Model.ResponeDto;


namespace PaymentLayer.PaypalService.Interface
{
    public interface IPaypal
    {
        // Token
        void SetToken(string token);
        Task<AuthorizationResponseData> GetAuthorizationRequestAsync();
        

        // Product
        Task<CreatePlanResponse> CreatePlanAsync(CreatePlanRequest request);
        Task<CreateProductResponse> CreateProductAsync(CreatProductRequsetDto requset);
        Task<bool> EditProduct(string productId,EditProductRequest request);


        // Subscription
        Task<CreateSubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request);
        Task<CreateSubscriptionResponse> GetSubscriptionAsync(string subscriptionId);
        Task<bool>ActiveSubscriptionAsync(string subscriptionId);
        Task<bool>CancelSubscriptionAsync(string subscriptionId);
        Task<bool>SuspendSubscriptionAsync(string subscriptionId);





    }
}
