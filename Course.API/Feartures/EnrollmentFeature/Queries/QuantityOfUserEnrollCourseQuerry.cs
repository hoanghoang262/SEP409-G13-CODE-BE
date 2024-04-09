using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.EnrollmentFeature.Queries
{
    public class QuantityOfUserEnrollCourseQuerry : IRequest<IActionResult>
    {
        public int CourseId { get;set; }

        public class QuantityOfUserEnrollCourseQuerryHandler : IRequestHandler<QuantityOfUserEnrollCourseQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            public QuantityOfUserEnrollCourseQuerryHandler(CourseContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Handle(QuantityOfUserEnrollCourseQuerry request, CancellationToken cancellationToken)
            {
                var userEnroll= await _context.Enrollments.Where(c=>c.CourseId==request.CourseId).ToListAsync();

                var number = userEnroll.Count();


                return new OkObjectResult(number);
            }
        }

    }
}
