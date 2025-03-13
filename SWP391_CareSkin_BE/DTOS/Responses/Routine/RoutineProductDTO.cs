namespace SWP391_CareSkin_BE.DTOS.Responses.Routine
{
    public class RoutineProductDTO
    {
        public int RoutineProductId { get; set; }
        public int RoutineStepId { get; set; }
        public int ProductId { get; set; }

        // Chỉ hiển thị thông tin sản phẩm cần thiết cho routine
        public ProductForRoutineDTO Product { get; set; }
    }
}
