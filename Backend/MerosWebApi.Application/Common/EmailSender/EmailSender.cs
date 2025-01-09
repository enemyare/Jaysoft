using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Application.Common.Exceptions.EmailExceptions;

namespace MerosWebApi.Application.Common.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailConfiguration _configuration;

        private readonly MailboxAddress myAddress;

        public EmailSender(IEmailConfiguration configuration)
        {
            _configuration = configuration;
            if (!MailboxAddress.TryParse(configuration.EmailAddress,out myAddress))
                throw new ArgumentException("Not Valid EmailAddress");
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string htmlContent)
        {
            var msg = new MimeMessage();
            msg.From.Add(myAddress);
            msg.To.Add(MailboxAddress.Parse(toEmail));
            msg.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlContent };
            msg.Body = bodyBuilder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_configuration.EmailHost, _configuration.EmailHostPort, _configuration.SecureSocketOptions);
                await smtp.AuthenticateAsync(_configuration.EmailAddress, _configuration.Password);

                var response =  await smtp.SendAsync(msg);
                smtp.Disconnect(true);
            }
            catch (SmtpCommandException smtpEx)
            {
                throw new EmailNotSentException($"Ошибка отправки сообщения по адресу {toEmail}");
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки, если необходимо
                throw new EmailNotSentException(ex.Message);
            }

            return true;
        }
    }
}
