namespace MerosWebApi.Application.Common.DTOs.MeroService.ReqDtos
{
    public class AnswerReqDto
    {
        public string QuestionTitle { get; set; }

        public List<string> QuestionAnswers { get; set; }
    }
}
