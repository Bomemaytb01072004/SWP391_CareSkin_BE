using System;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOS.RatingFeedback
{
    public class RatingFeedbackDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAvatar { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Rating { get; set; }
        public string FeedBack { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsVisible { get; set; }
        public List<RatingFeedbackImageDTO> Images { get; set; }
    }
}
