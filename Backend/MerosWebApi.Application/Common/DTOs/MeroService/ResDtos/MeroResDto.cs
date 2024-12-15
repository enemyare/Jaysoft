using MerosWebApi.Application.Common.DTOs.MeroService.ResDtos;
using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class MeroResDto
    {
        public string Id { get; set; }

        public string UniqueInviteCode { get; set; }

        public string MeetName { get; set; }

        public string Description { get; set; }

        public string CreatorEmail { get; set; }

        public List<TimePeriodsResDto> Periods { get; set; }

        public List<FieldResDto> Fields { get; set; }

        public static MeroResDto Map(Mero mero)
        {
            return new MeroResDto
            {
                Id = mero.Id,
                UniqueInviteCode = mero.UniqueInviteCode,
                MeetName = mero.Name,
                CreatorEmail = mero.CreatorEmail,
                Description = mero.Description,
                Fields = mero.Fields
                    .Select(f => FieldResDto.Map(f))
                    .ToList(),
                Periods = mero.TimePeriods
                    .Select(p => TimePeriodsResDto.Map(p)).ToList(),
            };
        }
    }
}
