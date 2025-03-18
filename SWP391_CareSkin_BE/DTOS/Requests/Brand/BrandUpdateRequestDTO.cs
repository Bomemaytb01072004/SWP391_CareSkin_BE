    using Microsoft.AspNetCore.Http;

namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class BrandUpdateRequestDTO
    {
        public string Name { get; set; }
        public IFormFile? PictureFile { get; set; }
    }
}
