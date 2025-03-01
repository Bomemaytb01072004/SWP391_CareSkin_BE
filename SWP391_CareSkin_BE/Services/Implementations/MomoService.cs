using System.Text.Json;
using RestSharp;
using SWP391_CareSkin_BE.DTOs.Requests;
using SWP391_CareSkin_BE.DTOs.Responses;
using SWP391_CareSkin_BE.Helpers;

namespace SWP391_CareSkin_BE.Services.Implementations
{
    public class MomoService
   {
    private readonly string _partnerCode;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly string _endpoint;

    public MomoService(IConfiguration configuration)
    {
        _partnerCode = configuration["Momo:PartnerCode"];
        _accessKey = configuration["Momo:AccessKey"];
        _secretKey = configuration["Momo:SecretKey"];
        _endpoint = configuration["Momo:Endpoint"];
    }

    public string CreatePayment(MomoRequest request)
    {
        var requestId = Guid.NewGuid().ToString();
        var extraData = "";

        string rawData = $"accessKey={_accessKey}&amount={request.Amount}&extraData={extraData}&orderId={request.OrderId}" +
                         $"&partnerCode={_partnerCode}&requestId={requestId}&requestType=captureWallet&returnUrl={request.ReturnUrl}&notifyUrl={request.NotifyUrl}";

        string signature = MomoHelper.CreateSignature(rawData, _secretKey);

        var momoRequest = new
        {
            partnerCode = _partnerCode,
            orderId = request.OrderId,
            requestId = requestId,
            amount = request.Amount,
            orderInfo = "Payment for Order " + request.OrderId,
            returnUrl = request.ReturnUrl,
            notifyUrl = request.NotifyUrl,
            extraData = extraData,
            requestType = "captureWallet",
            signature = signature
        };

        var client = new RestClient(_endpoint);
        var restRequest = new RestRequest(_endpoint, Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        restRequest.AddJsonBody(momoRequest);

        var response = client.Execute(restRequest);
        var momoResponse = JsonSerializer.Deserialize<MomoResponse>(response.Content);

        return momoResponse?.PayUrl ?? "";
    }
}
}
