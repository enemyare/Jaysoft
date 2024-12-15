using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.MeroService.ResDtos;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Common.Exceptions.Common;
using MerosWebApi.Application.Common.Exceptions.MeroLogicExceptions;
using MerosWebApi.Application.Common.SecurityHelpers.Generators;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Exceptions;
using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.PhormAnswer;
using MerosWebApi.Core.Models.QuestionFields;
using MerosWebApi.Core.Repository;
using MongoDB.Bson;

namespace MerosWebApi.Application.Services
{
    public class MeroService : IMeroService
    {
        const int INVITE_CODE_LENGTH = 8;

        private readonly IMeroRepository _repository;
        public MeroService(IMeroRepository repository)
        {
            _repository = repository;
        }

        public async Task<MeroResDto> GetMeroByIdAsync(string id)
        {
            var mero = await _repository.GetMeroByIdAsync(id);

            if (mero == null)
                throw new EntityNotFoundException("Мероприятие не было найдено");

            return MeroResDto.Map(mero);
        }

        public async Task<MeroResDto> GetMeroByInviteCodeAsync(string inviteCode)
        {
            var mero = await _repository.GetMeroByInviteCodeAsync(inviteCode);

            if (mero == null)
                throw new EntityNotFoundException("Мероприятие не было найдено");

            return MeroResDto.Map(mero);
        }

        public async Task<MeroResDto> CreateNewMeroAsync(string creatorId, MeroReqDto createReqDto)
        {
            var meroId = ObjectId.GenerateNewId().ToString();

            var timePeriods = CreateMeroTimePeriods(createReqDto);

            var fields = CreateMeroFields(createReqDto);

            var uniqInviteCode = await CreateUniqueInviteCode();

            var mero = Mero.CreateMero(meroId, uniqInviteCode, createReqDto.MeetName, creatorId, createReqDto.CreatorEmail,
                createReqDto.Description, timePeriods, fields, null);

            await _repository.AddMeroAsync(mero);

            return MeroResDto.Map(mero);
        }

        public async Task<QuerryStatus> DelereMeroByIdAsync(string userId, string meroId)
        {
            var mero = await _repository.GetMeroByIdAsync(meroId);
            if (mero == null)
                throw new EntityNotFoundException("Мероприятие не было найдено");

            if (mero.CreatorId != userId)
                throw new ForbiddenException("Доступ для удаления запрещен.");

            return await _repository.DeleteMeroByIdAsync(meroId);
        }

        public async Task<MeroResDto> FullMeroUpdateAsync(string userId, string meroId, MeroReqDto updateMeroData)
        {
            var meroInDb = await _repository.GetMeroByIdAsync(meroId);
            if (meroInDb == null)
                throw new EntityNotFoundException("Мероприятие не было найдено");

            if (meroInDb.CreatorId != userId)
                throw new ForbiddenException("Доступ для обновления запрещен.");

            if (meroInDb.TimePeriods.Any(p => p.BookedPlaces > 0))
                throw new NotPossibleUpdateException("Не возможно обновить на мероприятие уже есть записавшиеся");

            var timePeriods = CreateMeroTimePeriods(updateMeroData);

            var fields = CreateMeroFields(updateMeroData);

            var mero = Mero.CreateMero(meroId, meroInDb.UniqueInviteCode, updateMeroData.MeetName, userId,
                updateMeroData.CreatorEmail, updateMeroData.Description, timePeriods, fields, null);

            var querryStatus = await _repository.DeleteMeroByIdAsync(meroId);
            if (!querryStatus.IsSuccess)
                throw new NotPossibleUpdateException($"Не возможно обновить - {querryStatus.Message}");


            await _repository.AddMeroAsync(mero);

            return MeroResDto.Map(mero);
        }

        public async Task<PhormAnswerResDto> CreateNewPhormAnswerAsync(string userId, PhormAnswerReqDto phormAnswerReqDto)
        {
            var phormMero = await _repository.GetMeroByIdAsync(phormAnswerReqDto.MeroId);

            if (phormMero == null)
                throw new EntityNotFoundException("Соответсвующее мероприятие не было найдено");
            if (phormMero.Fields.Count != phormAnswerReqDto.Answers.Count)
                throw new PhormAnswerFieldException(
                    "Число полей в форме ответа должно быть равно числу полей в анкете мероприятия");

            var phormMeroAnswers = new List<Answer>();

            for (int i = 0; i < phormMero.Fields.Count; i++)
            {
                var field = phormMero.Fields[i];
                var phormMeroFieldText = field.Text;
                var phormAnswerFieldText = phormAnswerReqDto.Answers[i].QuestionText;

                if (phormMeroFieldText != phormAnswerFieldText)
                    throw new PhormAnswerFieldException(
                        "Строка вопроса в анкете мероприя должна быть равна строке в форме ответа");

                var fieldAnswers = phormAnswerReqDto.Answers[i].QuestionAnswers.ToArray();

                var validateAnswers = field.SelectAnswer(fieldAnswers);

                phormMeroAnswers.Add(new Answer(phormMeroFieldText, validateAnswers));
            }

            var timePeriod = await _repository.GetTimePeriodsAsync(
                new List<string>() { phormAnswerReqDto.TimePeriodId });

            if (timePeriod == null || timePeriod.Count != 1)
                throw new EntityNotFoundException($"Соответсвующее время записи {phormAnswerReqDto.TimePeriodId} не найдено");
            if (timePeriod[0].BookedPlaces >= timePeriod[0].TotalPlaces)
                throw new TimePeriodBusyException("Все места на данное время заняты");

            var phormAnswer = PhormAnswer.Create(ObjectId.GenerateNewId().ToString(), phormMero.Id, userId, phormMeroAnswers,
                timePeriod[0], DateTime.Now);

            var createIsCorrect = await _repository.AddMeroPhormAnswerAsync(phormAnswer);

            if (!createIsCorrect)
                throw new NotCreatedException("Анкета не была создана, повторите немного позже");

            return PhormAnswerResDto.Map(phormAnswer);
        }

