using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PaymentService.Base;
using PaymentService.Common;
using PaymentService.Models;
using PaymentService.ServicePayment.MoMo;
using PaymentService.ServicePayment.VnPay;

namespace PaymentService.Fearture.Payments.Command
{
    public class ProcessMomoPaymentReturn : MomoOneTimePaymentResultRequest, IRequest<BaseResultWithData<(PaymentReturnDTO, string)>>
    {
        public class ProcessMomoPaymentReturnHandler : IRequestHandler<ProcessMomoPaymentReturn, BaseResultWithData<(PaymentReturnDTO, string)>>
        {
            private readonly PaymentContext _context;

            private readonly MomoConfig momoConfig;

            public ProcessMomoPaymentReturnHandler(IOptions<MomoConfig> momoConfigOptions, PaymentContext context)
            {

                this.momoConfig = momoConfigOptions.Value;
                _context = context;
            }

            public async Task<BaseResultWithData<(PaymentReturnDTO, string)>> Handle(ProcessMomoPaymentReturn request, CancellationToken cancellationToken)
            {
                string returnUrl = string.Empty;
                var result = new BaseResultWithData<(PaymentReturnDTO, string)>();

                try
                {
                    var resultData = new PaymentReturnDTO();
                    var isValidSignature = request.IsValidSignature(momoConfig.AccessKey, momoConfig.SecretKey);

                    if (isValidSignature)
                    {
                        var payment = await _context.PaymentTransactions.FirstOrDefaultAsync(x => x.Id.Equals(request.orderId));

                        if (payment != null)
                        {
                            //var merchant = await _context.Merchants.FirstOrDefaultAsync(x => x.Id.Equals(payment.MerchantId));
                            //returnUrl = merchant?.MerchantReturnUrl ?? string.Empty;

                            if (request.resultCode == 0)
                            {

                                resultData.PaymentStatus = "0000";
                                resultData.PaymentId = payment.PaymentId;
                                resultData.PaidAmount = payment.TransAmount;
                                resultData.UserCreateCourseId=payment.UserCreateCourseId;
                                resultData.CourseId=payment.CourseId;
    
                                resultData.Signature = Guid.NewGuid().ToString();
                            }
                            else
                            {
                                resultData.PaymentStatus = "10";
                                resultData.PaymentMessage = "Payment process failed";
                                _context.PaymentTransactions.Remove(payment);
                                _context.SaveChanges(); 
                                
                            }

                            result.Success = true;
                            result.Message = MessageContants.OK;
                            result.Data = (resultData, returnUrl);
                        }
                        else
                        {
                            resultData.PaymentStatus = "11";
                            resultData.PaymentMessage = "Can't find payment at payment service";
                        }
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
    
}
