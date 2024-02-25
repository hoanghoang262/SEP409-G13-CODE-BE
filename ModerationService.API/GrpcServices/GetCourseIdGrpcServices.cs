using CourseGRPC;

namespace ModerationService.API.GrpcServices
{
    public class GetCourseIdGrpcServices 
    {
        private readonly GetCourseByIdService.GetCourseByIdServiceClient service;
        public GetCourseIdGrpcServices(GetCourseByIdService.GetCourseByIdServiceClient _service)
        {
            service = _service;
        }

        public async Task<GetCourseIdResponse> SendCourseId(int courseId)
        {
            try
            {
                var courseIdRequest = new GetCourseIdRequest { CourseId = courseId };
                return await service.GetCourseIdAsync(courseIdRequest);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
