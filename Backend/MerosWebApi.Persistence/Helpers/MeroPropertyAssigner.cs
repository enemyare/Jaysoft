using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Persistence.Entites;

namespace MerosWebApi.Persistence.Helpers
{
    public class MeroPropertyAssigner
            : IPropertyAssigner<Mero, DatabaseMero>,
            IPropertyValuesAssigner<DatabaseMero, Mero>
    {
        public static DatabaseMero MapFrom(Mero from)
        {
            return new DatabaseMero
            {
                Id = from.Id,
                UniqueInviteCode = from.UniqueInviteCode,
                CreatorEmail = from.CreatorEmail,
                CreatorId = from.CreatorId,
                Description = from.Description,
                Fields = from.Fields
                    .Select(f => FieldPropertyAssigner.MapFrom(f))
                    .ToList(),
                Files = from.Files,
                Name = from.Name,
                TimePeriods = from.TimePeriods
                    .Select(p => p.Id).ToList()
            };
        }

        public static void AssignPropertyValues(DatabaseMero to, Mero from)
        {
            to.Id = from.Id;
            to.UniqueInviteCode = from.UniqueInviteCode;
            to.CreatorEmail = from.CreatorEmail;
            to.CreatorId = from.CreatorId;
            to.Description = from.Description;
            to.Fields = from.Fields
                .Select(f => FieldPropertyAssigner.MapFrom(f))
                .ToList();
            to.Files = from.Files;
            to.Name = from.Name;
            to.TimePeriods = from.TimePeriods
                .Select(p => p.Id).ToList();
        }
    }
}
