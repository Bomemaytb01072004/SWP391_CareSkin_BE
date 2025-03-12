using System.ComponentModel.DataAnnotations;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class SkinTypeUpdateRequestDTO
    {
        [Required(ErrorMessage = "TypeName is required")]
        [StringLength(100, ErrorMessage = "TypeName cannot exceed 100 characters")]
        public string TypeName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
}
