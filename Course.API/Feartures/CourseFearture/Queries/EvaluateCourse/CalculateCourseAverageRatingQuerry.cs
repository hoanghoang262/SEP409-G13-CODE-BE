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
                var course = await _context.Courses.FindAsync(request.CourseId);
                if (course == null)
                {
                    return new BadRequestObjectResult(Message.MSG25);
                }

                var averageRating = await _context.CourseEvaluations
                                           .Where(e => e.CourseId == request.CourseId && e.Star != null)
                                           .Select(e => e.Star.Value)
                                           .DefaultIfEmpty()
                                           .AverageAsync(cancellationToken);

                return new OkObjectResult(Math.Round(averageRating, 2));
            }
        }
    }
}
