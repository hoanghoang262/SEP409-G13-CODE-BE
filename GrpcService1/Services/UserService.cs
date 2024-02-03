using Grpc.Core;
using UserGrpc;
using UserGrpc.Models;


namespace UserGrpc.Services
{
    public class UserService : UserCourseService.UserCourseServiceBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly AuthenticationContext _context;
        public UserService(ILogger<UserService> logger, AuthenticationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override async Task<GetUserCoursesResponse> GetUserCourses(GetUserCourseRequest request, ServerCallContext context)
        {
            var response = _context.Users.Find(request.UserId);

            var result = new GetUserCoursesResponse()
            {
                Id = response.Id,
                Name = response.Email,

            };

            return result;
        }

    }
}
