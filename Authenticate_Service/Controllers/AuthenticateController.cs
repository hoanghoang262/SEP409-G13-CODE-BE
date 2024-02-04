using Account.API.Model;
using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authenticate_Service.Feature.AuthenticateFearture.Command;

using MediatR;
using Authenticate_Service.Common;
using MassTransit;
using EventBus.Message.IntegrationEvent.Event;
using Authenticate_Service.Feature.AuthenticateFearture.Command.Login;



namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
          
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
    }
 }
