using AuthenticateService.API.Feature.AuthenticateFearture.Command.Users.UserCommand;

using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticateService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator mediator;
        
        public ProfileController(IMediator _mediator)
        {
            mediator= _mediator;
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
       
    }
}
