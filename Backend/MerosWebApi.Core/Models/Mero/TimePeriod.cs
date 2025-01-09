using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.Mero
{
    public class TimePeriod
    {
        public string Id { get; }

        public DateTime StartTime { get; }

        public int TotalPlaces { get; }

        public int BookedPlaces { get; private set; }

        public static TimePeriod CreateTimePeriod(string id, DateTime startTime,
            int totalPlaces, int bookedPlaces)
        {
            return new TimePeriod(id, startTime, totalPlaces, bookedPlaces);
        }

        private TimePeriod(string id, DateTime startTime,
            int totalPlaces, int bookedPlaces = 0)
        {
            if (totalPlaces <= 0)
                throw new NotValidTimePeriodException("Число мест на период мероприятия должно быть больше нуля");

            Id = id;
            StartTime = startTime;
            TotalPlaces = totalPlaces;
            BookedPlaces = bookedPlaces;
        }
    }
}
