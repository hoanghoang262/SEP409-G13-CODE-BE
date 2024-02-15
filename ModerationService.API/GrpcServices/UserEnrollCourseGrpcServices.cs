
using CourseGRPC;
namespace CourseGRPC.Services
{
    public class UserEnrollCourseGrpcServices 
    {
        private readonly UserEnrollCourseService.UserEnrollCourseServiceClient service ;
        public UserEnrollCourseGrpcServices(UserEnrollCourseService.UserEnrollCourseServiceClient _service)
        {
            service = _service;
        }

        public async Task<GetUserEnrollCoursesResponse> SendCourseId(int courseId)
        {
            try
            {
                var courseIdRequest = new GetUserEnrollCourseRequest { CourseId = courseId };
                return await service.GetUserEnrollCoursesAsync(courseIdRequest);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
