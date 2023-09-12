using Microsoft.Extensions.Options;
using WebAPI.StandaloneServices.EmailService.Dtos;
using WebAPI.StandaloneServices.MailConfigModels;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebAPI.StandaloneServices.EmailServiceNamespace
{
    public class EmailService
    {
        private readonly MailServiceSettings _mailSettings;
        public EmailService(IOptions<MailServiceSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            /**
             * Setting basic mail data.
             */
            //Adding sender mail address
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //Adding reciver mail address
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            //Setting mail subject
            email.Subject = mailRequest.Subject;


            var builder = new BodyBuilder();
            //Adding mail attachments.
            if (mailRequest.Attachment != null)
            {
               builder.Attachments.Add(mailRequest.Attachment.FileName, mailRequest.Attachment.File);
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
