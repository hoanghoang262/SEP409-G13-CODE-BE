using CourseGRPC.Models;
using Grpc.Core;


namespace CourseGRPC.Services
{
    public class CheckCourseIdServices : CheckCourseIdService.CheckCourseIdServiceBase
    {
        private readonly CourseContext _courseContext;
        public CheckCourseIdServices(CourseContext courseContext)
        {
            _courseContext = courseContext;
        }
        public override Task<CheckCourseIdResponse> CheckCourseId(CheckCourseIdRequest request, ServerCallContext context)
        {
            var response = new CheckCourseIdResponse();
            var course =  _courseContext.Courses.FirstOrDefault(e => e.Id == request.CourseId);
            if (course == null)
            {

                 response = new CheckCourseIdResponse()
                {
                    IsCourseExist = 0,
                    CourseName=""
                };
            }
            else
            {
                response = new CheckCourseIdResponse()
                {
                    IsCourseExist = request.CourseId,
                    CourseName=course.Name,
                };

            }
            
            return Task.FromResult(response);

        }

    }
}
