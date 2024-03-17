using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PaymentService.Base;
using PaymentService.Common;
using PaymentService.Interface;
using PaymentService.Models;
using PaymentService.ServicePayment.MoMo;
using PaymentService.ServicePayment.VnPay;
using System;

namespace PaymentService.Fearture.Payments.Command
{
    public class ProcessMomoPaymentIpn : MomoOneTimePaymentResultRequest,
        IRequest<BaseResult>
    {
    }
    public class ProcessMomoPaymentIpnHandler : IRequestHandler<ProcessMomoPaymentIpn, BaseResult>
    {
      
        private readonly ICurrentUserService currentUserService;
        private readonly MomoConfig momoConfig;
        private readonly PaymentContext _context;

        public ProcessMomoPaymentIpnHandler(
            ICurrentUserService currentUserService, PaymentContext context,
            IOptions<MomoConfig> momoConfigOptions)
        {
           
            this.currentUserService = currentUserService;
            this.momoConfig = momoConfigOptions.Value;
            _context= context;
        }

        public Task<BaseResult> Handle(ProcessMomoPaymentIpn request, CancellationToken cancellationToken)
        {
            var result = new BaseResult();

            try
            {
                var isValidSignature = request.IsValidSignature(momoConfig.AccessKey, momoConfig.SecretKey);

                if (isValidSignature)
                {
                    /// Get payment request

                    var payment = _context.Payments.FirstOrDefault(x=>x.PaymentId.Equals(request.orderId));

                    if (payment != null)
                    {
                        if (payment.RequriedAmount == request.amount)
                        {
                            if (payment.PaymentStatus != "0")
                            {
                                string message = "";
                                string status = "";

                                if (request.resultCode == 0)
                                {
                                    status = "0";
                                    message = "Tran success";
                                }
                                else
                                {
                                    status = "-1";
                                    message = "Tran error";
                                }

                                /// Update database
                                //paramters = new SqlParameter[]
                                //{
                                //    new SqlParameter("@Id", DateTime.Now.Ticks.ToString()),
                                //    new SqlParameter("@TranMessage", message),
                                //    new SqlParameter("@TranPayload", JsonConvert.SerializeObject(request)),
                                //    new SqlParameter("@TranStatus", status),
                                //    new SqlParameter("@TranAmount", request.amount),
                                //    new SqlParameter("@TranDate", DateTime.Now),
                                //    new SqlParameter("@PaymentId", request.orderId),
                                //    new SqlParameter("@InsertUser", currentUserService.UserId ?? string.Empty),
                                //};
                                //(var affectedRows, sqlError) = sqlService.ExecuteNonQuery(connectionString,
                                //    PaymentTransactionContants.InsertSprocName, paramters);
                                var paytransaction = new PaymentTransaction
                                {
                                    Id=DateTime.Now.Ticks.ToString(),
                                    TranMessage=message,
                                    TranStatus=status,
                                    TransAmount=request.amount,
                                    TranDate=DateTime.Now,
                                    PaymentId=request.orderId
                                };
                                _context.PaymentTransactions.Add(paytransaction);
                                var affectedRows = _context.SaveChanges();

                             
                                if (affectedRows >= 1)
                                {
                                    result.Set(true, "Confirm success");
                                }
                                else
                                {
                                    result.Set(false, "Input required data");
                                }
                            }
                            else
                            {
                                result.Set(false, "Payment already confirmed");
                            }
                        }
                        else
                        {
                            result.Set(false, "Invalid amount");
                        }
                    }
                    else
                    {
                        result.Set(false, "Payment not found");
                    }
                }
                else
                {
                    result.Set(false, "Invalid signature");
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

            return Task.FromResult(result);
        }
    }
}
