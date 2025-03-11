namespace SWP391_CareSkin_BE.DTOS.Responses.Answer
{
    public class AnswerDTO
    {   
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string AnswersText { get; set; }
        public int Score { get; set; }
    }
}
