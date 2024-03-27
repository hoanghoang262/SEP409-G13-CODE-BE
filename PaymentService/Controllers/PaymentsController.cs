using EventBus.Message.Event;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IPublishEndpoint publish;
        public PaymentsController(IMediator _mediator, IPublishEndpoint _publish, PaymentContext context)
        {
            mediator = _mediator;
            publish = _publish;
            _context = context;

        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(CreatePayment request)
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
                returnModel = processResult.Data.Item1;
                returnUrl = processResult.Data.Item2;
            }
            var outputIdParam = Guid.NewGuid();

            var payment = new Payment
            {
                PaymentId = outputIdParam.ToString(),
                PaidAmount = returnModel.PaidAmount,
                MerchantId = "MER001",
                PaymentLanguage = "vn",
                PaymentCurrency = "VND",
                UserCreateCourseId = returnModel.UserCreateCourseId,
                CourseId = returnModel.CourseId,
                RequriedAmount = returnModel.PaidAmount
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();
            var Enroll = new UserEnrollEvent
            {
                UserId = returnModel.UserCreateCourseId,
                CourseId = returnModel.CourseId,

            };
            await publish.Publish(Enroll);

            if (returnUrl.EndsWith("/"))
            {
                returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
            }

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
