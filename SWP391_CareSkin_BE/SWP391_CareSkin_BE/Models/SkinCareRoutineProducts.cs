using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("SkinCareRoutineProduct")]
    public class SkinCareRoutineProducts
    {
        [Key]
        public int SkinCareRoutineId { get; set; }
        
        [Key]
        public int ProductId { get; set; }

        public virtual SkinCareRoutines SkinCareRoutine { get; set; }
        public virtual Products Product { get; set; }

    }
}
