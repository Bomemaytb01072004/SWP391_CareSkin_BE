using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table ("SkinType")]
    public class SkinType
    {
        [Key]
        public int Id { get; set; }

       public string Type_Name { get; set; }

       public string Description { get; set; }

        public virtual ICollection<Results> Results { get; set; } = new List<Results>();

        public virtual ICollection<SkinCareRoutine> SkinCareRoutines { get; set; } = new List<SkinCareRoutine>();

    }
}
