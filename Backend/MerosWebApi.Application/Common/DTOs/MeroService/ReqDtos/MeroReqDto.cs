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
