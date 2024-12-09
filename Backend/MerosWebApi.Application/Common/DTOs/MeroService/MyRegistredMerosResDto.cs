using MerosWebApi.Core.Models.Mero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
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
