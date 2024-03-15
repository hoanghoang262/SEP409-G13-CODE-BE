using MediatR;
using Microsoft.Extensions.Options;
using PaymentService.Common;
using PaymentService.Interface;
using PaymentService.Models;
using PaymentService.ServicePayment.VnPay;

namespace PaymentService.Fearture.Payments.Command
{
    public class CreatePayment : IRequest<PaymentDTO>
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = string.Empty;
        public string PaymentRefId { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
        public string? PaymentLanguage { get; set; } = string.Empty;
        public string? MerchantId { get; set; } = string.Empty;
        public string? PaymentDestinationId { get; set; } = string.Empty;
       

        public class CreatePaymentHandler : IRequestHandler<CreatePayment, PaymentDTO>
        {
            private readonly PaymentContext _context;
            private readonly VnpayConfig vnpayConfig;
            private readonly ICurrentUserService currentUserService;
            public CreatePaymentHandler(PaymentContext context, IOptions<VnpayConfig> vnpayConfigOptions, ICurrentUserService _currentUserService)
            {
                _context = context;
                vnpayConfig = vnpayConfigOptions.Value;
                currentUserService = _currentUserService;
            }
            public async Task<PaymentDTO> Handle(CreatePayment request, CancellationToken cancellationToken)
            {

                var outputIdParam = Guid.NewGuid();
                var payment = new Payment
                {
                    PaymentId = outputIdParam.ToString(),
                    PaymentCurrency = request.PaymentCurrency,
                    PaymentRefId = request.PaymentRefId,
                    RequriedAmount = request.RequiredAmount,
                    PaymentDate = request.PaymentDate,
                    ExpireDate = request.ExpireDate,
                    PaymentLanguage = request.PaymentLanguage,
                    MerchantId = request.MerchantId,
                    PaymentDestinationId = request.PaymentDestinationId,
                 
                };
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                var paymentUrl = string.Empty;

                switch (request.PaymentDestinationId)
                {
                    case "VNPAY":
                        var vnpayPayRequest = new VnpayPayRequest(vnpayConfig.Version,
                            vnpayConfig.TmnCode, DateTime.Now, currentUserService.IpAddress ?? string.Empty, request.RequiredAmount ?? 0, request.PaymentCurrency ?? string.Empty,
                            "other", request.PaymentContent ?? string.Empty, vnpayConfig.ReturnUrl, outputIdParam.ToString() ?? string.Empty);
                        paymentUrl = vnpayPayRequest.GetLink(vnpayConfig.PaymentUrl, vnpayConfig.HashSecret);
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
