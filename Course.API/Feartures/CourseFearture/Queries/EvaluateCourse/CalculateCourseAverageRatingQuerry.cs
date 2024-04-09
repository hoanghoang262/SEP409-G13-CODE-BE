using Contract.Service.Message;
using CourseGRPC;
using CourseGRPC.Services;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.EvaluateCourse
{
    public class CalculateCourseAverageRatingQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
        public class CalculateCourseAverageRatingHandler : IRequestHandler<CalculateCourseAverageRatingQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly CheckCourseIdServicesGrpc _services;

            public CalculateCourseAverageRatingHandler(CourseContext context, CheckCourseIdServicesGrpc service)
            {
                _context = context;
                _services = service;
            }

            public async Task<IActionResult> Handle(CalculateCourseAverageRatingQuerry request, CancellationToken cancellationToken)
            {
                if (request.CourseId <= 0)
                {
                    return new BadRequestObjectResult("CourseId phải lớn hơn 0 ");
                }
                var course = await _services.SendCourseId(request.CourseId);
                if (course.IsCourseExist == 0)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }
                var averageRating = await _context.CourseEvaluations
                    .Where(e => e.CourseId == request.CourseId && e.Star != null)
                    .AverageAsync(e => e.Star.Value, cancellationToken);

                return new OkObjectResult(Math.Round(averageRating, 2));
            }
        }
    }
}
