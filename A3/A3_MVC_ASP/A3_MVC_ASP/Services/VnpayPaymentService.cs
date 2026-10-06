using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace A3_MVC_ASP.Services
{
    public static class VnpayPaymentService
    {
        public static string TmnCode => (ConfigurationManager.AppSettings["Vnpay:TMN_CODE"] ?? "").Trim();
        public static string HashSecret => (ConfigurationManager.AppSettings["Vnpay:HASH_SECRET"] ?? "").Trim();
        public static string UrlPay => (ConfigurationManager.AppSettings["Vnpay:URL_PAY"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html").Trim();
        public static string ReturnPath => (ConfigurationManager.AppSettings["Vnpay:RETURN_PATH"] ?? "/Vnpay/Return").Trim();
        public static string BankCode => (ConfigurationManager.AppSettings["Vnpay:BANK_CODE"] ?? "").Trim();

        private static string TempFile => System.IO.Path.Combine(System.IO.Path.GetTempPath(), "vnpay_debug.txt");

        private static string EncodeForSign(string s)
        {
            // Bám theo sortObject của demo VNPAY: encodeURIComponent + thay %20 thành '+'
            // (tức space sẽ là '+', không phải %20)
            var x = Uri.EscapeDataString(s ?? string.Empty);
            return x.Replace("%20", "+");
        }

        private static SortedDictionary<string, string> SortAndEncode(IDictionary<string, string> input)
        {
            var dict = new SortedDictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in input)
            {
                var k = EncodeForSign(kv.Key ?? "");
                var v = EncodeForSign(kv.Value ?? "");
                dict[k] = v;
            }
            return dict;
        }

        private static string NormalizeIp(string ip)
        {
            // Sandbox VNPAY hay gặp lỗi với IPv6 (::1). Ép về IPv4 loopback nếu không phải IPv4.
            if (string.IsNullOrWhiteSpace(ip)) return "127.0.0.1";
            ip = ip.Trim();
            if (ip.Contains(":")) return "127.0.0.1";
            return ip;
        }

        // Chỉ 1 method HmacSha512 duy nhất
        public static string HmacSha512(string key, string data)
        {
            key = (key ?? "").Trim();
            var keyBytes = Encoding.UTF8.GetBytes(key);

            using (var h = new HMACSHA512(keyBytes))
            {
                var bytes = h.ComputeHash(Encoding.UTF8.GetBytes(data));
                var sb = new StringBuilder(bytes.Length * 2);
                // VNPAY thường dùng HEX chữ HOA
                foreach (var b in bytes) sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }

        public static VnpayCreateResponse CreatePayment(VnpayCreateRequest req)
        {
            if (string.IsNullOrWhiteSpace(TmnCode) || string.IsNullOrWhiteSpace(HashSecret))
                throw new InvalidOperationException("Chưa cấu hình VNPAY trong Web.config.");

            if (req == null) throw new ArgumentNullException(nameof(req));
            var amount = (req.AmountVnd * 100).ToString();
            var createDate = (req.CreateDate == default(DateTime) ? DateTime.Now : req.CreateDate).ToString("yyyyMMddHHmmss");

            var vnpRaw = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["vnp_Amount"] = amount,
                ["vnp_Command"] = "pay",
                ["vnp_CreateDate"] = createDate,
                ["vnp_CurrCode"] = "VND",
                ["vnp_IpAddr"] = NormalizeIp(req.IpAddress),
                ["vnp_Locale"] = "vn",
                ["vnp_OrderInfo"] = req.OrderInfo ?? "",
                ["vnp_OrderType"] = "other",
                ["vnp_ReturnUrl"] = (req.BaseUrl ?? "").TrimEnd('/') + ReturnPath,
                ["vnp_TmnCode"] = TmnCode,
                ["vnp_TxnRef"] = req.TxnRef ?? "",
                ["vnp_Version"] = "2.1.0"
            };

            var bank = string.IsNullOrWhiteSpace(req.BankCode) ? BankCode : req.BankCode.Trim();
            if (!string.IsNullOrWhiteSpace(bank))
                vnpRaw["vnp_BankCode"] = bank;

            // Demo VNPAY: sortObject -> encode key/value trước, rồi stringify encode:false
            var vnp = SortAndEncode(vnpRaw);
            var signData = string.Join("&", vnp.Select(kv => kv.Key + "=" + kv.Value));
            var secureHash = HmacSha512(HashSecret, signData);
            var query = signData; // đã encode sẵn theo đúng format
            // Nhiều sandbox VNPAY không cần/không dùng vnp_SecureHashType ở request tạo URL
            var finalUrl = UrlPay + "?" + query + "&vnp_SecureHash=" + secureHash;

            try
            {
                System.IO.File.WriteAllText(TempFile,
                    "=== CREATE ===\n" +
                    "HashSecret.Length: " + HashSecret.Length + "\n" +
                    "HashSecret: [" + HashSecret + "]\n" +
                    "TmnCode: [" + TmnCode + "]\n\n" +
                    "SignData (sortObject/encoded):\n" + signData + "\n\n" +
                    "SecureHash (" + secureHash.Length + " chars):\n" + secureHash + "\n\n" +
                    "Final URL:\n" + finalUrl);
            }
            catch { }

            return new VnpayCreateResponse
            {
                PaymentUrl = finalUrl,
                SignData = signData,
                SecureHash = secureHash
            };
        }

        public static bool VerifyReturn(IDictionary<string, string> qs)
        {
            if (qs == null || qs.Count == 0) return false;
            if (!qs.TryGetValue("vnp_SecureHash", out var hash)) return false;

            var raw = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in qs)
            {
                if (string.Equals(kv.Key, "vnp_SecureHash", StringComparison.OrdinalIgnoreCase)) continue;
                if (string.Equals(kv.Key, "vnp_SecureHashType", StringComparison.OrdinalIgnoreCase)) continue;
                if (!kv.Key.StartsWith("vnp_", StringComparison.OrdinalIgnoreCase)) continue;
                raw[kv.Key] = kv.Value ?? "";
            }

            var data = SortAndEncode(raw);
            var signData = string.Join("&", data.Select(kv => kv.Key + "=" + kv.Value));
            var sign = HmacSha512(HashSecret, signData);
            var matched = string.Equals(sign, hash, StringComparison.OrdinalIgnoreCase);

            try
            {
                System.IO.File.AppendAllText(TempFile,
                    "\n\n=== VERIFY RETURN ===\n" +
                    "SignData (sortObject/encoded):\n" + signData + "\n\n" +
                    "Sign tính lại (" + sign.Length + " chars):\n" + sign + "\n\n" +
                    "Sign từ VNPAY (" + hash.Length + " chars):\n" + hash + "\n\n" +
                    "Khớp: " + matched);
            }
            catch { }

            return matched;
        }
    }
}