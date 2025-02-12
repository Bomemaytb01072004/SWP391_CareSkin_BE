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


    }
}
