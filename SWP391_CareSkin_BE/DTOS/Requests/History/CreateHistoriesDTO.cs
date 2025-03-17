using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOS.Requests.History
{
    public class CreateHistoriesDTO
    {
        public List<CreateHistoryDTO> Histories { get; set; } = new List<CreateHistoryDTO>();
    }
}
