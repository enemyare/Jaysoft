using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
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
