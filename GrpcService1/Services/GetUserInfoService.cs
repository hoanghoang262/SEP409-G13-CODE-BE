


using Grpc.Core;
using UserGrpc.Models;

namespace UserGrpc.Services
{
    public class GetUserInfoService : GetUserService.GetUserServiceBase

    {
        private readonly AuthenticationContext _context;
        public GetUserInfoService(AuthenticationContext context)
        {

            _context = context;
        }

        public override Task<GetUserInfoResponse> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
        {
            var response = _context.Users.FirstOrDefault(u => u.Id.Equals(request.UserId));
            if (response == null)
            {
                return Task.FromResult<GetUserInfoResponse>(null);
            }

            return Task.FromResult(new GetUserInfoResponse()
            {
                Id = response.Id,
                Name = response.UserName,
                Picture = response.ProfilePict

            });
        }
    }
}
