using SWP391_CareSkin_BE.DTOS.Requests.UserQuizAttempt;
using SWP391_CareSkin_BE.DTOS.Responses.UserQuizAttempt;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Mappers
{
    public static class UserQuizAttemptMapper
    {
        public static UserQuizAttemptDTO ToDTO(UserQuizAttempt attempt, bool includeHistories = false)
        {
            var attemptDTO = new UserQuizAttemptDTO
            {
                UserQuizAttemptId = attempt.UserQuizAttemptId,
                CustomerId = attempt.CustomerId,
                QuizId = attempt.QuizId,
                AttemptDate = DateTime.Parse(attempt.AttemptDate.ToString()),
                AttemptNumber = attempt.AttemptNumber,
                IsCompleted = attempt.IsCompleted,
                CreatedAt = attempt.CreatedAt,
                CompletedAt = attempt.CompletedAt
            };

            if (includeHistories && attempt.Histories != null && attempt.Histories.Any())
            {
                attemptDTO.Histories = HistoryMapper.ToDTOList(attempt.Histories, true);
            }

            return attemptDTO;
        }

        public static List<UserQuizAttemptDTO> ToDTOList(IEnumerable<UserQuizAttempt> attempts, bool includeHistories = false)
        {
            return attempts.Select(a => ToDTO(a, includeHistories)).ToList();
        }

        public static UserQuizAttempt ToEntity(CreateUserQuizAttemptDTO createAttemptDTO, int attemptNumber)
        {
            return new UserQuizAttempt
            {
                CustomerId = createAttemptDTO.CustomerId,
                QuizId = createAttemptDTO.QuizId,
                AttemptDate = DateOnly.FromDateTime(DateTime.Now),
                AttemptNumber = attemptNumber,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
        }
        
        public static UserQuizAttempt CompleteAttempt(UserQuizAttempt attempt)
        {
            attempt.IsCompleted = true;
            attempt.CompletedAt = DateTime.Now;
            return attempt;
        }
    }
}
