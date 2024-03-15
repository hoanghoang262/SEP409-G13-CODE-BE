using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentService.Fearture.Payments.Command;
using PaymentService.ServicePayment.VnPay;
using System.Net;

namespace PaymentService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator mediator;
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
    }
}
