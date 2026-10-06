using System;

namespace A3_MVC_ASP.Models
{
    /// <summary>Bảng NguoiDung — VaiTro: QuanTri, KhachHang.</summary>
    public class NguoiDung
    {
        public int MaNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string VaiTro { get; set; }
        public string AnhDaiDien { get; set; }
        public bool ConHoatDong { get; set; }
        public DateTime NgayTao { get; set; }
    }

    public class DanhMuc
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public bool ConHoatDong { get; set; }
    }

    public class NhaCungCap
    {
        public int MaNhaCungCap { get; set; }
        public string TenNhaCungCap { get; set; }
        public string NguoiLienHe { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public bool ConHoatDong { get; set; }
    }

    public class SanPham
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int MaDanhMuc { get; set; }
        public int? MaNhaCungCap { get; set; }
        public string MoTa { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public decimal PhanTramGiam { get; set; }
        public int SoLuongTon { get; set; }
        public string DonViTinh { get; set; }
        public string HinhAnh { get; set; }
        public bool ConHoatDong { get; set; }
        public DateTime NgayTao { get; set; }
    }

    public class PhieuNhap
    {
        public int MaPhieuNhap { get; set; }
        public string MaPhieu { get; set; }
        public int? MaNhaCungCap { get; set; }
        public int? NguoiNhap { get; set; }
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayNhap { get; set; }
    }

    /// <summary>ThanhTien: cột computed (PERSISTED) trên SQL Server.</summary>
    public class ChiTietPhieuNhap
    {
        public int MaChiTiet { get; set; }
        public int MaPhieuNhap { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }
        public decimal? ThanhTien { get; set; }
    }

    public class DonHang
    {
        public int MaDonHang { get; set; }
        public string MaDon { get; set; }
        public int? MaNguoiDung { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChiGiao { get; set; }
        public decimal TongTienHang { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal TongThanhToan { get; set; }
        public string TrangThai { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string TrangThaiThanhToan { get; set; }
        public string GhiChu { get; set; }
        public DateTime NgayDat { get; set; }
    }

    // Log thanh toán (demo + thực tế đều dùng được)
    public class ThanhToan
    {
        public int MaThanhToan { get; set; }
        public int MaDonHang { get; set; }
        public string Provider { get; set; } // MoMo / MoMoDemo / ...
        public string MaGiaoDich { get; set; } // transId/requestId (nếu có)
        public decimal SoTien { get; set; }
        public string TrangThai { get; set; } // ThanhCong / ThatBai / Demo
        public DateTime NgayThanhToan { get; set; }
        public string RawJson { get; set; }
    }

    public class ChiTietDonHang
    {
        public int MaChiTiet { get; set; }
        public int MaDonHang { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaBan { get; set; }
        public decimal? ThanhTien { get; set; }
    }

    public class GioHang
    {
        public int MaGioHang { get; set; }
        public int? MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayThem { get; set; }
    }

    public class DanhGiaSanPham
    {
        public int MaDanhGia { get; set; }
        public int MaSanPham { get; set; }
        public int MaNguoiDung { get; set; }
        public byte SoSao { get; set; }
        public string NhanXet { get; set; }
        public DateTime NgayDanhGia { get; set; }
    }

    public class LienHe
    {
        public int MaLienHe { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public bool DaDoc { get; set; }
        public string PhanHoi { get; set; }
        public DateTime NgayGui { get; set; }
    }

    public class Banner
    {
        public int MaBanner { get; set; }
        public string TieuDe { get; set; }
        public string HinhAnh { get; set; }
        public string DuongDan { get; set; }
        public bool ConHoatDong { get; set; }
        public int ThuTuHienThi { get; set; }
    }
}
