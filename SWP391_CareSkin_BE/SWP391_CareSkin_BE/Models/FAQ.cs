using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("FAQ")]
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

    }
}
