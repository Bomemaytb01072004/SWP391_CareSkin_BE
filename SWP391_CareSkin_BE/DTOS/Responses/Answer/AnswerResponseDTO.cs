namespace SWP391_CareSkin_BE.DTOs.Responses.Answer
{
    public class AnswerResponseDTO
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string AnswersContext { get; set; }
        public int PointForSkinType { get; set; }
    }
}
