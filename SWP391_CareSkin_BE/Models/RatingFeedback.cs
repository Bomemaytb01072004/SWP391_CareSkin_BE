using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RatingFeedback")]
    public class RatingFeedback
    {
        public int RatingFeedbackId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string FeedBack { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Product Product { get; set; }
        
        public virtual ICollection<RatingFeedbackImage> RatingFeedbackImages { get; set; }
    }
}
