using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using A3_MVC_ASP.Filters;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    [QuanTriAuthorize]
    public class BaoCaoController : Controller
    {
        private static bool LaDonDaXacNhan(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai)) return false;
            var s = trangThai.Trim().ToLowerInvariant();
            return s == "daxacnhan" || s == "da xac nhan" || s == "đã xác nhận" || s == "đãxácnhận";
        }

        private sealed class BaoCaoData
        {
            public ThongKeTongQuanDto ThongKe { get; set; }
            public List<NhapXuatTonRow> NhapXuatTon { get; set; }
            public List<DoanhThuThangRow> DoanhThuThang { get; set; }
        }

        public ActionResult Index(int nxPage = 1)
        {
            ViewBag.Title = "Báo cáo — Thống kê";
            ViewBag.AdminHeading = "Báo cáo — Thống kê";
            ViewBag.AdminBreadcrumbs = new List<AdminBreadcrumbItem>
            {
                new AdminBreadcrumbItem { Text = "Báo cáo", Url = Url.Action("Index", "BaoCao") },
                new AdminBreadcrumbItem { Text = "Thống kê tổng quan", Url = null }
            };
            var data = LoadBaoCaoData();
            const int pageSize = 10;
            if (nxPage < 1) nxPage = 1;
            var totalItems = data.NhapXuatTon.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (nxPage > totalPages) nxPage = totalPages;
            var pagedNx = data.NhapXuatTon.Skip((nxPage - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.ThongKe = data.ThongKe;
            ViewBag.NhapXuatTon = pagedNx;
            ViewBag.DoanhThuThang = data.DoanhThuThang;
            ViewBag.NxPage = nxPage;
            ViewBag.NxTotalPages = totalPages;
            ViewBag.NxTotalItems = totalItems;
            return View();
        }

        public ActionResult XuatExcel()
        {
            var data = LoadBaoCaoData();
            var sb = new StringBuilder();
            sb.AppendLine("<html><head><meta charset='utf-8'/>");
            sb.AppendLine("<style>");
            sb.AppendLine("body{font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#111;}");
            sb.AppendLine("h2{margin:0 0 10px 0;} h3{margin:18px 0 8px 0;}");
            sb.AppendLine("table{border-collapse:collapse;width:100%;margin-bottom:14px;}");
            sb.AppendLine("th,td{border:1px solid #333;padding:6px 8px;}");
            sb.AppendLine("th{background:#f2f2f2;font-weight:bold;}");
            sb.AppendLine(".right{text-align:right;} .center{text-align:center;}");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine("<h2>BÁO CÁO THỐNG KÊ</h2>");
            sb.AppendLine("<div>Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "</div>");

            if (data.ThongKe != null)
            {
                sb.AppendLine("<h3>Tổng quan</h3><table>");
                sb.AppendLine("<tr><th>Tổng đơn hàng</th><th>Doanh thu (đã xác nhận)</th><th>Sản phẩm</th><th>Khách hàng</th><th>Chờ duyệt</th><th>Sắp hết (&lt;10)</th></tr>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td class='center'>" + data.ThongKe.TongDonHang + "</td>");
                sb.AppendLine("<td class='right'>" + data.ThongKe.TongDoanhThu.ToString("N0") + " đ</td>");
                sb.AppendLine("<td class='center'>" + data.ThongKe.TongSanPham + "</td>");
                sb.AppendLine("<td class='center'>" + data.ThongKe.TongKhachHang + "</td>");
                sb.AppendLine("<td class='center'>" + data.ThongKe.DonChuaDuyet + "</td>");
                sb.AppendLine("<td class='center'>" + data.ThongKe.SanPhamSapHet + "</td>");
                sb.AppendLine("</tr></table>");
            }

            sb.AppendLine("<h3>Doanh thu theo tháng</h3><table>");
            sb.AppendLine("<tr><th>Năm</th><th>Tháng</th><th>Số đơn</th><th>Doanh thu</th></tr>");
            foreach (var r in data.DoanhThuThang)
            {
                sb.AppendLine("<tr>" +  
                              "<td class='center'>" + r.Nam + "</td>" +
                              "<td class='center'>" + r.Thang + "</td>" +
                              "<td class='center'>" + r.TongDonHang + "</td>" +
                              "<td class='right'>" + r.DoanhThu.ToString("N0") + " đ</td>" +
                              "</tr>");
            }
            sb.AppendLine("</table>");

            sb.AppendLine("<h3>Nhập - Xuất - Tồn</h3><table>");
            sb.AppendLine("<tr><th>SP</th><th>Tên</th><th>Danh mục</th><th>Nhập</th><th>Bán</th><th>Tồn</th><th>Tiền nhập</th><th>Doanh thu</th></tr>");
            foreach (var r in data.NhapXuatTon)
            {
                sb.AppendLine("<tr>" +
                              "<td class='center'>" + r.MaSanPham + "</td>" +
                              "<td>" + HttpUtility.HtmlEncode(r.TenSanPham) + "</td>" +
                              "<td>" + HttpUtility.HtmlEncode(r.TenDanhMuc) + "</td>" +
                              "<td class='center'>" + r.TongSoLuongNhap.ToString("N0") + "</td>" +
                              "<td class='center'>" + r.TongSoLuongBan.ToString("N0") + "</td>" +
                              "<td class='center'>" + r.TonKhoHienTai + "</td>" +
                              "<td class='right'>" + r.TongTienNhap.ToString("N0") + "</td>" +
                              "<td class='right'>" + r.TongDoanhThu.ToString("N0") + "</td>" +
                              "</tr>");
            }
            sb.AppendLine("</table></body></html>");

            var html = sb.ToString();
            var bom = Encoding.UTF8.GetPreamble();
            var bodyBytes = Encoding.UTF8.GetBytes(html);
            var fileBytes = new byte[bom.Length + bodyBytes.Length];
            Buffer.BlockCopy(bom, 0, fileBytes, 0, bom.Length);
            Buffer.BlockCopy(bodyBytes, 0, fileBytes, bom.Length, bodyBytes.Length);

            var fileName = "BaoCao_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls";
            return File(fileBytes, "application/vnd.ms-excel; charset=utf-8", fileName);
        }

        private BaoCaoData LoadBaoCaoData()
        {
            using (var db = new DoAnVatA3Context())
            {
                var thongKe = db.Database.SqlQuery<ThongKeTongQuanDto>("EXEC sp_ThongKeTongQuan").FirstOrDefault()
                              ?? new ThongKeTongQuanDto();

                var donDaXacNhan = db.DonHangs
                    .Select(x => new
                    {
                        x.TrangThai,
                        x.TongThanhToan,
                        x.NgayDat
                    })
                    .ToList()
                    .Where(x => LaDonDaXacNhan(x.TrangThai))
                    .ToList();

                thongKe.TongDoanhThu = donDaXacNhan
                    .Sum(x => x.TongThanhToan);

                return new BaoCaoData
                {
                    ThongKe = thongKe,
                    NhapXuatTon = db.Database.SqlQuery<NhapXuatTonRow>(
                        "SELECT MaSanPham, TenSanPham, TenDanhMuc, TongSoLuongNhap, TongSoLuongBan, TonKhoHienTai, TongTienNhap, TongDoanhThu FROM vw_NhapXuatTon ORDER BY MaSanPham").ToList(),
                    DoanhThuThang = donDaXacNhan
                        .GroupBy(x => new { x.NgayDat.Year, x.NgayDat.Month })
                        .Select(g => new DoanhThuThangRow
                        {
                            Nam = g.Key.Year,
                            Thang = g.Key.Month,
                            TongDonHang = g.Count(),
                            DoanhThu = g.Sum(x => x.TongThanhToan)
                        })
                        .OrderByDescending(x => x.Nam)
                        .ThenByDescending(x => x.Thang)
                        .ToList()
                };
            }
        }
    }
}
