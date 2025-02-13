using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RatingFeedbacks")]
    public class RatingFeedbacks
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string FeedBack { get; set; }

        public virtual Customers Customers { get; set; }

        public virtual Product Product { get; set; }
    }
}
