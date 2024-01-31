using Account.API.Model;
using Authenticate_Service.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authenticate_Service.Feature.AuthenticateFearture.Command;
using Authenticate_Service.Feature.AuthenticateFearture.LoginGoogle;
using MediatR;
using Authenticate_Service.Common;
using MassTransit;
using EventBus.Message.IntegrationEvent.Event;



namespace Authenticated.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly AuthenticationContext context;

        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;

        
        public AuthenticateController(IPublishEndpoint publishEndpoint,IMediator mediator, IConfiguration configuration, AuthenticationContext _context)
        {

            _configuration = configuration;
            context = _context;
            _mediator = mediator;
            _publishEndpoint=publishEndpoint;


        }
        [HttpPost]
        public async Task<IActionResult> LoginGoogle(LoginGoogleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModels model)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == model.Email
                                                && u.Password == model.Password);
            
            if (user != null)
            {
                var userId = user.Id;
                var userRoles = (from u in context.Users
                                 where user.Email == model.Email
                                 join role in context.Roles on u.RoleId equals role.Id
                                 select role.Name).ToList();

                var tokenGenerator = new GenerateJwtToken(_configuration);

                var token= tokenGenerator.GenerateToken(userId,user.Email,userRoles);

                await _publishEndpoint.Publish(new UserIdMessage { UserId=userId});
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
 }
