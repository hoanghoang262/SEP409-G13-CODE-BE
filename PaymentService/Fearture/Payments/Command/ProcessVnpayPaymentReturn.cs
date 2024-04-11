using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PaymentService.Base;
using PaymentService.Common;
using PaymentService.Models;
using PaymentService.ServicePayment.VnPay;

namespace PaymentService.API.Fearture.Payments.Command
{
    public class ProcessVnpayPaymentReturn : VnpayPayResponse,
        IRequest<BaseResultWithData<(PaymentReturnDTO, string)>>
    {

    }

    public class ProcessVnpayPaymentReturnHandler
        : IRequestHandler<ProcessVnpayPaymentReturn, BaseResultWithData<(PaymentReturnDTO, string)>>
    {
        private readonly PaymentContext _context;
        private readonly VnpayConfig vnpayConfig;

        public ProcessVnpayPaymentReturnHandler(
          
            IOptions<VnpayConfig> vnpayConfigOptions,
             PaymentContext context)
        {
       
            this.vnpayConfig = vnpayConfigOptions.Value;
            _context = context;
        }
        public async Task<BaseResultWithData<(PaymentReturnDTO, string)>> Handle(
            ProcessVnpayPaymentReturn request, CancellationToken cancellationToken)
        {
            string returnUrl = string.Empty;
            var result = new BaseResultWithData<(PaymentReturnDTO, string)>();

            try
            {
                var resultData = new PaymentReturnDTO();
                var isValidSignature = request.IsValidSignature(vnpayConfig.HashSecret);

                if (isValidSignature)
                {
                    var payment = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Id.Equals(request.vnp_TxnRef));
                    if (request.vnp_ResponseCode == "00")
                    {
                        resultData.PaymentStatus = "0000";
                        resultData.PaymentId = payment.PaymentId;
                        resultData.PaidAmount = payment.TransAmount;
                        resultData.UserCreateCourseId = payment.UserCreateCourseId;
                        resultData.CourseId = payment.CourseId;
                        resultData.BuyerId = payment.BuyerId;
                        resultData.Signature = Guid.NewGuid().ToString();

                       
                    }
                    else
                    {
                        resultData.PaymentStatus = "10";
                        resultData.PaymentMessage = "Payment process failed";
                    }

                    result.Success = true;
                    result.Message = MessageContants.OK;
                    result.Data = (resultData, returnUrl);
                }
                else
                {
                    resultData.PaymentStatus = "99";
                    resultData.PaymentMessage = "Invalid signature in response";

                }


            }
            catch (Exception ex)
            {
                result.Set(false, MessageContants.Error);
                result.Errors.Add(new BaseError()
                {
                    Code = MessageContants.Exception,
                    Message = ex.Message,
                });
            }

            return await Task.FromResult(result);
        }
    }
}
