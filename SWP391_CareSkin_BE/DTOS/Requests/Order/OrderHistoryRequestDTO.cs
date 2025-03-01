using System;

namespace SWP391_CareSkin_BE.DTOs.Requests.Order
{
    public class OrderHistoryRequestDTO
    {
        public int? CustomerId { get; set; }
        public int? StatusId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
