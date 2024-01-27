
using Authenticate_Service.Service.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Text;
using System.Threading.Tasks;

namespace Authenticate_Service.Service
{
    public class SMTPEmailService : IEmailService
    {
      
        private readonly SmtpEmailSetting _setting;
        public SMTPEmailService(IOptions<SmtpEmailSetting> options)
        {
             
           this._setting = options.Value;
        }

        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_setting.DisplayName, request.From ?? _setting.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };
            if(request.ToAddresses.Any())
            {
                foreach(var  address in request.ToAddresses) { 
                     emailMessage.To.Add(MailboxAddress.Parse(address));
                }
            }
            else
            {
                var toAdress = request.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAdress));
            }
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                await smtpClient.ConnectAsync(_setting.SMTPServer, _setting.Port, _setting.UseSsl, cancellationToken);
                await smtpClient.AuthenticateAsync(_setting.Username,_setting.Password,cancellationToken);

                await smtpClient.SendAsync(emailMessage);
                await smtpClient.DisconnectAsync(true,cancellationToken);

            }
            catch(Exception e)
            {

            }
            finally
            {
                await smtpClient.DisconnectAsync(true,cancellationToken);
                smtpClient.Dispose();
            }
        }
    }
}
