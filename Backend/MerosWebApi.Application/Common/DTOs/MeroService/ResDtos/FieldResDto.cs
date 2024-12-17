using MerosWebApi.Core.Models;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
{
    public class FieldResDto
    {
        public string Label { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }

        public List<string>? Answers { get; set; }

        public static FieldResDto Map(Field field)
        {
            return new FieldResDto
            {
                Label = field.Text,
                Type = field.Type,
                Required = field.Required,
                Answers = field.Answers,
            };
        }
    }
}
