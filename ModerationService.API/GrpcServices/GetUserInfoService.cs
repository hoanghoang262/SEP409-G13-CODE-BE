using UserGrpc;

namespace ModerationService.API.GrpcServices
{
    public class GetUserInfoService
    {
        private readonly GetUserService.GetUserServiceClient service;
        public GetUserInfoService(GetUserService.GetUserServiceClient _service)
        {
            service = _service;
        }
        public async Task<GetUserInfoResponse> SendUserId(int userId)
        {
            try
            {
                var userIdRequest = new GetUserInfoRequest { UserId = userId };
                return await service.GetUserInfoAsync(userIdRequest);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

