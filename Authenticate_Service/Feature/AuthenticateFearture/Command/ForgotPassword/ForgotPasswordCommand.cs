using Authenticate_Service.Models;
using Contract.Service;
using Contract.Service.Configuration;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authenticate_Service.Feature.AuthenticateFearture.Command.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, IActionResult>
        {
            private readonly IEmailService<MailRequest> _emailService;
            private readonly AuthenticationContext _context;
            public ForgotPasswordHandler(IEmailService<MailRequest> emailService,AuthenticationContext context)
            {
                _emailService = emailService;
                _context = context;
            }
            public async Task<IActionResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email.Equals(request.Email));
                if (user == null) {
                    return new BadRequestObjectResult("Please check your email");
                }
                var verificationCode = new Random().Next(100000, 999999).ToString();
                user.VerificationCode=verificationCode;
                await _context.SaveChangesAsync();
                var message = new MailRequest
                {
                    Body = $@"
<html>
<head>
<style>
    body {{
        font-family: 'Arial', sans-serif;
        color: #333;
        background-color: #f4f4f4;
        padding: 20px;
    }}
    .container {{
        max-width: 600px;
        margin: auto;
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 0 10px rgba(0,0,0,0.1);
    }}
    h1 {{
        color: #007bff;
        text-align: center;
    }}
    .code {{
        font-size: 24px;
        font-weight: bold;
        text-align: center;
        margin: 20px 0;
        padding: 10px;
        background-color: #eee;
        border-radius: 4px;
    }}
</style>
</head>
<body>
    <div class='container'>
        <h1>Your Verification Code Is:</h1>
        <div class='code'>{verificationCode}</div>
        <p>Please enter this code to verify your email address.</p>
    </div>
</body>
</html>",
                    ToAddress = request.Email,
                    Subject = "Verify Code"
                };
                await  _emailService.SendEmailAsync(message);

                return new OkObjectResult("We have sent the confirmation code to your email: "+request.Email);

            }
        }
    }

}
