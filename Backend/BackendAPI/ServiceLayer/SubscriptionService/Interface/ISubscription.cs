using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.Dtos.SubscriptionDtos;

namespace ServiceLayer.SubscriptionService.Interface
{
    public interface ISubscription
    {
        // Role => FinancialAccounts
        Task<SubsResponseDto> CreateSubscriptionAsync(CreatSubscriptionDTo creatSubscriptionDTo);
        Task<SubsResponseDto> EditSubscriptionAsync(CreatSubscriptionDTo creatSubscriptionDTo,int planId);
        Task<SubsResponseDto> DeleteSubscriptionAsync(int PlanId);
        Task<SubsResponseDto> GetSubscriptionByIdAsync(string SubName);
        Task<SubsResponseDto> GetAllSubscriptionsAsync();
        // Role => User
        Task<SubsResponseDto> GetAllSubscriptionsIsActiveAsync();

        //Task<SubsResponseDto> SubscribeToPlanAsync(int planId);
        //Task<SubsResponseDto> GetUserActiveSubscriptionsAsync(int userId);
        //Task<SubsResponseDto> CancelSubscriptionAsync(int subscriptionId);
        //Task<bool> HasActiveSubscriptionAsync(int userId);
        //Task<SubsResponseDto> RenewSubscriptionAsync(int subscriptionId);
    }

}
