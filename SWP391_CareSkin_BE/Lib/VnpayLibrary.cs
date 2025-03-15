using SWP391_CareSkin_BE.DTOs.Responses.Vnpay;
using SWP391_CareSkin_BE.Lib;
using System.Net;
using System.Security.Cryptography;
using System.Text;

public class VnpayLibrary
{
    public static String HmacSHA512(string key, String inputData)
    {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            foreach (var theByte in hashValue)
            {
                hash.Append(theByte.ToString("x2"));
            }
        }

        return hash.ToString();
    }

    private SortedList<string, string> _requestData = new SortedList<string, string>(new PaymentCompare());
    private SortedList<string, string> _responseData = new SortedList<string, string>(new PaymentCompare());

    //------------------------REQUEST DATA----------------------------------------
    public void AddRequestData(string key, string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            _requestData.Add(key, value);
        }
    }

    public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
    {
        StringBuilder data = new StringBuilder();
        foreach (KeyValuePair<string, string> kv in _requestData)
        {
            if (!String.IsNullOrEmpty(kv.Value))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }
        }
        string queryString = data.ToString();

        baseUrl += "?" + queryString;
        String signData = queryString;
        if (signData.Length > 0)
        {

            signData = signData.Remove(data.Length - 1, 1);
        }
        string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
        baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

        return baseUrl;
    }




    //------------------------RESPONSE DATA----------------------------------------
    public void AddResponseData(string key, string value)
    {
        if (!String.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }

    public string GetResponseData(string key)
    {
        string retValue;
        if (_responseData.TryGetValue(key, out retValue))
        {
            return retValue;
        }
        else
        {
            return string.Empty;
        }
    }

    private string GetResponseData()
    {

        StringBuilder data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
        {
            _responseData.Remove("vnp_SecureHashType");
        }
        if (_responseData.ContainsKey("vnp_SecureHash"))
        {
            _responseData.Remove("vnp_SecureHash");
        }
        foreach (KeyValuePair<string, string> kv in _responseData)
        {
            if (!String.IsNullOrEmpty(kv.Value))
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }
        }
        //remove last '&'
        if (data.Length > 0)
        {
            data.Remove(data.Length - 1, 1);
        }
        return data.ToString();
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        string rspRaw = GetResponseData();
        string myChecksum = HmacSHA512(secretKey, rspRaw);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    public string GetIpAddress(HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "127.0.0.1";
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
            OrderId = int.TryParse(GetResponseData("vnp_TxnRef").Split("_")[0], out var orderId) ? orderId : 0,
            TransactionId = GetResponseData("vnp_TransactionNo"),
            PaymentMethod = "VnPay",
            OrderDescription = GetResponseData("vnp_OrderInfo"),
            Token = GetResponseData("vnp_SecureHash"),
            Amount = double.Parse(GetResponseData("vnp_Amount")) / 100,
            VnPayResponseCode = GetResponseData("vnp_ResponseCode")
        };
    }
}

