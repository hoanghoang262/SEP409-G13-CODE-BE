

using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using UserGrpc.Models;

namespace UserGrpc.Services
{
    public class GetUsersService : GetAllUserService.GetAllUserServiceBase
    {
        private readonly AuthenticationContext _context;
        public GetUsersService(AuthenticationContext context)
        {

            _context = context;
        }

        public override async Task<GetAllUserResponse> GetAllUser(GetAllUserRequest request, ServerCallContext context)
        {
            var query = await _context.Users
                       .Select(user => new UserInfo { Id = user.Id, Name = user.UserName })
                       .ToListAsync();
            var response = new GetAllUserResponse();
            response.Info.AddRange(query);

            return await Task.FromResult(response);
        }

    }

}
