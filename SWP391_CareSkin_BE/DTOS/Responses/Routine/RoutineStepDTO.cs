using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.DTOS.Responses.Routine
{
    public class RoutineStepDTO
    {
        public int RoutineStepId { get; set; }
        public int RoutineId { get; set; }
        public int RoutineProductId { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; }
        public string Description { get; set; }
        
        // Include product information for display
        public string RoutineName { get; set; }
        public ProductDTO Product { get; set; }
        public List<RoutineProductDTO>? RoutineProducts { get; internal set; }
    }
}
