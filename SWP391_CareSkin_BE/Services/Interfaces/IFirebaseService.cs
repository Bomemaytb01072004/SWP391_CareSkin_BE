namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName);
        Task<bool> DeleteImageAsync(string fileName);
    }
}
