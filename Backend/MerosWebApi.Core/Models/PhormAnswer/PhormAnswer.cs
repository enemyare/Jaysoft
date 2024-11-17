using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Core.Models.PhormAnswer
{
    public class PhormAnswer
    {
        public string Id { get; }

        public string MeroId { get; }

        public string UserId { get; }

        public List<Answer> Answers { get; }

        public TimePeriod TimePeriod { get; }

        public DateTime CreatedTime { get; }
    }
}
