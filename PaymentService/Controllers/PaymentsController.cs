using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Base;
using PaymentService.Common;
using PaymentService.Fearture.Payments.Command;
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
    }
}
