namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseField
    {
        public string Title { get; set; }

        public string Type { get; set; }

        public List<string> PossibleAnswers { get; set; }
    }
}
