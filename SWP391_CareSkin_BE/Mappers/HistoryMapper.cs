using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public class HistoryMapper
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

        public static History ToEntity(HistoryDTO dto)
        {
            return new History
            {
                HistoryId = dto.HistoryId,
                CustomerId = dto.CustomerId,
                QuestionId = dto.QuestionId,
                AnswerId = dto.AnswerId
            };
        }
    }
}
