using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using A3_MVC_ASP.Helpers;
using A3_MVC_ASP.Models;
using A3_MVC_ASP.Services;
using Newtonsoft.Json;

namespace A3_MVC_ASP.Controllers
{
    public class VnpayController : Controller
    {
        public ActionResult Create(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                if (dh == null) return HttpNotFound();

                if (!string.Equals(dh.HinhThucThanhToan, "ChuyenKhoan", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("HoanTat", "DonHang", new { id });

                var baseUrl = Request.Url?.GetLeftPart(UriPartial.Authority) ?? "";
                var ip = Request.UserHostAddress;
                var txnRef = dh.MaDon;
                var amount = (long)Math.Round(dh.TongThanhToan, 0, MidpointRounding.AwayFromZero);
                // Chỉ dùng ASCII đơn giản để tránh lỗi encode/checksum
                var info = "Thanh_toan_don_hang_" + dh.MaDon;

                try
                {
                    var res = VnpayPaymentService.CreatePayment(new VnpayCreateRequest
                    {
                        BaseUrl = baseUrl,
                        TxnRef = txnRef,
                        AmountVnd = amount,
                        OrderInfo = info,
                        IpAddress = ip,
                        BankCode = null,
                        CreateDate = DateTime.Now
                    });
                    return Redirect(res.PaymentUrl);
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Không tạo được thanh toán VNPAY: " + ex.Message;
                    return RedirectToAction("HoanTat", "DonHang", new { id });
                }
            }
        }

        public ActionResult Return()
        {
            var qs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var k in Request.QueryString.AllKeys)
            {
                if (k == null) continue;
                qs[k] = Request.QueryString[k];
            }

            var txnRef = qs.TryGetValue("vnp_TxnRef", out var v) ? v : "";
            var rsp = qs.TryGetValue("vnp_ResponseCode", out var rc) ? rc : "";
            var transNo = qs.TryGetValue("vnp_TransactionNo", out var tn) ? tn : "";

            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDon == txnRef);
                if (dh == null) return HttpNotFound();

                if (rsp == "00")
                {
                    dh.TrangThaiThanhToan = "DaThanhToan";
                    if (string.Equals(dh.TrangThai, "ChoDuyet", StringComparison.OrdinalIgnoreCase))
                        dh.TrangThai = "DaXacNhan";

                    db.ThanhToans.Add(new ThanhToan
                    {
                        MaDonHang = dh.MaDonHang,
                        Provider = "VNPAYSandbox",
                        MaGiaoDich = string.IsNullOrWhiteSpace(transNo) ? dh.MaDon : transNo,
                        SoTien = dh.TongThanhToan,
                        TrangThai = "ThanhCong",
                        NgayThanhToan = DateTime.Now,
                        RawJson = JsonConvert.SerializeObject(qs)
                    });
                    db.SaveChanges();

                    GioHangSessionHelper.Ghi(Session, new System.Collections.Generic.List<CartLine>());
                    TempData["Msg"] = "VNPAY Sandbox: thanh toán thành công. Mã đơn: " + dh.MaDon;
                }
                else
                {
                    dh.TrangThaiThanhToan = "ChuaThanhToan";
                    dh.TrangThai = "DaHuy";

                    db.ThanhToans.Add(new ThanhToan
                    {
                        MaDonHang = dh.MaDonHang,
                        Provider = "VNPAYSandbox",
                        MaGiaoDich = string.IsNullOrWhiteSpace(transNo) ? dh.MaDon : transNo,
                        SoTien = dh.TongThanhToan,
                        TrangThai = "ThatBai",
                        NgayThanhToan = DateTime.Now,
                        RawJson = JsonConvert.SerializeObject(qs)
                    });

                    db.SaveChanges();
                    TempData["Msg"] = "VNPAY Sandbox: thanh toán thất bại (code " + rsp + "). Mã đơn: " + dh.MaDon;
                }

                return RedirectToAction("HoanTat", "DonHang", new { id = dh.MaDonHang });
            }
        }
    }
}