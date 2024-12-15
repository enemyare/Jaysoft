using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Persistence.Entites;

namespace MerosWebApi.Persistence.Helpers
{
    public class TimePeriodPropertyAssigner
        : IPropertyAssigner<TimePeriod, DatabaseTimePeriod>,
            IPropertyAssigner<DatabaseTimePeriod, TimePeriod>,
            IPropertyValuesAssigner<DatabaseTimePeriod, TimePeriod>
    {
        public static DatabaseTimePeriod MapFrom(TimePeriod source)
        {
            return new DatabaseTimePeriod
            {
                Id = source.Id,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                BookedPlaces = source.BookedPlaces,
                TotalPlaces = source.TotalPlaces
            };
        }

        public static TimePeriod MapFrom(DatabaseTimePeriod source)
        {
            return TimePeriod.CreateTimePeriod(source.Id, source.StartTime, source.EndTime,
                source.TotalPlaces, source.BookedPlaces);
        }

        public static void AssignPropertyValues(DatabaseTimePeriod to, TimePeriod from)
        {
            to.Id = from.Id;
            to.StartTime = from.StartTime;
            to.EndTime = from.EndTime;
            to.TotalPlaces = from.TotalPlaces;
            to.BookedPlaces = from.BookedPlaces;
        }
    }
}
