using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Core.Models.PhormAnswer
{
    public class PhormAnswer
    {
        public string Id { get; }

        public string MeroId { get; }

        public string UserId { get; }

        public List<Answer> Answers { get; }

        public TimePeriod TimePeriod { get; }

        public DateTime CreatedTime { get; }

        private PhormAnswer(string id, string meroId, string userId, List<Answer> answers,
            TimePeriod timePeriod, DateTime createDateTime)
        {
            Id = id;
            MeroId = meroId;
            UserId = userId;
            Answers = answers;
            TimePeriod = timePeriod;
            CreatedTime = createDateTime;
        }

        public static PhormAnswer Create(string id, string meroId, string userId, List<Answer> answers,
            TimePeriod timePeriod, DateTime createDateTime)
        {
            return new PhormAnswer(id, meroId, userId, answers, timePeriod, createDateTime);
        }
    }
}
