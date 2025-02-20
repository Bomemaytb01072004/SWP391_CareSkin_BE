namespace SWP391_CareSkin_BE.Services.Interfaces
{
    public interface ISupabaseService
    {
        /// <summary>
        /// Upload file lên Supabase Storage và trả về URL của file.
        /// </summary>
        Task<string> UploadImageAsync(Stream fileStream, string fileName);

        /// <summary>
        /// Xóa file khỏi Supabase Storage theo tên file.
        /// </summary>
        Task<bool> DeleteImageAsync(string fileName);
    }
}
