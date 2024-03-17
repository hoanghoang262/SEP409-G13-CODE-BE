using MediatR;
using Microsoft.Extensions.Options;
using PaymentService.Common;
using PaymentService.Interface;
using PaymentService.Models;
using PaymentService.ServicePayment.MoMo;
using PaymentService.ServicePayment.VnPay;

namespace PaymentService.Fearture.Payments.Command
{
    public class CreatePayment : IRequest<PaymentDTO>
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = "VND";
        public string PaymentRefId { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
        public int? UserCreateCourseId { get; set; }
        public int? CourseId { get; set; }
        public string? PaymentDestinationId { get; set; } ="MOMO";
       

        public class CreatePaymentHandler : IRequestHandler<CreatePayment, PaymentDTO>
        {
            private readonly PaymentContext _context;
            private readonly VnpayConfig vnpayConfig;
            private readonly MomoConfig momoConfig;
            private readonly ICurrentUserService currentUserService;
            public CreatePaymentHandler(PaymentContext context, IOptions<VnpayConfig> vnpayConfigOptions, IOptions<MomoConfig> _momo, ICurrentUserService _currentUserService)
            {
                _context = context;
                vnpayConfig = vnpayConfigOptions.Value;
                currentUserService = _currentUserService;
                momoConfig = _momo.Value;
            }
            public async Task<PaymentDTO> Handle(CreatePayment request, CancellationToken cancellationToken)
            {

                var outputIdParam = Guid.NewGuid();
                
                var paymentUrl = string.Empty;
                var payment = new Payment
                {
                    PaymentId = outputIdParam.ToString(),
                    PaidAmount=request.RequiredAmount,
                    MerchantId="MER001",
                    PaymentLanguage="vn",
                    PaymentCurrency="VND",
                    UserCreateCourseId=request.UserCreateCourseId,
                    CourseId=request.CourseId,
                    RequriedAmount=request.RequiredAmount
                };
                 _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                switch (request.PaymentDestinationId)
                {
                    case "VNPAY":
                        var vnpayPayRequest = new VnpayPayRequest(vnpayConfig.Version,
                            vnpayConfig.TmnCode, DateTime.Now, currentUserService.IpAddress ?? string.Empty, request.RequiredAmount ?? 0, request.PaymentCurrency ?? string.Empty,
                            "other", request.PaymentContent ?? string.Empty, vnpayConfig.ReturnUrl, outputIdParam.ToString() ?? string.Empty);
                        paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
                        break;
                    case "MOMO":
                        var momoOneTimePayRequest = new MomoOneTimePaymentRequest(momoConfig.PartnerCode,
                            outputIdParam.ToString() ?? string.Empty, (long)request.RequiredAmount!, outputIdParam.ToString() ?? string.Empty,
                            request.PaymentContent ?? string.Empty, momoConfig.ReturnUrl, momoConfig.IpnUrl, "captureWallet",
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

                return result;

            }
        }

    }
   
}
