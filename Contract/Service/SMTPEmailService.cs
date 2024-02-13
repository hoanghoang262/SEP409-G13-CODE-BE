

using Contract.Service.Configuration;
using MailKit.Net.Smtp;

using MimeKit;



namespace Contract.Service
{
    public class SMTPEmailService : IEmailService<MailRequest>
    {
      
        private readonly SmtpEmailSetting _setting;
        public SMTPEmailService( SmtpEmailSetting options)
        {
             
           _setting = options;
        }

        public async Task SendEmailasync(MailRequest request, CancellationToken cancellationToken = default)
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
