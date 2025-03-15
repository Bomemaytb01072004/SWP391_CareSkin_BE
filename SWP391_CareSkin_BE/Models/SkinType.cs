using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWP391_CareSkin_BE.Models
{
    [Table ("SkinType")]
    public class SkinType
    {
        public int SkinTypeId { get; set; }

        public string TypeName { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Result> Results { get; set; } = new List<Result>();

        public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();

        public virtual ICollection<ProductForSkinType> ProductForSkinTypes { get; set; } = new List<ProductForSkinType>();
    }
}
