using System.Linq;
using System.Web.Mvc;
using A3_MVC_ASP.Helpers;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class GioHangController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng";
            var gio = GioHangSessionHelper.Doc(Session);
            return View(gio);
        }

        /// <summary>Thêm sản phẩm vào giỏ (query: id, sl).</summary>
        public ActionResult Them(int? id, int? sl)
        {
            if (id == null)
                return RedirectToAction("Index", "SanPham");

            var soLuong = sl ?? 1;
            if (soLuong < 1) soLuong = 1;

            using (var db = new DoAnVatA3Context())
            {
                var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == id && x.ConHoatDong);
                if (sp == null)
                {
                    TempData["Msg"] = "Không tìm thấy sản phẩm.";
                    return RedirectToAction("Index", "SanPham");
                }

                if (soLuong > sp.SoLuongTon)
                {
                    TempData["Msg"] = "Không đủ hàng trong kho (tồn: " + sp.SoLuongTon + ").";
                    return RedirectToAction("ChiTiet", "SanPham", new { id });
                }

                var gio = GioHangSessionHelper.Doc(Session);
                var line = gio.FirstOrDefault(x => x.MaSanPham == id);
                if (line != null)
                {
                    var slMoi = line.SoLuong + soLuong;
                    if (slMoi > sp.SoLuongTon)
                    {
                        TempData["Msg"] = "Số lượng trong giỏ vượt tồn kho.";
                        return RedirectToAction("ChiTiet", "SanPham", new { id });
                    }
                    line.SoLuong = slMoi;
                }
                else
                {
                    gio.Add(new CartLine
                    {
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        GiaBan = sp.GiaBan,
                        PhanTramGiam = sp.PhanTramGiam,
                        SoLuong = soLuong,
                        HinhAnh = sp.HinhAnh,
                        DonViTinh = sp.DonViTinh
                    });
                }
                GioHangSessionHelper.Ghi(Session, gio);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhat(int id, int soLuong)
        {
            var gio = GioHangSessionHelper.Doc(Session);
            var line = gio.FirstOrDefault(x => x.MaSanPham == id);
            if (line == null)
                return RedirectToAction("Index");

            using (var db = new DoAnVatA3Context())
            {
                var sp = db.SanPhams.First(x => x.MaSanPham == id);
                if (soLuong < 1) soLuong = 1;
                if (soLuong > sp.SoLuongTon) soLuong = sp.SoLuongTon;
                line.SoLuong = soLuong;
            }
            GioHangSessionHelper.Ghi(Session, gio);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoa(int id)
        {
            var gio = GioHangSessionHelper.Doc(Session);
            gio.RemoveAll(x => x.MaSanPham == id);
            GioHangSessionHelper.Ghi(Session, gio);
            return RedirectToAction("Index");
        }
    }
}
