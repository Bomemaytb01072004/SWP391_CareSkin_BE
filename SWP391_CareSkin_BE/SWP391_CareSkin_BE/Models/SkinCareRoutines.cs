using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table("SkinCareRoutine")]
    public class SkinCareRoutines
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int SkinTypeId { get; set; }

        public virtual SkinTypes SkinType { get; set; }
        public virtual ICollection<SkinCareRoutineProducts> SkinCareRoutineProducts { get; set; } = new List<SkinCareRoutineProducts>();

    }
}
