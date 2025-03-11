using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class SkinTypeCreateRequestDTO
    {
        [Required(ErrorMessage = "TypeName is required")]
        [StringLength(100, ErrorMessage = "TypeName cannot exceed 100 characters")]
        public string TypeName { get; set; }

        [Required(ErrorMessage = "MinScore is required")]
        [Range(0, int.MaxValue, ErrorMessage = "MinScore must be a non-negative number")]
        public int MinScore { get; set; }

        [Required(ErrorMessage = "MaxScore is required")]
        [Range(0, int.MaxValue, ErrorMessage = "MaxScore must be a non-negative number")]
        public int MaxScore { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
}
