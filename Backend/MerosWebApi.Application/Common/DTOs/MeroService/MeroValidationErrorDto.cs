namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class MeroValidationErrorDto
    {
        public object ErorrObject { get; set; }

        public string Message { get; set; }

        public MeroValidationErrorDto(object erorrObject, string message)
        {
            ErorrObject = erorrObject;
            Message = message;
        }
    }
}
