using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Supports")]
    public class Supports
    {
        public int SuppportId { get; set; }

        public int CustomerId { get; set; }

        public int StaffId { get; set; }
    }
}
