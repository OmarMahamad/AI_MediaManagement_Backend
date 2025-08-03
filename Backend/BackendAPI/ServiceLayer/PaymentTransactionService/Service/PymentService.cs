using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using PaymentLayer.PaypalService.Interface;
using RepositoryLayer.Entitys.SubscriptionEntity;
using RepositoryLayer.Interface;
using ServiceLayer.Dtos;
using ServiceLayer.Dtos.PaymentMethodDto;
using ServiceLayer.PaymentTransactionService.Interface;

namespace ServiceLayer.PaymentTransactionService.Service
{
    public class PymentService : IPayment
    {
        private readonly IRepository<Subscription> _subrepo;
        private readonly IRepository<PaymentTransaction> _transrepo;
        private readonly IPaypal _paypal;
        private readonly MessageService _message;
        public PymentService(IRepository<Subscription> subrepo, IPaypal paypal, MessageService message, IRepository<PaymentTransaction> transrepo)
        {
            _paypal = paypal;
            _subrepo = subrepo;
            _message = message;
            _transrepo = transrepo;
        }

        public async Task<bool> CheakPaymentProsessingAsync(string SubscriptionID)
        {
            var tranEntity=await _transrepo.FirstOrderAsync(t=>t.SubscriptionPaypalId==SubscriptionID);
            if (tranEntity==null)
                return false;
            var respone =await _paypal.GetSubscriptionAsync(SubscriptionID);
            if (respone.status== "ACTIVE")
            {
                tranEntity.PaymentTransactionStatus=StatusType.Approved;
                await _transrepo.EditItemAsync(tranEntity.PaymentTransactionId,tranEntity);
                return true;
            }
            return false;
        }

        public async Task<bool> EditPaymentAsync(string SubscriptionPaypalId, PaymentEditRequestDto request)
        {
            var traEntity=await _transrepo.FirstOrderAsync(t=>t.SubscriptionPaypalId == SubscriptionPaypalId);
            if (traEntity==null) return false;

            switch (request.op)
            {
                case "activate":
                    await _paypal.ActiveSubscriptionAsync(SubscriptionPaypalId);
                    traEntity.PaymentTransactionStatus= StatusType.Active;
                    await _transrepo.EditItemAsync(traEntity.PaymentTransactionId,traEntity);
                    return true;
                case "cancel":
                    await _paypal.CancelSubscriptionAsync(SubscriptionPaypalId);
                    traEntity.PaymentTransactionStatus = StatusType.Cancelled;
                    await _transrepo.EditItemAsync(traEntity.PaymentTransactionId, traEntity);
                    return true;
                case "suspend":
                    await _paypal.SuspendSubscriptionAsync(SubscriptionPaypalId);
                    traEntity.PaymentTransactionStatus = StatusType.Suspended;
                    await _transrepo.EditItemAsync(traEntity.PaymentTransactionId, traEntity);
                    break;
                default : 
                    break;
            }
            return false;
        }

        public async Task<CreatePlanResponeDto> PreparePlanAsync(string ProductId)
        {
            var subEntity=await _subrepo.FirstOrderAsync(p=> p.ProductId == ProductId);
            if (subEntity==null)
                return new CreatePlanResponeDto
                {
                    IsSuccess = false,
                    Message=_message.GetMessage(MessageKeys.SubNotFound,Language.English)
                };
            var request = new CreatePlanRequest
            {
                product_id = subEntity.ProductId,
                name = subEntity.Name,
                description = subEntity.description,
                status = subEntity.IsActive == true ? "Active" : "INACTIVE",
                billing_cycles = new List<BillingCycle>
                {
                    new BillingCycle
                    {
                       frequency =new Frequency
                       {
                           interval_unit="DAY",
                           interval_count=7
                       },
                       tenure_type="TRIAL",
                       sequence=1,
                       total_cycles=1,
                       pricing_scheme =new PricingScheme
                       {
                           fixed_price=new FixedPrice
                           {
                               currency_code="USD",
                               value="0"
                           }
                       }
                    },
                    new BillingCycle
                    {
                        frequency = new Frequency
                        {
                            interval_unit = "MONTH",
                            interval_count = 1
                        },
                        tenure_type = "REGULAR",
                        sequence = 2,
                        total_cycles = 0, // 0 = مستمرة
                        pricing_scheme = new PricingScheme
                        {
                            fixed_price = new FixedPrice
                            {
                                currency_code = "USD",
                                value = subEntity.Amount.ToString() // أو قيمة اشتراكك الفعلية
                            }
                        }
                    }

                },
                
                payment_preferences =new PaymentPreferences
                {
                    auto_bill_outstanding=true,
                    setup_fee=new SetupFee
                    {
                        currency_code="USD",
                        value="10"
                    },
                    payment_failure_threshold= 3,
                    setup_fee_failure_action= "CONTINUE"
                },
                taxes=new Taxes
                {
                    inclusive=false,
                    percentage="10"
                }
                
            };
            var PlaneRespone = await _paypal.CreatePlanAsync(request);
            return new CreatePlanResponeDto
            {
                IsSuccess = true,
                id = PlaneRespone.id,
                Message = _message.GetMessage(MessageKeys.Success, Language.English)
            };
        }

        public async Task<Link> PrepareSubscrASync(string PlanId, int userid,int SubscriptionID)
        {
            var request = new CreateSubscriptionRequest
            {
                plan_id = PlanId,
                application_context = new ApplicationContext
                {
                    return_url = "https://example.com/return",
                    cancel_url = "https://example.com/cancel"
                }
            };
            var SubEntity = await _subrepo.GetItemAsync(SubscriptionID);
            if (SubEntity == null)
                return null;
            var result=await _paypal.CreateSubscriptionAsync(request);
            var transection = new PaymentTransaction
            {
                PaymentMethod=PaymentMethodType.Paypal,
                PaymentTransactionStatus= StatusType.ApprovalPending,
                UserId= userid,
                SubscriptionPaypalId=result.id,
                SubscriptionId=SubEntity.SubscriptionId
            };
            await _transrepo.AddItemAsync(transection);

            var approveUrl= result.links.FirstOrDefault(x => x.rel == "approve");
            return approveUrl;
        }

    }
}
