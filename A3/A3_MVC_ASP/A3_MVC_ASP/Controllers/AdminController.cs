using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using A3_MVC_ASP.Filters;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    [QuanTriAuthorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Tổng quan";
            SetAdminPage("Tổng quan", new AdminBreadcrumbItem { Text = "Bảng điều khiển", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                var tk = db.Database.SqlQuery<ThongKeTongQuanDto>("EXEC sp_ThongKeTongQuan").FirstOrDefault()
                         ?? new ThongKeTongQuanDto();
                var recentOrders = db.DonHangs
                    .OrderByDescending(x => x.NgayDat)
                    .Take(8)
                    .Select(x => new DashboardOrderItemVm
                    {
                        MaDonHang = x.MaDonHang,
                        MaDon = x.MaDon,
                        TenKhachHang = x.TenKhachHang,
                        NgayDat = x.NgayDat,
                        TongThanhToan = x.TongThanhToan,
                        TrangThai = x.TrangThai
                    })
                    .ToList();

                var lowStocks = (from sp in db.SanPhams
                                 join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                                 where sp.ConHoatDong
                                 orderby sp.SoLuongTon ascending, sp.TenSanPham
                                 select new DashboardLowStockVm
                                 {
                                     MaSanPham = sp.MaSanPham,
                                     TenSanPham = sp.TenSanPham,
                                     TenDanhMuc = dm.TenDanhMuc,
                                     SoLuongTon = sp.SoLuongTon
                                 })
                    .Take(8)
                    .ToList();

                // Dashboard revenue: only confirmed orders; accept both code and localized text, trim/case-insensitive.
                var doanhThuQuery = db.DonHangs.Where(x =>
                    x.TrangThai != null &&
                    (
                        x.TrangThai.Trim().ToLower() == "daxacnhan" ||
                        x.TrangThai.Trim().ToLower() == "đã xác nhận"
                    ));

                tk.TongDoanhThu = doanhThuQuery.Sum(x => (decimal?)x.TongThanhToan) ?? 0m;

                var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5);
                var rawRevenue = doanhThuQuery
                    .Where(x => x.NgayDat >= start)
                    .GroupBy(x => new { x.NgayDat.Year, x.NgayDat.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        DoanhThu = g.Sum(x => x.TongThanhToan)
                    })
                    .ToList();

                var labels = new List<string>();
                var values = new List<decimal>();
                for (var i = 5; i >= 0; i--)
                {
                    var d = DateTime.Now.AddMonths(-i);
                    var hit = rawRevenue.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month);
                    labels.Add("T" + d.Month + "/" + d.Year);
                    values.Add(hit != null ? hit.DoanhThu : 0);
                }

                ViewBag.RecentOrders = recentOrders;
                ViewBag.LowStockProducts = lowStocks;
                ViewBag.RevenueLabels = labels;
                ViewBag.RevenueValues = values;
                return View(tk);
            }
        }

        #region Danh mục

        public ActionResult DanhMuc()
        {
            ViewBag.Title = "Danh mục";
            SetAdminPage("Danh sách danh mục",
                new AdminBreadcrumbItem { Text = "Danh mục sản phẩm", Url = Url.Action("DanhMuc") },
                new AdminBreadcrumbItem { Text = "Danh sách", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                return View(db.DanhMucs.OrderBy(x => x.MaDanhMuc).ToList());
            }
        }

        public ActionResult DanhMucTao()
        {
            ViewBag.Title = "Thêm danh mục";
            SetAdminPage("Thêm danh mục",
                new AdminBreadcrumbItem { Text = "Danh mục sản phẩm", Url = Url.Action("DanhMuc") },
                new AdminBreadcrumbItem { Text = "Thêm mới", Url = null });
            return View(new DanhMucFormVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DanhMucTao(DanhMucFormVm vm)
        {
            ViewBag.Title = "Thêm danh mục";
            SetAdminPage("Thêm danh mục",
                new AdminBreadcrumbItem { Text = "Danh mục sản phẩm", Url = Url.Action("DanhMuc") },
                new AdminBreadcrumbItem { Text = "Thêm mới", Url = null });
            if (!ModelState.IsValid)
                return View(vm);
            using (var db = new DoAnVatA3Context())
            {
                db.DanhMucs.Add(new DanhMuc
                {
                    TenDanhMuc = vm.TenDanhMuc.Trim(),
                    MoTa = vm.MoTa,
                    HinhAnh = "",
                    ConHoatDong = vm.ConHoatDong
                });
                db.SaveChanges();
                TempData["Msg"] = "Đã thêm danh mục.";
                return RedirectToAction("DanhMuc");
            }
        }

        public ActionResult DanhMucSua(int id)
        {
            ViewBag.Title = "Sửa danh mục";
            using (var db = new DoAnVatA3Context())
            {
                var dm = db.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == id);
                if (dm == null) return HttpNotFound();
                SetAdminPage("Cập nhật danh mục",
                    new AdminBreadcrumbItem { Text = "Danh mục sản phẩm", Url = Url.Action("DanhMuc") },
                    new AdminBreadcrumbItem { Text = dm.TenDanhMuc ?? "Chi tiết", Url = null });
                var vm = new DanhMucFormVm
                {
                    MaDanhMuc = dm.MaDanhMuc,
                    TenDanhMuc = dm.TenDanhMuc,
                    MoTa = dm.MoTa,
                    ConHoatDong = dm.ConHoatDong
                };
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DanhMucSua(DanhMucFormVm vm)
        {
            ViewBag.Title = "Sửa danh mục";
            SetAdminPage("Cập nhật danh mục",
                new AdminBreadcrumbItem { Text = "Danh mục sản phẩm", Url = Url.Action("DanhMuc") },
                new AdminBreadcrumbItem { Text = "Chỉnh sửa", Url = null });
            if (!ModelState.IsValid || vm.MaDanhMuc == null)
                return View(vm);
            using (var db = new DoAnVatA3Context())
            {
                var dm = db.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == vm.MaDanhMuc);
                if (dm == null) return HttpNotFound();
                dm.TenDanhMuc = vm.TenDanhMuc.Trim();
                dm.MoTa = vm.MoTa;
                dm.ConHoatDong = vm.ConHoatDong;
                db.SaveChanges();
                TempData["Msg"] = "Đã cập nhật danh mục.";
                return RedirectToAction("DanhMuc");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DanhMucXoa(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                if (db.SanPhams.Any(x => x.MaDanhMuc == id))
                {
                    TempData["Err"] = "Không xóa được: vẫn còn sản phẩm thuộc danh mục này.";
                    return RedirectToAction("DanhMuc");
                }
                var dm = db.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == id);
                if (dm == null)
                    return RedirectToAction("DanhMuc");
                db.DanhMucs.Remove(dm);
                db.SaveChanges();
                TempData["Msg"] = "Đã xóa danh mục.";
            }
            return RedirectToAction("DanhMuc");
        }

        #endregion

        #region Liên hệ (quản trị)

        public ActionResult QuanLyLienHe()
        {
            ViewBag.Title = "Liên hệ khách gửi";
            SetAdminPage("Tin nhắn liên hệ",
                new AdminBreadcrumbItem { Text = "Liên hệ", Url = Url.Action("QuanLyLienHe") },
                new AdminBreadcrumbItem { Text = "Danh sách", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                var list = db.LienHes.OrderByDescending(x => x.NgayGui).ToList();
                return View(list);
            }
        }

        public ActionResult QuanLyLienHeUnreadCount()
        {
            using (var db = new DoAnVatA3Context())
            {
                var count = db.LienHes.Count(x => !x.DaDoc);
                return Json(new { count }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TopbarStats()
        {
            using (var db = new DoAnVatA3Context())
            {
                var unreadContacts = db.LienHes.Count(x => !x.DaDoc);
                var pendingOrders = db.DonHangs.Count(x => x.TrangThai == "ChoDuyet");
                return Json(new { unreadContacts, pendingOrders }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult QuanLyLienHeXem(int id)
        {
            ViewBag.Title = "Chi tiết liên hệ";
            using (var db = new DoAnVatA3Context())
            {
                var lh = db.LienHes.FirstOrDefault(x => x.MaLienHe == id);
                if (lh == null) return HttpNotFound();
                if (!lh.DaDoc)
                {
                    lh.DaDoc = true;
                    db.SaveChanges();
                }
                SetAdminPage("Chi tiết liên hệ",
                    new AdminBreadcrumbItem { Text = "Liên hệ", Url = Url.Action("QuanLyLienHe") },
                    new AdminBreadcrumbItem { Text = lh.TieuDe ?? "Chi tiết", Url = null });
                return View(lh);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuanLyLienHeXoa(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var lh = db.LienHes.FirstOrDefault(x => x.MaLienHe == id);
                if (lh == null)
                    return RedirectToAction("QuanLyLienHe");

                db.LienHes.Remove(lh);
                db.SaveChanges();
                TempData["Msg"] = "Đã xóa tin nhắn liên hệ.";
            }
            return RedirectToAction("QuanLyLienHe");
        }

        #endregion

        #region Tài khoản

        public ActionResult QuanLyTaiKhoan(int page = 1)
        {
            ViewBag.Title = "Tài khoản người dùng";
            SetAdminPage("Danh sách tài khoản",
                new AdminBreadcrumbItem { Text = "Quản lý tài khoản", Url = Url.Action("QuanLyTaiKhoan") },
                new AdminBreadcrumbItem { Text = "Danh sách", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                const int pageSize = 10;
                if (page < 1) page = 1;

                var query = db.NguoiDungs.OrderByDescending(x => x.MaNguoiDung);
                var totalItems = query.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                if (totalPages == 0) totalPages = 1;
                if (page > totalPages) page = totalPages;

                var data = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuanLyTaiKhoanXoa(int id, int page = 1)
        {
            using (var db = new DoAnVatA3Context())
            {
                var u = db.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == id);
                if (u == null) return RedirectToAction("QuanLyTaiKhoan", new { page });
                if (u.VaiTro == "QuanTri")
                {
                    TempData["Err"] = "Không thể xóa tài khoản quản trị.";
                    return RedirectToAction("QuanLyTaiKhoan", new { page });
                }

                var current = Session["MaNguoiDung"] != null ? Convert.ToInt32(Session["MaNguoiDung"]) : 0;
                if (current == id)
                {
                    TempData["Err"] = "Không thể tự xóa tài khoản đang đăng nhập.";
                    return RedirectToAction("QuanLyTaiKhoan", new { page });
                }

                // Dọn các dữ liệu liên quan đến user trước khi xóa để tránh lỗi FK.
                db.GioHangs.RemoveRange(db.GioHangs.Where(x => x.MaNguoiDung == id));
                db.DanhGiaSanPhams.RemoveRange(db.DanhGiaSanPhams.Where(x => x.MaNguoiDung == id));

                var donHangs = db.DonHangs.Where(x => x.MaNguoiDung == id).ToList();
                foreach (var dh in donHangs)
                    dh.MaNguoiDung = null;

                var phieuNhaps = db.PhieuNhaps.Where(x => x.NguoiNhap == id).ToList();
                foreach (var pn in phieuNhaps)
                    pn.NguoiNhap = null;

                db.NguoiDungs.Remove(u);
                db.SaveChanges();
                TempData["Msg"] = "Đã xóa tài khoản.";
            }
            return RedirectToAction("QuanLyTaiKhoan", new { page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuanLyTaiKhoanKhoa(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var u = db.NguoiDungs.FirstOrDefault(x => x.MaNguoiDung == id);
                if (u != null && u.VaiTro != "QuanTri")
                {
                    u.ConHoatDong = !u.ConHoatDong;
                    db.SaveChanges();
                    TempData["Msg"] = u.ConHoatDong ? "Đã mở khóa tài khoản." : "Đã khóa tài khoản.";
                }
            }
            return RedirectToAction("QuanLyTaiKhoan");
        }

        #endregion

        #region Sản phẩm

        public ActionResult SanPham(int page = 1)
        {
            SetAdminPage("Danh sách sản phẩm",
                new AdminBreadcrumbItem { Text = "Sản phẩm", Url = Url.Action("SanPham") },
                new AdminBreadcrumbItem { Text = "Danh sách", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                const int pageSize = 10;
                if (page < 1) page = 1;

                var query = from sp in db.SanPhams
                    join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                    orderby sp.MaSanPham
                    select new SanPhamAdminRowVm
                    {
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        TenDanhMuc = dm.TenDanhMuc,
                        GiaBan = sp.GiaBan,
                        SoLuongTon = sp.SoLuongTon,
                        ConHoatDong = sp.ConHoatDong
                    };
                var totalItems = query.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                if (totalPages == 0) totalPages = 1;
                if (page > totalPages) page = totalPages;

                var list = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;
                return View(list);
            }
        }

        public ActionResult SanPhamXem(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var detail = (from sp in db.SanPhams
                              join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                              join ncc in db.NhaCungCaps on sp.MaNhaCungCap equals ncc.MaNhaCungCap into nccJoin
                              from ncc in nccJoin.DefaultIfEmpty()
                              where sp.MaSanPham == id
                              select new SanPhamAdminDetailVm
                              {
                                  MaSanPham = sp.MaSanPham,
                                  TenSanPham = sp.TenSanPham,
                                  TenDanhMuc = dm.TenDanhMuc,
                                  TenNhaCungCap = ncc != null ? ncc.TenNhaCungCap : "(Không có)",
                                  GiaNhap = sp.GiaNhap,
                                  GiaBan = sp.GiaBan,
                                  PhanTramGiam = sp.PhanTramGiam,
                                  GiaSauGiam = sp.GiaBan * (1m - (sp.PhanTramGiam / 100m)),
                                  SoLuongTon = sp.SoLuongTon,
                                  DonViTinh = sp.DonViTinh,
                                  ConHoatDong = sp.ConHoatDong,
                                  NgayTao = sp.NgayTao,
                                  HinhAnh = sp.HinhAnh,
                                  MoTa = sp.MoTa
                              }).FirstOrDefault();
                if (detail == null) return HttpNotFound();

                SetAdminPage("Chi tiết sản phẩm",
                    new AdminBreadcrumbItem { Text = "Sản phẩm", Url = Url.Action("SanPham") },
                    new AdminBreadcrumbItem { Text = detail.TenSanPham ?? ("SP #" + id), Url = null });

                return View(detail);
            }
        }

        public ActionResult SanPhamTao()
        {
            SetAdminPage("Thêm sản phẩm",
                new AdminBreadcrumbItem { Text = "Sản phẩm", Url = Url.Action("SanPham") },
                new AdminBreadcrumbItem { Text = "Thêm mới", Url = null });
            LoadDropdowns();
            return View(new SanPhamFormVm { ConHoatDong = true, DonViTinh = "Goi", GiaNhap = 0, GiaBan = 0, PhanTramGiam = 0, SoLuongTon = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SanPhamTao(SanPhamFormVm vm, System.Web.HttpPostedFileBase AnhUpload)
        {
            SetAdminPage("Thêm sản phẩm",
                new AdminBreadcrumbItem { Text = "Sản phẩm", Url = Url.Action("SanPham") },
                new AdminBreadcrumbItem { Text = "Thêm mới", Url = null });
            LoadDropdowns();
            if (!ModelState.IsValid)
                return View(vm);

            using (var db = new DoAnVatA3Context())
            {
                var tenFileAnh = string.IsNullOrWhiteSpace(vm.HinhAnh) ? "no-image.png" : vm.HinhAnh.Trim();
                if (AnhUpload != null && AnhUpload.ContentLength > 0)
                {
                    var ext = Path.GetExtension(AnhUpload.FileName) ?? "";
                    var safeExt = ext.ToLowerInvariant();
                    var ok = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    if (!ok.Contains(safeExt))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ cho phép ảnh .jpg, .jpeg, .png, .gif, .webp");
                        return View(vm);
                    }

                    var folder = Server.MapPath("~/Content/SanPhamImages/");
                    Directory.CreateDirectory(folder);
                    tenFileAnh = "sp_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + safeExt;
                    AnhUpload.SaveAs(Path.Combine(folder, tenFileAnh));
                }

                var sp = new SanPham
                {
                    TenSanPham = vm.TenSanPham.Trim(),
                    MaDanhMuc = vm.MaDanhMuc,
                    MaNhaCungCap = vm.MaNhaCungCap,
                    MoTa = vm.MoTa,
                    GiaNhap = vm.GiaNhap,
                    GiaBan = vm.GiaBan,
                    PhanTramGiam = vm.PhanTramGiam,
                    SoLuongTon = vm.SoLuongTon,
                    DonViTinh = vm.DonViTinh ?? "Goi",
                    HinhAnh = tenFileAnh,
                    ConHoatDong = vm.ConHoatDong,
                    NgayTao = DateTime.Now
                };
                db.SanPhams.Add(sp);
                db.SaveChanges();
                TempData["Msg"] = "Đã thêm sản phẩm.";
                return RedirectToAction("SanPham");
            }
        }

        public ActionResult SanPhamSua(int id)
        {
            LoadDropdowns();
            using (var db = new DoAnVatA3Context())
            {
                var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == id);
                if (sp == null) return HttpNotFound();
                SetAdminPage("Cập nhật sản phẩm",
                    new AdminBreadcrumbItem { Text = "Sản phẩm", Url = Url.Action("SanPham") },
                    new AdminBreadcrumbItem { Text = sp.TenSanPham ?? "Chi tiết", Url = null });
                var vm = new SanPhamFormVm
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    MaDanhMuc = sp.MaDanhMuc,
                    MaNhaCungCap = sp.MaNhaCungCap,
                    MoTa = sp.MoTa,
                    GiaNhap = sp.GiaNhap,
                    GiaBan = sp.GiaBan,
                    PhanTramGiam = sp.PhanTramGiam,
                    SoLuongTon = sp.SoLuongTon,
                    DonViTinh = sp.DonViTinh,
                    HinhAnh = sp.HinhAnh,
                    ConHoatDong = sp.ConHoatDong
                };
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SanPhamSua(SanPhamFormVm vm, System.Web.HttpPostedFileBase AnhUpload)
        {
            SetAdminPage("Cập nhật sản phẩm",
                new AdminBreadcrumbItem { Text = "Sản phẩm", Url = Url.Action("SanPham") },
                new AdminBreadcrumbItem { Text = "Chỉnh sửa", Url = null });
            LoadDropdowns();
            if (!ModelState.IsValid || vm.MaSanPham == null)
                return View(vm);

            using (var db = new DoAnVatA3Context())
            {
                var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == vm.MaSanPham);
                if (sp == null) return HttpNotFound();

                var tenFileAnh = string.IsNullOrWhiteSpace(vm.HinhAnh) ? "no-image.png" : vm.HinhAnh.Trim();
                if (AnhUpload != null && AnhUpload.ContentLength > 0)
                {
                    var ext = Path.GetExtension(AnhUpload.FileName) ?? "";
                    var safeExt = ext.ToLowerInvariant();
                    var ok = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    if (!ok.Contains(safeExt))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ cho phép ảnh .jpg, .jpeg, .png, .gif, .webp");
                        return View(vm);
                    }

                    var folder = Server.MapPath("~/Content/SanPhamImages/");
                    Directory.CreateDirectory(folder);
                    tenFileAnh = "sp_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + safeExt;
                    AnhUpload.SaveAs(Path.Combine(folder, tenFileAnh));
                }

                sp.TenSanPham = vm.TenSanPham.Trim();
                sp.MaDanhMuc = vm.MaDanhMuc;
                sp.MaNhaCungCap = vm.MaNhaCungCap;
                sp.MoTa = vm.MoTa;
                sp.GiaNhap = vm.GiaNhap;
                sp.GiaBan = vm.GiaBan;
                sp.PhanTramGiam = vm.PhanTramGiam;
                sp.SoLuongTon = vm.SoLuongTon;
                sp.DonViTinh = vm.DonViTinh ?? "Goi";
                sp.HinhAnh = tenFileAnh;
                sp.ConHoatDong = vm.ConHoatDong;
                db.SaveChanges();
                TempData["Msg"] = "Đã cập nhật.";
                return RedirectToAction("SanPham");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SanPhamNgung(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == id);
                if (sp != null)
                {
                    sp.ConHoatDong = false;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SanPham");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SanPhamXoa(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == id);
                if (sp == null)
                    return RedirectToAction("SanPham");

                // Xóa hẳn: dọn các bảng tham chiếu để tránh lỗi FK
                db.GioHangs.RemoveRange(db.GioHangs.Where(x => x.MaSanPham == id));
                db.DanhGiaSanPhams.RemoveRange(db.DanhGiaSanPhams.Where(x => x.MaSanPham == id));
                db.ChiTietDonHangs.RemoveRange(db.ChiTietDonHangs.Where(x => x.MaSanPham == id));
                db.ChiTietPhieuNhaps.RemoveRange(db.ChiTietPhieuNhaps.Where(x => x.MaSanPham == id));

                db.SanPhams.Remove(sp);
                db.SaveChanges();
                TempData["Msg"] = "Đã xóa sản phẩm (xóa hẳn).";
            }
            return RedirectToAction("SanPham");
        }

        private void LoadDropdowns()
        {
            using (var db = new DoAnVatA3Context())
            {
                ViewBag.DanhMucs = db.DanhMucs.Where(x => x.ConHoatDong).OrderBy(x => x.TenDanhMuc).ToList();
                ViewBag.NhaCungCaps = db.NhaCungCaps.Where(x => x.ConHoatDong).OrderBy(x => x.TenNhaCungCap).ToList();
            }
        }

        #endregion

        #region Phiếu nhập

        public ActionResult PhieuNhap()
        {
            SetAdminPage("Danh sách phiếu nhập",
                new AdminBreadcrumbItem { Text = "Phiếu nhập kho", Url = Url.Action("PhieuNhap") },
                new AdminBreadcrumbItem { Text = "Danh sách", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                var list = from p in db.PhieuNhaps
                    join n in db.NhaCungCaps on p.MaNhaCungCap equals n.MaNhaCungCap into gj
                    from n in gj.DefaultIfEmpty()
                    orderby p.NgayNhap descending
                    select new PhieuNhapRowVm
                    {
                        MaPhieuNhap = p.MaPhieuNhap,
                        MaPhieu = p.MaPhieu,
                        TenNhaCungCap = n != null ? n.TenNhaCungCap : "",
                        NgayNhap = p.NgayNhap,
                        TongTien = p.TongTien
                    };
                return View(list.ToList());
            }
        }

        public ActionResult PhieuNhapTao()
        {
            SetAdminPage("Tạo phiếu nhập",
                new AdminBreadcrumbItem { Text = "Phiếu nhập kho", Url = Url.Action("PhieuNhap") },
                new AdminBreadcrumbItem { Text = "Thêm mới", Url = null });
            LoadDropdowns();
            using (var db = new DoAnVatA3Context())
            {
                ViewBag.SanPhams = db.SanPhams.Where(x => x.ConHoatDong).OrderBy(x => x.TenSanPham).ToList();
            }
            return View(new PhieuNhapTaoVm { SoLuong = 1, DonGiaNhap = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhieuNhapTao(PhieuNhapTaoVm vm)
        {
            SetAdminPage("Tạo phiếu nhập",
                new AdminBreadcrumbItem { Text = "Phiếu nhập kho", Url = Url.Action("PhieuNhap") },
                new AdminBreadcrumbItem { Text = "Thêm mới", Url = null });
            LoadDropdowns();
            using (var db = new DoAnVatA3Context())
            {
                ViewBag.SanPhams = db.SanPhams.Where(x => x.ConHoatDong).OrderBy(x => x.TenSanPham).ToList();
            }
            if (!ModelState.IsValid)
                return View(vm);

            var maNguoi = Session["MaNguoiDung"] != null ? (int?)Convert.ToInt32(Session["MaNguoiDung"]) : null;

            using (var db = new DoAnVatA3Context())
            {
                if (db.PhieuNhaps.Any(x => x.MaPhieu == vm.MaPhieu.Trim()))
                {
                    ModelState.AddModelError("MaPhieu", "Mã phiếu đã tồn tại.");
                    return View(vm);
                }

                var pn = new PhieuNhap
                {
                    MaPhieu = vm.MaPhieu.Trim(),
                    MaNhaCungCap = vm.MaNhaCungCap,
                    NguoiNhap = maNguoi,
                    GhiChu = vm.GhiChu,
                    NgayNhap = DateTime.Now,
                    TongTien = 0
                };
                db.PhieuNhaps.Add(pn);
                db.SaveChanges();

                db.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                {
                    MaPhieuNhap = pn.MaPhieuNhap,
                    MaSanPham = vm.MaSanPham,
                    SoLuong = vm.SoLuong,
                    DonGiaNhap = vm.DonGiaNhap
                });
                db.SaveChanges();
                TempData["Msg"] = "Đã tạo phiếu nhập.";
                return RedirectToAction("PhieuNhap");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhieuNhapXoa(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var pn = db.PhieuNhaps.FirstOrDefault(x => x.MaPhieuNhap == id);
                if (pn == null)
                    return RedirectToAction("PhieuNhap");

                var chiTiets = db.ChiTietPhieuNhaps.Where(x => x.MaPhieuNhap == id).ToList();

                // Khi xóa phiếu nhập, hoàn tồn kho theo số lượng đã nhập.
                foreach (var ct in chiTiets)
                {
                    var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == ct.MaSanPham);
                    if (sp == null) continue;

                    if (sp.SoLuongTon < ct.SoLuong)
                    {
                        TempData["Err"] = "Không thể xóa phiếu nhập này vì tồn kho hiện tại đã thấp hơn số lượng nhập ban đầu.";
                        return RedirectToAction("PhieuNhap");
                    }
                }

                foreach (var ct in chiTiets)
                {
                    var sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == ct.MaSanPham);
                    if (sp != null)
                        sp.SoLuongTon -= ct.SoLuong;
                }

                db.ChiTietPhieuNhaps.RemoveRange(chiTiets);
                db.PhieuNhaps.Remove(pn);
                db.SaveChanges();
                TempData["Msg"] = "Đã xóa phiếu nhập.";
            }
            return RedirectToAction("PhieuNhap");
        }

        #endregion

        #region Đơn hàng

        public ActionResult DonHang(int page = 1)
        {
            SetAdminPage("Danh sách đơn hàng",
                new AdminBreadcrumbItem { Text = "Đơn hàng", Url = Url.Action("DonHang") },
                new AdminBreadcrumbItem { Text = "Danh sách", Url = null });
            using (var db = new DoAnVatA3Context())
            {
                const int pageSize = 10;
                if (page < 1) page = 1;

                var query = db.DonHangs.OrderByDescending(x => x.NgayDat);
                var totalItems = query.Count();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                if (totalPages == 0) totalPages = 1;
                if (page > totalPages) page = totalPages;

                var list = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.Page = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalItems = totalItems;
                return View(list);
            }
        }

        public ActionResult DonHangXem(int id)
        {
            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                if (dh == null) return HttpNotFound();
                SetAdminPage("Chi tiết đơn hàng",
                    new AdminBreadcrumbItem { Text = "Đơn hàng", Url = Url.Action("DonHang") },
                    new AdminBreadcrumbItem { Text = dh.MaDon ?? ("Đơn #" + dh.MaDonHang), Url = null });
                var ct = from c in db.ChiTietDonHangs
                    join sp in db.SanPhams on c.MaSanPham equals sp.MaSanPham
                    where c.MaDonHang == id
                    select new DonHangCtVm
                    {
                        TenSanPham = sp.TenSanPham,
                        SoLuong = c.SoLuong,
                        DonGiaBan = c.DonGiaBan,
                        ThanhTien = c.ThanhTien
                    };
                ViewBag.ChiTiet = ct.ToList();
                return View(dh);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonHangXoa(int id, int page = 1)
        {
            try
            {
                using (var db = new DoAnVatA3Context())
                {
                    var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                    if (dh == null)
                        return RedirectToAction("DonHang", new { page });

                    var ctDonHangs = db.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
                    var thanhToans = db.ThanhToans.Where(x => x.MaDonHang == id).ToList();

                    // Xóa dữ liệu con trước và lưu riêng để tránh lỗi thứ tự xóa FK.
                    if (ctDonHangs.Count > 0)
                        db.ChiTietDonHangs.RemoveRange(ctDonHangs);
                    if (thanhToans.Count > 0)
                        db.ThanhToans.RemoveRange(thanhToans);
                    if (ctDonHangs.Count > 0 || thanhToans.Count > 0)
                        db.SaveChanges();

                    db.DonHangs.Remove(dh);
                    db.SaveChanges();
                    TempData["Msg"] = "Đã xóa đơn hàng.";
                }
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException != null && ex.InnerException.InnerException != null
                    ? ex.InnerException.InnerException.Message
                    : (ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                TempData["Err"] = "Không thể xóa đơn hàng do đang có dữ liệu liên kết. Chi tiết: " + inner;
            }
            return RedirectToAction("DonHang", new { page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonHangTrangThai(int id, string trangThai)
        {
            var hopLe = new[] { "ChoDuyet", "DaXacNhan", "DangGiao", "DaGiao", "DaHuy" };
            if (!hopLe.Contains(trangThai))
                return RedirectToAction("DonHang");

            using (var db = new DoAnVatA3Context())
            {
                var dh = db.DonHangs.FirstOrDefault(x => x.MaDonHang == id);
                if (dh != null)
                {
                    dh.TrangThai = trangThai;
                    if (trangThai == "DaGiao")
                        dh.TrangThaiThanhToan = "DaThanhToan";
                    db.SaveChanges();
                }
            }
            return RedirectToAction("DonHangXem", new { id });
        }

        #endregion

        private void SetAdminPage(string heading, params AdminBreadcrumbItem[] items)
        {
            ViewBag.AdminHeading = heading;
            ViewBag.Title = heading;
            var list = new List<AdminBreadcrumbItem>();
            if (items != null && items.Length > 0)
                list.AddRange(items);
            ViewBag.AdminBreadcrumbs = list;
        }
    }
}
