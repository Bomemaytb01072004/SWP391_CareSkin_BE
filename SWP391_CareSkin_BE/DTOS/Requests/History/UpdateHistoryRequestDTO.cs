namespace SWP391_CareSkin_BE.DTOs.Requests.History
{
    public class UpdateHistoryRequestDTO
    {
        public int CustomerId { get; set; } 
        public int QuestionId { get; set; }
        public int HistoryId { get; set; }
        public int AnswerId { get; set; }
    }
}
