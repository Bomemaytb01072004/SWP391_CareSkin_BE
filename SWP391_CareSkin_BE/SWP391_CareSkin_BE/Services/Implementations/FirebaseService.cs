using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using SWP391_CareSkin_BE.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SWP391_CareSkin_BE.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseService(IConfiguration configuration)
        {
            // Đọc đường dẫn file JSON từ appsettings.json
            var credentialFilePath = configuration["Firebase:CredentialFilePath"]
                                     ?? "D:\\SWP391_CareSkin_BE\\SWP391_CareSkin_BE\\SWP391_CareSkin_BE\\firebasekey.json";

            // Tạo credential từ file JSON
            var credential = GoogleCredential.FromFile(credentialFilePath);

            // Khởi tạo StorageClient
            _storageClient = StorageClient.Create(credential);

            // Tên bucket: nên là "careskin-fb129.appspot.com"
            // (dự phòng nếu đọc từ config không có)
            _bucketName = configuration["Firebase:BucketName"] ?? "careskin-fb129.appspot.com";

            if (string.IsNullOrEmpty(_bucketName))
            {
                throw new Exception("Firebase bucket name is missing. Please set Firebase:BucketName in appsettings.json or code.");
            }
        }

        /// <summary>
        /// Upload file lên Firebase Storage kèm metadata và trả về URL công khai (?alt=media).
        /// </summary>
        /// <param name="fileStream">Luồng dữ liệu của file.</param>
        /// <param name="fileName">Tên file (có thể kèm folder con).</param>
        /// <returns>URL công khai của file sau khi upload.</returns>
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            // 1) Chuẩn bị metadata của file
            //    - ContentType nên để chính xác (ví dụ: "image/jpeg")
            //    - ContentDisposition = "inline" để hiển thị trực tiếp thay vì tải xuống
            var storageObject = new Google.Apis.Storage.v1.Data.Object
            {
                Bucket = _bucketName,
                Name = fileName,
                ContentType = "image/jpeg",        // Hoặc tự động xác định dựa vào đuôi file
                ContentDisposition = "inline"
            };

            // 2) Upload file kèm metadata
            await _storageClient.UploadObjectAsync(storageObject, fileStream);

            // 3) Tạo URL cho file dưới dạng Firebase Storage API với ?alt=media
            //    => Xem được file thô (raw file) thay vì JSON
            string encodedFileName = Uri.EscapeDataString(fileName);
            string publicUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucketName}/o/{encodedFileName}?alt=media";
            return publicUrl;
        }

        /// <summary>
        /// Xóa file khỏi Firebase Storage.
        /// </summary>
        /// <param name="fileName">Tên file cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu file không tồn tại.</returns>
        public async Task<bool> DeleteImageAsync(string fileName)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, fileName);
                return true;
            }
            catch (Google.GoogleApiException ex) when (ex.Error.Code == 404)
            {
                // File không tồn tại
                return false;
            }
        }
    }
}