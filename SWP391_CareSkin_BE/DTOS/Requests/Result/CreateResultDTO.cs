namespace SWP391_CareSkin_BE.DTOS.Requests.Result
{
    public class CreateResultDTO
    {
        public int CustomerId { get; set; }
        public int QuizId { get; set; }
        public int UserQuizAttemptId { get; set; }
    }
}
