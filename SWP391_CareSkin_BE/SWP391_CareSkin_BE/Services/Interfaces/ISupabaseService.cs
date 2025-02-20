namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ISupabaseService
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName, string userId);
        Task<bool> DeleteImageAsync(string fileName);
        string GetCurrentUserId();
    }
}
