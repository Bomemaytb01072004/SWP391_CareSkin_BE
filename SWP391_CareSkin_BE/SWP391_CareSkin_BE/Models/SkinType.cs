using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table ("SkinType")]
    public class SkinType
    {
       public int SkinTypeId { get; set; }

       public string TypeName { get; set; }

       public string Description { get; set; }

        public virtual ICollection<Result> Results { get; set; } = new List<Result>();

        public virtual ICollection<SkinCareRoutine> SkinCareRoutines { get; set; } = new List<SkinCareRoutine>();

    }
}
