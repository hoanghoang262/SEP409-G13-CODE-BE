

using Grpc.Core;

namespace GrpcService.Services
{
    public class UserService : UserRPC.UserRPCBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetUserByIdResponse
            {
                Name = "Hello " + request.UserId,
                
            });
        }
    }
}
