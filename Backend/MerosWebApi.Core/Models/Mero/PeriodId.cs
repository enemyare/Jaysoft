using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.Mero
{
    public class PeriodId
    {
        public PeriodId(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
    }
}
