using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.GrpcServices;
using PaymentService.API.Models;
using PaymentService.Common;
using PaymentService.Interface;
using PaymentService.Models;

namespace PaymentService.Fearture.Payments.Querry
{
    public class GetHistoryPaymentsOfUserBuyQuerry : IRequest<IActionResult>
    {
        public int Id { get; set; }
    }
    public class GetPaymentOfUserBuyHandler : IRequestHandler<GetHistoryPaymentsOfUserBuyQuerry, IActionResult>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly PaymentContext context;
        private readonly GetCourseInfoService serviceCourse;
        public GetPaymentOfUserBuyHandler(ICurrentUserService currentUserService, PaymentContext _context, GetCourseInfoService _serviceCourse)
        {
            this.currentUserService = currentUserService;
            context = _context;
            serviceCourse = _serviceCourse;

        }

        public async Task<IActionResult> Handle(GetHistoryPaymentsOfUserBuyQuerry request, CancellationToken cancellationToken)
        {
            var pay = await context.Payments.Where(x => x.BuyerId.Equals(request.Id)).ToListAsync();
            if (pay == null)
            {
                return new NotFoundObjectResult(pay);
            }
            List<PaymentDtos> payDto = new List<PaymentDtos>();
            foreach (var p in pay)
            {
                var course = await serviceCourse.SendCourseId((int)p.CourseId);
                var payDtos = new PaymentDtos
                {

                    CourseId = p.CourseId,
                    Money = p.RequriedAmount,
                    PaymentId = p.PaymentId,
                    TransactionDate = p.PaymentDate,
                    CourseName=course.Name,
                    CoursePicture=course.Picture,
                    
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
