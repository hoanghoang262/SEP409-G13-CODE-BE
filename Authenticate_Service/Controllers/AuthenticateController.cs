using Authenticate_Service.Common;
using Authenticate_Service.Feature.AuthenticateFearture.Command.Login;
using Authenticate_Service.Models;
using AuthenticateService.API.Common.DTO;
using Contract.SeedWork;
using Contract.Service;
using Contract.Service.Configuration;
using Contract.Service.Message;
using MassTransit.Initializers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            // Validate input
            if (String.IsNullOrEmpty(request.UserName)
                || String.IsNullOrEmpty(request.Password)
                || String.IsNullOrEmpty(request.Email))
            {
                return new BadRequestObjectResult(Message.MSG11);
            }

            // Validate username
            string userNamePattern = @"^[^\s]{8,32}$";
            if (!Regex.IsMatch(request.UserName, userNamePattern))
            {
                return new BadRequestObjectResult(Message.MSG21);
            }

            // Validate email
            string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(request.Email, emailPattern))
            {
                return new BadRequestObjectResult(Message.MSG09);
            }

            // Validate password
            string passwordPattern = "^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()-_+=])[A-Za-z0-9!@#$%^&*()-_+=]{8,32}$";
            if (!Regex.IsMatch(request.Password, passwordPattern))
            {
                return new BadRequestObjectResult(Message.MSG17);
            }

            // Check email and username exist
            if (context.Users.Any(u => u.Email == request.Email))
            {
                return new BadRequestObjectResult(Message.MSG06);
            }
            else if (context.Users.Any(u => u.UserName == request.UserName))
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
            var user = await context.Users.Include(c => c.Role).FirstOrDefaultAsync(u => u.Id.Equals(id));

            if (user == null || user.Role == null)
            {
                return BadRequest(Message.MSG01);
            }

            var userDto = new UserDto()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.Phone,
                Address = user.Address,
                BirthDate = user.BirthDate,
                FacebookLink = user.FacebookLink,
                ProfilePict = user.ProfilePict,
                Status = user.Status,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };

            return Ok(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudent(string? Search, bool? Status, int Page = 1, int PageSize = 5)
        {
            IQueryable<User> query = context.Users.Include(u => u.Role).Where(u => u.Role.Id == 1);
            if (!string.IsNullOrEmpty(Search))
            {
                query = query.Where(u => u.UserName.Contains(Search) || u.Email.Contains(Search));
            }

          
            if (Status.HasValue)
            {
                query = query.Where(u => u.Status == Status.Value);
            }

            var totalItems = await query.CountAsync();
            var userList = await query.Skip((Page - 1) * PageSize).Take(PageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserName = u.UserName,
                    Phone = u.Phone,
                    Address = u.Address,
                    BirthDate = u.BirthDate,
                    FacebookLink = u.FacebookLink,
                    ProfilePict = u.ProfilePict,
                    Status = u.Status,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name
                }).ToListAsync();

            var result = new PageList<UserDto>(userList, totalItems, Page, PageSize);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdminBussiness(string? Search, bool? Status, int Page = 1, int PageSize = 5)
        {
            IQueryable<User> query = context.Users.Include(u => u.Role).Where(u => u.Role.Id == 2);
            if (!string.IsNullOrEmpty(Search))
            {
                query = query.Where(u => u.UserName.Contains(Search) || u.Email.Contains(Search));
            }


            if (Status.HasValue)
            {
                query = query.Where(u => u.Status == Status.Value);
            }

            var totalItems = await query.CountAsync();
            var userList = await query.Skip((Page - 1) * PageSize).Take(PageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    UserName = u.UserName,
                    Phone = u.Phone,
                    Address = u.Address,
                    BirthDate = u.BirthDate,
                    FacebookLink = u.FacebookLink,
                    ProfilePict = u.ProfilePict,
                    Status = u.Status,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name
                }).ToListAsync();

            var result = new PageList<UserDto>(userList, totalItems, Page, PageSize);

            return Ok(result);
        }


        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if (user == null)
            {
                return BadRequest(Message.MSG01);
            }

            user.Status = !user.Status;
            context.Users.Update(user);
            await context.SaveChangesAsync();

            return Ok(Message.MSG16);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest(Message.MSG01);
            }

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
                return Ok(Message.MSG06);
            }
            return Ok(Message.MSG10);
        }
    }
}
