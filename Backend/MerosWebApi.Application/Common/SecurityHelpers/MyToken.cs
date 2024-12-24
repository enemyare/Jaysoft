namespace MerosWebApi.Application.Common.SecurityHelpers
{
    public class MyToken
    {
        public required string Token { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Expires { get; set; }
    }
}
