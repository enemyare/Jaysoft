namespace MerosWebApi.Core.Repository
{
    public class QuerryStatus
    {
        public bool IsSuccess { get; set; }

        public bool NotFound { get; set; }

        public string Message { get; set; }

        public QuerryStatus(bool success, bool notFound, string message)
        {
            IsSuccess = success;
            NotFound = notFound;
            Message = message;
        }
    }
}
