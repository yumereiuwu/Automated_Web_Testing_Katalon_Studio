using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class HomeController : Controller
    {
        private static string GetDanhMucHomeImage(string tenDanhMuc)
        {
            if (string.IsNullOrWhiteSpace(tenDanhMuc))
                return "douong.jpg";

            var key = tenDanhMuc.Trim().ToLowerInvariant();
            if (key.Contains("uống") || key.Contains("uong"))
                return "douong.jpg";
            if ((key.Contains("đồ") || key.Contains("do")) && key.Contains("ăn liền"))
                return "doanlien.jpg";
            if (key.Contains("do an lien"))
                return "doanlien.jpg";
            if (key.Contains("kẹo") || key.Contains("keo") || key.Contains("bánh") || key.Contains("banh"))
                return "banhkeo.jpg";
            if (key.Contains("healthy"))
                return "doanhealthy.png";
            if (key.Contains("mặn") || key.Contains("man"))
                return "doanman.jpg";
            if (key.Contains("khô") || key.Contains("kho"))
                return "snack.png";
            if (key.Contains("sấy") || key.Contains("say"))
                return "doanhealthy.png";
            if (key.Contains("snack"))
                return "snack.png";

            return "snack.png";
        }

        public ActionResult Index()
        {
            using (var db = new DoAnVatA3Context())
            {
                var danhMucTrangChuCoDinh = new[] { "Đồ ăn liền", "Đồ khô", "Đồ mặn", "Đồ sấy" };
                var danhMucDangHoatDong = db.DanhMucs
                    .Where(x => x.ConHoatDong)
                    .Select(x => new { x.MaDanhMuc, x.TenDanhMuc })
                    .ToList();

                var danhMucs = danhMucTrangChuCoDinh
                    .Select(tenDanhMuc =>
                    {
                        var dm = danhMucDangHoatDong.FirstOrDefault(x =>
                            string.Equals((x.TenDanhMuc ?? string.Empty).Trim(), tenDanhMuc, StringComparison.CurrentCultureIgnoreCase));
                        var soLuong = dm != null
                            ? db.SanPhams.Count(sp => sp.ConHoatDong && sp.MaDanhMuc == dm.MaDanhMuc)
                            : 0;
                        return new HomeDanhMucVm
                        {
                            MaDanhMuc = dm != null ? dm.MaDanhMuc : 0,
                            TenDanhMuc = tenDanhMuc,
                            HinhAnh = GetDanhMucHomeImage(tenDanhMuc),
                            SoLuongSanPham = soLuong
                        };
                    })
                    .ToList();

                var sanPhamBanChays = (from ct in db.ChiTietDonHangs
                                       join dh in db.DonHangs on ct.MaDonHang equals dh.MaDonHang
                                       join sp in db.SanPhams on ct.MaSanPham equals sp.MaSanPham
                                       join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                                       where sp.ConHoatDong && dm.ConHoatDong && dh.TrangThai != "DaHuy"
                                       group new { ct, dh, sp, dm } by new
                                       {
                                           sp.MaSanPham,
                                           sp.TenSanPham,
                                           dm.TenDanhMuc,
                                           sp.HinhAnh,
                                           sp.GiaBan,
                                           sp.PhanTramGiam
                                       }
                    into g
                                       orderby g.Select(x => x.ct.MaDonHang).Distinct().Count() descending,
                                           g.Sum(x => x.ct.SoLuong) descending
                                       select new HomeSanPhamVm
                                       {
                                           MaSanPham = g.Key.MaSanPham,
                                           TenSanPham = g.Key.TenSanPham,
                                           TenDanhMuc = g.Key.TenDanhMuc,
                                           HinhAnh = g.Key.HinhAnh,
                                           GiaBan = g.Key.GiaBan,
                                           PhanTramGiam = g.Key.PhanTramGiam
                                       })
                    .Take(4)
                    .ToList();

                if (!sanPhamBanChays.Any())
                {
                    sanPhamBanChays = (from sp in db.SanPhams
                                       join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                                       where sp.ConHoatDong && dm.ConHoatDong
                                       orderby sp.NgayTao descending
                                       select new HomeSanPhamVm
                                       {
                                           MaSanPham = sp.MaSanPham,
                                           TenSanPham = sp.TenSanPham,
                                           TenDanhMuc = dm.TenDanhMuc,
                                           HinhAnh = sp.HinhAnh,
                                           GiaBan = sp.GiaBan,
                                           PhanTramGiam = sp.PhanTramGiam
                                       })
                        .Take(4)
                        .ToList();
                }

                var sanPhamGiamGias = (from sp in db.SanPhams
                                       join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                                       where sp.ConHoatDong && dm.ConHoatDong && sp.PhanTramGiam > 0
                                       orderby sp.PhanTramGiam descending, sp.NgayTao descending
                                       select new HomeSanPhamVm
                                       {
                                           MaSanPham = sp.MaSanPham,
                                           TenSanPham = sp.TenSanPham,
                                           TenDanhMuc = dm.TenDanhMuc,
                                           HinhAnh = sp.HinhAnh,
                                           GiaBan = sp.GiaBan,
                                           PhanTramGiam = sp.PhanTramGiam
                                       })
                    .Take(4)
                    .ToList();

                var baiVietNoiBats = new List<HomeBaiVietVm>
                {
                    new HomeBaiVietVm
                    {
                        MaBaiViet = 1,
                        TieuDe = "Bạn có biết xúc xích ăn liền bao nhiêu calo không?",
                        TomTat = "Sự thật có thể khiến bạn bất ngờ khi chọn món ăn vặt mỗi ngày.",
                        NgayText = "09 Th7"
                    },
                    new HomeBaiVietVm
                    {
                        MaBaiViet = 2,
                        TieuDe = "Ăn trái cây sấy có tốt không?",
                        TomTat = "Lợi ích và lưu ý khi dùng trái cây sấy trong chế độ ăn lành mạnh.",
                        NgayText = "12 Th7"
                    },
                    new HomeBaiVietVm
                    {
                        MaBaiViet = 3,
                        TieuDe = "Cách làm khô heo cháy tỏi ngon chuẩn vị",
                        TomTat = "Hướng dẫn công thức đơn giản để làm món khô heo tại nhà.",
                        NgayText = "08 Th10"
                    },
                    new HomeBaiVietVm
                    {
                        MaBaiViet = 4,
                        TieuDe = "Cách làm khô gà xé sợi tại nhà thơm ngon",
                        TomTat = "Mẹo nêm nếm và sấy khô để món khô gà mềm, dai đúng chuẩn.",
                        NgayText = "08 Th10"
                    }
                };

                var vm = new HomeIndexVm
                {
                    DanhMucs = danhMucs,
                    SanPhamBanChays = sanPhamBanChays,
                    SanPhamGiamGias = sanPhamGiamGias,
                    BaiVietNoiBats = baiVietNoiBats
                };

                return View(vm);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Cửa hàng đồ ăn vặt — giao nhanh, đủ loại bánh kẹo, đồ uống.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Liên hệ đặt hàng hoặc góp ý.";
            return View();
        }
    }
}