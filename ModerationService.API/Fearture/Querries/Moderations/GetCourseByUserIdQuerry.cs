using MediatR;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;
using System.Security.Cryptography.Pkcs;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetCourseByUserIdQuerry : IRequest<List<Course>>
    {
        public int UserId { get; set; }

        public class GetCourseByUserIdQuerryHandler : IRequestHandler<GetCourseByUserIdQuerry, List<Course>>
        {
            private readonly Content_ModerationContext _context;

            public GetCourseByUserIdQuerryHandler(Content_ModerationContext context)
            {
                _context = context;

            }
            public async Task<List<Course>> Handle(GetCourseByUserIdQuerry request, CancellationToken cancellationToken)
            {
                var course = await _context.Courses.Where(x => x.CreatedBy.Equals(request.UserId)).ToListAsync();
                if (course == null)
                {
                    return null;
                }

                return course;
            }

        }

    }
}

