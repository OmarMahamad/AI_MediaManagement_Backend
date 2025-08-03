using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentLayer.PaypalService.Interface;
using RepositoryLayer.Entitys.UserEntity;
using ServiceLayer.Dtos.PaymentMethodDto;
using ServiceLayer.PaymentTransactionService.Interface;
using ServiceLayer.PaymentTransactionService.Service;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Writer")]
    public class PaymentsController : ControllerBase
    {
        
        private readonly IPayment _payment;
        //private readonly IUsre

        public PaymentsController(IPayment payment)
        {
            _payment= payment;
        }

        [HttpPost("CreatePlan/{ProductId}")]
        public async Task<IActionResult> Create_plan(string ProductId)
        {
            var respone = await _payment.PreparePlanAsync(ProductId);
            return Ok(respone.id);
        }

        [HttpPost("CreatePayment/{PlaneId}")]
        public async Task<IActionResult> CreatePayment(string PlaneId,[FromBody]int SubscriptionID)
        {
            var UserClimeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(UserClimeId, out int userId))
                return NotFound();

            var respone = await _payment.PrepareSubscrASync(PlaneId, userId, SubscriptionID);
            return Ok(respone.href);
        }

        [HttpGet("{SubscriptionPaypalId}")]
        public async Task<IActionResult>GetPayment(string SubscriptionPaypalId)
        {
            if (string.IsNullOrEmpty(SubscriptionPaypalId))
            {
                return BadRequest();
            }
            var result = await _payment.CheakPaymentProsessingAsync(SubscriptionPaypalId);
            if(!result)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPatch("{SubscriptionPaypalId}")]
        public async Task<IActionResult> Edit( string SubscriptionPaypalId,PaymentEditRequestDto request)
        {
            if (!string.IsNullOrEmpty(SubscriptionPaypalId))
                return BadRequest("invalid Request");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respone = await _payment.EditPaymentAsync(SubscriptionPaypalId, request);
            if (!respone)
                return BadRequest();

            return Ok(request);

        }


    }
}
