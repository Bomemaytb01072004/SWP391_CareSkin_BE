using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOs.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
