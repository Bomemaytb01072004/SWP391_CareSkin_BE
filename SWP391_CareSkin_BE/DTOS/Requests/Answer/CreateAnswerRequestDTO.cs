namespace SWP391_CareSkin_BE.DTOs.Requests.Answer
{
    public class CreateAnswerRequestDTO
    {
        public int QuestionId { get; set; }
        public string AnswersContext { get; set; }
        public int PointForSkinType { get; set; }
    }
}
