using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Supports")]
    public class Supports
    {
        [Key]
        public int SuppportId { get; set; }

        public int CustomerId { get; set; }

        public int StaffId { get; set; }

        public virtual Staffs Staff { get; set; }

        public virtual Customers Customers { get; set; }
    }
}
