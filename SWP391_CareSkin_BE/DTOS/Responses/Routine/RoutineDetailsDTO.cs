using System.Collections.Generic;

namespace SWP391_CareSkin_BE.DTOS.Responses.Routine
{
    public class RoutineDetailsDTO
    {
        public int RoutineId { get; set; }
        public string RoutineName { get; set; }
        public string RoutinePeriod { get; set; }
        public string Description { get; set; }
        public int SkinTypeId { get; set; }
        public List<RoutineStepDTO> Steps { get; set; }
        public List<RoutineProductDTO> Products { get; set; }
    }
}
