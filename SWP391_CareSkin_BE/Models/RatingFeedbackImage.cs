using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RatingFeedbackImage")]
    public class RatingFeedbackImage
    {
        public int RatingFeedbackImageId { get; set; }

        public int RatingFeedbackId { get; set; }

        public string FeedbackImageUrl { get; set; }

        public virtual RatingFeedback RatingFeedback { get; set; }
    }
}
