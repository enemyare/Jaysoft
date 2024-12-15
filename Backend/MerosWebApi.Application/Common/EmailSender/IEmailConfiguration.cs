namespace MerosWebApi.Application.Common.EmailSender
{
    public interface IEmailConfiguration
    {
        public string EmailAddress { get; }

        public string Password { get; }

        public string EmailHost { get; }

        public int EmailHostPort { get; }
    }
}
