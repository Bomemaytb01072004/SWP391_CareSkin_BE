using SWP391_CareSkin_BE.DTOS.Requests;
using SWP391_CareSkin_BE.DTOS.Responses;

namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ISkinTypeService
    {
        Task<List<SkinTypeDTO>> GetAllAsync();
        Task<List<SkinTypeDTO>> GetActiveSkinTypesAsync();
        Task<List<SkinTypeDTO>> GetInactiveSkinTypesAsync();
        Task<SkinTypeDTO> GetByIdAsync(int id);
        Task<SkinTypeDTO> CreateAsync(SkinTypeCreateRequestDTO request);
        Task<SkinTypeDTO> UpdateAsync(int id, SkinTypeUpdateRequestDTO request);
        Task DeleteAsync(int id);
    }
}
