using Google.Apis.Auth.OAuth2;
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
            // Lấy đường dẫn file JSON từ cấu hình hoặc dùng giá trị mặc định
            var credentialFilePath = configuration["Firebase:CredentialFilePath"]
                                     ?? "C:\\Users\\Administrator\\Documents\\careskin-fb129-firebase-adminsdk-fbsvc-e5573965f0.json";

            // Tạo credential từ file JSON
            var credential = GoogleCredential.FromFile(credentialFilePath);

            // Tạo StorageClient sử dụng credential
            _storageClient = StorageClient.Create(credential);

            // Lấy tên bucket từ cấu hình hoặc dùng giá trị mặc định (thường có dạng "careskin-fb129.appspot.com")
            _bucketName = configuration["Firebase:BucketName"] ?? "careskin-fb129.firebasestorage.app";

            if (string.IsNullOrEmpty(_bucketName))
            {
                throw new Exception("Firebase bucket name is missing. Please set Firebase:BucketName in appsettings.json or code.");
            }
        }

        /// <summary>
        /// Upload file lên Firebase Storage và trả về URL công khai.
        /// </summary>
        /// <param name="fileStream">Luồng dữ liệu của file.</param>
        /// <param name="fileName">Tên file (có thể chứa folder con nếu cần).</param>
        /// <returns>URL công khai của file sau khi upload.</returns>
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            // Upload file lên bucket (contentType để null để Google tự đoán)
            await _storageClient.UploadObjectAsync(_bucketName, fileName, null, fileStream);

            // Tạo URL công khai cho file (bucket cần được cấu hình public)
            string publicUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}";
            return publicUrl;
        }

        /// <summary>
        /// Xóa file khỏi Firebase Storage.
        /// </summary>
        /// <param name="fileName">Tên file cần xóa.</param>
        /// <returns>True nếu xóa thành công, false nếu file không tồn tại.</returns>
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
