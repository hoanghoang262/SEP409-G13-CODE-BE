using CourseService.API.Models;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseService.API.Feartures.CourseFearture.Queries.CourseQueries
{
    public class GetCourseQuantityQuerry: IRequest<int>
    {

        public class GetCourseQuantityQuerryHandler : IRequestHandler<GetCourseQuantityQuerry, int>
        {
            private readonly CourseContext _context;
            public GetCourseQuantityQuerryHandler(CourseContext context)
            {
                _context = context;

            }
            public async Task<int> Handle(GetCourseQuantityQuerry request, CancellationToken cancellationToken)
            {
                var course = await _context.Courses.ToListAsync();
                if(course == null)
                {
                    return 0;   
                }
                var count = course.Count();

                return count;
            }
        }
    }
}
