namespace SWP391_CareSkin_BE.Models
{
    public class ZaloPayConfig
    {
        public string AppId { get; set; }

        // Key1 dùng để tạo MAC khi gọi API tạo đơn hàng, query đơn hàng, v.v.
        public string Key1 { get; set; }

        // Key2 dùng để xác thực callback và redirect từ ZaloPay
        public string Key2 { get; set; }

        // URL API tạo đơn hàng
        public string CreateOrderUrl { get; set; }

        // URL API truy vấn trạng thái đơn hàng
        public string QueryOrderUrl { get; set; }

        // (Tuỳ chọn) URL API lấy danh sách ngân hàng (nếu bạn sử dụng)
        public string GetBankListUrl { get; set; }

        // Callback URL mà ZaloPay sẽ gọi sau khi thanh toán thành công
        public string CallbackUrl { get; set; }

        // Redirect URL mà ZaloPay sẽ chuyển hướng người dùng về sau khi thanh toán
        public string RedirectUrl { get; set; }

    }
}
