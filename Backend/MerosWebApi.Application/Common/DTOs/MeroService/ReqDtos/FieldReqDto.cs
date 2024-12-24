namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class FieldReqDto
    {
        public string Label { get; set; }

        public string Type { get; set; }

        public bool Required { get; set; }

        public List<string>? Answers { get; set; }
    }
}
