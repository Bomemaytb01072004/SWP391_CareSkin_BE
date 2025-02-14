using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table ("SkinType")]
    public class SkinTypes
    {
        [Key]
        public int Id { get; set; }

       public string TypeName { get; set; }

       public string Description { get; set; }

        public virtual ICollection<Results> Results { get; set; } = new List<Results>();

        public virtual ICollection<SkinCareRoutines> SkinCareRoutines { get; set; } = new List<SkinCareRoutines>();

    }
}
