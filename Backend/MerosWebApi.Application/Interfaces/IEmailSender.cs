﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Interfaces
{
    public interface IEmailSender
    {
        public Task<bool> SendAsync(string toEmail, string subject,
            string htmlContent);
    }
}
