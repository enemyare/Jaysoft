using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
{
    public class MyRegistredMerosResDto
    {
        public string Id { get; set; }

        public string MeetName { get; set; }

        public string Description { get; set; }

        public TimePeriodBookedResDto Periods { get; set; }

        public static MyRegistredMerosResDto Map(Mero mero)
        {
            return new MyRegistredMerosResDto
            {
                Id = mero.Id,
                MeetName = mero.Name,
                Description = mero.Description,
                Periods = TimePeriodBookedResDto.Map(mero.TimePeriods.FirstOrDefault()),
            };
        }
    }
}
