using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthenticateService.API.MessageOutput;

namespace AuthenticateService.API.Feature.AuthenticateFearture.Command.Users.UserCommand
{
    public class UpdateProfileCommand : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePict { get; set; }
        public bool? Status { get; set; }
        public string? UserName { get; set; }

    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, IActionResult>
    {
        private readonly AuthenticationContext _context;

        public UpdateProfileCommandHandler(AuthenticationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new BadRequestObjectResult(Message.MSG01);
            }

            user.FullName = request.FullName ?? user.FullName;
            user.LastName = request.LastName ?? user.LastName;
            user.ProfilePict = request.ProfilePict ?? user.ProfilePict;
            user.Status = request.Status ?? user.Status;
            user.UserName = request.UserName ?? user.UserName;

            await _context.SaveChangesAsync(cancellationToken);

            return new OkObjectResult(Message.MSG19);
        }
    }
}
