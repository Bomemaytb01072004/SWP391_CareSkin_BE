namespace SWP391_CareSkin_BE.DTOS.Responses.Routine
{
    public class RoutineStepDTO
    {
        public int RoutineStepId { get; set; }
        public int RoutineId { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; }
        public string Description { get; set; }

        // Nếu một bước có thể chứa nhiều sản phẩm, sử dụng List để chứa chúng
        public List<RoutineProductDTO> RoutineProducts { get; set; } = new List<RoutineProductDTO>();
    }
}
