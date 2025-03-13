using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SWP391_CareSkin_BE.Models
{
    [Table("RoutineStep")]
    public class RoutineStep
    {
        [Key]
        public int RoutineStepId { get; set; }

        public int RoutineId { get; set; }

        public int StepOrder { get; set; }

        [Required]
        public string StepName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public virtual Routine Routine { get; set; }
        
        // Navigation property to RoutineProduct to get the product details
        public virtual ICollection<RoutineProduct> RoutineProducts { get; set; } = new List<RoutineProduct>();
    }
}
