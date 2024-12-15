using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class TimePeriodShortResDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public static TimePeriodShortResDto Map(TimePeriod timePeriod)
        {
            return new TimePeriodShortResDto
            {
                StartTime = timePeriod.StartTime,
                EndTime = timePeriod.EndTime,
            };
        }
    }
}
