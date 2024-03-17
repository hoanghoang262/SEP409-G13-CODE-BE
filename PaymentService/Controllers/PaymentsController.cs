using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Base;
using PaymentService.Common;
using PaymentService.Fearture.Payments.Command;
using PaymentService.Fearture.Payments.Querry;
using PaymentService.Models;
using PaymentService.ServicePayment.MoMo;


namespace PaymentService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly PaymentContext _context;
        // private readonly VnpayConfig vnpayConfig;
        public PaymentsController(IMediator _mediator) 
        { 
            mediator = _mediator;
          
        }
        [HttpPost]
      
        public async Task<IActionResult> Create( CreatePayment request)
        {
 
            var response = await mediator.Send(request);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> MomoReturn([FromQuery] MomoOneTimePaymentResultRequest response)
        {
            string returnUrl = string.Empty;
            var returnModel = new PaymentReturnDTO();
            var processResult = await mediator.Send(response.Adapt<ProcessMomoPaymentReturn>());

            if (processResult.Success)
            {
                returnModel = processResult.Data.Item1 as PaymentReturnDTO;
                returnUrl = processResult.Data.Item2 as string;
            }


            if (returnUrl.EndsWith("/"))
                returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
            return Ok(returnModel);
        }
        [HttpGet]
        public async Task<ActionResult<List<PaymentDtos>>> GetHistoryPaymentsOfUser(int userId)
        {
            var query = new GetHistoryPaymentsOfUserQuerry { Id = userId };
            var result = await mediator.Send(query);

            if (result.Result is NoContentResult)
            {
                return NoContent();
            }
            return Ok(result.Value);
        }
    }
}
