using SWP391_CareSkin_BE.DTOs;
using SWP391_CareSkin_BE.DTOs.Requests.History;
using SWP391_CareSkin_BE.DTOs.Responses.History;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class HistoryMapper
    {
        public static History ToEntity(CreateHistoryRequestDTO dto)
        {
            return new History
            {
                CustomerId = dto.CustomerId,
                QuestionId = dto.QuestionId,
                AnswerId = dto.AnswerId
            };
        }

        public static History ToEntity(UpdateHistoryRequestDTO dto, History history)
        {
            history.AnswerId = dto.AnswerId;
            return history;
        }

        public static HistoryResponseDTO ToDTO(History history)
        {
            return new HistoryResponseDTO
            {
                HistoryId = history.HistoryId,
                CustomerId = history.CustomerId,
                QuestionId = history.QuestionId,
                AnswerId = history.AnswerId
            };
        }
    }


}
