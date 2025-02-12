using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("SkinCareRoutineProduct")]
    public class SkinCareRoutineProduct
    {
        [Key]
        public int SkinCare_Routine_Id { get; set; }
        
        [Key]
        public int Product_Id { get; set; }

        public virtual SkinCareRoutine SkinCareRoutine { get; set; }
        public virtual Product Product { get; set; }

    }
}
