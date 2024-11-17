using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class TimePeriodsReqDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int TotalPlaces { get; set; }
    }
}
