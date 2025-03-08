namespace SWP391_CareSkin_BE.DTOs.Requests.Answer
{
    public class UpdateAnswerRequestDTO
    {
        public int AnswerId { get; set; }
        public string AnswersContext { get; set; }
        public int PointForSkinType { get; set; }
    }
}
