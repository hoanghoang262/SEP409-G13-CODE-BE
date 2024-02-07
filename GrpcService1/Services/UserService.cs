using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using UserGrpc;
using UserGrpc.Models;


namespace UserGrpc.Services
{
    public class UserService : UserCourseService.UserCourseServiceBase
    {
       
        private readonly AuthenticationContext _context;
        public UserService( AuthenticationContext context)
        {
           
            _context = context;
        }

        public override async Task<GetUserCoursesResponse> GetUserCourses(GetUserCourseRequest request, ServerCallContext context)
        {
            var response = await _context.Users.FirstOrDefaultAsync(u=>u.Id.Equals(request.UserId));

            return await Task.FromResult(new GetUserCoursesResponse()
            {
                Id = response.Id,
                Name = response.UserName,

            });
        }

    }
}
