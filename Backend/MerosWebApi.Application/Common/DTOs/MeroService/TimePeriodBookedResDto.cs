using MerosWebApi.Core.Models.Mero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class TimePeriodBookedResDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int BookedPlaces { get; set; }

        public static TimePeriodBookedResDto Map(TimePeriod timePeriod)
        {
            return new TimePeriodBookedResDto
            {
                StartTime = timePeriod.StartTime,
                EndTime = timePeriod.EndTime,
                BookedPlaces = timePeriod.BookedPlaces
            };
        }
    }
}
