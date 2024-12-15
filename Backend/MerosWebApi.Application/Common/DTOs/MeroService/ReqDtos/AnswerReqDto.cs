namespace MerosWebApi.Application.Common.DTOs.MeroService.ReqDtos
{
    public class AnswerReqDto
    {
        public string QuestionText { get; set; }

        public List<string> QuestionAnswers { get; set; }
    }
}
