namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class TimePeriodsReqDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int TotalPlaces { get; set; }
    }
}
