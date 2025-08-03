using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PaymentLayer.PaypalService.Interface;
using PaymentLayer.PaypalService.Model;
using PaymentLayer.PaypalService.Model.RequestsDto;
using PaymentLayer.PaypalService.Model.ResponeDto;
using RepositoryLayer.Entitys.SubscriptionEntity;
using RepositoryLayer.Interface;
using ServiceLayer.Dtos.SubscriptionDtos;
using ServiceLayer.SubscriptionService.Interface;

namespace ServiceLayer.SubscriptionService.Service
{
    public class SubscriptionService : ISubscription
    {
        private readonly IRepository<Subscription> _repository;
        private readonly IRepository<PaymentTransaction> _pymentrepo;
        private readonly MessageService _message;
        private readonly IPaypal _paypal;

        public SubscriptionService(IPaypal paypal,IRepository<Subscription> repository, MessageService message, IRepository<PaymentTransaction> pymentrepo)
        {
            _paypal = paypal;
            _message = message;
            _repository = repository;
            _pymentrepo = pymentrepo;
        }

        public async Task<SubsResponseDto> CreateSubscriptionAsync(CreatSubscriptionDTo creatSubscriptionDTo)
        {
            var subentity=await _repository.FirstOrderAsync(s=>s.Name==creatSubscriptionDTo.Name);
            if (subentity != null)
            {
                return new SubsResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.FoundName, Language.English)
                };
            }

            var ProductRequest = new CreatProductRequsetDto
            {
                name = creatSubscriptionDTo.Name,
                description = "Subscription",
                
            };

            var responeProduct=await _paypal.CreateProductAsync(ProductRequest);

            var subscri = new Subscription
            {
                Name = creatSubscriptionDTo.Name,
                Amount = creatSubscriptionDTo.Amount,
                Currency = creatSubscriptionDTo.Currency,
                IsActive = creatSubscriptionDTo.IsActive,
                AcousticCharacteristics = creatSubscriptionDTo.AcousticCharacteristics,
                NumberOfLetters = creatSubscriptionDTo.NumberOfLetters,
                TimeOfSubscription = creatSubscriptionDTo.TimeOfSubscription,
                VideoQuality = creatSubscriptionDTo.VideoQuality,
                ProductId = responeProduct.id,
                description=responeProduct.description
            };
            await _repository.AddItemAsync(subscri);
            return new SubsResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English)
            };
        }

        public async Task<SubsResponseDto> DeleteSubscriptionAsync(int PlanId)
        {
            var subentity=await _repository.GetItemAsync(PlanId);
            var request = new EditProductRequest
            {
                op = "remove",
            };

            if (!await _paypal.EditProduct(subentity.ProductId, request))
            {
                return new SubsResponseDto
                {
                    IsSuccess = false,
                };
            }
            await _repository.DeleteItemAsync(PlanId);
            return new SubsResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English)
            };
        }

        public async Task<SubsResponseDto> EditSubscriptionAsync(CreatSubscriptionDTo creatSubscriptionDTo, int planId)
        {
            var  subsEntity=await _repository.GetItemAsync(planId);
            if (subsEntity == null)
            {
                return new SubsResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.SubNotFound, Language.English)
                };
            }
            subsEntity.Name= creatSubscriptionDTo.Name;
            subsEntity.NumberOfLetters= creatSubscriptionDTo.NumberOfLetters;
            subsEntity.TimeOfSubscription= creatSubscriptionDTo.TimeOfSubscription;
            subsEntity.AcousticCharacteristics= creatSubscriptionDTo.AcousticCharacteristics;
            subsEntity.Currency= creatSubscriptionDTo.Currency;
            subsEntity.Amount= creatSubscriptionDTo.Amount;
            subsEntity.VideoQuality= creatSubscriptionDTo.VideoQuality;
            subsEntity.IsActive = creatSubscriptionDTo.IsActive;
            await _repository.EditItemAsync(planId, subsEntity);
            return new SubsResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English)
            };
        }

        public async Task<SubsResponseDto> GetAllSubscriptionsAsync()
        {
            var subEntitys = await _repository.FilterByWhereAsync();
            return new SubsResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English),
                Subscriptions = subEntitys.ToList(),
            };
        }

        public async Task<SubsResponseDto> GetAllSubscriptionsIsActiveAsync()
        {
            var ActiveItem = await _repository.FilterByWhereAsync(s=>s.IsActive);
            return new SubsResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English),
                Subscriptions = ActiveItem.ToList(),
            };
        }

        public async Task<SubsResponseDto> GetSubscriptionByIdAsync(string SubName)
        {
           var subEntity=await _repository.FirstOrderAsync(s=>s.Name==SubName);
            if (subEntity == null)
                return new SubsResponseDto
                {
                    IsSuccess = false,
                    Message = _message.GetMessage(MessageKeys.SubNotFound, Language.English)
                };
            return new SubsResponseDto
            {
                IsSuccess = true,
                Message = _message.GetMessage(MessageKeys.Success, Language.English),
                Subscription = subEntity
            };
        }

    }
}
