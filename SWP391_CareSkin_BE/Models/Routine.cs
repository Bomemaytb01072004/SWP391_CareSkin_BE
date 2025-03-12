using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("Routine")]
    public class Routine
    {
        [Key]
        public int RoutineId { get; set; }

        public string RoutineName { get; set; }

        public string RoutinePeriod { get; set; }

        public string Description { get; set; }

        [ForeignKey("SkinType")]
        public int SkinTypeId { get; set; }

        public virtual SkinType SkinType { get; set; }
        public virtual ICollection<RoutineStep> RoutineSteps { get; set; } = new List<RoutineStep>();
    }
}
