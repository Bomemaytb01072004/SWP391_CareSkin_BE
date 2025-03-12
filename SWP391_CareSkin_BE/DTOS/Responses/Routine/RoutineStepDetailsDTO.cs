namespace SWP391_CareSkin_BE.DTOS.Responses.Routine
{
    public class RoutineStepDetailsDTO
    {
        public int RoutineStepId { get; set; }
        public int RoutineId { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; }
        public string Description { get; set; }
        public List<RoutineProductDTO> Products { get; set; }
    }
}
