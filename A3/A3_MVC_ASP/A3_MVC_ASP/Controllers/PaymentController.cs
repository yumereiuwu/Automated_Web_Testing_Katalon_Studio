using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using A3_MVC_ASP.Helpers;
using A3_MVC_ASP.Models;
using A3_MVC_ASP.Services;
using Newtonsoft.Json;

namespace A3_MVC_ASP.Controllers
{
    public class PaymentController : Controller
    {
        // GET: /Payment/MomoCreate?id=123
        public ActionResult MomoCreate(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                if (dh == null) return HttpNotFound();

                if (!string.Equals(dh.HinhThucThanhToan, "MoMo", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("HoanTat", "DonHang", new { id });

                var baseUrl = Request.Url?.GetLeftPart(UriPartial.Authority) ?? "";
                var returnUrl = baseUrl + Url.Action("MomoReturn", "Payment", new { id });
                var notifyUrl = baseUrl + Url.Action("MomoIpn", "Payment");
                var orderId = dh.MaDon ?? ("DH" + dh.MaDonHang);
                var orderInfo = "Thanh toan don hang " + orderId;

                try
                {
                    var res = MomoPaymentService.CreatePayment(orderId, dh.TongThanhToan, returnUrl, notifyUrl, orderInfo);
                    if (res == null || string.IsNullOrWhiteSpace(res.payUrl))
                        throw new InvalidOperationException("MoMo không trả về payUrl.");
                    if (res.resultCode != 0)
                        throw new InvalidOperationException("MoMo lỗi: " + res.resultCode + " - " + (res.message ?? ""));

                    return Redirect(res.payUrl);
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Không tạo được thanh toán MoMo: " + ex.Message;
                    return RedirectToAction("HoanTat", "DonHang", new { id });
                }
            }
        }

        // GET: /Payment/MomoReturn?id=123
        public ActionResult MomoReturn(int id)
        {
          
            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                if (dh == null) return HttpNotFound();

                var autoMark = string.Equals(ConfigurationManager.AppSettings["Momo:AutoMarkPaidOnReturn"], "true", StringComparison.OrdinalIgnoreCase);
                if (autoMark
                    && string.Equals(dh.HinhThucThanhToan, "MoMo", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(dh.TrangThaiThanhToan, "DaThanhToan", StringComparison.OrdinalIgnoreCase))
                {
            
                    dh.TrangThaiThanhToan = "DaThanhToan";
                    if (string.Equals(dh.TrangThai, "ChoDuyet", StringComparison.OrdinalIgnoreCase))
                        dh.TrangThai = "DaXacNhan";

                    db.ThanhToans.Add(new ThanhToan
                    {
                        MaDonHang = dh.MaDonHang,
                        Provider = "MoMoDemo",
                        // Lưu rõ mã đơn để dễ tra cứu trong DB
                        MaGiaoDich = dh.MaDon,
                        SoTien = dh.TongThanhToan,
                        TrangThai = "Demo",
                        NgayThanhToan = DateTime.Now,
                        RawJson = ""
                    });
                    db.SaveChanges();
                    TempData["Msg"] = "Đã ghi nhận thanh toán (demo) cho mã đơn: " + dh.MaDon;
                }

            
                if (string.Equals(dh.TrangThaiThanhToan, "DaThanhToan", StringComparison.OrdinalIgnoreCase))
                    GioHangSessionHelper.Ghi(Session, new System.Collections.Generic.List<CartLine>());

                return RedirectToAction("HoanTat", "DonHang", new { id });
            }
        }

        // POST: /Payment/MomoIpn
        [HttpPost]
        public ActionResult MomoIpn()
        {
            try
            {
                Request.InputStream.Position = 0;
                string body;
                using (var r = new StreamReader(Request.InputStream))
                    body = r.ReadToEnd();

                var ipn = JsonConvert.DeserializeObject<MomoIpnDto>(body);
                if (ipn == null)
                    return new HttpStatusCodeResult(400, "Invalid payload");

                if (!MomoPaymentService.VerifyIpnSignature(ipn))
                    return new HttpStatusCodeResult(400, "Invalid signature");

                using (var db = new DoAnVatA3Context())
                {
                    
                    var dh = db.DonHangs.FirstOrDefault(x => x.MaDon == ipn.orderId);
                    if (dh == null)
                        return new HttpStatusCodeResult(404, "Order not found");

                   
                    if (string.Equals(dh.TrangThaiThanhToan, "DaThanhToan", StringComparison.OrdinalIgnoreCase))
                        return Json(new { result = "OK" });

                    if (ipn.resultCode == 0)
                    {
                        dh.TrangThaiThanhToan = "DaThanhToan";
                        if (string.Equals(dh.TrangThai, "ChoDuyet", StringComparison.OrdinalIgnoreCase))
                            dh.TrangThai = "DaXacNhan";
                        db.SaveChanges();
                    }
                    else
                    {
                    
                        dh.TrangThaiThanhToan = "ChuaThanhToan";
                        if (!string.Equals(dh.TrangThai, "DaHuy", StringComparison.OrdinalIgnoreCase))
                            dh.TrangThai = "DaHuy";
                        db.SaveChanges();
                    }
                }

                return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }
    }
}

