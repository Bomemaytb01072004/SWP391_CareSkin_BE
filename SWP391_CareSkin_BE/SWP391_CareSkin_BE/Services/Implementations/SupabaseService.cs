using Supabase;
using System;
using System.IO;
using System.Threading.Tasks;
using SWP391_CareSkin_BE.Services.Interfaces;

namespace SWP391_CareSkin_BE.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly Supabase.Client _supabaseClient;
        private const string BucketName = "product-images"; // Thay bằng tên bucket của bạn

        public SupabaseService()
        {
            // 1. Thông tin dự án Supabase
            var url = "https://YOUR_PROJECT_ID.supabase.co"; // Thay bằng URL dự án
            var key = "YOUR_ANON_KEY";                       // Thay bằng Anon Key

            // 2. Khởi tạo client (bản cũ có thể chỉ cần (url, key))
            _supabaseClient = new Supabase.Client(url, key);

            // 3. Thông thường, bản cũ không cần _supabaseClient.InitializeAsync()
            // Nếu code của bạn yêu cầu, có thể gọi ở đây: _supabaseClient.InitializeAsync().Wait();
        }

        /// <summary>
        /// Upload file lên Supabase Storage, trả về Public URL
        /// </summary>
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            // A) Chuyển Stream -> byte[]
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            // B) Gọi Upload(byte[] data, string path)
            // fileName là đường dẫn/ tên file lưu trên Supabase (vd: "folder/myImage.jpg")
            await _supabaseClient
                .Storage
                .From(BucketName)
                .Upload(fileBytes, fileName);

            // C) Lấy public URL của file vừa upload
            var publicUrl = _supabaseClient
                .Storage
                .From(BucketName)
                .GetPublicUrl(fileName);

            return publicUrl;
        }

        /// <summary>
        /// Xóa file khỏi Supabase Storage
        /// </summary>
        public async Task<bool> DeleteImageAsync(string fileName)
        {
            // Dùng Remove(...) để xóa file
            var removeResult = await _supabaseClient
                .Storage
                .From(BucketName)
                .Remove(new string[] { fileName });

            // Nếu mảng trả về có phần tử => xóa thành công
            return removeResult.Length > 0;
        }
    }
}
