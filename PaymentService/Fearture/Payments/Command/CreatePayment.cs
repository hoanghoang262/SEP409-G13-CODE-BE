using Contract.Service.Message;
using CourseService.API.GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaymentService.API.GrpcServices;
using PaymentService.Common;
using PaymentService.Interface;
using PaymentService.Models;
using PaymentService.ServicePayment.MoMo;
using PaymentService.ServicePayment.VnPay;

namespace PaymentService.Fearture.Payments.Command
{
    public class CreatePayment : IRequest<IActionResult>
    {
        public string PaymentContent { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public int UserCreateCourseId { get; set; }
        public int CourseId { get; set; }
        public int UserBuyId { get; set; }
        public class CreatePaymentHandler : IRequestHandler<CreatePayment, IActionResult>
        {
            private readonly PaymentContext _context;
            private readonly VnpayConfig vnpayConfig;
            private readonly MomoConfig momoConfig;
            private readonly ICurrentUserService currentUserService;
            private readonly GetUserInfoService service;
            private readonly GetCourseInfoService serviceCourse;
            public CreatePaymentHandler(PaymentContext context, 
                IOptions<VnpayConfig> vnpayConfigOptions, 
                IOptions<MomoConfig> _momo, 
                ICurrentUserService _currentUserService,
                GetUserInfoService _service,
                GetCourseInfoService _serviceCourse)
            {
                _context = context;
                vnpayConfig = vnpayConfigOptions.Value;
                currentUserService = _currentUserService;
                momoConfig = _momo.Value;
                service=_service;
                serviceCourse = _serviceCourse;
            }
            public async Task<IActionResult> Handle(CreatePayment request, CancellationToken cancellationToken)
            {
                var userCreate = await service.SendUserId(request.UserCreateCourseId);
                var userId = await service.SendUserId(request.UserBuyId);
                var course= await serviceCourse.SendCourseId(request.CourseId);
                if (course.Id == 0)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }

                if(userCreate.Id ==0 || userId.Id==0) 
                {
                    return new BadRequestObjectResult(Message.MSG24);
                }
                if (request.RequiredAmount != null && request.RequiredAmount < 0)
                {
                    return new BadRequestObjectResult(Message.MSG26);
                }

                var outputIdParam = Guid.NewGuid();
                var paymentUrl = string.Empty;
                var paymenttrans = new PaymentTransaction
                {
                    Id = outputIdParam.ToString(),
                    UserCreateCourseId = request.UserCreateCourseId,
                    CourseId = request.CourseId,
                    TransAmount = request.RequiredAmount,
                    BuyerId=request.UserBuyId,
                };
                _context.PaymentTransactions.Add(paymenttrans);
                await _context.SaveChangesAsync();

                switch ("MOMO")
                {
                    //case "VNPAY":
                    //    var vnpayPayRequest = new VnpayPayRequest(vnpayConfig.Version,
                    //        vnpayConfig.TmnCode, DateTime.Now, currentUserService.IpAddress ?? string.Empty, request.RequiredAmount ?? 0, request.PaymentCurrency ?? string.Empty,
                    //        "other", request.PaymentContent ?? string.Empty, vnpayConfig.ReturnUrl, outputIdParam.ToString() ?? string.Empty);
                    //    paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
                    //    break;
                    case "MOMO":
                        var momoOneTimePayRequest = new MomoOneTimePaymentRequest(momoConfig.PartnerCode,
                            outputIdParam.ToString() ?? string.Empty, (long)request.RequiredAmount!, outputIdParam.ToString() ?? string.Empty,
                            request.PaymentContent ?? string.Empty, momoConfig.ReturnUrl, momoConfig.IpnUrl, "captureWallet",
                            request.UserCreateCourseId, request.CourseId,
                            string.Empty);
                        momoOneTimePayRequest.MakeSignature(momoConfig.AccessKey, momoConfig.SecretKey);
                        (bool createMomoLinkResult, string? createMessage) = momoOneTimePayRequest.GetLink(momoConfig.PaymentUrl);
                        if (createMomoLinkResult)
                        {
                            paymentUrl = createMessage;
                        }

                        break;
                }
                var result = new PaymentDTO()
                {
                    PaymentId = outputIdParam.ToString() ?? string.Empty,
                    PaymentUrl = paymentUrl,
                };

                return new OkObjectResult(result);

            }
        }

    }

}
