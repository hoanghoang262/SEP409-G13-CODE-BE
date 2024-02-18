using Authenticate_Service.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Authenticate_Service.Feature.AuthenticateFearture.Command.Login;
using Contract.Service;
using Authenticate_Service.LoginModel;
using Microsoft.EntityFrameworkCore;
using Contract.Service.Configuration;
using Authenticate_Service.Common;



namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly AuthenticationContext context;
        private readonly IEmailService<MailRequest> _emailService;
        private readonly HassPaword hash = new HassPaword();
        
        public AuthenticateController(IMediator mediator, AuthenticationContext _context, IEmailService<MailRequest> emailService)
        {
            _mediator = mediator;
            context = _context;
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> LoginGoogle(LoginGoogleCommand command)
        {
           
            return Ok(await _mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel request)
        {

            var hashPass = hash.HashPassword(request.Password);
            if (context.Users.Any(u => u.Email == request.Email))
            {
                return new BadRequestObjectResult("A user is already registered with this e-mail address.");
            }
            if (context.Users.Any(u => u.UserName == request.UserName))
            {
                return new BadRequestObjectResult("A user is already registered with this username.");
            }
            else
            {
                var newUser = new User { Email = request.Email, UserName = request.UserName, Password = hashPass, RoleId = 1 };
                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                var callbackUrl = Url.Page(
                          "/ConfirmEmail",
                           pageHandler : null,
                          new { userId = newUser.Id },
                         protocol:Request.Scheme);

                var message = new MailRequest
                {
                    Body = @$"
<html>
    <body>
        <div style='font-family: Arial, sans-serif; color: #333;'>
            <h2 style='color: #0056b3;'>Welcome to Happy Learning, {request.UserName}!</h2>
            <p>Thank you for signing up. Please confirm your email address to activate your account.</p>
            <p style='margin: 20px 0;'>
                <a href='{callbackUrl}' style='background-color: #0056b3; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Email</a>
            </p>
            <p>If you did not create an account using this email address, please ignore this email.</p>
        </div>
    </body>
</html>",
                    
                    ToAddress = request.Email,
                    Subject = "Confirm Your Email "
                };
                await _emailService.SendEmailasync(message);

                return new OkObjectResult("Please confirm the email that have sent to you");

            }
           
        }
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {

            var user= await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));

            return Ok(user);    
        }
       
    }
 }
