
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.Data
.Data
{
    [Table("HangHoa")]
    public class HangHoa
    {
       
        
            [Key]
            public Guid MaHh { get; set; } 
            [MaxLength(100)]
            public string TenHh { get; set; }

            public string MoTa { get; set; }

            [Range(0, double.MaxValue)]
            public double DonGia { get; set; }

            public byte GiamGia { get; set; }
        
    }
}
