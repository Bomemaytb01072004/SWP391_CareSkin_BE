using SWP391_CareSkin_BE.DTOS.Responses.Answer;

namespace SWP391_CareSkin_BE.DTOS.Responses.Question
{
    public class QuestionDTO
    {
        public int QuestionsId { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerDTO>? Answers { get; set; }
    }
}
