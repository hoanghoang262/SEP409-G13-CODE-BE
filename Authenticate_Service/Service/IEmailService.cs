using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authenticate_Service.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = new CancellationToken());
    }
}
