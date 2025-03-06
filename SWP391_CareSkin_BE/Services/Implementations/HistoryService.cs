using Microsoft.EntityFrameworkCore.Migrations;
using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly HistoryRepository _historyRepository;

        public HistoryService(HistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<History> SaveHistoryAsync(HistoryDTO dto)
        {
            var history = new History
            {
                CustomerId = dto.CustomerId,
                QuestionId = dto.QuestionId,
                AnswerId = dto.AnswerId
            };

            return await _historyRepository.AddAsync(history);
        }
    }
}