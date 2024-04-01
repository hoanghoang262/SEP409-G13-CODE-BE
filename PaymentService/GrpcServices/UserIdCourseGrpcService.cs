
using UserGrpc;
namespace GrpcServices
{
    public class UserIdCourseGrpcService 
    {
        private readonly UserCourseService.UserCourseServiceClient service;
        public UserIdCourseGrpcService(UserCourseService.UserCourseServiceClient _service)
        {
            service= _service;  
        }
        public async Task<GetUserCoursesResponse> SendUserId(int userId)
        {
            try
            {
                var userIdRequest = new GetUserCourseRequest { UserId = userId };
                return await service.GetUserCoursesAsync(userIdRequest);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
