namespace SWP391_CareSkin_BE.DTOS.Requests
{
    public class ProductSearchRequestDTO
    {
        public string? Keyword { get; set; }
        public string? Category { get; set; }
        public int? BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinMl { get; set; }
        public int? MaxMl { get; set; }
        public string? SortBy { get; set; } // name, price_asc, price_desc
        public int? PageSize { get; set; } = 10;
        public int? PageNumber { get; set; } = 1;
    }
}
