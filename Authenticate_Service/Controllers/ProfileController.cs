using Authenticate_Service.Feature.AuthenticateFearture.Command.ChangePassword;
using Authenticate_Service.Models;
using AuthenticateService.API.Feature.AuthenticateFearture.Command.Users.UserCommand;
using Contract.Service.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AuthenticateService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly AuthenticationContext _context;
        public ProfileController(IMediator _mediator,AuthenticationContext context)
        {
            mediator= _mediator;
            _context= context;
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(int id, UpdateProfileCommand updateUserCommand)
        {
           
            
                if (id != updateUserCommand.UserId)
                {
                    return BadRequest(Message.MSG30);
                }
                var result = await mediator.Send(updateUserCommand);
                return Ok(result);
     
         
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string? email)
        {
            // Validate input
            if (string.IsNullOrEmpty(email))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            // Validate email
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                return new BadRequestObjectResult(Message.MSG09);
            }

            var user = await _context.Users.FirstOrDefaultAsync(c => c.Email.Equals(email));
            if (user == null)
            {
                return BadRequest(Message.MSG01);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(Message.MSG16);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(ChangePasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
