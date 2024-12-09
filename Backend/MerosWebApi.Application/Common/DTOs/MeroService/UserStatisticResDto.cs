using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class UserStatisticResDto
    {
        public int CreatedMerosCount { get; set; }
        public int ParticipantsCount { get; set; }
        public int UserRegistredMerosCount { get; set; }
    }
}
