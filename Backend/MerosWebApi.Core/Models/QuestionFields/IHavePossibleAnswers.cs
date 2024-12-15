namespace MerosWebApi.Core.Models.Questions
{
    public interface IHavePossibleAnswers
    {
        void AddPossibleAnswer(string answer);

        void RemovePossibleAnswer(string answer);
    }
}
