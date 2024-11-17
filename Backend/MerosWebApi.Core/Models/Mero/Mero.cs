using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.Mero
{
    public class Mero
    {
        public string Id { get; }

        public string Name { get; }

        public string CreatorId { get; }

        public string CreatorEmail { get; }

        public string Description { get; }

        public List<TimePeriod> TimePeriods { get; }

        public List<Field> Fields { get; }

        public List<MeroFile> Files { get; }

        private Mero(string id, string name, string creatorId, string creatorEmail,
            string description, List<TimePeriod> timePeriods, List<Field> fields,
            List<MeroFile> files)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            CreatorEmail = creatorEmail;
            Description = description;
            TimePeriods = timePeriods;
            Fields = fields;
            Files = files;
        }

        public static Mero CreateMero(string id, string name, string creatorId, string creatorEmail, 
            string description, List<TimePeriod> timePeriods, List<Field> fields,
            List<MeroFile> files)
        {
            return new Mero(id, name, creatorId, creatorEmail, 
                description, timePeriods, fields, files);
        }

        public void AddQuestions(List<Field> fields)
        {
            foreach (var field in fields)
            {
                Fields.Add(field);
            }
        }

        public void AddTimePeriods(List<TimePeriod> periods)
        {
            foreach (var period in periods)
            {
                if (period != null)
                    TimePeriods.Add(period);
            }
        }

        public void AddFiles(MeroFile file)
        {

        }
    }
}
