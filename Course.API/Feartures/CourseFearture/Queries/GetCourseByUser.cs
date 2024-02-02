using Course.API.GrpcServices;
using CourseService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Course.API.Feartures.CourseFearture.Queries
{
    public class GetCourseByUser : IRequest<IActionResult>
    {
        public int UserId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByUser, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly UserIdCourseGrpcService service;

            public GetCourseByUserHandler(CourseContext context,UserIdCourseGrpcService userIdCourseGrpcService) { 
                     _context=context;
                     service=userIdCourseGrpcService;
                
            }
            public async Task<IActionResult> Handle(GetCourseByUser request, CancellationToken cancellationToken)
            {
                var user= await service.SendUserId(request.UserId);

                return new OkObjectResult(user);
            }
        }
    }
}
