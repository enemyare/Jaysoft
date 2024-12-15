using MerosWebApi.Application.Interfaces;
using System.Net.Mail;

namespace MerosWebApi.Application.Common.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailConfiguration _configuration;

        private readonly MailAddress myAddress;

        public EmailSender(IEmailConfiguration configuration)
        {
            _configuration = configuration;
            if (!MailAddress.TryCreate(configuration.EmailAddress, out myAddress))
                throw new ArgumentException("Not Valid EmailAddress");
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string htmlContent)
        {
            var email = new MailMessage();
            email.From = myAddress;
            email.To.Add(new MailAddress(toEmail));
            email.Subject = subject;
            email.Body = htmlContent;

            try
            {
                using var smtp = new SmtpClient(_configuration.EmailHost, _configuration.EmailHostPort);

                await smtp.SendMailAsync(email);
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки, если необходимо
                return false; // В случае исключения возвращаем false
            }

            return true; // Если всё прошло успешно
        }
    }
}
