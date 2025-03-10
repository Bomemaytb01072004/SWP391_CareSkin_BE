using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Globalization;
using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;

namespace SWP391_CareSkin_BE.Lib
{
    public class VnpayLibrary
    {
        private readonly SortedDictionary<string, string> _requestData = new();
        private readonly SortedDictionary<string, string> _responseData = new();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData[key] = value;
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData[key] = value;
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var value) ? value : string.Empty;
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var queryString = string.Join("&", _requestData.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
            var signData = queryString;
            var vnpSecureHash = HmacSha512(hashSecret, signData);
            return $"{baseUrl}?{queryString}&vnp_SecureHash={vnpSecureHash}";
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var rawData = GetRawResponseData();
            var computedHash = HmacSha512(secretKey, rawData);
            return string.Equals(computedHash, inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public VnpayResponseDTO GetFullResponseData(IQueryCollection collection, string hashSecret)
        {
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    AddResponseData(key, value);
                }
            }

            var isValidSignature = ValidateSignature(GetResponseData("vnp_SecureHash"), hashSecret);
            return new VnpayResponseDTO
            {
                Success = isValidSignature,
                OrderId = int.TryParse(GetResponseData("vnp_TxnRef"), out var orderId) ? orderId : 0,
                TransactionId = GetResponseData("vnp_TransactionNo"),
                PaymentMethod = "VnPay",
                OrderDescription = GetResponseData("vnp_OrderInfo"),
                Token = GetResponseData("vnp_SecureHash"),
                VnPayResponseCode = GetResponseData("vnp_ResponseCode")
            };
        }

        private string GetRawResponseData()
        {
            var data = _responseData.Where(kv => kv.Key != "vnp_SecureHash" && kv.Key != "vnp_SecureHashType")
                                         .OrderBy(kv => kv.Key)
                                         .Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}");
            var rawData = string.Join("&", data);

            // Add logging here
            Console.WriteLine($"Raw data for signature: {rawData}");

            return rawData;
        }

        private string HmacSha512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            return string.Concat(hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData)).Select(b => b.ToString("x2")));
        }

        public string GetIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";
        }
    }
}
