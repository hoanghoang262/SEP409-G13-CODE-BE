//using MediatR;
//using Microsoft.Data.SqlClient;
//using PaymentService.Common;
//using PaymentService.Interface;

//namespace PaymentService.Fearture.Payments.Querry
//{
//    public class GetPayment : IRequest<PaymentDTO>
//    {
//        public string Id { get; set; } = string.Empty;
//    }
//    public class GetPaymentHandler : IRequestHandler<GetPayment, PaymentDTO>
//    {
//        private readonly ICurrentUserService currentUserService;


//        public GetPaymentHandler(ICurrentUserService currentUserService)
//        {
//            this.currentUserService = currentUserService;

//        }

//        public Task<PaymentDTO> Handle(GetPayment request, CancellationToken cancellationToken)
//        {
//            var result = new PaymentDTO();

//            try
//            {
//                string connectionString = connectionService.Datebase ?? string.Empty;
//                var paramters = new SqlParameter[]
//                {
//                    new SqlParameter("@PaymentId", request.Id),
//                };
//                (var data, string sqlError) = sqlService.FillDataTable(connectionString,
//                    PaymentConstants.SelectByIdSprocName, paramters);
//                var payment = data.AsListObject<PaymentDtos>()?.SingleOrDefault();

//                if (payment != null)
//                {
//                    result.Set(true, MessageContants.OK, payment);
//                }
//                else
//                {
//                    result.Set(false, MessageContants.NotFound);
//                }

//            }
//            catch (Exception ex)
//            {
//                result.Set(false, MessageContants.Error);
//                result.Errors.Add(new BaseError()
//                {
//                    Code = MessageContants.Exception,
//                    Message = ex.Message,
//                });
//            }

//            return Task.FromResult(result);
//        }
//    }
//}
