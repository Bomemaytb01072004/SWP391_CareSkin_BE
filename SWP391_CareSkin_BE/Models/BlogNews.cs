using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("BlogNews")]
    public class BlogNews
    {
        [Key]
        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string? PictureUrl { get; set; }

        public DateTime UploadDate { get; set; }

        public bool IsActive { get; set; }

        public int? AdminId { get; set; }
        [ForeignKey("AdminId")]
        public virtual Admin Admin { get; set; }

        public int? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }
}