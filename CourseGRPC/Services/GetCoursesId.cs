using CourseGRPC.Models;
using Grpc.Core;

namespace CourseGRPC.Services
{
    public class GetCoursesId : GetCourseByIdService.GetCourseByIdServiceBase
    {
        private readonly CourseContext _context;
        public GetCoursesId(CourseContext context)
        {
            _context = context;
        }

        public override  Task<GetCourseIdResponse> GetCourseId(GetCourseIdRequest request, ServerCallContext context)
        {
            var course = _context.Courses.FirstOrDefault(e => e.Id == request.CourseId);
                               
                               

            if (course == null)
            {
                return Task.FromResult<GetCourseIdResponse>(null);
            }

            var response = new GetCourseIdResponse()
            {
                Id = course.Id,
                Name = course.Name,
            };
          

            return Task.FromResult(response);
        }
    }
}
