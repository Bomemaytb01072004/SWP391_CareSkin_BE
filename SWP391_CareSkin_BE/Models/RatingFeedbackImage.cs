using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RatingFeedbackImage")]
    public class RatingFeedbackImage
    {
        public int Id { get; set; }

        public int RatingFeedbackId { get; set; }

        public string ImageUrl { get; set; }

        public virtual RatingFeedback RatingFeedback { get; set; }
    }
}
