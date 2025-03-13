using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.DTOS.Responses.Routine
{
    public class ProductForRoutineDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string BrandName { get; set; }
        public string PictureUrl { get; set; }
        public double AverageRating { get; set; }
        public List<ProductVariationDTO> Variations { get; set; }
    }
}
