using SWP391_CareSkin_BE.DTOs.Responses.Question;
using SWP391_CareSkin_BE.DTOS.Requests.History;
using SWP391_CareSkin_BE.DTOS.Responses.Answer;
using SWP391_CareSkin_BE.DTOS.Responses.History;
using SWP391_CareSkin_BE.Mappers;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class HistoryMapper
    {
        public static HistoryDTO ToDTO(History history, bool includeDetails = false)
        {
            var historyDTO = new HistoryDTO
            {
                HistoryId = history.HistoryId,
                AttemmptId = history.AttemmptId,
                QuestionId = history.QuestionId,
                AnswerId = history.AnswerId,
                CreatedAt = history.CreatedAt,
                UpdatedAt = history.UpdatedAt
            };

            if (includeDetails)
            {
                if (history.Question != null)
                {
                    historyDTO.Question = new List<QuestionDTO> { QuestionMapper.ToDTO(history.Question) };
                }

                if (history.Answer != null)
                {
                    historyDTO.Answer = new List<AnswerDTO> { AnswerMapper.ToDTO(history.Answer) };
                }
            }

            return historyDTO;
        }

        public static List<HistoryDTO> ToDTOList(IEnumerable<History> histories, bool includeDetails = false)
        {
            return histories.Select(h => ToDTO(h, includeDetails)).ToList();
        }

        public static History ToEntity(int attemptId, CreateHistoryDTO createHistoryDTO)
        {
            return new History
            {
                AttemmptId = attemptId,
                QuestionId = createHistoryDTO.QuestionId,
                AnswerId = createHistoryDTO.AnswerId,
                CreatedAt = DateTime.Now
            };
        }

        public static History UpdateEntity(History history, CreateHistoryDTO updateHistoryDTO)
        {
            history.QuestionId = updateHistoryDTO.QuestionId;
            history.AnswerId = updateHistoryDTO.AnswerId;
            history.UpdatedAt = DateTime.Now;

            return history;
        }
    }
}
