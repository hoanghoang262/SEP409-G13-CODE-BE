using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Linq;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.SignUp
{
    public class SignUpCommand :  IRequest<IActionResult>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }

        public class SignUpCommandHandler : IRequestHandler<SignUpCommand, IActionResult>
        {
            private readonly AuthenticationContext context;
            private readonly IHttpContextAccessor _httpContextAccessor;


            public SignUpCommandHandler(AuthenticationContext _context, IHttpContextAccessor httpContextAccessor)
            {
                context = _context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<IActionResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
            {
                return null;

            }
        }
    }
}
