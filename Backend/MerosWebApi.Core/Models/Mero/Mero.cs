namespace MerosWebApi.Core.Models.Mero
{
    public class Mero
    {
        public string Id { get; }

        public string UniqueInviteCode { get; }

        public string Name { get; }

        public string CreatorId { get; }

        public string CreatorEmail { get; }

        public string Description { get; }

        public List<TimePeriod> TimePeriods { get; }

        public List<Field> Fields { get; }

        public List<MeroFile> Files { get; }

        private Mero(string id, string uniqueInviteCode, string name, string creatorId, string creatorEmail,
            string description, List<TimePeriod> timePeriods, List<Field> fields,
            List<MeroFile> files)
        {
            Id = id;
            UniqueInviteCode = uniqueInviteCode;
            Name = name;
            CreatorId = creatorId;
            CreatorEmail = creatorEmail;
            Description = description;
            TimePeriods = timePeriods;
            Fields = fields;
            Files = files;
        }

        public static Mero CreateMero(string id, string uniqueInviteCode, string name, string creatorId, string creatorEmail,
            string description, List<TimePeriod> timePeriods, List<Field> fields,
            List<MeroFile> files)
        {
            return new Mero(id, uniqueInviteCode, name, creatorId, creatorEmail,
                description, timePeriods, fields, files);
        }
    }
}
