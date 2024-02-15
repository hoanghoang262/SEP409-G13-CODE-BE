using CourseGRPC.Models;
using Grpc.Core;


namespace CourseGRPC.Services
{
    public class UserEnrollCourse : UserEnrollCourseService.UserEnrollCourseServiceBase
    {
        private readonly CourseContext _context;
        public UserEnrollCourse(CourseContext context)
        {
            _context = context;
        }

        public override Task<GetUserEnrollCoursesResponse> GetUserEnrollCourses(GetUserEnrollCourseRequest request, ServerCallContext context)
        {
            var userEnrollCourses = _context.Enrollments
                                .Where(e => e.CourseId == request.CourseId)
                                .Select(e => e.UserId)
                                .ToList();

            var response = new GetUserEnrollCoursesResponse();
            foreach(var id in  userEnrollCourses)
            {
                response.UserId.Add((int)id);
            }
           

            return Task.FromResult(response);
        }
    }
}
