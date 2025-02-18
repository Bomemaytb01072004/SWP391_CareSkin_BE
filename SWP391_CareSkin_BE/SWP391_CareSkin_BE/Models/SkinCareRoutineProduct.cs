using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("SkinCareRoutineProduct")]
    public class SkinCareRoutineProduct
    {
        public int SkinCareRoutineId { get; set; }
        
        public int ProductId { get; set; }

        public virtual SkinCareRoutine SkinCareRoutine { get; set; }
        public virtual Product Product { get; set; }

    }
}
