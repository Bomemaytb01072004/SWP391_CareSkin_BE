﻿namespace SWP391_CareSkin_BE.DTOs.Requests.Quiz
{
    public class UpdateQuizRequestDTO
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
