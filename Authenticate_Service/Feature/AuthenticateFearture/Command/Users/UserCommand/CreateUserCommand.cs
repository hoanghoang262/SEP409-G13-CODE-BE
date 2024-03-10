using Authenticate_Service.Models;
using MediatR;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.Users.UserCommand
{
    public class CreateUserCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string ProfilePict { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }
    }

    // CreateUserCommandHandler.cs
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly AuthenticationContext _dbContext;

        public CreateUserCommandHandler(AuthenticationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                ProfilePict = request.ProfilePict,
                Status = request.Status,
                RoleId = request.RoleId
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser.Id;
        }
    }
}
