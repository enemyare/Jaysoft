using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.User
{
    public class UserStatistic
    {
        public int CreatedMerosCount { get; set; }
        public int ParticipantsCount { get; set; }
        public int UserRegistredMerosCount { get; set; }

        public UserStatistic()
        {
            CreatedMerosCount = 0;
            ParticipantsCount = 0;
            UserRegistredMerosCount = 0;
        }
    }
}
