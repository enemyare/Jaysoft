using MerosWebApi.Core.Models;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
{
    public class FieldResDto
    {
        public string Title { get; set; }

        public string Type { get; set; }

        public List<string>? Answers { get; set; }

        public static FieldResDto Map(Field field)
        {
            return new FieldResDto
            {
                Title = field.Title,
                Type = field.Type,
                Answers = field.Answers,
            };
        }
    }
}
