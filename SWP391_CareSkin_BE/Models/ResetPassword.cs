using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.Models
{
    [Table("ResetPassword")]
    public class ResetPassword
    {
        
            [Key]
            public int Id { get; set; }

            [Required]
            public int CustomerId { get; set; }

            [Required]
            [MaxLength(6)]
            public string ResetPin { get; set; } 

            public string Token { get; set; } = Guid.NewGuid().ToString();

            [Required]
            public DateTime ExpiryTime { get; set; } 

            [ForeignKey("CustomerId")]
            public virtual Customer Customer { get; set; }
        }
    
}
