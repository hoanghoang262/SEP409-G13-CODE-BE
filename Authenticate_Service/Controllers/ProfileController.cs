using Authenticate_Service.Feature.AuthenticateFearture.Command.ChangePassword;
using Authenticate_Service.Models;
using AuthenticateService.API.Feature.AuthenticateFearture.Command.Users.UserCommand;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                return BadRequest();
            }

            try
            {
                var result = await mediator.Send(updateUserCommand);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Email.Equals(email));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Delete Ok");
        }
        [HttpPost]

        public async Task<IActionResult> ChangePass(ChangePasswordCommand command)
        {
            return Ok(await mediator.Send(command));
        }
    }
}
