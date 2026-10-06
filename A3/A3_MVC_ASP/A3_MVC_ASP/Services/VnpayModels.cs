using System;

namespace A3_MVC_ASP.Services
{
    public class VnpayCreateRequest
    {
        public string TxnRef { get; set; }          // vnp_TxnRef (mã đơn)
        public long AmountVnd { get; set; }         // số tiền VND (chưa *100)
        public string OrderInfo { get; set; }       // vnp_OrderInfo
        public string IpAddress { get; set; }       // vnp_IpAddr
        public string BaseUrl { get; set; }         // https://host
        public string BankCode { get; set; }        // vnp_BankCode (VNPAYQR)
        public DateTime CreateDate { get; set; }    // vnp_CreateDate
    }

    public class VnpayCreateResponse
    {
        public string PaymentUrl { get; set; }
        public string SignData { get; set; }
        public string SecureHash { get; set; }
    }
}

