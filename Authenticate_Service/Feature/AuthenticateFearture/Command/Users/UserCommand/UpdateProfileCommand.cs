using Authenticate_Service.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Globalization;
using Contract.Service.Message;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AuthenticateService.API.Feature.AuthenticateFearture.Command.Users.UserCommand
{
    public class UpdateProfileCommand : IRequest<IActionResult>
    {
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? BirthDate { get; set; }
        public string? FacebookLink { get; set; }
        public string? ProfilePict { get; set; }
        public string? UserName { get; set; }
        public string? Phone { get; set; }

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
            if(request.UserId == null || String.IsNullOrEmpty(request.UserName) || String.IsNullOrEmpty(request.FullName))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            // Validate username
            string userNamePattern = @"^[^\s]{8,32}$";
            if (!String.IsNullOrEmpty(request.UserName) && !Regex.IsMatch(request.UserName, userNamePattern))
            {
                return new BadRequestObjectResult(Message.MSG21);
            }

            // Validate phone number
            string phonePattern = "^0\\d{9}$";
            if (!String.IsNullOrEmpty(request.Phone) && !Regex.IsMatch(request.Phone, phonePattern))
            {
                return new BadRequestObjectResult(Message.MSG20);
            }

            // Check user is exist
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return new BadRequestObjectResult(Message.MSG01);
            }

            // Check username is exist
            var userExist = await _context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(request.UserName));
            if (userExist != null && userExist.Id != user.Id)
            {
                return new BadRequestObjectResult(Message.MSG07);
            }

            user.FullName = request.FullName ?? user.FullName;
            user.Address = request.Address ?? user.Address;
            user.BirthDate = !String.IsNullOrEmpty(request.BirthDate) ? DateTime.ParseExact(request.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture) : user.BirthDate;
            user.FacebookLink = request.FacebookLink ?? user.FacebookLink;
            user.ProfilePict = request.ProfilePict ?? user.ProfilePict;
            user.UserName = request.UserName ?? user.UserName;
            user.Phone = request.Phone ?? user.Phone;

            await _context.SaveChangesAsync(cancellationToken);

            return new OkObjectResult(Message.MSG19);
        }
    }
}
