using MerosWebApi.Core.Models;
using MerosWebApi.Persistence.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Mero;

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
