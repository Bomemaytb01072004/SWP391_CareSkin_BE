using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class HistoryMapper
    {
        public static HistoryDTO ToDTO(History history)
        {
            return new HistoryDTO
            {
                HistoryId = history.HistoryId,
                CustomerId = history.CustomerId,
                QuestionId = history.QuestionId,
                AnswerId = history.AnswerId
            };
        }

        public static History ToEntity(HistoryDTO historyDTO)
        {
            return new History
            {
                HistoryId = historyDTO.HistoryId,
                CustomerId = historyDTO.CustomerId,
                QuestionId = historyDTO.QuestionId,
                AnswerId = historyDTO.AnswerId
            };
        }
    }

}
