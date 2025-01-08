using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.EmailSender.Configurations
{
    internal class RelizeConfiguration : IEmailConfiguration
    {
        private readonly string _emailAddress;
        public string EmailAddress => _emailAddress;

        private readonly string _emailPassword;
        public string Password => _emailPassword;

        private readonly string _emailHost;
        public string EmailHost => _emailHost;

        private readonly int _emailPort;
        public int EmailHostPort => _emailPort;

        private readonly SecureSocketOptions _secureOptions;
        public SecureSocketOptions SecureSocketOptions => _secureOptions;

        public RelizeConfiguration(string emailAddress, string password,
            string host, int port, SecureSocketOptions secureOptions)
        {
            _emailAddress = emailAddress;
            _emailPassword = password;
            _emailHost = host;
            _emailPort = port;
            _secureOptions = secureOptions;
        }
    }
}