        public async Task<PhormAnswerResDto> GetMeroPhormAnswerByIdAsync(string phormId)
        {
            var phormAnswer = await _repository.GetMeroPhormAnswerByIdAsync(phormId);

            if (phormAnswer == null)
                throw new PhormAnswerNotFoundException("Форма ответа не найдена");

            return PhormAnswerResDto.Map(phormAnswer);
        }

        public async Task<List<ShowWritenPhromResDto>> GetMeroPhormsListByMeroAsync(int startIndex, int count, string meroId)
        {
            if (startIndex < 0 || count < 1)
                throw new AppException("Переданные значения запроса неверны - startIndex должен быть >= 0 && count > 0");

            var phormAnswers = await _repository.GetListMeroPhormAnswersByMeroAsync(startIndex, count, meroId);

            return phormAnswers
                .Select(p => ShowWritenPhromResDto.Map(p))
                .ToList(); ;
        }

        public async Task<List<MyCreatedMerosResDto>> GetListMyMeroListForCreator(int startIndex, int count,
            string creatorId)
        {
            if (startIndex < 0 || count < 1)
                throw new AppException("Переданные значения запроса неверны - startIndex должен быть >= 0 && count > 0");

            var meros = await _repository.GetListMerosWhereCreator(startIndex, count, creatorId);

            return meros.Select(m => MyCreatedMerosResDto.Map(m)).ToList();
        }

        public async Task<List<MyRegistredMerosResDto>> GetListMyMeroListForUser(int startIndex, int count,
            string userId)
        {
            if (startIndex < 0 || count < 1)
                throw new AppException("Переданные значения запроса неверны - startIndex должен быть >= 0 && count > 0");

            var meros = await _repository.GetListMerosWhereUser(startIndex, count, userId);

            return meros.Select(m => MyRegistredMerosResDto.Map(m)).ToList();
        }

        #region Helpers

        private List<TimePeriod> CreateMeroTimePeriods(MeroReqDto createReqDto)
        {
            var timePeriods = new List<TimePeriod>();

            foreach (var periodDto in createReqDto.Periods)
            {
                try
                {
                    var periodId = ObjectId.GenerateNewId().ToString();

                    var timePeriod = TimePeriod.CreateTimePeriod(periodId, periodDto.StartTime, periodDto.EndTime,
                        periodDto.TotalPlaces, 0);

                    _repository.AddTimePeriodAsync(timePeriod);
                    timePeriods.Add(timePeriod);
                }
                catch (NotValidTimePeriodException ex)
                {
                    throw new MeroTimeException(periodDto, ex.Message);
                }
            }

            return timePeriods;
        }

        private List<Field> CreateMeroFields(MeroReqDto createReqDto)
        {
            var fields = new List<Field>();

            foreach (var fieldReqDto in createReqDto.Fields)
            {
                try
                {
                    var field = FieldFactoryMethod.CreateField(fieldReqDto.Text, fieldReqDto.Type, fieldReqDto.Required,
                        fieldReqDto.Answers);

                    fields.Add(field);
                }
                catch (FieldTypeException ex)
                {
                    throw new MeroFieldException(fieldReqDto, ex.Message);
                }
                catch (MerosWebApi.Core.Models.Exceptions.FieldException ex)
                {
                    throw new MeroFieldException(fieldReqDto, ex.Message);
                }
            }

            return fields;
        }

        private async Task<string> CreateUniqueInviteCode()
        {
            var inviteCode = RandomStringGenerator.GenerateRandomString(INVITE_CODE_LENGTH);

            while (await _repository.GetMeroByInviteCodeAsync(inviteCode) != null)
            {
                inviteCode = RandomStringGenerator.GenerateRandomString(INVITE_CODE_LENGTH);
            }

            return inviteCode;
        }

        #endregion
    }
}
