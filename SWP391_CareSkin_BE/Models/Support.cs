using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Support")]
    public class Support
    {
        public int SuppportId { get; set; }

        public int CustomerId { get; set; }

        public int StaffId { get; set; }

        public string Context { get; set; }

        public virtual Staff Staff { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
