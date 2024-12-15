using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.PhormAnswer;

namespace MerosWebApi.Core.Repository
{
    public interface IMeroRepository
    {
        public Task<Mero> GetMeroByIdAsync(string meroId);

        public Task<Mero> GetMeroByInviteCodeAsync(string uniqueInviteCode);

        public Task AddMeroAsync(Mero mero);

        public Task AddTimePeriodAsync(TimePeriod period);

        public Task<List<TimePeriod>> GetTimePeriodsAsync(IEnumerable<string> ids);

        public Task<QuerryStatus> DeleteMeroByIdAsync(string meroId);

        public Task<bool> AddMeroPhormAnswerAsync(PhormAnswer phormAnswer);

        public Task<PhormAnswer> GetMeroPhormAnswerByIdAsync(string phormId);

        public Task<List<PhormAnswer>> GetListMeroPhormAnswersByMeroAsync(int startIndex, int count, string meroId);

        public Task<List<Mero>> GetListMerosWhereCreator(int startIndex, int count, string creatorId);

        public Task<List<Mero>> GetListMerosWhereUser(int startIndex, int count, string userId);
    }
}
