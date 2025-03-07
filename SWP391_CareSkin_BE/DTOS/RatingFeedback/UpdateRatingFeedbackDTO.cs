using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.RatingFeedback
{
    public class UpdateRatingFeedbackDTO
    {
        // Optional customerId for testing with Swagger
        public int? CustomerId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        public string FeedBack { get; set; }

        public List<IFormFile> NewImages { get; set; }
        
        public List<int> ImagesToDelete { get; set; }
    }
}
