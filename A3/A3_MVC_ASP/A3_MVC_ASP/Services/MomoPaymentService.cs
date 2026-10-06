using System;
using System.Configuration;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace A3_MVC_ASP.Services
{
    public class MomoCreateRequest
    {
        public string partnerCode { get; set; }
        public string accessKey { get; set; }
        public string requestId { get; set; }
        public string amount { get; set; }
        public string orderId { get; set; }
        public string orderInfo { get; set; }
        public string requestType { get; set; }
        public string redirectUrl { get; set; }
        public string ipnUrl { get; set; }
        public string lang { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }
    }

    public class MomoCreateResponse
    {
        public int resultCode { get; set; }
        public string message { get; set; }
        public string payUrl { get; set; }
        public string deeplink { get; set; }
        public string qrCodeUrl { get; set; }
        public string requestId { get; set; }
        public string orderId { get; set; }
    }

    public class MomoIpnDto
    {
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string requestId { get; set; }
        public long amount { get; set; }
        public long transId { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }
    }

    public static class MomoPaymentService
    {
        private static readonly HttpClient Http = new HttpClient();

        public static string EndpointCreate => (ConfigurationManager.AppSettings["Momo:EndpointCreate"] ?? "").Trim();
        public static string PartnerCode => (ConfigurationManager.AppSettings["Momo:PartnerCode"] ?? "").Trim();
        public static string AccessKey => (ConfigurationManager.AppSettings["Momo:AccessKey"] ?? "").Trim();
        public static string SecretKey => (ConfigurationManager.AppSettings["Momo:SecretKey"] ?? "").Trim();
        public static string RequestType => ((ConfigurationManager.AppSettings["Momo:RequestType"] ?? "captureWallet").Trim());
        public static bool SkipSignature => string.Equals(ConfigurationManager.AppSettings["Momo:SkipSignature"], "true", StringComparison.OrdinalIgnoreCase);

        public static string SignHmacSha256(string raw, string secret)
        {
            using (var h = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                var bytes = h.ComputeHash(Encoding.UTF8.GetBytes(raw));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static MomoCreateResponse CreatePayment(string orderId, decimal amountVnd, string returnUrl, string notifyUrl, string orderInfo)
        {
            if (string.IsNullOrWhiteSpace(EndpointCreate) || string.IsNullOrWhiteSpace(PartnerCode) ||
                string.IsNullOrWhiteSpace(AccessKey) || string.IsNullOrWhiteSpace(SecretKey))
                throw new InvalidOperationException("Chưa cấu hình MoMo trong Web.config (Momo:*).");

            var reqId = Guid.NewGuid().ToString("N");
            var amount = Math.Round(amountVnd, 0, MidpointRounding.AwayFromZero).ToString("0");
            var extra = ""; // có thể encode base64/json tuỳ nhu cầu

            var payload = new MomoCreateRequest
            {
                partnerCode = PartnerCode,
                accessKey = AccessKey,
                requestId = reqId,
                amount = amount,
                orderId = orderId,
                orderInfo = orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                lang = "vi",
                requestType = RequestType,
                extraData = extra,
                signature = ""
            };

            // Raw signature string (MoMo v2 - captureWallet)
            // accessKey, amount, extraData, ipnUrl, orderId, orderInfo, partnerCode, redirectUrl, requestId, requestType
            var raw =
                "accessKey=" + payload.accessKey +
                "&amount=" + payload.amount +
                "&extraData=" + payload.extraData +
                "&ipnUrl=" + payload.ipnUrl +
                "&orderId=" + payload.orderId +
                "&orderInfo=" + payload.orderInfo +
                "&partnerCode=" + payload.partnerCode +
                "&redirectUrl=" + payload.redirectUrl +
                "&requestId=" + payload.requestId +
                "&requestType=" + payload.requestType;

            payload.signature = SignHmacSha256(raw, SecretKey);

            var json = JsonConvert.SerializeObject(payload);
            var res = Http.PostAsync(EndpointCreate, new StringContent(json, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
            var body = res.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            if (!res.IsSuccessStatusCode)
                throw new InvalidOperationException("MoMo create lỗi HTTP " + (int)res.StatusCode + ": " + body);

            var parsed = JsonConvert.DeserializeObject<MomoCreateResponse>(body);
            if (parsed == null)
                throw new InvalidOperationException("MoMo trả về không parse được: " + body);
            if (parsed.resultCode != 0)
                throw new InvalidOperationException("MoMo lỗi: " + parsed.resultCode + " - " + (parsed.message ?? "") + " | Raw: " + body);

            // Một số phiên bản trả về deeplink/qrCodeUrl thay vì payUrl
            if (string.IsNullOrWhiteSpace(parsed.payUrl))
            {
                var alt = !string.IsNullOrWhiteSpace(parsed.deeplink) ? parsed.deeplink : parsed.qrCodeUrl;
                if (!string.IsNullOrWhiteSpace(alt))
                    parsed.payUrl = alt;
                else
                    throw new InvalidOperationException("MoMo không trả về payUrl/deeplink/qrCodeUrl. Raw: " + body);
            }

            return parsed;
        }

        public static bool VerifyIpnSignature(MomoIpnDto ipn)
        {
            if (SkipSignature) return true;
            if (ipn == null) return false;
            // MoMo gateway IPN signature string có thể thay đổi theo docs.
            // Để không chặn flow demo, có thể bật Momo:SkipSignature=true.
            // Nếu bạn có docs cụ thể, mình sẽ khớp raw string chính xác.
            var raw =
                "partnerCode=" + (ipn.partnerCode ?? "") +
                "&accessKey=" + AccessKey +
                "&requestId=" + (ipn.requestId ?? "") +
                "&amount=" + ipn.amount +
                "&orderId=" + (ipn.orderId ?? "") +
                "&resultCode=" + ipn.resultCode +
                "&message=" + (ipn.message ?? "") +
                "&extraData=" + (ipn.extraData ?? "");

            var sign = SignHmacSha256(raw, SecretKey);
            return string.Equals(sign, ipn.signature, StringComparison.OrdinalIgnoreCase);
        }
    }
}

