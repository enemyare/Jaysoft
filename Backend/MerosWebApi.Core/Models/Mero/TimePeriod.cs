using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.Mero
{
    public class TimePeriod
    {
        public string Id { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public int TotalPlaces { get; }

        public int BookedPlaces { get; private set; }

        public static TimePeriod CreateTimePeriod(string id, DateTime startTime, DateTime endTime,
            int totalPlaces, int bookedPlaces)
        {
            return new TimePeriod(id, startTime, endTime, totalPlaces, bookedPlaces);
        }

        private TimePeriod(string id, DateTime startTime, DateTime endTime,
            int totalPlaces, int bookedPlaces = 0)
        {
            if (startTime < DateTime.Now || endTime <= startTime)
                throw new NotValidTimePeriodException("Не правильный формат периода мероприятия");

            if (totalPlaces <= 0)
                throw new NotValidTimePeriodException("Число мест на период мероприятия должно быть больше нуля");
            
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            TotalPlaces = totalPlaces;
            BookedPlaces = bookedPlaces;
        }
    }
}
