namespace MerosWebApi.Application.Interfaces
{
    public interface IEmailSender
    {
        public Task<bool> SendAsync(string toEmail, string subject,
            string htmlContent);
    }
}
