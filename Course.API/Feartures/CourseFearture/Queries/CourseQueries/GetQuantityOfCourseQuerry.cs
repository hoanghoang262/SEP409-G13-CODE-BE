using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetQuantityOfCourseQuerry : IRequest<IActionResult>
    {

        public class GetQuantityOfCourseQuerryHandler : IRequestHandler<GetQuantityOfCourseQuerry, IActionResult>
        {
            private readonly CourseContext _dbContext;

            public GetQuantityOfCourseQuerryHandler(CourseContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<IActionResult> Handle(GetQuantityOfCourseQuerry request, CancellationToken cancellationToken)
            {
                var querry = await _dbContext.Courses.ToListAsync();
                if (querry == null)
                {
                    return new OkObjectResult(0);
                }
                var count = querry.Count();

                return new OkObjectResult(count);
            }
        }
    }
}
