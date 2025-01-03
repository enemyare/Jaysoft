﻿using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.MeroService.ResDtos;
using MerosWebApi.Core.Repository;

namespace MerosWebApi.Application.Interfaces
{
    public interface IMeroService
    {
        public Task<MeroResDto> GetMeroByIdAsync(string id);

        public Task<MeroResDto> GetMeroByInviteCodeAsync(string inviteCode);

        public Task<MeroResDto> CreateNewMeroAsync(string creatorId, MeroReqDto createResDto);

        public Task<MeroResDto> FullMeroUpdateAsync(string userId, string meroId, MeroReqDto updateMeroData);

        public Task<QuerryStatus> DelereMeroByIdAsync(string userId, string meroId);

        public Task<PhormAnswerResDto> CreateNewPhormAnswerAsync(string userId, PhormAnswerReqDto phormAnswerReqDto);

        public Task<PhormAnswerResDto> GetMeroPhormAnswerByIdAsync(string phormId);

        public Task<List<ShowWritenPhromResDto>> GetMeroPhormsListByMeroAsync(int startIndex, int count, string meroId);

        public Task<List<MyCreatedMerosResDto>> GetListMyMeroListForCreator(int startIndex, int count, string creatorId);

        public Task<List<MyRegistredMerosResDto>> GetListMyMeroListForUser(int startIndex, int count, string userId);
    }
}
