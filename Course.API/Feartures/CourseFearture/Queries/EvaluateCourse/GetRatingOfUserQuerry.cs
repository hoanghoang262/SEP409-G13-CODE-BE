using Contract.Service.Message;
using CourseGRPC.Services;
using CourseService.API.GrpcServices;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.API.Feartures.CourseFearture.Queries.EvaluateCourse
{
    public class GetRatingOfUserQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }

        public class GetRatingOfUserQuerryHandler : IRequestHandler<GetRatingOfUserQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CheckCourseIdServicesGrpc _services;
            private readonly GetUserInfoService _service;

            public GetRatingOfUserQuerryHandler(CourseContext context, CheckCourseIdServicesGrpc services,GetUserInfoService service)
            {
                _context = context;
                _services = services;
                _service=service;
            }
            public async Task<IActionResult> Handle(GetRatingOfUserQuerry request, CancellationToken cancellationToken)
            {
                var user = await _service.SendUserId(request.UserId);
                if(user.Id == 0) 
                {
                    return new BadRequestObjectResult(Message.MSG01);
                }

                var course = _context.Courses.Find(request.CourseId);
                if(course == null)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }

                var rate = _context.CourseEvaluations.FirstOrDefault(e => e.UserId == request.UserId && e.CourseId == request.CourseId);
                if (rate == null)
                {
                    return new NotFoundObjectResult(Message.MSG43);
                }

                return new OkObjectResult(rate.Star);
            }
        }
    }
}
