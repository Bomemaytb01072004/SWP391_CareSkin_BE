using SWP391_CareSkin_BE.DTOS.Responses;
using SWP391_CareSkin_BE.DTOS.Responses.UserQuizAttempt;

namespace SWP391_CareSkin_BE.DTOS.Responses.Result
{
    public class ResultDTO
    {
        public int ResultId { get; set; }
        public int CustomerId { get; set; }
        public int QuizId { get; set; }
        public int UserQuizAttemptId { get; set; }
        public int SkinTypeId { get; set; }
        public int TotalScore { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime LastQuizTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public SkinTypeDTO SkinType { get; set; }
        public UserQuizAttemptDTO UserQuizAttempt { get; set; }
    }
}
