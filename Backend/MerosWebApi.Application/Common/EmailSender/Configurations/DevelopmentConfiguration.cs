namespace MerosWebApi.Application.Common.EmailSender.Configurations
{
    public class DevelopmentConfiguration : IEmailConfiguration
    {
        private readonly string _emailAddress;
        public string EmailAddress => _emailAddress;

        private readonly string _emailPassword;
        public string Password => _emailPassword;

        private readonly string _emailHost;
        public string EmailHost => _emailHost;

        private readonly int _emailPort;
        public int EmailHostPort => _emailPort;

        public DevelopmentConfiguration(string emailAddress, string password,
            string host, int port)
        {
            _emailAddress = emailAddress;
            _emailPassword = password;
            _emailHost = host;
            _emailPort = port;
        }
    }
}
