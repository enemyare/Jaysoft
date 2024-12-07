using MerosWebApi.Core.Models.Mero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class MyMeroResDto
    {
        public string Id { get; set; }

        public string MeetName { get; set; }

        public string Description { get; set; }

        public List<TimePeriodBookedResDto> Periods { get; set; }

        public static MyMeroResDto Map(Mero mero)
        {
            return new MyMeroResDto
            {
                Id = mero.Id,
                MeetName = mero.Name,
                Description = mero.Description,
                Periods = mero.TimePeriods
                    .Select(p => TimePeriodBookedResDto.Map(p)).ToList(),
            };
        }
    }
}
