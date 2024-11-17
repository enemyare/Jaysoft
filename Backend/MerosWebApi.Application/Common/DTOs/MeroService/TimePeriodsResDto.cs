using MerosWebApi.Core.Models.Mero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class TimePeriodsResDto
    {
        public string Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int TotalPlaces { get; set; }

        public int BookedPlaces { get; set; }

        public static TimePeriodsResDto Map(TimePeriod timePeriod)
        {
            return new TimePeriodsResDto
            {
                Id = timePeriod.Id,
                StartTime = timePeriod.StartTime,
                EndTime = timePeriod.EndTime,
                TotalPlaces = timePeriod.TotalPlaces,
                BookedPlaces = timePeriod.BookedPlaces
            };
        }
    }
}
