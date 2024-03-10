using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthenticateService.API.MessageOutput;
using Microsoft.EntityFrameworkCore;

namespace AuthenticateService.API.Feature.AuthenticateFearture.Command.Users.UserCommand
{
    public class UpdateProfileCommand : IRequest<IActionResult>
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string? FacebookLink { get; set; }
        public string? ProfilePict { get; set; }
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
            // Check user is exist
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new BadRequestObjectResult(Message.MSG01);
            }

            user.FullName = request.FullName ?? user.FullName;
            user.Address = request.Address ?? user.Address;
            user.BirthDate = request.BirthDate;
            user.FacebookLink = request.FacebookLink ?? user.FacebookLink;
            user.ProfilePict = request.ProfilePict ?? user.ProfilePict;
            user.UserName = request.UserName ?? user.UserName;

            // Check username is exist
            var userExist = await _context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(user.UserName));
            if (userExist != null)
            {
                return new BadRequestObjectResult(Message.MSG06);
            }


            await _context.SaveChangesAsync(cancellationToken);

            return new OkObjectResult(Message.MSG19);
        }
    }
}
