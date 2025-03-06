namespace SWP391_CareSkin_BE.DTOs.Responses.BlogNews
{
    public class BlogNewsDTO
    {
        public int CustomerId { get; set; }

        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PictureUrl { get; set; }
    }
}
