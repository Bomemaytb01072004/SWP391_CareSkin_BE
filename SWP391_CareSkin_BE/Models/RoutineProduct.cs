using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RoutineProduct")]
    public class RoutineProduct
    {
        [Key]
        public int RoutineProductId { get; set; }

        [ForeignKey("Routine")]
        public int RoutineId { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        
        [ForeignKey("RoutineStep")]
        public int? RoutineStepId { get; set; }

        public virtual Product Product { get; set; }
        public virtual RoutineStep RoutineStep { get; set; }
    }
}
