using Authenticate_Service.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Authenticate_Service.Feature.AuthenticateFearture.Command.Login;
using Contract.Service;
using Microsoft.EntityFrameworkCore;
using Contract.Service.Configuration;
using Authenticate_Service.Common;
using Microsoft.AspNetCore.Http.Extensions;
using AuthenticateService.API.Common.DTO;
using AuthenticateService.API.Message;



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
            if (context.Users.Any(u => u.Email == request.Email))
            {
                return new BadRequestObjectResult(Message.MSG06);
            }
            if (context.Users.Any(u => u.UserName == request.UserName))
            {
                return new BadRequestObjectResult(Message.MSG07);
            }
            else
            {
                var newUser = new User { Email = request.Email, UserName = request.UserName, Password = request.Password, RoleId = 1, EmailConfirmed = false };
                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                var callbackUrl = Url.Action(
                                "ConfirmEmail",
                                 "Authenticate",
                                 new { userId = newUser.Id },
                                 Request.Scheme);

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

                return new OkObjectResult(Message.MSG08);

            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));

            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var user = await context.Users.FindAsync(userId);

            user.EmailConfirmed = true;
            await context.SaveChangesAsync();

            return RedirectToPage("/ConfirmEmail");

        }
        [HttpPost]
        public async Task<IActionResult> CheckEmailExist(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(c => c.Email.Equals(email));
            if (user != null)
            {
                return BadRequest("Email has exist");
            }
            return Ok("Not found Email");
        }


    }
}
