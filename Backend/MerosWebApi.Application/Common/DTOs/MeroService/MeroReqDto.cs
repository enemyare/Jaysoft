using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class MeroReqDto
    {
        public string MeetName { get; set; }

        public string Description { get; set; }

        public string CreatorEmail { get; set; }

        public List<TimePeriodsReqDto> Periods { get; set; }

        public List<FieldReqDto> Fields { get; set; }
    }
}
