using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RoutineProduct")]
    public class RoutineProduct
    {
        public int RoutineProductId { get; set; }

        public int RoutineId { get; set; }
        
        public int ProductId { get; set; }

        public virtual Routine Routine { get; set; }
        public virtual Product Product { get; set; }

    }
}
