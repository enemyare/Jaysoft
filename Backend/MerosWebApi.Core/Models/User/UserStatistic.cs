namespace MerosWebApi.Core.Models.User
{
    public class UserStatistic
    {
        public int CreatedMerosCount { get; set; }
        public int ParticipantsCount { get; set; }
        public int UserRegistredMerosCount { get; set; }

        public UserStatistic() : this(0, 0, 0)
        {
        }

        public UserStatistic(int createdMeros, int participantsCount, int userRegistred)
        {
            CreatedMerosCount = createdMeros;
            ParticipantsCount = participantsCount;
            UserRegistredMerosCount = userRegistred;
        }
    }
}
