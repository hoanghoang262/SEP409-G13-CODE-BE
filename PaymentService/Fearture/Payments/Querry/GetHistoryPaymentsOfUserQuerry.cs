using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PaymentService.Common;
using PaymentService.Interface;
using PaymentService.Models;

namespace PaymentService.Fearture.Payments.Querry
{
    public class GetHistoryPaymentsOfUserQuerry : IRequest<ActionResult<List<PaymentDtos>>>
    {
        public int Id { get; set; } 
    }
    public class GetPaymentHandler : IRequestHandler<GetHistoryPaymentsOfUserQuerry, ActionResult<List<PaymentDtos>>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly PaymentContext context;
        public GetPaymentHandler(ICurrentUserService currentUserService,PaymentContext _context)
        {
            this.currentUserService = currentUserService;
            context=_context;

        }

        public async Task<ActionResult<List<PaymentDtos>>> Handle(GetHistoryPaymentsOfUserQuerry request, CancellationToken cancellationToken)
        {
            var pay = await context.Payments.Where(x => x.UserCreateCourseId.Equals(request.Id)).ToListAsync();
            if(pay == null) 
            {
                return new NoContentResult();
            }
            List<PaymentDtos> payDto = new List<PaymentDtos>();
            foreach(var p in pay)
            {
                var payDtos = new PaymentDtos
                {
                    CourseId=p.CourseId,
                    Money=p.RequriedAmount,
                    PaymentId=p.PaymentId,
                    TransactionDate=p.PaymentDate
                };
                payDto.Add(payDtos);
            }
            
            //var result = new PaymentDTO();

            //try
            //{
            //    string connectionString = connectionService.Datebase ?? string.Empty;
            //    var paramters = new SqlParameter[]
            //    {
            //        new SqlParameter("@PaymentId", request.Id),
            //    };
            //    (var data, string sqlError) = sqlService.FillDataTable(connectionString,
            //        PaymentConstants.SelectByIdSprocName, paramters);
            //    var payment = data.AsListObject<PaymentDtos>()?.SingleOrDefault();

            //    if (payment != null)
            //    {
            //        result.Set(true, MessageContants.OK, payment);
            //    }
            //    else
            //    {
            //        result.Set(false, MessageContants.NotFound);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    result.Set(false, MessageContants.Error);
            //    result.Errors.Add(new BaseError()
            //    {
            //        Code = MessageContants.Exception,
            //        Message = ex.Message,
            //    });
            //}

            return new OkObjectResult(payDto);
        }
    }
}
