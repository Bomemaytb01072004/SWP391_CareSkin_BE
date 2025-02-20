using Supabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly Supabase.Client _supabaseClient;
        private const string BucketName = "CareSkin"; // Thay bằng tên bucket của bạn

        public SupabaseService()
        {
            // Thay bằng URL & Key của dự án Supabase của bạn
            var url = "https://gnofmcrvrfybjtlrbreb.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imdub2ZtY3J2cmZ5Ymp0bHJicmViIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDAwMzUxNTYsImV4cCI6MjA1NTYxMTE1Nn0.zJB_-Xy5EjL4yKyz51llx9OgUPlLC7DVlyGBDC_toq0";

            // Sử dụng gói monolithic (ví dụ Supabase 1.1.1 hay các phiên bản tương thích)
            _supabaseClient = new Supabase.Client(url, key);

            // Nếu phiên bản của bạn yêu cầu InitializeAsync, có thể gọi ở đây.
            // _supabaseClient.InitializeAsync().Wait();
        }

        /// <summary>
        /// Upload file lên Supabase Storage, trả về public URL.
        /// (Phiên bản này chuyển Stream → byte[] trước khi gọi Upload(byte[] data, string path).)
        /// </summary>
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName, string userId)
        {
            // Chuyển Stream thành byte[]
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            // Tạo metadata chứa owner
            var fileOptions = new Supabase.Storage.FileOptions
            {
                CacheControl = "3600",
                Upsert = true,
                ContentType = "image/jpeg" // Add appropriate content type
            };

            // Upload file lên Supabase kèm metadata
            await _supabaseClient
                .Storage
                .From(BucketName)
                .Upload(fileBytes, fileName, fileOptions);

            // Lấy public URL của file vừa upload
            var publicUrl = _supabaseClient
                .Storage
                .From(BucketName)
                .GetPublicUrl(fileName);

            return publicUrl;
        }

        /// <summary>
        /// Xóa file khỏi Supabase Storage.
        /// Ở phiên bản này, hàm Remove nhận vào List<string> paths.
        /// </summary>
        public async Task<bool> DeleteImageAsync(string fileName)
        {
            // Gọi hàm Remove với 1 List chứa fileName cần xóa
            var result = await _supabaseClient
                .Storage
                .From(BucketName)
                .Remove(new List<string> { fileName });

            // Nếu kết quả không null và có phần tử, coi như xóa thành công
            return result != null && result.Any();
        }

        public string GetCurrentUserId()
        {
            var user = _supabaseClient.Auth.CurrentUser;
            return user?.Id.ToString(); // Lấy ID của user nếu đã đăng nhập
        }
    }
}
