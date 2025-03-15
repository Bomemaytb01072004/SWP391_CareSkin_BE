namespace SWP391_CareSkin_BE.DTOS.Responses
{
    public class SkinTypeDTO
    {
        public int SkinTypeId { get; set; }
        public string TypeName { get; set; }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
