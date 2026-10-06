using System;
using System.ComponentModel.DataAnnotations;

namespace A3_MVC_ASP.Models
{
    public class AdminBreadcrumbItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }

    public class DanhMucFormVm
    {
        public int? MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Nhập tên danh mục")]
        [StringLength(200)]
        public string TenDanhMuc { get; set; }

        [StringLength(2000)]
        public string MoTa { get; set; }

        public bool ConHoatDong { get; set; } = true;
    }
    public class SanPhamAdminRowVm
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenDanhMuc { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public bool ConHoatDong { get; set; }
    }

    public class SanPhamAdminDetailVm
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenDanhMuc { get; set; }
        public string TenNhaCungCap { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public decimal PhanTramGiam { get; set; }
        public decimal GiaSauGiam { get; set; }
        public int SoLuongTon { get; set; }
        public string DonViTinh { get; set; }
        public bool ConHoatDong { get; set; }
        public DateTime NgayTao { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
    }

    public class PhieuNhapRowVm
    {
        public int MaPhieuNhap { get; set; }
        public string MaPhieu { get; set; }
        public string TenNhaCungCap { get; set; }
        public DateTime NgayNhap { get; set; }
        public decimal TongTien { get; set; }
    }

    public class DonHangCtVm
    {
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaBan { get; set; }
        public decimal? ThanhTien { get; set; }
    }

    public class DashboardOrderItemVm
    {
        public int MaDonHang { get; set; }
        public string MaDon { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongThanhToan { get; set; }
        public string TrangThai { get; set; }
    }

    public class DashboardLowStockVm
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenDanhMuc { get; set; }
        public int SoLuongTon { get; set; }
    }
}
