using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entitys.UserEntity;
using ServiceLayer.Dtos.SubscriptionDtos;
using ServiceLayer.SubscriptionService.Interface;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Financial_Accounts")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscription _subscription;
        public SubscriptionController(ISubscription subscription)
        {
            _subscription = subscription;
        }

        [HttpPost("CreateSubscription")]
        public async Task<IActionResult> CreateSubscription(CreatSubscriptionDTo request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var SubRespone = await _subscription.CreateSubscriptionAsync(request);
            if (!SubRespone.IsSuccess)
                return BadRequest(SubRespone.Message);
            return Ok(SubRespone);
        }
        [HttpGet("GetSubscriptionByName/{name}")]
        public async Task<IActionResult> GetSubscriptionByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
                return BadRequest("Invalid Input");
            var respone=await _subscription.GetSubscriptionByIdAsync(name);
            if (!respone.IsSuccess)
                return BadRequest(respone.Message);
            return Ok(respone);
        }


        [HttpGet("GetAllSubscription")]
        public async Task<IActionResult> GetAllSubscription()
        {
            var sub = await _subscription.GetAllSubscriptionsAsync();
            return Ok(sub);
        }

        [HttpGet("GetAllSubscriptionActive")]
        public async Task<IActionResult> GetAllSubscriptionActive()
        {
            var sub = await _subscription.GetAllSubscriptionsIsActiveAsync();
            return Ok(sub);
        }

        [HttpPatch("{SubscriptionID}")]
        public async Task<IActionResult> DeleteSubscription(int SubscriptionID, [FromBody] CreatSubscriptionDTo requst)
        {
            if (SubscriptionID < 0)
                return BadRequest("Invalid ID.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var respone = await _subscription.EditSubscriptionAsync(requst, SubscriptionID);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.Message);
            }
            return Ok(respone);
        }

        [HttpDelete("{SubscriptionID}")]
        public async Task<IActionResult> DeleteSubscription(int SubscriptionID)
        {
            if (SubscriptionID <= 0)
                return BadRequest("Invalid ID.");

            var result = await _subscription.DeleteSubscriptionAsync(SubscriptionID);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

    }
}
