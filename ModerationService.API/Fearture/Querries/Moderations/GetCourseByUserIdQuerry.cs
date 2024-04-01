using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModerationService.API.Models;

namespace ModerationService.API.Fearture.Querries.Moderations
{
    public class GetCourseByUserIdQuerry : IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public class GetCourseByUserIdQuerryHandler : IRequestHandler<GetCourseByUserIdQuerry, IActionResult>
        {
            private readonly Content_ModerationContext _context;

            public GetCourseByUserIdQuerryHandler(Content_ModerationContext context)
            {
                _context = context;

            }
            public async Task<IActionResult> Handle(GetCourseByUserIdQuerry request, CancellationToken cancellationToken)
            {
                var course = await _context.Courses.Where(x => x.CreatedBy.Equals(request.UserId)).ToListAsync();
                if (course == null)
                {
                    return new NotFoundObjectResult(course);
                }

                return new OkObjectResult(course);
            }

        }

    }
}

