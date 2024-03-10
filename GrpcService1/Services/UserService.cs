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

        public override  Task<GetUserCoursesResponse> GetUserCourses(GetUserCourseRequest request, ServerCallContext context)
        {
            var response = _context.Users.FirstOrDefault(u => u.Id.Equals(request.UserId));
            if (response == null)
            {
                return Task.FromResult<GetUserCoursesResponse>(null);
            }

            var userInfoResponse = new GetUserCoursesResponse()
            {
                Id = response.Id,
                Name = response.UserName
            };

            // Kiểm tra xem response.ProfilePict có null không
            if (response.ProfilePict != null)
            {
                userInfoResponse.Picture = response.ProfilePict;
            }
            else
            {
               
                userInfoResponse.Picture = ""; 
            }

            return Task.FromResult(userInfoResponse);
        }




    }
}
