using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Web.Mvc;
using A3_MVC_ASP.Helpers;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class DonHangController : Controller
    {
        private static readonly string[] DanhSachTinhThanh = new[]
        {
            "Hà Nội","Hà Giang","Cao Bằng","Bắc Kạn","Tuyên Quang","Lào Cai","Yên Bái","Thái Nguyên","Lạng Sơn","Bắc Giang",
            "Phú Thọ","Vĩnh Phúc","Bắc Ninh","Quảng Ninh","Hải Dương","Hải Phòng","Hưng Yên","Thái Bình","Nam Định","Ninh Bình",
            "Hà Nam","Thanh Hóa","Nghệ An","Hà Tĩnh","Quảng Bình","Quảng Trị","Thừa Thiên Huế","Đà Nẵng","Quảng Nam","Quảng Ngãi",
            "Bình Định","Phú Yên","Khánh Hòa","Ninh Thuận","Bình Thuận","Kon Tum","Gia Lai","Đắk Lắk","Đắk Nông","Lâm Đồng",
            "Bình Phước","Tây Ninh","Bình Dương","Đồng Nai","Bà Rịa - Vũng Tàu","TP. Hồ Chí Minh","Long An","Tiền Giang","Bến Tre","Trà Vinh",
            "Vĩnh Long","Đồng Tháp","An Giang","Kiên Giang","Cần Thơ","Hậu Giang","Sóc Trăng","Bạc Liêu","Cà Mau"
        };

        private static readonly HashSet<string> TinhMienBac = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Hà Nội","Hà Giang","Cao Bằng","Bắc Kạn","Tuyên Quang","Lào Cai","Yên Bái","Thái Nguyên","Lạng Sơn","Bắc Giang",
            "Phú Thọ","Vĩnh Phúc","Bắc Ninh","Quảng Ninh","Hải Dương","Hải Phòng","Hưng Yên","Thái Bình","Nam Định","Ninh Bình","Hà Nam"
        };

        private static decimal TinhPhiVanChuyen(string tinhThanh)
        {
            return TinhMienBac.Contains((tinhThanh ?? "").Trim()) ? 15000m : 20000m;
        }

        private static IEnumerable<SelectListItem> TaoDanhSachTinhThanh(string tinhDaChon = null)
        {
            return DanhSachTinhThanh.Select(t => new SelectListItem
            {
                Text = t,
                Value = t,
                Selected = string.Equals(t, tinhDaChon, StringComparison.OrdinalIgnoreCase)
            });
        }

        public ActionResult DatHang()
        {
            ViewBag.Title = "Đặt hàng — Thanh toán";
            var gio = GioHangSessionHelper.Doc(Session);
            if (!gio.Any())
            {
                TempData["Msg"] = "Giỏ hàng trống.";
                return RedirectToAction("Index", "GioHang");
            }

            var vm = new DatHangVm
            {
                TinhThanh = "Hà Nội",
                PhiVanChuyen = TinhPhiVanChuyen("Hà Nội"),
                HinhThucThanhToan = "TienMat"
            };

            if (Session["HoTen"] != null)
            {
                vm.TenKhachHang = Session["HoTen"] as string;
                using (var db = new DoAnVatA3Context())
                {
                    var ma = Convert.ToInt32(Session["MaNguoiDung"]);
                    var u = db.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == ma);
                    if (u != null)
                    {
                        vm.SoDienThoai = u.SoDienThoai;
                        vm.DiaChiGiao = u.DiaChi;
                    }
                }
            }

            ViewBag.GioHang = gio;
            ViewBag.TongTienHang = GioHangSessionHelper.TongTien(Session);
            ViewBag.TinhThanhOptions = TaoDanhSachTinhThanh(vm.TinhThanh);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang(DatHangVm model)
        {
            ViewBag.Title = "Đặt hàng — Thanh toán";
            var gio = GioHangSessionHelper.Doc(Session);
            ViewBag.GioHang = gio;
            ViewBag.TongTienHang = GioHangSessionHelper.TongTien(Session);
            ViewBag.TinhThanhOptions = TaoDanhSachTinhThanh(model != null ? model.TinhThanh : null);

            if (!gio.Any())
            {
                TempData["Msg"] = "Giỏ hàng trống.";
                return RedirectToAction("Index", "GioHang");
            }

            if (!ModelState.IsValid)
                return View(model);

            model.PhiVanChuyen = TinhPhiVanChuyen(model.TinhThanh);

            int? maKh = Session["MaNguoiDung"] as int?;

            using (var db = new DoAnVatA3Context())
            using (var tx = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var line in gio)
                    {
                        var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == line.MaSanPham);
                        if (sp == null || line.SoLuong > sp.SoLuongTon)
                        {
                            try { tx.Rollback(); } catch { /* ignore rollback errors; show real error */ }
                            ModelState.AddModelError("", "Sản phẩm " + line.TenSanPham + " không đủ tồn kho.");
                            return View(model);
                        }
                    }

                    var tongHang = gio.Sum(x => x.ThanhTienHang);
                    var tongTT = tongHang + model.PhiVanChuyen;
                    var maDonStr = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);

                    if (maDonStr.Length > 20)
                        maDonStr = maDonStr.Substring(0, 20);

                    var dh = new DonHang
                    {
                        MaDon = maDonStr,
                        MaNguoiDung = maKh,
                        TenKhachHang = string.IsNullOrWhiteSpace(model.TenKhachHang) ? (Session["HoTen"] as string ?? "Khách") : model.TenKhachHang.Trim(),
                        SoDienThoai = model.SoDienThoai,
                        DiaChiGiao = model.DiaChiGiao.Trim(),
                        TongTienHang = tongHang,
                        PhiVanChuyen = model.PhiVanChuyen,
                        TongThanhToan = tongTT,
                        TrangThai = "ChoDuyet",
                        HinhThucThanhToan = model.HinhThucThanhToan ?? "TienMat",
                        TrangThaiThanhToan = "ChuaThanhToan",
                        GhiChu = model.GhiChu,
                        NgayDat = DateTime.Now
                    };
                    db.DonHangs.Add(dh);
                    db.SaveChanges();

                    var maDh = dh.MaDonHang;
                    foreach (var line in gio)
                    {
                        var giaBan = line.GiaBan * (100 - line.PhanTramGiam) / 100m;
                        giaBan = Math.Round(giaBan, 0, MidpointRounding.AwayFromZero);
                        db.ChiTietDonHangs.Add(new ChiTietDonHang
                        {
                            MaDonHang = maDh,
                            MaSanPham = line.MaSanPham,
                            SoLuong = line.SoLuong,
                            DonGiaBan = giaBan
                        });
                    }
                    db.SaveChanges();

                    tx.Commit();
                    // Nếu MoMo: chuyển qua cổng thanh toán; trạng thái đơn sẽ được IPN cập nhật.
                    if (string.Equals(dh.HinhThucThanhToan, "MoMo", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("MomoCreate", "Payment", new { id = maDh });
                    }
                    // Nếu chuyển khoản: dùng VNPAY Sandbox
                    if (string.Equals(dh.HinhThucThanhToan, "ChuyenKhoan", StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Create", "Vnpay", new { id = maDh });
                    }

                    GioHangSessionHelper.Ghi(Session, new System.Collections.Generic.List<CartLine>());
                    TempData["Msg"] = "Đặt hàng thành công. Mã đơn: " + maDonStr;
                    return RedirectToAction("HoanTat", new { id = maDh });
                }
                catch (Exception ex)
                {
                    try { tx.Rollback(); } catch { /* ignore rollback errors; show original exception */ }
                    ModelState.AddModelError("", "Không thể tạo đơn: " + BuildEfError(ex));
                    return View(model);
                }
            }
        }

        public ActionResult HoanTat(int id)
        {
            ViewBag.Title = "Hoàn tất đơn hàng";
            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                if (dh == null) return HttpNotFound();
                var pay = db.ThanhToans.Where(x => x.MaDonHang == id).OrderByDescending(x => x.NgayThanhToan).FirstOrDefault();
                ViewBag.ThanhToan = pay;
                return View(dh);
            }
        }

        private static string BuildEfError(Exception ex)
        {
            // Lấy thông báo lỗi sâu nhất (SQL/validation) để debug nhanh.
            if (ex == null) return "Lỗi không xác định.";

            var sb = new StringBuilder();
            sb.Append(ex.Message);

            // DbUpdateException thường bọc lỗi SQL trong InnerException.InnerException
            var cur = ex.InnerException;
            var depth = 0;
            while (cur != null && depth < 6)
            {
                sb.Append(" | ");
                sb.Append(cur.Message);
                cur = cur.InnerException;
                depth++;
            }

            // nếu là DbUpdateException thì thêm info entity state (nếu có)
            var du = ex as DbUpdateException;
            if (du != null && du.Entries != null)
            {
                var names = du.Entries.Select(e => e.Entity != null ? e.Entity.GetType().Name : "Unknown").Distinct().ToList();
                if (names.Count > 0)
                    sb.Append(" | Entities: ").Append(string.Join(",", names));
            }

            return sb.ToString();
        }
    }
}
